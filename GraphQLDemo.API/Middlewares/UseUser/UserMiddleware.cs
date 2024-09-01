using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.Models;
using HotChocolate.Resolvers;
using System.Security.Claims;

namespace GraphQLDemo.API.Middlewares.UseUser
{
    public class UserMiddleware
    {
        private readonly FieldDelegate _next;
        public const string UserContextDataKey = "User";

        public UserMiddleware(FieldDelegate next) => _next = next;

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            if (context.ContextData.TryGetValue("ClaimsPrincipal", out var rawClaimsPrincipal)
                && rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
            {
                var user = new User
                {
                    Id = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID),
                    Email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL),
                    Username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME),
                    EmailVerified = bool.TryParse(claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED), out var emailVerified)
                        ? emailVerified
                        : false
                };

                context.ContextData[UserContextDataKey] = user;

                await _next(context);
            }
        }
    }
}
