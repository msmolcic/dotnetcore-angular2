using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoggingTracker.Model.Entity;
using JoggingTracker.Service.Implementation.Users;
using JoggingTracker.Service.Model;
using JoggingTracker.WebApi.Infrastructure.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JoggingTracker.WebApi.Controllers
{
    public class UsersController : WebApiControllerBase
    {
        public UsersController(ILoggerFactory loggerFactory, IMediator mediator)
            : base(loggerFactory.CreateLogger<UsersController>(), mediator)
        {
        }

        /// <summary>
        /// Parses the user identity from Authorization token.
        /// </summary>
        /// <returns><see cref="UserIdentity"/> of the action caller.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("Identity")]
        [ProducesResponseType(typeof(UserIdentity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserIdentity()
        {
            return this.Ok(UserIdentity.FromPrincipal(this.User));
        }

        /// <summary>
        /// Returns a list of all users.
        /// </summary>
        /// <returns><see cref="List{UpdateUserCommand}"/> of application users.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [AuthorizeRoles(Role.AdminRole, Role.UserManagerRole)]
        [ProducesResponseType(typeof(List<UpdateUserCommand>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return await this.ProcessAsync(new GetUsersQuery());
        }

        /// <summary>
        /// Returns existing user data.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns><see cref="UpdateUserCommand"/> of the user with provided identifier.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UpdateUserCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await this.ProcessAsync(new GetSingleUserQuery(id));
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="command">Application user registration data.</param>
        /// <returns><see cref="UpdateUserCommand"/> of the registered user.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid registration parameters.</response>
        /// <response code="500">Internal server error.</response>
        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(UpdateUserCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            return await this.ProcessAsync(command);
        }

        /// <summary>
        /// Updates the existing user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="command">New user data.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid update parameters.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            return await this.ProcessAsync(command);
        }

        /// <summary>
        /// Deletes the existing user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <response code="200">Success.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id:guid}")]
        [AuthorizeRoles(Role.AdminRole, Role.UserManagerRole)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await this.ProcessAsync(new DeleteUserCommand(id));
        }

        /// <summary>
        /// Creates JsonWebToken as a result of user login action.
        /// </summary>
        /// <param name="command">User login credentials.</param>
        /// <returns><see cref="LoginResponse"/> for the user login action.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid user credentials.</response>
        /// <response code="500">Internal server error.</response>
        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            return await this.ProcessAsync(command);
        }
    }
}
