using TrackItAll.Api.Dtos;
using TrackItAll.Domain.Entities;

namespace TrackItAll.Api.Mappers;

/// <summary>
/// Category mapper class to map category model from the application project to the api project dtos
/// </summary>
public static class CategoryMapper
{
    public static CategoryResponseDto ToResponseDto(this Category category)
    {
        return new CategoryResponseDto(category.Id, category.Name);
    }
    
    public static List<CategoryResponseDto> ToResponseDto(this List<Category> categories)
    {
        return categories.ConvertAll(ToResponseDto);
    }
}