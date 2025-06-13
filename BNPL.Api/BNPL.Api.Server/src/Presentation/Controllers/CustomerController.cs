using Core.Persistence.Interfaces;
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
        IUseCase<CreateCustomerRequestUseCase, Result<CreateCustomerResponse, Error>> createUseCase,
        IUseCase<UpdateCustomerRequestUseCase, Result<CustomerDto, Error>> updateUseCase,
        IUseCase<InactivateCustomerRequestUseCase, Result<bool, Error>> inactivateUseCase,
        GetCustomerByIdUseCase getByIdUseCase,
        GetCustomersByAffiliateUseCase getByAffiliateUseCase,
        GetCustomersByPartnerUseCase getByPartnerUseCase
    ) : ControllerBase
    {
        [HttpPost("{affiliateId:guid}")]
        [ProducesResponseType(typeof(Result<CreateCustomerResponse, Error>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<CreateCustomerResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CreateCustomerResponse, Error>>> Create(
            Guid affiliateId,
            [FromBody] CreateCustomerRequest request)
        {
            var useCaseRequest = new CreateCustomerRequestUseCase(affiliateId, request);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);

            if (result.TryGetSuccess(out var successValue))
                return CreatedAtAction(nameof(GetById), new { id = successValue.Id }, result);

            return BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result<CustomerDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CustomerDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CustomerDto, Error>>> Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            var useCaseRequest = new UpdateCustomerRequestUseCase(id, request);
            var result = await updateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Inactivate(Guid id)
        {
            var useCaseRequest = new InactivateCustomerRequestUseCase(id);
            var result = await inactivateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<CustomerDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CustomerDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<CustomerDto, Error>>> GetById(Guid id)
        {
            var result = await getByIdUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<CustomerDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CustomerDto>, Error>>> GetByAffiliate(Guid affiliateId)
        {
            var result = await getByAffiliateUseCase.ExecuteAsync(affiliateId);
            return Ok(result);
        }

        [HttpGet("by-partner/{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<CustomerDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CustomerDto>, Error>>> GetByPartner(Guid partnerId)
        {
            var result = await getByPartnerUseCase.ExecuteAsync(partnerId);
            return Ok(result);
        }
    }
}