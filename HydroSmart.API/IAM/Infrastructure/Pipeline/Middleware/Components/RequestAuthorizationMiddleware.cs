using HydroSmart.API.IAM.Application.Internal.OutboundServices;
using HydroSmart.API.IAM.Domain.Model.Queries;
using HydroSmart.API.IAM.Domain.Services;
using HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
/// RequestAuthorizationMiddleware is a custom middleware.
/// It validates the Authorization Bearer token and sets the user in HttpContext.Items["User"].
/// </summary>
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

        Console.WriteLine($"[AUTH MIDDLEWARE] Method: {context.Request.Method}");
        Console.WriteLine($"[AUTH MIDDLEWARE] Path: {path}");

        // 1. Allow CORS preflight requests
        if (context.Request.Method == HttpMethods.Options)
        {
            Console.WriteLine("[AUTH MIDDLEWARE] OPTIONS request - skipping authorization");
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return;
        }

        // 2. Allow public endpoints manually
        if (IsPublicPath(path))
        {
            Console.WriteLine("[AUTH MIDDLEWARE] Public path - skipping authorization");
            await next(context);
            return;
        }

        // 3. Allow endpoints decorated with custom [AllowAnonymous]
        var endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            var allowAnonymous = endpoint.Metadata.Any(metadata =>
                metadata.GetType() == typeof(AllowAnonymousAttribute));

            Console.WriteLine($"[AUTH MIDDLEWARE] AllowAnonymous attribute: {allowAnonymous}");

            if (allowAnonymous)
            {
                Console.WriteLine("[AUTH MIDDLEWARE] AllowAnonymous endpoint - skipping authorization");
                await next(context);
                return;
            }
        }

        // 4. Validate Authorization header
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
        {
            Console.WriteLine("[AUTH MIDDLEWARE] Authorization header missing");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("""
            {
              "message": "Unauthorized request. Authorization header is missing."
            }
            """);

            return;
        }

        var authHeader = authHeaderValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader) ||
            !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("[AUTH MIDDLEWARE] Invalid Authorization header format");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("""
            {
              "message": "Unauthorized request. Bearer token is required."
            }
            """);

            return;
        }

        var token = authHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrWhiteSpace(token))
        {
            Console.WriteLine("[AUTH MIDDLEWARE] Token is empty");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("""
            {
              "message": "Unauthorized request. Token is empty."
            }
            """);

            return;
        }

        try
        {
            // 5. Validate token
            var userId = await tokenService.ValidateToken(token);

            if (userId == null)
            {
                Console.WriteLine("[AUTH MIDDLEWARE] Token validation failed");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync("""
                {
                  "message": "Unauthorized request. Invalid token."
                }
                """);

                return;
            }

            // 6. Get user by id
            var getUserByIdQuery = new GetUserByIdQuery(userId.Value);
            var user = await userQueryService.Handle(getUserByIdQuery);

            if (user == null)
            {
                Console.WriteLine("[AUTH MIDDLEWARE] User not found");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync("""
                {
                  "message": "Unauthorized request. User not found."
                }
                """);

                return;
            }

            // 7. Set user in HttpContext
            context.Items["User"] = user;

            Console.WriteLine("[AUTH MIDDLEWARE] Authorization successful");

            await next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AUTH MIDDLEWARE] Authorization error: {ex.Message}");

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("""
            {
              "message": "Unauthorized request."
            }
            """);

            return;
        }
    }

    private static bool IsPublicPath(string path)
    {
        return path.StartsWith("/api/v1/authentication/sign-in") ||
               path.StartsWith("/api/v1/authentication/sign-up") ||
               path.StartsWith("/swagger") ||
               path.StartsWith("/v3/api-docs") ||
               path.StartsWith("/favicon.ico") ||
               path == "/";
    }
}