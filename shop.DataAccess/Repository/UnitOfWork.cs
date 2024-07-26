using shop.DataAccess.Repository.IRepository;
using shop.DataAcess.Data;

namespace shop.DataAccess.Repository
{
    public class unitOfWork : IUnitOfWork
	{
		private AppDbContext _db;
		public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }



        public unitOfWork(AppDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }

        public void Save()
		{
			_db.SaveChanges();
		}
	}
}
