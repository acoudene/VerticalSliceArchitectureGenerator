namespace Core.ViewObjects;

public interface ITimestampedViewObject
{
  public DisplayableDateTime CreatedAt { get; set; }
  public DisplayableDateTime UpdatedAt { get; set; }
}
