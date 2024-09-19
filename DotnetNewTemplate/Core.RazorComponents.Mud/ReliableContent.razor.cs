using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Core.RazorComponents.Mud;

public partial class ReliableContent
{
  public bool IsBusy { get; set; } = true;

  public ErrorBoundary? RefErrorBoundary { get; set; }

  [Inject]
  protected ISnackbar Snackbar { get; set; } = null!;

  [Inject]
  protected IStringLocalizer<ReliableContent> Localizer { get; set; } = null!;

  [Parameter]
  public int MaximumErrorCount { get; set; } = 2;

  [Parameter]
  public Func<Task>? LongRunningTask { get; set; }

  [Parameter]
  public RenderFragment? LoadingContentBody { get; set; }

  [Parameter]
  public RenderFragment? Body { get; set; }

  [Parameter]
  public RenderFragment? ChildContent { get => Body; set => Body = value; }

  [Parameter]
  public RenderFragment? ErrorBody { get; set; }

  protected void RecoverError()
  {
    RefErrorBoundary?.Recover();
  }

  protected override void OnParametersSet()
  {
    RecoverError();
  }

  protected override async Task OnInitializedAsync()
  {
    try
    {
      IsBusy = true;

      if (LongRunningTask is not null)
      {
        await LongRunningTask();
      }
    }
    catch (Exception ex)
    {
      Snackbar.Add(ex.Message, Severity.Error);
    }
    finally
    {
      IsBusy = false;
    }
  }
}
