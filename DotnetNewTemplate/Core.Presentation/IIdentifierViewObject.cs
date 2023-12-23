namespace Core.Presentation;

public interface IIdentifierViewObject : IViewObject
{
  Guid Id { get; set; }
}
