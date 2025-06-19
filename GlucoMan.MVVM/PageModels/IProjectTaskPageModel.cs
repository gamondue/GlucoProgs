using CommunityToolkit.Mvvm.Input;
using GlucoMan.MVVM.Models;

namespace GlucoMan.MVVM.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}