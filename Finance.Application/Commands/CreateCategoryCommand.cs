namespace Finance.Application.Commands;

public record CreateCategoryCommand(
    string Name,
    string UserId);
