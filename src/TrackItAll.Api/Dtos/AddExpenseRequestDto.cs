using System.ComponentModel.DataAnnotations;

namespace TrackItAll.Api.Dtos;

public record AddExpenseRequestDto(
    [Required] double? Amount,
    [Required] string? Description,
    [Required] int? CategoryId,
    [Required] DateTime? Date
);