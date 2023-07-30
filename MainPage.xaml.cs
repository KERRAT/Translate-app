using Maui.DataGrid;
using System.Collections.ObjectModel;
using Syncfusion.Maui.DataGrid;

namespace Translate_app;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        InitializeComponent();
        TranslationRepository viewModel = new TranslationRepository();
		dataGrid.ItemsSource = viewModel.translationCollection;
    }

    private void dataGrid_TranslatedTextEndEdit(object sender, DataGridCurrentCellEndEditEventArgs e)
    {
        Console.WriteLine(e.ToString());

    }
}
