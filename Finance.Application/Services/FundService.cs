using Finance.Application.Abstractions;
using Finance.Application.Commands;
using Finance.Application.Dtos;
using Finance.Application.Exceptions;
using Finance.Application.Queries;
using Finance.Application.Responses;
using Finance.Domain.Entities;

namespace Finance.Application.Services;

public class FundService(
    ICurrencyService currencyService,
    IFundRepository fundRepository,
    IUserRepository userRepository,
    ICategoryRepository categoryRepository,
    ICurrencyRepository currencyRepository)
{
    private readonly IFundRepository fundRepository = fundRepository;

    private readonly IUserRepository userRepository = userRepository;

    private readonly ICategoryRepository categoryRepository = categoryRepository;

    private readonly ICurrencyService currencyService = currencyService;

    private readonly ICurrencyRepository currencyRepository = currencyRepository;

    private async Task ThrowIfFundDoesNotBelongToUser(int fundId, string userId)
    {
        if (!await fundRepository.DoesFundBelongToUserAsync(fundId, userId))
        {
            throw new EntityNotFoundException("Fund", fundId);
        }
    }

    private async Task ThrowIfUserNotExistAsync(string userId)
    {
        if (!await userRepository.ExistsAsync(userId))
        {
            throw new EntityNotFoundException("User", userId);
        }
    }

    public async Task CreateAsync(CreateFundCommand command)
    {
        await ThrowIfUserNotExistAsync(command.UserId);
        if (!await categoryRepository.DoesCategoryBelongToUserAsync(command.CategoryId, command.UserId))
        {
            throw new EntityNotFoundException("Category", command.CategoryId);
        }
        if (!await currencyRepository.ExistsAsync(command.CurrencyId))
        {
            throw new EntityNotFoundException("Currency", command.CurrencyId);
        }
        DateOnly dateOfBirth = await userRepository.GetDateOfBirthAsync(command.UserId);
        if (command.Date < dateOfBirth)
        {
            throw new ArgumentException("Fund date cannot be earlier than user's date of birth.");
        }
        Fund fund = await command.AsFundAsync(currencyService);
        await fundRepository.CreateAsync(fund);
    }

    public async Task DeleteAsync(DeleteFundCommand command)
    {
        await ThrowIfUserNotExistAsync(command.UserId);
        await ThrowIfFundDoesNotBelongToUser(command.FundId, command.UserId);
        await fundRepository.DeleteAsync(command.FundId);
    }

    public async Task<DetailedFundResponse> GetAsync(FundQuery query)
    {
        await ThrowIfUserNotExistAsync(query.UserId);
        await ThrowIfFundDoesNotBelongToUser(query.FundId, query.UserId);
        DetailedFundDto dto = await fundRepository.GetAsync(query.FundId) ?? throw new EntityNotFoundException("Fund", query.FundId);
        CurrencyResponse currencyResponse = await currencyRepository.GetAsync(dto.CurrencyId) ?? throw new EntityNotFoundException("Currency", dto.CurrencyId);
        string categoryName = await categoryRepository.GetNameAsync(dto.CategoryId) ?? throw new EntityNotFoundException("Category", dto.CategoryId);
        return new(
            dto.Id,
            dto.FundType.ToString(), 
            dto.Amount,
            currencyResponse.Name,
            currencyResponse.Code,
            categoryName,
            dto.Date,
            dto.Description,
            dto.ExchangeRateInUsd);
    }

    public async Task<IEnumerable<FundResponse>> GetAllAsync(string userId)
    {
        await ThrowIfUserNotExistAsync(userId);
        return ToFundResponseEnumerable(await fundRepository.GetAllAsync(userId));
    }

    public async Task<IEnumerable<FundResponse>> GetAllAsync(FundsQuery query)
    {
        await ThrowIfUserNotExistAsync(query.UserId);
        IEnumerable<FundResponse> fundResponses = ToFundResponseEnumerable(await fundRepository.GetAllAsync(query.UserId, query.FundFilterQuery?.ToFundFilterDto()));
        return fundResponses;
    }

    public async Task<IEnumerable<FundResponse>> GetAllAsync(FundWithinRangeQuery query)
    {
        await ThrowIfUserNotExistAsync(query.UserId);
        FundFilterDto? dto = query.FundFilterQuery?.ToFundFilterDto();
        DateOnly? from = query.From, to = query.To;
        if (!from.HasValue && to.HasValue)
        {
            from = await userRepository.GetDateOfBirthAsync(query.UserId);
        }
        else if (from.HasValue && !to.HasValue)
        {
            to = DateOnly.FromDateTime(DateTime.Now);
        }
        IEnumerable<FundDto> dtos;
        if (from.HasValue && to.HasValue)
        {
            if (from > to)
            {
                (from, to) = (to, from);
            }
            dtos = await fundRepository.GetAllAsync(query.UserId, from.Value, to.Value);
        }
        else
        {
            dtos = await fundRepository.GetAllAsync(query.UserId, dto);
        }
        return ToFundResponseEnumerable(dtos);
    }

    public async Task<IEnumerable<FundResponse>> GetAllAsync(FundInDateQuery query)
    {
        await ThrowIfUserNotExistAsync(query.UserId);
        FundFilterDto? dto = query.FundFilterQuery?.ToFundFilterDto();
        int year, fromMonth, fromDay, toMonth, toDay;
        if (query.Day.HasValue)
        {
            fromDay = toDay = query.Day.Value;
            fromMonth = toMonth = query.Month ?? throw new ArgumentException("Month must have a value.");
            year = query.Year ?? throw new ArgumentException("Year must have a value.");
        }
        else if (query.Month.HasValue)
        {
            year = query.Year ?? throw new ArgumentException("Year must have a value.");
            fromMonth = toMonth = query.Month.Value;
            fromDay = 1;
            toDay = DateTime.DaysInMonth(year, toMonth);
        }
        else if (query.Year.HasValue)
        {
            year = query.Year.Value;
            fromMonth = fromDay = 1;
            toMonth = 12;
            toDay = 31;
        }
        else
        {
            return ToFundResponseEnumerable(await fundRepository.GetAllAsync(query.UserId, dto));
        }
        DateOnly from = new(year, fromMonth, fromDay), to = new(year, toMonth, toDay);
        return ToFundResponseEnumerable(await fundRepository.GetAllAsync(query.UserId, from, to, dto));
    }

    private static IEnumerable<FundResponse> ToFundResponseEnumerable(IEnumerable<FundDto> dtos)
    {
        return dtos.Select(f => f.ToFundResponse());
    }

    public async Task UpdateAsync(UpdateFundCommand command)
    {
        await ThrowIfUserNotExistAsync(command.UserId);
        await ThrowIfFundDoesNotBelongToUser(command.FundId, command.UserId);
        decimal? exchangeRateInUsd = null;
        if (command.Date.HasValue && command.CurrencyId.HasValue)
        {
            exchangeRateInUsd = await currencyService.GetExchangeRateFromUsdAsync(command.Date.Value, command.CurrencyId.Value);
        }
        else if (command.CurrencyId.HasValue)
        {
            DateOnly date = await fundRepository.GetDateAsync(command.FundId);
            exchangeRateInUsd = await currencyService.GetExchangeRateFromUsdAsync(date, command.CurrencyId.Value);
        }
        UpdateFundDto dto = command.ToUpdateFundDto(exchangeRateInUsd);
        await fundRepository.UpdateAsync(dto);
    }
}
