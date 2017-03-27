using System;
using System.Threading.Tasks;
using FluentValidation;
using JoggingTracker.Shared.Configuration;
using JoggingTracker.Shared.Exceptions;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JoggingTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces(ContentTypes.ApplicationJson)]
    public abstract class WebApiControllerBase : Controller
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public WebApiControllerBase(ILogger logger, IMediator mediator)
        {
            ArgumentChecker.CheckNotNull(new { logger, mediator });

            this._logger = logger;
            this._mediator = mediator;
        }

        protected async Task<IActionResult> ProcessAsync(IRequest request)
        {
            return await this.HandleRequestAsync(async () =>
            {
                await this._mediator.Send(request);
                return this.NoContent();
            });
        }

        protected async Task<IActionResult> ProcessAsync<TResult>(IRequest<TResult> request)
        {
            return await this.HandleRequestAsync(async () =>
            {
                var result = await this._mediator.Send(request);

                if (result == null)
                    return this.NotFound();

                return this.Ok(result);
            });
        }

        private async Task<IActionResult> HandleRequestAsync(Func<Task<IActionResult>> request)
        {
            try
            {
                return await request();
            }
            catch (ValidationException exception)
            {
                foreach (var error in exception.Errors)
                    this.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return this.BadRequest(this.ModelState);
            }
            catch (UnauthorizedAccessException exception)
            {
                this._logger.LogError(exception.Message, exception);

                return this.Unauthorized();
            }
            catch (ObjectNotFoundException exception)
            {
                this._logger.LogError(exception.Message, exception);

                this.ModelState.AddModelError(string.Empty, exception.Message);

                return this.NotFound(this.ModelState);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);

                this.ModelState.AddModelError(string.Empty, "An error occured while processing your request. Please try again.");

                return this.StatusCode(StatusCodes.Status500InternalServerError, this.ModelState);
            }
        }
    }
}
