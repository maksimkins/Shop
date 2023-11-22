using System.ComponentModel.DataAnnotations;

namespace SharedProj.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [MaxLength(20)]
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreationalDate { get; set; }
    public double Price { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
