using Finance.Api.Extensions;
using Finance.Api.Requests;
using Finance.Api.Responses;
using Finance.Application.Abstractions;
using Finance.Application.Queries;
using Finance.Application.Responses;
using Finance.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Controllers;

[Route("funds")]
[ApiController]
public class FundsController(
    FundService fundService,
    TotalCalculatorService totalCalculatorService,
    ICurrencyRepository currencyRepository,
    IValidator<CreateFundRequest> createFundRequestValidator,
    IValidator<UpdateFundRequest> updateFundRequestValidator) : ControllerBase
{
    private readonly FundService fundService = fundService;

    private readonly TotalCalculatorService totalCalculatorService = totalCalculatorService;

    private readonly ICurrencyRepository currencyRepository = currencyRepository;

    private readonly IValidator<CreateFundRequest> createFundRequestValidator = createFundRequestValidator;

    private readonly IValidator<UpdateFundRequest> updateFundRequestValidator = updateFundRequestValidator;

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateAsync([FromBody] CreateFundRequest request)
    {
        await createFundRequestValidator.ValidateAndThrowAsync(request);
        await fundService.CreateAsync(
            new(request.Amount,
                request.CurrencyId,
                request.Date,
                request.Description,
                User.GetId(),
                request.CategoryId,
                request.FundType)
            );
        return Created();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await fundService.DeleteAsync(new(id, User.GetId()));
        return NoContent();
    }

    [HttpGet("range")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FundResponse>>> GetAsync(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to, 
        [FromQuery] string? fundType, 
        [FromQuery] int? categoryId, 
        [FromQuery] int? currencyId)
    {
        return Ok(await fundService.GetAllAsync(new FundWithinRangeQuery(User.GetId(), from, to, new(fundType, currencyId, categoryId))));
    }

    [HttpPost("range")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FundResponse>>> GetAsync([FromBody] GetFundWithinRangeRequest request)
    {
        return Ok(await fundService.GetAllAsync(new FundWithinRangeQuery(User.GetId(), request.From, request.To, new(request.FundFilter?.FundType, request.FundFilter?.CurrencyId, request.FundFilter?.CategoryId))));
    }

    [HttpGet("date")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FundResponse>>> GetAsync(
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] int? day,
        [FromQuery] string? fundType,
        [FromQuery] int? categoryId,
        [FromQuery] int? currencyId)
    {
        return Ok(await fundService.GetAllAsync(new FundInDateQuery(User.GetId(), year, month, day, new(fundType, currencyId, categoryId))));
    }

    [HttpPost("date")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FundResponse>>> GetAsync([FromBody] GetFundInDateRequest request)
    {
        return Ok(await fundService.GetAllAsync(new FundInDateQuery(User.GetId(), request.Year, request.Month, request.Day, new(request.FundFilter?.FundType, request.FundFilter?.CurrencyId, request.FundFilter?.CategoryId))));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FundResponse>>> GetAsync(
        [FromQuery] string? fundType,
        [FromQuery] int? categoryId,
        [FromQuery] int? currencyId)
    {
        return Ok(await fundService.GetAllAsync(new FundsQuery(User.GetId(), new(fundType, currencyId, categoryId))));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<DetailedFundResponse>> GetAsync([FromRoute] int id)
    {
        return Ok(await fundService.GetAsync(new FundQuery(id, User.GetId())));
    }

    [HttpGet("total")]
    [Authorize]
    public async Task<ActionResult<TotalCalculationResponse>> GetTotalAsync([FromQuery] int currencyId, [FromQuery] string? fundType)
    {
        CurrencyResponse? currencyResponse = await currencyRepository.GetAsync(currencyId);
        if (currencyResponse is null)
        {
            return Problem(detail: $"No currency with id of ({currencyId}).", statusCode: 404, title: "Resource not found.");
        }
        decimal total = await totalCalculatorService.CalculateAsync(new(User.GetId(), currencyId, fundType));
        return Ok(new TotalCalculationResponse(total, currencyResponse.Code, currencyResponse.Name));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateFundRequest request)
    {
        await updateFundRequestValidator.ValidateAndThrowAsync(request);
        await fundService.UpdateAsync(
            new(id,
                request.Amount,
                request.CurrencyId,
                request.Date,
                request.Description,
                User.GetId(),
                request.CategoryId,
                request.FundType));
        return NoContent();
    }
}
