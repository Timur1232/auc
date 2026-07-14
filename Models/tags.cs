namespace App.Models;

public class TagsModel(AuctionDbContext db)
{
    public async Task<(Tag?, ModelError)> DeleteById(uint id)
    {
        var tag = await db.tags.FindAsync(id);
        if (tag == null) {
            return (null, ModelError.NotExist);
        }
        db.tags.Remove(tag);
        if (!await db.TrySaveChangesAsync()) {
            return (null, ModelError.DbError);
        }
        return (tag, ModelError.None);
    }

    public async Task<(Tag? updated_tag, ModelError err)> UpdateById(uint id, Tag.CreateRequest req)
    {
        var tag = await db.tags.FindAsync(id);
        if (tag == null) {
            return (null, ModelError.NotExist);
        }
        tag.name = req.name;
        db.tags.Update(tag);
        if (!await db.TrySaveChangesAsync()) {
            return (null, ModelError.DbError);
        }
        return (tag, ModelError.None);
    }
}
