using Microsoft.AspNetCore.Mvc;
using App.Attributes;
using App.Models;
using App.Services;

namespace App.Controllers;

[ApiController]
[Route("auth")]
[Title("Авторизация")]
[HtmxViewData]
[HtmxRedirect]
public class AuthController(
    PasswordHasher password_hasher,
    JwtTokenProvider jwt_token_provider,
    AuctionDbContext db)
    : Controller
{
    AuthModel model = new AuthModel(db, password_hasher);

    [HttpGet("login")]
    [HtmxLayout]
    public async Task<IActionResult> Login() => View("login");

    [HttpPost("login")]
    [HtmxLayout]
    public async Task<IActionResult> Login([FromForm] User.LoginRequest login_req)
    {
        (User? user, AuthModel.Error err) = await model.ValidateLoginForm(login_req);

        if (err != AuthModel.Error.None || user == null) {
            ViewData["errors"] = AuthModel.ErrorToString(err);
            if (G.IsHtmx(Request)) return View("errors");
            return View("login");
        }

        jwt_token_provider.AppendTokenCookie(Response.Cookies, user);

        return Redirect("/");
    }

    [HttpGet("register")]
    [HtmxLayout]
    public async Task<IActionResult> Register() => View("register");

    [HttpPost("register")]
    [HtmxLayout]
    public async Task<IActionResult> Register([FromForm] User.RegisterRequest register_req)
    {
        (User? new_user, List<AuthModel.Error> errors) = await model.RegisterNewUser(register_req);

        if (errors.Count > 0 || new_user == null) {
            ViewData["errors"] = errors.Select(AuthModel.ErrorToString);
            if (G.IsHtmx(Request)) return View("errors");
            return View("register");
        }

        jwt_token_provider.AppendTokenCookie(Response.Cookies, new_user);

        return Redirect("/");
    }

    [Htmx]
    [HttpPost("login/user_exists")]
    public async Task<IActionResult> LoginCheckUserExists([FromForm] string? login_or_email)
    {
        if (!await model.IsUserExists(login_or_email)) {
            ViewData["errors"] = AuthModel.ErrorToString(AuthModel.Error.UserNotExists);
            return View("errors");
        }
        return Ok();
    }

    [Htmx]
    [HttpPost("register/user_exists")]
    public async Task<IActionResult> RegisterCheckUserExists([FromForm] string? login, [FromForm] string? email)
    {
        var errors = await model.ValidateRegisterForm(new User.RegisterRequest(
            login ?? "",
            email,
            "",
            ""
        ));

        if (errors.Count > 0) {
            ViewData["errors"] = errors.Select(AuthModel.ErrorToString);
            return View("errors");
        }

        return Ok();
    }
}
