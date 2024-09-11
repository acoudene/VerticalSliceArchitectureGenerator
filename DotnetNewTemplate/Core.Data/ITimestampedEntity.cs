namespace Core.Data;

public interface ITimestampedEntity
{
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}
