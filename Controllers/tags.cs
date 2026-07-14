using Microsoft.AspNetCore.Mvc;
using App.Models;
namespace App.Controllers;

[ApiController]
[Route("tags")]
public class TagsController(AuctionDbContext db) : Controller
{
    public TagsModel model = new(db);

    [HttpGet]
    public IActionResult GetAllTags()
    {
        return Ok(db.tags);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        var tag = await db.tags.FindAsync(id);
        if (tag == null) {
            return NotFound();
        }
        return Ok(tag);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Tag.CreateRequest req)
    {
        var entry = db.tags.Add(Tag.From(req));
        if (!await db.TrySaveChangesAsync()) {
            Response.StatusCode = 400;
            return Content("Unable to create tag");
        }
        return Ok(entry.Entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById(uint id)
    {
        var (deleted_tag, err) = await model.DeleteById(id);
        if (err != ModelError.None || deleted_tag == null) {
            Response.StatusCode = 400;
            return Content(err.GetMessage());
        }
        return Ok(deleted_tag);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateById(uint id, [FromForm] Tag.CreateRequest req)
    {
        var (updated_tag, err) = await model.UpdateById(id, req);
        if (err != ModelError.None || updated_tag == null) {
            Response.StatusCode = 400;
            return Content(err.GetMessage());
        }
        return Ok(updated_tag);
    }
}
