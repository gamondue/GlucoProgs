namespace GlucoMan.Maui;

public partial class StatisticsPage : ContentPage
{
    private string _dataType;
    private DateTime _dateFrom;
    private DateTime _dateTo;
    
    public StatisticsPage(string dataType, DateTime dateFrom, DateTime dateTo)
    {
        InitializeComponent();
        
        _dataType = dataType;
        _dateFrom = dateFrom;
        _dateTo = dateTo;
        
        // Display the parameters
        lblDataType.Text = $"Data Type: {dataType}";
        lblDateRange.Text = $"From: {dateFrom:dd/MM/yyyy HH:mm} - To: {dateTo:dd/MM/yyyy HH:mm}";
    }
}
