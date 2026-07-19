using Microsoft.EntityFrameworkCore;
namespace App.Models;

public class LotsModel(AuctionDbContext db, IWebHostEnvironment env)
{
    public const int DEFAULT_PAGE_SIZE = 15;

    public async Task<Lot?> GetById(uint id)
    {
        var lot = await db.lots.FindAsync(id);
        return lot;
    }

    public IQueryable<Lot> GetPage(uint? tag_id, int page, int page_size = DEFAULT_PAGE_SIZE) {
        var lots_query = tag_id switch {
            null => db.lots,
            _ => db.lots.Where(l => l.tag_id == tag_id)
        };
        return lots_query.Skip(page*page_size).Take(page_size);
    }

    public async Task<HomePageModel> HomePage(uint? tag_id, int page, int page_size = DEFAULT_PAGE_SIZE)
    {
        var lots = GetPage(tag_id, page, page_size);
        var lot_cards = new List<HomePageModel.LotCard>();
        foreach (var lot in lots) {
            var image = await lot.GetImages(db).FirstOrDefaultAsync();
            lot_cards.Add(new HomePageModel.LotCard{
                id = lot.id,
                image_path = image?.image_path,
                title = lot.title,
                price = lot.current_price,
            });
        }
        var tags = await db.tags.ToListAsync();
        var home_page = new HomePageModel {
            cards = lot_cards,
            tags = tags,
        };
        return home_page;
    }

    public async Task<(Lot? deleted_lot, ModelError err)> DeleteById(uint id)
    {
        var lot = await db.lots.FindAsync(id);
        if (lot == null) {
            return (null, ModelError.NotExist);
        }

        var images = await db.lot_images.Where(i => i.lot_id == id).ToListAsync();

        var uploadsPath = Path.Combine(env.WebRootPath, "uploads", "lot_images");
        foreach (var img in images) {
            var fileName = Path.GetFileName(img.image_path);
            var filePath = Path.Combine(uploadsPath, fileName);
            if (System.IO.File.Exists(filePath)) {
                System.IO.File.Delete(filePath);
            }
        }

        db.lot_images.RemoveRange(images);
        db.lots.Remove(lot);

        if (!await db.TrySaveChangesAsync()) {
            return (null, ModelError.DbError);
        }

        return (lot, ModelError.None);
    }

    public async Task<(Lot? updated_lot, ModelError err)> UpdateById(uint id, Lot.CreateRequest req)
    {
        var lot = await db.lots.FindAsync(id);
        if (lot == null) {
            return (null, ModelError.NotExist);
        }
        var entry = db.lots.Update(lot);
        if (!await db.TrySaveChangesAsync()) {
            return (null, ModelError.DbError);
        }
        return (entry.Entity, ModelError.None);
    }

    public async Task<(Lot? lot, List<string> errors)> CreateLot(Lot.CreateRequest req, string userLogin)
    {
        var errors = ValidateCreate(req);
        if (errors.Count > 0) return (null, errors);

        var lot = Lot.From(req, userLogin);
        db.lots.Add(lot);
        if (!await db.TrySaveChangesAsync()) {
            errors.Add("Ошибка сохранения лота в базу данных.");
            return (null, errors);
        }

        var uploads_path = Path.Combine(env.WebRootPath, "uploads", "lot_images");
        Directory.CreateDirectory(uploads_path);

        var images_to_save = new List<IFormFile>();
        if (req.thumbnail != null) images_to_save.Add(req.thumbnail);
        if (req.images != null)    images_to_save.AddRange(req.images);

        foreach (var file in images_to_save) {
            if (file.Length == 0) continue;
            var ext = Path.GetExtension(file.FileName);
            var file_name = $"{Guid.NewGuid()}{ext}";
            var file_path = Path.Combine(uploads_path, file_name);

            using (var stream = new FileStream(file_path, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }

            var lot_image = new LotImage {
                lot_id = lot.id,
                image_path = $"/uploads/lot_images/{file_name}"
            };
            db.lot_images.Add(lot_image);
        }

        if (!await db.TrySaveChangesAsync()) {
            errors.Add("Ошибка сохранения изображений.");
            return (null, errors);
        }

        return (lot, errors);
    }

    public List<string> ValidateCreate(Lot.CreateRequest req)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(req.title))
            errors.Add("Название не может быть пустым.");

        if (req.count < 1)
            errors.Add("Количество должно быть не меньше 1.");

        if (req.price <= 0)
            errors.Add("Цена должна быть положительной.");

        if (req.end_time <= DateTimeOffset.Now)
            errors.Add("Дата окончания должна быть в будущем.");

        Console.WriteLine(req.ToString());
        var tag_exists = db.tags.Any(t => t.id == req.tag_id);
        if (!tag_exists)
            errors.Add("Выбранная категория не существует.");

        if (req.thumbnail == null && (req.images == null || req.images.Count == 0))
            errors.Add("Необходимо добавить хотя бы одно изображение.");

        return errors;
    }

    public async Task<List<UserLotCard>> GetUserLots(string user_login)
    {
        var lots = await db.lots
            .Where(l => l.user_login == user_login)
            .OrderByDescending(l => l.id)
            .ToListAsync();

        var cards = new List<UserLotCard>();
        foreach (var lot in lots) {
            var firstImage = await db.lot_images
                .Where(i => i.lot_id == lot.id)
                .OrderBy(i => i.id)
                .FirstOrDefaultAsync();

            cards.Add(new UserLotCard {
                id = lot.id,
                title = lot.title,
                current_price = lot.current_price,
                thumbnail_path = firstImage?.image_path,
            });
        }
        return cards;
    }
}
