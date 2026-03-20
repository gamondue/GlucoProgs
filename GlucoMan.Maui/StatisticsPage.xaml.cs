using GlucoMan.Maui.ViewModels;

namespace GlucoMan.Maui;

public partial class StatisticsPage : ContentPage
{
    private StatisticsPageViewModel _viewModel;

    public StatisticsPage(DateTime dateFrom, DateTime dateTo)
    {
        InitializeComponent();

        _viewModel = new StatisticsPageViewModel(dateFrom, dateTo);
        BindingContext = _viewModel;

        // Calculate all statistics
        _viewModel.CalculateAllStatistics();
    }
}
