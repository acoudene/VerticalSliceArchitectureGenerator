namespace Core.Data;

public interface ITimestampedMongoEntity
{
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}
