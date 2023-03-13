﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Exceptions;

namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUserAsync(CreateUserCommand command)
        {
            return Ok(await this.Mediator.Send(command));
        }

        [HttpPost("{userId}/solutions/{solutionId}/preferences")]
        public async Task<ActionResult<UserResponse>> CreateUserPreferencesAsync([FromRoute] Int32 userId, [FromRoute] Int32 solutionId, UpsertPreferencesToUserCommand command)
        {
            command.UserId = userId;
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

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetUsersAsync()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserResponse>> GetUserByIdAsync([FromRoute] Int32 userId)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery() { UserId = userId }));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateUserCommand command)
        {
            try
            {
                await Mediator.Send(new UpdateUserCommand { Id = id, Name = command.Name });

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
                await Mediator.Send(new DeleteUserCommand { Id = id });

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
