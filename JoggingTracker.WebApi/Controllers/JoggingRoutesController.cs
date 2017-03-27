using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoggingTracker.Model.Entity;
using JoggingTracker.Service.Implementation.JoggingRoutes;
using JoggingTracker.Service.Model;
using JoggingTracker.WebApi.Infrastructure.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JoggingTracker.WebApi.Controllers
{
    [AuthorizeRoles(Role.UserRole, Role.AdminRole)]
    [Route("~/api/Users/{userId:guid}/[controller]")]
    public class JoggingRoutesController : WebApiControllerBase
    {
        public JoggingRoutesController(ILoggerFactory loggerFactory, IMediator mediator)
            : base(loggerFactory.CreateLogger<JoggingRoutesController>(), mediator)
        {
        }

        /// <summary>
        /// Returns a list of jogging routes for the specified user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="parameters">Filter parameters.</param>
        /// <returns><see cref="List{JoggingRouteViewModel}"/> for the user.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid filter parameters.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<JoggingRouteViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid userId, [FromQuery] JoggingRoutesQueryParameters parameters)
        {
            var query = new GetJoggingRoutesQuery()
            {
                UserId = userId,
                FromDate = parameters.FromDate,
                UntilDate = parameters.UntilDate
            };

            return await this.ProcessAsync(query);
        }

        /// <summary>
        /// Returns a single jogging route.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="id">Jogging route identifier.</param>
        /// <returns><see cref="UpdateJoggingRouteCommand"/> for the requested jogging route.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="404">Jogging route not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UpdateJoggingRouteCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid userId, Guid id)
        {
            var query = new GetSingleJoggingRouteQuery()
            {
                Id = id,
                UserId = userId
            };

            return await this.ProcessAsync(query);
        }

        /// <summary>
        /// Creates a new jogging route.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="command">Jogging route data.</param>
        /// <returns><see cref="UpdateJoggingRouteCommand"/> for the created jogging route.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid jogging route data.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UpdateJoggingRouteCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid userId, [FromBody] CreateJoggingRouteCommand command)
        {
            command.UserId = userId;
            return await this.ProcessAsync(command);
        }

        /// <summary>
        /// Updates an existing jogging route.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="id">Jogging route identifier.</param>
        /// <param name="command">New jogging route data.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid jogging route data.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="404">Jogging route not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid userId, Guid id, [FromBody] UpdateJoggingRouteCommand command)
        {
            command.UserId = userId;
            command.Id = id;
            return await this.ProcessAsync(command);
        }

        /// <summary>
        /// Deletes an existing jogging route.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="id">Jogging route identifier.</param>
        /// <response code="200">Success.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="404">Jogging route not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid userId, Guid id)
        {
            var command = new DeleteJoggingRouteCommand()
            {
                Id = id,
                UserId = userId
            };

            return await this.ProcessAsync(command);
        }

        /// <summary>
        /// Returns the average speed and distance for weeks of the year
        /// where user was active.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns><see cref="List{WeeklyRecord}"/> for the user.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Action caller is unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [Route("~/api/Users/{userId:guid}/[action]")]
        [ProducesResponseType(typeof(List<WeeklyRecord>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> WeeklyRecords(Guid userId)
        {
            var query = new GetWeeklyRecordsQuery()
            {
                UserId = userId
            };

            return await this.ProcessAsync(query);
        }
    }
}
