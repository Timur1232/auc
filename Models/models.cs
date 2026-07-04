using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Models;

[Index(nameof(email), IsUnique = true)]
public class User
{
    [Key]          public required string login {get; set;}
    [EmailAddress] public required string? email {get; set;}
    [Required]     public required string password_hash {get; set;}

    [InverseProperty(nameof(Lot.user))]
    public List<Lot> lots {get; set;} = null!;

    public record LoginRequest(string login_or_email, string password);
    public record RegisterRequest(string login, string? email, string password, string password_confirm);
}

public class Lot
{
    [Key] public uint id {get; set;}

    [Required, ForeignKey(nameof(User))]
    public required string user_login {get; set;}

    [Required] public required string name {get; set;}
               public int count {get; set;} = 1;
    [Required] public decimal initial_price {get; set;}
               public decimal current_price {get; set;}
    [Required] public DateTimeOffset end_time {get; set;}

    public User user {get; set;} = null!;

    [InverseProperty(nameof(Tag.lots))]
    public List<Tag> tags {get; set;} = null!;
}

[Index(nameof(name), IsUnique = true)]
public class Tag
{
    [Key] public uint id {get; set;}
    [Required] public required string name {get; set;}

    [InverseProperty(nameof(Lot.tags))]
    public List<Lot> lots {get; set;} = null!;
}
