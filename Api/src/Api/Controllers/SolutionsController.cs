using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    using Application.Solutions.Commands;
    using Application.Solutions.Queries;
    using Application.SolutionsPreferences.Commands;
    using Domain.Exceptions;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/solutions")]
    [ApiController]
    public class SolutionsController : ApiControllerBase
    {
        [Authorize("write:solution")]
        [HttpPost]
        public async Task<ActionResult<SolutionResponse>> CreateSolutionAsync(CreateSolutionCommand command)
        {
            return Ok(await this.Mediator.Send(command));
        }
        
        // TODO: For solutions that doesn't have a preference, then global ones will be added as solution preferences.
        [Authorize("write:solution")]
        [HttpPost("{solutionId}/preferences")]
        public async Task<ActionResult<SolutionResponse>> CreateSolutionPreferencesAsync([FromRoute] Int32 solutionId, UpsertPreferencesToSolutionCommand command)
        {
            command.SolutionId = solutionId;
            try
            {
                return Ok(await this.Mediator.Send(command));
            }
            catch (GlobalPreferenceDoesNotExist ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidIdException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize("read:solution")]
        [HttpGet]
        public async Task<ActionResult<SolutionResponse>> GetSolutionsAsync()
        {
            return Ok(await Mediator.Send(new GetAllSolutionsQuery()));
        }
        
        [Authorize("read:solution")]
        [HttpGet("{solutionId}")]
        public async Task<ActionResult<SolutionResponse>> GetSolutionByIdAsync([FromRoute] int solutionId)
        {
            return Ok(await Mediator.Send(new GetSolutionByIdQuery() { SolutionId = solutionId }));
        }

        [Authorize("write:solution")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateSolutionCommand command)
        {
            try
            {
                // TODO: Return solution response
                await Mediator.Send(new UpdateSolutionCommand { Id = id, Name = command.Name });
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize("write:solution")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteSolutionCommand { Id = id });

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
