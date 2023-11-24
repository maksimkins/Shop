using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProj.Models;

public class ProductDTO
{
    public string? Title { get; set; }
    public string? Text { get; set; }
    public double? PriceFrom { get; set; }
    public double? PriceTo { get; set; }
}
