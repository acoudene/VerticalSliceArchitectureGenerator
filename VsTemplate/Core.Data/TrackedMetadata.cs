namespace $safeprojectname$;

public record TrackedMetadata : ITrackedMetadata
{
  public DateTimeOffset LoggedAt { get; set; }
}
