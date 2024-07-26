using shop.DataAccess.Repository.IRepository;
using shop.DataAcess.Data;
using Shop.Models;

namespace shop.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
		private AppDbContext _db;

		public CategoryRepository(AppDbContext db) : base(db)
		{
			_db = db;
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public void Update(Category obj)
		{
			_db.Categories.Update(obj);
		}
	}
}
