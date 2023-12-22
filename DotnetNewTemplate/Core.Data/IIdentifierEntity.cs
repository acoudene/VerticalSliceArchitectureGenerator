namespace Core.Data;

public interface IIdentifierEntity : IEntity
{
    Guid Id { get; set; }
}
