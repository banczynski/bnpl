using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.UseCases.Customer;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CustomerController(
        CreateCustomerUseCase createUseCase,
        UpdateCustomerUseCase updateUseCase,
        InactivateCustomerUseCase inactivateUseCase,
        GetCustomerByIdUseCase getByIdUseCase,
        GetCustomersByAffiliateUseCase getByAffiliateUseCase,
        GetCustomersByPartnerUseCase getByPartnerUseCase
    ) : ControllerBase
    {
        [HttpPost("{affiliateId:guid}")]
        public async Task<ActionResult<Result<CreateCustomerResponse, string>>> Create(
            Guid affiliateId,
            [FromBody] CreateCustomerRequest request)
        {
            var result = await createUseCase.ExecuteAsync(affiliateId, request);
            return CreatedAtAction(nameof(GetById), new { id = result is Result<CreateCustomerResponse, string>.Success s ? s.Value.Id : Guid.Empty }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<CustomerDto, string>>> Update(Guid id, [FromBody] UpdateCustomerRequest request)
            => Ok(await updateUseCase.ExecuteAsync(id, request));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Result<string, string>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<CustomerDto, string>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<CustomerDto>, string>>> GetByAffiliate(Guid affiliateId, [FromQuery] bool onlyActive = true)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId, onlyActive));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<CustomerDto>, string>>> GetByPartner(Guid partnerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId, onlyActive));
    }
}
