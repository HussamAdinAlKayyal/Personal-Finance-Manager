namespace Finance.Application.Exceptions;

[Serializable]
public class EntityNotFoundException(string entityName, object key) : Exception($"{entityName} with ({key}) was not found.")
{
}