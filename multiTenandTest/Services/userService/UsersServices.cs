using System.Security.Claims;

public class UsersServices : IUsersService
{
    private readonly HttpContext httpContext;

    public UsersServices(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContext = httpContextAccessor.HttpContext!;
    }

    public string getUserId()
    {
        if (httpContext.User.Identity!.IsAuthenticated)
        {
            var idClaim = httpContext
                .User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault();

            if (idClaim is null)
                throw new ApplicationException("El usuario no tiene claim del ID");

            return idClaim.Value;
        }
        throw new ApplicationException("El usuario no esta authenticado");
    }
}
