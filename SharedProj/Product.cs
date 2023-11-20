using System.ComponentModel.DataAnnotations;

namespace SharedProj;

public class Product
{
    [Key]
    public int Id { get; set; }
    [MaxLength(20)]
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreationalDate { get; set; }
    public double Price { get; set; }
}
