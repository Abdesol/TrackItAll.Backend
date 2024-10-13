using System.ComponentModel.DataAnnotations;

namespace TrackItAll.Api.Dtos;

public record UpdateExpenseRequestDto(
    [Required] string? Id,
    double? Amount,
    string? Description,
    DateTime? Date,
    int? CategoryId
);