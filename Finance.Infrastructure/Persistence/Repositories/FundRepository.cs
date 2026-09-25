using Finance.Application.Abstractions;
using Finance.Application.Dtos;
using Finance.Application.Exceptions;
using Finance.Application.Mappers;
using Finance.Application.Responses;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Finance.Infrastructure.Persistence.Repositories;

public class FundRepository(AppDbContext context) : IFundRepository
{
    private readonly AppDbContext context = context;

    public async Task CreateAsync(Fund fund)
    {
        await context.Funds.AddAsync(fund);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int fundId)
    {
        Fund fund = await context.Funds.FindAsync(fundId) ??
            throw new EntityNotFoundException("Fund", fundId);
        context.Funds.Remove(fund);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DoesFundBelongToUserAsync(int fundId, string userId)
    {
        return await context.Funds.AnyAsync(f => f.UserId == userId && f.Id == fundId);
    }

    private async Task<IEnumerable<FundDto>> GetAllAsync(Expression<Func<Fund, bool>> predicate, FundFilterDto? dto = null)
    {
        IQueryable<Fund> query = context.Funds.AsNoTracking().Where(predicate);

        if (dto?.FundType is not null)
        {
            query = query.Where(f => f.FundType == dto.FundType.Value);
        }
        if (dto?.CurrencyId is not null)
        {
            query = query.Where(f => f.Money.CurrencyId == dto.CurrencyId.Value);
        }
        if (dto?.CategoryId is not null)
        {
            query = query.Where(f => f.CategoryId == dto.CategoryId.Value);
        }

        var funds = await query.Join(context.Currencies, f => f.Money.CurrencyId, c => c.Id, (f, c) => new
        {
            f.CategoryId,
            f.Id,
            f.Amount,
            f.Date,
            f.Description,
            f.FundType,
            c.Code
        })
        .Join(context.Categories, f => f.CategoryId, c => c.Id, (f, c) => new
        {
            FundId = f.Id,
            f.Amount,
            f.Code,
            f.Date,
            f.Description,
            f.FundType,
            CategoryName = c.Name
        })
        .Select(f => new FundDto(
            f.FundId,
            f.Amount,
            f.Code,
            f.Date,
            f.Description ?? string.Empty,
            f.CategoryName,
            f.FundType))
        .ToListAsync();

        return funds;
    }

    //private async Task<IEnumerable<FundDto>> GetAllAsync(
    //Expression<Func<Fund, bool>> predicate,
    //FundFilterDto? dto = null)
    //{
    //    IQueryable<Fund> query = context.Funds
    //        .AsNoTracking()
    //        .Where(predicate);

    //    if (dto?.FundType is not null)
    //    {
    //        query = query.Where(f => f.FundType == dto.FundType.Value);
    //    }

    //    if (dto?.CurrencyId is not null)
    //    {
    //        query = query.Where(f => f.Money.CurrencyId == dto.CurrencyId.Value);
    //    }

    //    if (dto?.CategoryId is not null)
    //    {
    //        query = query.Where(f => f.CategoryId == dto.CategoryId.Value);
    //    }

    //    return await query
    //        .Join(context.Currencies, f => f.Money.CurrencyId, c => c.Id, (f, c) => new
    //        {
    //            Fund = f,
    //            c.Code
    //        })
    //        .Join(context.Categories, f => f.Fund.CategoryId, c => c.Id, (f, c) => new
    //        {
    //            FundId = f.Fund.Id,
    //            f.Fund.Money.Amount,
    //            f.Code,
    //            f.Fund.Date,
    //            f.Fund.Description,
    //            f.Fund.FundType,
    //            CategoryName = c.Name
    //        })
    //        .Select(f => new FundDto(
    //            f.FundId,
    //            f.Amount,
    //            f.Code,
    //            f.Date,
    //            f.Description ?? string.Empty,
    //            f.CategoryName,
    //            f.FundType))
    //        .ToListAsync();
    //}

    public async Task<IEnumerable<FundDto>> GetAllAsync(string userId, FundFilterDto? dto = null)
    {
        return await GetAllAsync(f => f.UserId == userId, dto);
    }

    public async Task<IEnumerable<FundDto>> GetAllAsync(string userId, DateOnly from, DateOnly to, FundFilterDto? dto = null)
    {
        return await GetAllAsync(f => f.UserId == userId && f.Date >= from && f.Date <= to, dto);
    }

    public async Task<DetailedFundDto?> GetAsync(int fundId)
    {
        Fund? fund = await context.Funds.FindAsync(fundId);
        return fund?.ToDetailedFundDto();
    }

    public async Task<IEnumerable<FinancialFundInformationResponse>> GetFinancialFundInformationAsync(string userId)
    {
        return await context.Funds
            .Where(u => u.UserId == userId)
            .Select(f => f.ToFinancialFundInformationResponse())
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialInformationResponse>> GetFinancialFundInformationAsync(string userId, FundType fundType)
    {
        return await context.Funds
            .Where(u => u.UserId == userId && u.FundType == fundType)
            .Select(f => f.ToFinancialInformationResponse()) 
            .ToListAsync();
    }

    public async Task<DateOnly> GetDateAsync(int fundId)
    {
        IQueryable<Fund> query = context.Funds.Where(f => f.Id == fundId);
        if (!await query.AnyAsync())
        {
            throw new EntityNotFoundException("Fund", fundId);
        }
        return await query.Select(f => f.Date).FirstAsync();
    }

    public async Task UpdateAsync(UpdateFundDto dto)
    {
        Fund fund = await context.Funds.FindAsync(dto.FundId) ??
            throw new EntityNotFoundException("Fund", dto.FundId);
        EntityEntry<Fund> entry = context.Funds.Entry(fund);
        fund.SetCurrencyAndExchangeRateInUsd(dto.CurrencyId, dto.ExchangeRateInUsd);
        fund.SetDateAndExchangeRateInUsd(dto.Date, dto.ExchangeRateInUsd);
        fund.SetAmount(dto.Amount);
        fund.Description = dto.Description;
        fund.SetCategory(dto.CategoryId);
        fund.SetFundType(dto.FundType);
        entry.State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
}
