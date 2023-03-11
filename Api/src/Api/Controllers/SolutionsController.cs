﻿using Application.Users.Queries;

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
        [HttpPost]
        public async Task<ActionResult<SolutionResponse>> CreateSolutionAsync(CreateSolutionCommand command)
        {
            return Ok(await this.Mediator.Send(command));
        }

        [HttpPost("{solutionId}/preferences")]
        public async Task<ActionResult<SolutionResponse>> CreateSolutionPreferencesAsync([FromRoute] Int32 solutionId, UpsertPreferencesToSolutionCommand command)
        {
            command.SolutionId = solutionId;
            try
            {
                return Ok(await this.Mediator.Send(command));
            }
            catch (InvalidIdException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        public async Task<ActionResult<SolutionResponse>> GetSolutionsAsync()
        {
            return Ok(await Mediator.Send(new GetAllSolutionsQuery()));
        }
        
        [HttpGet("{solutionId})")]
        public async Task<ActionResult<SolutionResponse>> GetSolutionByIdAsync([FromRoute] Int32 solutionId)
        {
            return Ok(await Mediator.Send(new GetAllSolutionsQuery() { SolutionId = solutionId }));
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateSolutionCommand command)
        {
            try
            {
                await Mediator.Send(new UpdateSolutionCommand { Id = id, Name = command.Name });

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
