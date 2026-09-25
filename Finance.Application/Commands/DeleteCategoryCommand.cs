namespace Finance.Application.Commands;

public record DeleteCategoryCommand(
    int CategoryId,
    string UserId);