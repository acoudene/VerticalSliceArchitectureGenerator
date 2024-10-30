namespace Core.ViewObjects;

public interface ITimestampedViewObject
{
  public DisplayableDateTime CreatedAt { get; }
  public DisplayableDateTime UpdatedAt { get; }
}
