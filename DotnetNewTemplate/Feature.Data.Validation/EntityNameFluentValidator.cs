using Feature.Data.Entities;
using FluentValidation;

namespace Feature.Data.Validation;

/// <summary>
/// A standard AbstractValidator which contains multiple rules and can be shared with the back end API
/// </summary>
/// <typeparam name="EntityNameVo"></typeparam>
public class EntityNameFluentValidator : AbstractValidator<EntityName>
{
  public EntityNameFluentValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .MustAsync(async (value, cancellationToken) => await IsUniqueAsync(value));

    RuleFor(x => x.CreatedAt)
        .NotEmpty();

    RuleFor(x => x.UpdatedAt)
        .NotEmpty();

    // TODO - Complete with other validation rules
  }

  private Task<bool> IsUniqueAsync(Guid id)
  {
    // TODO - Api call for example
    if (id == Guid.Empty)
      return Task.FromResult(false);

    return Task.FromResult(true);
  }

  public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
  {
    var result = await ValidateAsync(ValidationContext<EntityName>.CreateWithOptions((EntityName)model, x => x.IncludeProperties(propertyName)));
    if (result.IsValid)
      return Array.Empty<string>();
    return result.Errors.Select(e => e.ErrorMessage);
  };
}
