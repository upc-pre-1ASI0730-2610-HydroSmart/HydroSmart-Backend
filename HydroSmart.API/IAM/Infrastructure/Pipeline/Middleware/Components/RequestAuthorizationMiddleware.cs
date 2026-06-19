using HydroSmart.API.IAM.Application.Internal.OutboundServices;
using HydroSmart.API.IAM.Domain.Model.Queries;
using HydroSmart.API.IAM.Domain.Services;
using HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
/// RequestAuthorizationMiddleware is a custom middleware.
/// This middleware is used to authorize requests.
/// It validates a token is included in the request header and that the token is valid.
/// If the token is valid then it sets the user in HttpContext.Items["User"].
/// </summary>
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    /// <summary>
    /// InvokeAsync is called by the ASP.NET Core runtime.
    /// It is used to authorize requests.
    /// It validates a token is included in the request header and that the token is valid.
    /// If the token is valid then it sets the user in HttpContext.Items["User"].
    /// </summary>
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        Console.WriteLine("Entering InvokeAsync");

        // Safely obtain endpoint metadata (GetEndpoint may return null)
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            Console.WriteLine("No endpoint metadata available - skipping authorization");
            await next(context);
            return;
        }

        // skip authorization if endpoint is decorated with [AllowAnonymous] attribute
        var allowAnonymous = endpoint.Metadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute));
        Console.WriteLine($"Allow Anonymous is {allowAnonymous}");
        if (allowAnonymous)
        {
            Console.WriteLine("Skipping authorization");
            // [AllowAnonymous] attribute is set, so skip authorization
            await next(context);
            return;
        }

        Console.WriteLine("Entering authorization");

        // get token from request header
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
        {
            Console.WriteLine("Authorization header missing");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        var token = authHeaderValues.FirstOrDefault()?.Split(' ').Last();

        // if token is null then return 401
        if (string.IsNullOrWhiteSpace(token))
        {
            Console.WriteLine("Token is null or empty");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        try
        {
            // validate token
            var userId = await tokenService.ValidateToken(token);

            // if token is invalid then return 401
            if (userId == null)
            {
                Console.WriteLine("Token validation failed");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            // get user by id
            var getUserByIdQuery = new GetUserByIdQuery(userId.Value);

            // set user in HttpContext.Items["User"]
            var user = await userQueryService.Handle(getUserByIdQuery);
            Console.WriteLine("Successful authorization. Updating Context...");
            context.Items["User"] = user;
            Console.WriteLine("Continuing with Middleware Pipeline");

            // call next middleware
            await next(context);
        }
        catch (Exception ex)
        {
            // Log and return 401 for token related and authorization errors
            Console.WriteLine($"Authorization error: {ex.Message}");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }
}
