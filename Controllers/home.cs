using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Attributes;
using App.Extentions;
namespace App.Controllers;

[ApiController]
[Route("/")]
[GetUser, HtmxServe, AddViewData]
[Title("Главная страница")]
public class HomePageController(AuctionDbContext db, IWebHostEnvironment env) : Controller
{
    public LotsModel lots_model = new(db, env);

    [HttpGet("lots"), HttpGet]
    public async Task<IActionResult> LotCards([FromQuery] HomePageQuery query)
    {
        if (query.page_size == 0) query = query with { page_size = LotsModel.DEFAULT_PAGE_SIZE };
        var home_page = await lots_model.HomePage(query);
        if (Request.IsHtmx()) {
            ViewData["pagination"] = true;
            return View("lot_cards_grid", home_page);
        } else {
            return View("index", home_page);
        }
    }
}
