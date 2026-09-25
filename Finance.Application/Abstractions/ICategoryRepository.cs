using Finance.Application.Dtos;
using Finance.Application.Responses;

namespace Finance.Application.Abstractions;

public interface ICategoryRepository
{
    Task CreateAsync(CreateCategoryDto dto);
    Task DeleteAsync(int id);
    Task<bool> DoesCategoryBelongToUserAsync(int categoryId, string userId);
    Task<IEnumerable<CategoryResponse>> GetAllAsync(string userId);
    Task<CategoryResponse?> GetAsync(int id);
    Task<string?> GetNameAsync(int id);
    Task<bool> IsCategoryNameUsedAsync(string userId, string categoryName);
    Task<bool> IsUsedAsync(int categoryId);
    Task UpdateAsync(UpdateCategoryDto dto);
}
