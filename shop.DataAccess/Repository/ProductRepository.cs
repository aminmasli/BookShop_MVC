using shop.DataAccess.Repository.IRepository;
using shop.DataAcess.Data;
using Shop.Models;

namespace shop.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
		private AppDbContext _db;

		public ProductRepository(AppDbContext db) : base(db)
		{
			_db = db;
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public void Update(Product obj)
		{
			var objFromDb = _db.Products.FirstOrDefault(u =>u.Id == obj.Id);
			
			
			if(objFromDb != null)
			{
				objFromDb.Title = obj.Title;
				objFromDb.Author = obj.Author;
				objFromDb.ISBN = obj.ISBN;
				objFromDb.Description = obj.Description;
				objFromDb.Price = obj.Price;
				objFromDb.Price50 = obj.Price50;
				objFromDb.ListPrice = obj.ListPrice;
				objFromDb.Price100 = obj.Price100;
				objFromDb.CategoryId = obj.CategoryId;

				if (objFromDb.ImageUrl != null )
				{
				objFromDb.ImageUrl = obj.ImageUrl;
				
				}


				
			} 
		}
	}
}
