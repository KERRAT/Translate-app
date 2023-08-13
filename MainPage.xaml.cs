using Syncfusion.Maui.DataGrid;
using Noisrev.League.IO.RST;
using Maui.DataGrid;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Translate_app;

public partial class MainPage : ContentPage
{
    private int currentPage = 1;

    public MainPage()
    {
        InitializeComponent();
        BindingContext = TranslationRepository.Instance;
        dataGrid.ItemsSource = TranslationRepository.Instance.translationCollection;

        LoadData();
    }

    private async void LoadData()
    {
        var Data = await DbCommunicator.GetData(1);


        MainThread.BeginInvokeOnMainThread(() => {
            TranslationRepository.Instance.AddRange(Data);
        });
    }



    private async void RstSelectClicked(object sender, EventArgs e)
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



    private void dataGrid_CellValueChanged(object sender, DataGridCellValueChangedEventArgs e)
    {
        Console.WriteLine(e.ToString());
    }

    private async void LoadMoreData(object sender, EventArgs e)
    {
        currentPage++; // move to the next page
        var newData = await DbCommunicator.GetData(currentPage);

        if (newData != null && newData.Any())
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() => {
                    TranslationRepository.Instance.AddRange(newData);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }
    }

}
