
using System.Linq.Expressions;

namespace Villa.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Models.Villa>> GetAllVillaAsync(Expression<Func<Models.Villa,bool>>filter=null);
        Task<Models.Villa> GetVillaAsync(Expression<Func<Models.Villa,bool>> filter = null,bool tracked=true);

        Task CreateAsync(Models.Villa entity);
        Task UpdateAsync(Models.Villa entity);
        Task RemoveAsync(Models.Villa entity);
        Task SaveAsync();
    }
}
