using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.Base;

using SharedProj;

public interface IProductRepository
{
    public IEnumerable<Product> GetAll();
    public Product GetById(int id);
    public void Post(Product product);
    public void Update(int id, Product product);
    public void Delete(int id);
}
