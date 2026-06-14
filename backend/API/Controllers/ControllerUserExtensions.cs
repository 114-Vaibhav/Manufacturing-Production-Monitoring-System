using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public static class ControllerUserExtensions
    {
        public static int GetCurrentUserId(this ControllerBase controller)
        {
            var userIdValue = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedAccessException("Logged in user id is missing from token.");
            }

            return userId;
        }
    }
}
