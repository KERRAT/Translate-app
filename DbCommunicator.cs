using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Translate_app
{
    internal class DbCommunicator
    {
        public static async Task PostUntranslatedFile(string json, string fileName)
        {
            using HttpClient httpClient = new HttpClient();
            // Встановіть URL кінцевої точки API
            string url = "https://func-translation-app.azurewebsites.net/api/AddUntraslatedFile?code=y5Qf_PoqeQWC7Oufb31iaTVyUaE1pSISGJTg-J4QjzvkAzFumidcqw==";

            url += $"&filename={fileName}";

            // Створіть вміст з JSON даними
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Надішліть POST запит
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            // Перевірте статус відповіді
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Дані успішно відправлено!");
            }
            else
            {
                Console.WriteLine("Помилка при відправці даних: " + response.StatusCode);
            }
        }

        public static async Task<ObservableCollection<Translation>> GetData(int pageNumber)
        {
            ObservableCollection<Translation> translations = new ObservableCollection<Translation>();

            try
            {
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:7261/api/GetTranslations?pageNumber={pageNumber}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var translationsData = JsonSerializer.Deserialize<List<TranslationData>>(json);

                    foreach (var translationData in translationsData)
                    {
                        long id = translationData.Id;
                        string untranslatedText = translationData.UntranslatedText;
                        string translatedText = translationData.TranslatedText;
                        string translateStatus = translationData.TranslateStatus;
                        string changedDate = translationData.ChangedDate.ToString();
                        string fileName = translationData.FileName;

                        try
                        {
                            translations.Add(new Translation(
                                id,
                                untranslatedText,
                                translatedText,
                                translateStatus,
                                changedDate,
                                fileName));
                        }
                        catch (ArgumentException e)
                        {
                            // Обробіть виняткову ситуацію тут, наприклад, запишіть журнал чи проігноруйте неправильний запис
                        }
                    }
                }
                else
                {
                    // Обробка випадку, коли запит завершився невдало
                    // Можна додати відповідний код обробки помилок тут
                }
            }
            catch (HttpRequestException e)
            {
                // Обробка виняткових ситуацій з мережею або запитом
                // Можна додати відповідний код обробки помилок тут
            }

            return translations;
        }

        public class TranslationData
        {
            public long Id { get; set; }
            public string UntranslatedText { get; set; }
            public string TranslatedText { get; set; }
            public string TranslateStatus { get; set; }
            public DateTime ChangedDate { get; set; }
            public string FileName { get; set; }
        }

    }
}