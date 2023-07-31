using Syncfusion.Maui.DataGrid;
using Noisrev.League.IO.RST;
using Maui.DataGrid;

namespace Translate_app;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = TranslationRepository.Instance;

        TranslationRepository viewModel = TranslationRepository.Instance;
        dataGrid.ItemsSource = viewModel.translationCollection;
    }

    private void dataGrid_TranslatedTextEndEdit(object sender, DataGridCurrentCellEndEditEventArgs e)
    {
        Console.WriteLine(e.ToString());

    }

    private async void rst_select_Clicked(object sender, EventArgs e)
    {
        try
        {
            PickOptions pickOptions = new PickOptions
            {
                PickerTitle = "Pick Barcode/QR Code Image",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { ".stringtable", ".rst" } }, // UTType values
                    { DevicePlatform.Android, new[] { ".stringtable", ".rst" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".stringtable", ".rst" } }, // file extension
                    { DevicePlatform.macOS, new[] { ".stringtable", ".rst" } }, // UTType values
                })
            };

            FileResult file = await FilePicker.Default.PickAsync(pickOptions);

            var fileSource = file.FullPath.ToString();

            var rst = RSTFile.Load(fileSource);

            string json = DataFormatter.ToJSON(rst);

            await DbCommunicator.PostUntranslatedFile(json, file.FileName);

        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }
        return;
    }

    private async void load_data_Clicked(object sender, EventArgs e)
    {
        dataGrid.ItemsSource = await DbCommunicator.GetData();
    }

}
