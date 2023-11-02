using System.Linq.Expressions;

namespace firstDotnetProject.Repository.iRepository;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);

}