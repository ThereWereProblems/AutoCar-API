using AutoCar.Exceptions;

namespace AutoCar.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            //catch (UnauthorizedException unauthorizedException)
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync(unauthorizedException.Message);
            //}
            //catch (PaymentRequiredException paymentRequiredException)
            //{
            //    context.Response.StatusCode = 402;
            //    await context.Response.WriteAsync(paymentRequiredException.Message);
            //}
            catch (ForbiddenException forbiddenException)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(forbiddenException.Message);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (ConflictException conflictException)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync(conflictException.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
