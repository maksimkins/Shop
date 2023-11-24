using ServerApp.Repositories.Base;
using ServerApp.Repositories.EF_Core.DbContext;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.EF_Core;

public class ProductEFCoreRepository : IProductRepository
{
    private readonly ShopDbContext dbcontext;

    public ProductEFCoreRepository() 
    {  
        dbcontext = new ShopDbContext(); 
    }
    public async void Delete(int id)
    {
        Product product = GetById(id);

        dbcontext.Remove<Product>(product);
        await dbcontext.SaveChangesAsync();
    }

    public IQueryable<Product> GetAll() => dbcontext.Products.AsQueryable<Product>();

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
        dbcontext.Products.Update(product);
        await dbcontext.SaveChangesAsync();
    }

    public IQueryable<Product> GetAllByUserId(int id)
    {
        return dbcontext.Products.Where(p => p.UserId == id);
    }
}
