using ServerApp.Repositories.Base;
using ServerApp.Repositories.EF_Core.DbContext;
using SharedProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.EF_Core;

public class ProductEFCoreRepositories : IProductRepository
{
    private readonly ProductDbContext dbcontext;

    public ProductEFCoreRepositories() 
    {  
        dbcontext = new ProductDbContext(); 
    }
    public void Delete(int id)
    {
        Product product = GetById(id);

        dbcontext.Remove<Product>(product);
        dbcontext.SaveChanges();
    }

    public IEnumerable<Product> GetAll() => dbcontext.Products;

    public Product GetById(int id)
    {
        return dbcontext.Products.FirstOrDefault(p => p.Id == id)
            ?? throw new ArgumentNullException("There is no product with such ID");
    }

    public void Post(Product product)
    {
        dbcontext.Products.Add(product);
        dbcontext.SaveChanges();
    }

    public void Update(int id, Product product)
    {
        Product currentProduct = GetById(id);
        currentProduct = product;
        dbcontext.SaveChanges();
    }
}
