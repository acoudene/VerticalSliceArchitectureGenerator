namespace UseCase.Data.Entities;

public record EntityName : IIdentifierEntity
{
    public Guid Id { get; set; }
}