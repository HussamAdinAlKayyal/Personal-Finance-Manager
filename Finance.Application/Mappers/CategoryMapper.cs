using Finance.Application.Responses;
using Finance.Domain.Entities;

namespace Finance.Application.Mappers;

public static class CategoryMapper
{
    public static CategoryResponse AsCategoryResponse(this Category category)
    {
        return new(category.Id, category.Name);
    }
}
