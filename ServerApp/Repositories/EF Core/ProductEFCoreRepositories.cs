using ServerApp.Repositories.Base;
using ServerApp.Repositories.EF_Core.DbContext;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.EF_Core;

public class ProductEFCoreRepositories : IProductRepository
{
    private readonly ShopDbContext dbcontext;

    public ProductEFCoreRepositories() 
    {  
        dbcontext = new ShopDbContext(); 
    }
    public async void Delete(int id)
    {
        Product product = GetById(id);

        dbcontext.Remove<Product>(product);
        await dbcontext.SaveChangesAsync();
    }

    public IEnumerable<Product> GetAll() => dbcontext.Products;

    public Product GetById(int id)
    {
        return dbcontext.Products.FirstOrDefault(p => p.Id == id)
            ?? throw new ArgumentNullException("There is no product with such ID");
    }

    public async void Post(Product product)
    {
        await dbcontext.Products.AddAsync(product);
        await dbcontext.SaveChangesAsync();
    }

    public async void Update(int id, Product product)
    {
        Product currentProduct = GetById(id);
        currentProduct = product;
        await dbcontext.SaveChangesAsync();
    }
}
