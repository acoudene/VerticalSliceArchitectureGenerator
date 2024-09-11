namespace Core.Dtos;

public interface ITimestampedDto
{
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}
