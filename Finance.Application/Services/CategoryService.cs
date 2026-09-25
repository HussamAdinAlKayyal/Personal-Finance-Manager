using Finance.Application.Abstractions;
using Finance.Application.Commands;
using Finance.Application.Dtos;
using Finance.Application.Exceptions;
using Finance.Application.Queries;
using Finance.Application.Responses;

namespace Finance.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository)
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;

    private readonly IUserRepository userRepository = userRepository;

    private async Task ThrowIfUserNotExistAsync(string userId)
    {
        if (!await userRepository.ExistsAsync(userId))
        {
            throw new EntityNotFoundException("User", userId);
        }
    }

    public async Task CreateAsync(CreateCategoryCommand command)
    {
        await ThrowIfUserNotExistAsync(command.UserId);
        if (await categoryRepository.IsCategoryNameUsedAsync(command.UserId, command.Name))
        {
            throw new InvalidOperationException("Category with the same name already exists for this user.");
        }
        CreateCategoryDto dto = new(command.Name, command.UserId);
        await categoryRepository.CreateAsync(dto);
    }

    public async Task DeleteAsync(DeleteCategoryCommand command)
    {
        await ThrowIfUserNotExistAsync(command.UserId);
        if (!await categoryRepository.DoesCategoryBelongToUserAsync(command.CategoryId, command.UserId))
        {
            throw new UnauthorizedAccessException("Cannot delete category that is not yours.");
        }
        if (await categoryRepository.IsUsedAsync(command.CategoryId))
        {
            throw new ArgumentException("Cannot delete used category.");
        }
        await categoryRepository.DeleteAsync(command.CategoryId);
    }

    public async Task<IEnumerable<CategoryResponse>> GetAsync(string userId)
    {
        await ThrowIfUserNotExistAsync(userId);
        return await categoryRepository.GetAllAsync(userId);
    }

    public async Task<CategoryResponse> GetAsync(CategoryQuery query)
    {
        await ThrowIfUserNotExistAsync(query.UserId);
        await categoryRepository.DoesCategoryBelongToUserAsync(query.CategoryId, query.UserId);
        return await categoryRepository.GetAsync(query.CategoryId) ?? throw new EntityNotFoundException("Category", query.CategoryId);
    }

    public async Task UpdateAsync(UpdateCategoryCommand command)
    {
        await ThrowIfUserNotExistAsync(command.UserId);
        if (!await categoryRepository.DoesCategoryBelongToUserAsync(command.CategoryId, command.UserId))
        {
            throw new UnauthorizedAccessException("Cannot update category that is not yours.");
        }
        UpdateCategoryDto dto = new(command.CategoryId, command.Name);
        await categoryRepository.UpdateAsync(dto);
    }
}
