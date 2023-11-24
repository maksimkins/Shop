using ServerApp.Repositories.Base;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.Logic_Classes;

public class ProductLogic
{
    private readonly IProductRepository productRepository;
    public ProductLogic(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public IEnumerable<Product> GetAll()
    {
        return productRepository.GetAll().AsEnumerable();
    }
    public Product GetById(int id)
    {
        return productRepository.GetById(id);
    }
    public void Post(Product product)
    {
        product.CreationalDate = DateTime.Now;
        productRepository.Post(product);
    }
    public void Update(int id, Product product)
    {
        product.CreationalDate = DateTime.Now;
        productRepository.Update(id, product);
    }
    public void Delete(int id)
    {
        productRepository.Delete(id);
    }
    public IEnumerable<Product> GetAllByUserId(int id)
    {
        return productRepository.GetAllByUserId(id).AsEnumerable();
    }

    public IEnumerable<Product> Filter(ProductDTO filter)
    {
        if(filter == null)
        {
            return this.GetAll();
        }

        return productRepository.GetAll().Where(delegate(Product p) 
        {
            return p.Title == (filter.Title ?? p.Title) &&
            p.Text == (filter.Text ?? p.Text) &&
            p.Price >= (filter.PriceFrom ?? p.Price) &&
            p.Price <= (filter.PriceTo ?? p.Price);
        });
    }
}
