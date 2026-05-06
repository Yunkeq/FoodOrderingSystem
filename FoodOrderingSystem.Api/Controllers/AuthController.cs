using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    public async Task<IActionResult> Login()
    {
        // For simplicity, I will just return a dummy token here. In a real application, you would validate the user's credentials and generate a JWT or similar token.
        var token = "dummy-token";
        return Ok(new { Token = token });
    }

    public async Task<IActionResult> Logout()
    {
        // For simplicity, I will just return a dummy token here. In a real application, you would validate the user's credentials and generate a JWT or similar token.
        var token = "dummy-token";
        return Ok(new { Token = token });
    }

    public async Task<IActionResult> LoginWithRefresh()
    {
        // For simplicity, I will just return a dummy token here. In a real application, you would validate the user's credentials and generate a JWT or similar token.
        var token = "dummy-token";
        return Ok(new { Token = token });
    }

    public async Task<IActionResult> Register()
    {
        // For simplicity, I will just return a dummy token here. In a real application, you would validate the user's credentials and generate a JWT or similar token.
        var token = "dummy-token";
        return Ok(new { Token = token });
    }
}
