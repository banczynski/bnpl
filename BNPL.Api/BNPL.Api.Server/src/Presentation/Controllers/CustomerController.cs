using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.UseCases.Customer;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
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
        [HttpPost]
        public async Task<ActionResult<ServiceResult<CreateCustomerResponse>>> Create([FromBody] CreateCustomerRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ServiceResult<CustomerDto>>> Update(Guid id, [FromBody] UpdateCustomerRequest request)
            => Ok(await updateUseCase.ExecuteAsync(id, request));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResult<CustomerDto>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<CustomerDto>>>> GetByAffiliate(Guid affiliateId, [FromQuery] bool onlyActive = true)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId, onlyActive));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<CustomerDto>>>> GetByPartner(Guid partnerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId, onlyActive));
    }
}
