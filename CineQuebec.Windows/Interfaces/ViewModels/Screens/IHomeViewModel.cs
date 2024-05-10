using System.Windows;

using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens;

public interface IHomeViewModel : IScreen
{
    IHeaderViewModel HeaderViewModel { get; }
    bool EstAdmin { get; }
    Visibility VisibilityAdmin { get; }
    bool NavigationIsEnabled { get; set; }
    void NavigateToFilmManagement();
    void NavigateToPreferences();
}