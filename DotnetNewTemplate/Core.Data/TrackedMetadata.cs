namespace Core.Data;

public record TrackedMetadata : ITrackedMetadata
{
  public DateTimeOffset LoggedAt { get; set; }
}
