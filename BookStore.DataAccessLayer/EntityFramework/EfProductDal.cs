using BookStore.DataAccessLayer.Abstract;
using BookStore.DataAccessLayer.Context;
using BookStore.DataAccessLayer.Repositories;
using BookStore.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccessLayer.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {
        public EfProductDal(BookStoreContext context) : base(context)
        {
        }

        public List<Product> GetProductByCategory(int id)
        {
            var context = new BookStoreContext();
            var values = context.Products.Where(c => c.CategoryId == id);
            return values.ToList();
        }

        public int GetProductCount()
        {
            var context = new BookStoreContext();
            int value = context.Products.Count();
            return value;
        }
    }
}
