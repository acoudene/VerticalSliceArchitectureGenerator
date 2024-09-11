namespace Core.ViewObjects;

public interface ITimestampedViewObject
{
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}
