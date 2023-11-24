using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProj.Models;

public class ProductDTO
{
    string? Title { get; set; }
    string? Text { get; set; }
    double? PriceFrom { get; set; }
    double? PriceTo { get; set; }
}
