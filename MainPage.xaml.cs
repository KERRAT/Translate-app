using Syncfusion.Maui.DataGrid;
using Noisrev.League.IO.RST;
using Maui.DataGrid;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Translate_app;

public partial class MainPage : ContentPage
{
    private int totalLoadedPages = 1;  // Змінена змінна
    private int itemsPerPage = 100;  // Визначення розміру сторінки
    private bool isLoading = false;


    public MainPage()
    {
        InitializeComponent();
        BindingContext = TranslationRepository.Instance;
        dataGrid.ItemsSource = TranslationRepository.Instance.translationCollection;

        dataGrid.QueryRowHeight += DataGrid_QueryRowHeight;

        pageEntry.Text = totalLoadedPages.ToString();  // <-- Add this line


        LoadData();

        dataGrid.Columns["TranslatedText"].AllowEditing = true;

    }

    private async void LoadData()
    {
        var Data = await DbCommunicator.GetData(totalLoadedPages);


        MainThread.BeginInvokeOnMainThread(() => {
            TranslationRepository.Instance.AddRange(Data);
            dataGrid.Columns["TranslatedText"].AllowEditing = true;
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

            if (file == null) return;

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

    private void ToggleButtons(bool enable)
    {
        rst_select.IsEnabled = enable;
        previousPageButton.IsEnabled = enable;
        goToPageButton.IsEnabled = enable;
        nextPageButton.IsEnabled = enable;
        refreshAllDataButton.IsEnabled = enable;

        loadingIndicator.IsVisible = !enable;
        loadingIndicator.IsRunning = !enable;
    }

    private void DataGrid_QueryRowHeight(object sender, DataGridQueryRowHeightEventArgs e)
    {
        if (e.RowIndex > 0)
        {
            e.Height = e.GetIntrinsicRowHeight(e.RowIndex);
            e.Handled = true;
        }
    }

    private async void LoadPreviousPage(object sender, EventArgs e)
    {
        if (totalLoadedPages > 1) // Перевірка, чи не перша сторінка
        {
            totalLoadedPages--;
            var data = await DbCommunicator.GetData(totalLoadedPages, itemsPerPage);
            UpdateData(data);
            pageEntry.Text = totalLoadedPages.ToString();  // <-- Add this line
        }
        else
        {
            // Повідомлення, що це перша сторінка
        }
    }

    private async void LoadNextPage(object sender, EventArgs e)
    {
        if (isLoading)
            return;

        isLoading = true;
        ToggleButtons(false);

        totalLoadedPages++;
        var data = await DbCommunicator.GetData(totalLoadedPages, itemsPerPage);
        if (data != null && data.Any())
        {
            UpdateData(data);
            pageEntry.Text = totalLoadedPages.ToString();  // <-- Add this line
        }
        else
        {
            // Повідомлення, що немає даних для наступної сторінки
            totalLoadedPages--; // Зменшуємо кількість загальних завантажених сторінок
        }
    }

    private async void GoToPage(object sender, EventArgs e)
    {
        if (int.TryParse(pageEntry.Text, out int pageNumber))
        {
            if (isLoading)
                return;

            isLoading = true;
            ToggleButtons(false);

            totalLoadedPages = pageNumber; // Set the current page to the entered page number
            var data = await DbCommunicator.GetData(totalLoadedPages, itemsPerPage);
            if (data != null && data.Any())
            {
                UpdateData(data);
                pageEntry.Text = totalLoadedPages.ToString();  // <-- Add this line
            }
            else
            {
                // Повідомлення, що немає даних для введеного номера сторінки
                totalLoadedPages--; // Зменшуємо кількість загальних завантажених сторінок
            }
        }
        else
        {
            // Повідомлення, що введено неправильний номер сторінки
        }
    }

    private void UpdateData(IEnumerable<Translation> data)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            TranslationRepository.Instance.translationCollection.Clear();
            TranslationRepository.Instance.AddRange(data);
            dataGrid.Columns["TranslatedText"].AllowEditing = true;
            isLoading = false;
            ToggleButtons(true);
        });
    }

    private async void RefreshCurrentPageData(object sender, EventArgs e)
    {

        if (isLoading)
            return;

        isLoading = true;
        ToggleButtons(false);

        // Завантаження даних для поточної сторінки
        var newData = await DbCommunicator.GetData(totalLoadedPages, itemsPerPage);

        if (newData != null && newData.Any())
        {
              
            MainThread.BeginInvokeOnMainThread(() =>
            {  // Очистка існуючих даних
                TranslationRepository.Instance.translationCollection.Clear();

                // Додавання нових даних для поточної сторінки
                TranslationRepository.Instance.AddRange(newData);
                dataGrid.Columns["TranslatedText"].AllowEditing = true;
                pageEntry.Text = totalLoadedPages.ToString();  // <-- Add this line
                isLoading = false;
                ToggleButtons(true);
            });
        }
        else
        {
            // Повідомлення, що немає даних для поточної сторінки
        }
    }



}
