namespace Core.Dtos;

public interface IIdentifierDto : IDto
{
    Guid Id { get; set; }
}
