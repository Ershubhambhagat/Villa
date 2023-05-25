using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa.Data;
using Villa.Repository.IRepository;

namespace Villa.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Models.Villa entity)
        {
            await _db.Villas.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<List<Models.Villa>> GetAllVillaAsync(Expression<Func<Models.Villa, bool>> filter = null)
        {
            IQueryable<Models.Villa> quary = _db.Villas;
            if (filter != null)
            {
                quary = quary.Where(filter);
            }
            return await quary.ToListAsync();
        }
        public async Task<Models.Villa> GetVillaAsync(Expression<Func<Models.Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Models.Villa> quary = _db.Villas;
            if (!tracked)
            {
                quary = quary.AsNoTracking();
            }
            if (filter != null)
            {
                quary = quary.Where(filter);
            }
            return await quary.FirstOrDefaultAsync();
        }
        public async Task RemoveAsync(Models.Villa entity)
        {
            _db.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAsync(Models.Villa entity)
        {
            _db.Villas.Update(entity);
            await SaveAsync();
        }
    }
}
