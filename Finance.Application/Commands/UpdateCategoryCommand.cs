namespace Finance.Application.Commands;

public record UpdateCategoryCommand(
    int CategoryId,
    string Name,
    string UserId);