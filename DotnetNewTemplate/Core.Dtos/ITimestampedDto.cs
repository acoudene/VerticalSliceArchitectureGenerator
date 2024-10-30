namespace Core.Dtos;

public interface ITimestampedDto
{
  public DateTimeOffset CreatedAt { get; }
  public DateTimeOffset UpdatedAt { get; }
}
