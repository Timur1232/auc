using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Attributes;
namespace App.Controllers;

[ApiController]
[Route("/")]
[GetUser, HtmxServe, AddViewData]
[Title("Главная страница")]
public class HomePageController(AuctionDbContext db, IWebHostEnvironment env) : Controller
{
    public LotsModel lots_model = new(db, env);

    [HttpGet]
    public async Task<IActionResult> MainPage([FromQuery] int? page, [FromQuery] int? page_size, [FromQuery] uint? tag_id)
    {
        var home_page = await lots_model.HomePage(tag_id, page ?? 0, page_size ?? LotsModel.DEFAULT_PAGE_SIZE);
        return View("index", home_page);
    }
}
