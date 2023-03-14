using Application.GlobalPreferences.Commands;
using Application.GlobalPreferences.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/preferences")]
    [ApiController]
    public class GlobalPreferencesController : ApiControllerBase
    {
        [Authorize("write:preference")]
        [HttpPost]
        public async Task<ActionResult<GlobalPreferenceResponse>> CreateGlobalPreferenceAsync(CreateGlobalPreferenceCommand command)
        {
            return Ok(await this.Mediator.Send(command));
        }

        [Authorize("read:preference")]
        [HttpGet]
        public async Task<ActionResult<GlobalPreferenceResponse>> GetGlobalPreferencesAsync()
        {
            return Ok(await Mediator.Send(new GetAllGlobalPreferencesQuery()));
        }

        [Authorize("write:preference")]
        [HttpPut("{name}")]
        public async Task<ActionResult> Update(string name, UpdateGlobalPreferenceCommand command)
        {
            try
            {
                await Mediator.Send(new UpdateGlobalPreferenceCommand { Name = name, Value = command.Value});

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize("write:preference")]
        [HttpDelete("{name}")]
        public async Task<ActionResult> Delete(string name)
        {
            try
            {
                await Mediator.Send(new DeleteGlobalPreferenceCommand { Name = name });

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
