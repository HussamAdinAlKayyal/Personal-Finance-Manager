using Finance.Application.Abstractions;
using Finance.Application.Dtos;
using Finance.Application.Exceptions;
using Finance.Application.Responses;
using Finance.Application.Mappers;
using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Finance.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext context = context;

    public async Task CreateAsync(CreateCategoryDto dto)
    {
        await context.Categories.AddAsync(new(dto.Name, dto.UserId));
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Category category = await context.Categories.FindAsync(id) ?? 
            throw new EntityNotFoundException("Category", id);
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DoesCategoryBelongToUserAsync(int categoryId, string userId)
    {
        return await context.Categories.AnyAsync(c => c.UserId == userId && c.Id == categoryId);
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync(string userId)
    {
        return await context.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .Select(c => new CategoryResponse(c.Id, c.Name))
            .ToListAsync();
    }

    public async Task<CategoryResponse?> GetAsync(int id)
    {
        return (await context.Categories.FindAsync(id))?.AsCategoryResponse();
    }
    
    public async Task<string?> GetNameAsync(int id)
    {
        return await context.Categories.Where(c => c.Id == id).Select(c => c.Name).FirstOrDefaultAsync();
    }


    public async Task<bool> IsCategoryNameUsedAsync(string userId, string categoryName)
    {
        return await context.Categories.AnyAsync(c => c.UserId == userId && c.Name == categoryName);
    }

    public async Task<bool> IsUsedAsync(int categoryId)
    {
        return await context.Funds.AnyAsync(f => f.CategoryId == categoryId);
    }

    public async Task UpdateAsync(UpdateCategoryDto dto)
    {
        Category category = await context.Categories.FindAsync(dto.CategoryId) ?? 
            throw new EntityNotFoundException("Category", dto.CategoryId);
        EntityEntry<Category> entry = context.Categories.Entry(category);
        category.SetName(dto.Name);
        if (entry.State == EntityState.Modified)
        {
            await context.SaveChangesAsync();
        }
    }
}
