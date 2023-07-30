using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translate_app
{
    public class TranslationRepository
    {
        private ObservableCollection<Translation> translation;

        public ObservableCollection<Translation> translationCollection
        {
            get { return translation; }
            set { this.translation = value; }
        }

        public TranslationRepository()
        {
            translation = new ObservableCollection<Translation>();
            this.GenerateOrders();
        }

        public void GenerateOrders()
        {
            var random = new Random();
            var statuses = Enum.GetNames(typeof(TranslationStatus));

            var translations = new List<Translation>
{
                new Translation(1001L, "Hello World", "Привіт Світ", statuses[random.Next(statuses.Length)], DateTime.Now.AddDays(-random.Next(30)).ToString(), "file1.txt"),
                new Translation(1002L, "Good morning", "Доброго ранку", statuses[random.Next(statuses.Length)], DateTime.Now.AddDays(-random.Next(30)).ToString(), "file2.txt"),
                new Translation(1003L, "How are you?", "Як справи?", statuses[random.Next(statuses.Length)], DateTime.Now.AddDays(-random.Next(30)).ToString(), "file3.txt"),
                new Translation(1004L, "I am fine", "У мене все добре", statuses[random.Next(statuses.Length)], DateTime.Now.AddDays(-random.Next(30)).ToString(), "file4.txt"),
                new Translation(1005L, "Nice to meet you", "Приємно зустрітися", statuses[random.Next(statuses.Length)], DateTime.Now.AddDays(-random.Next(30)).ToString(), "file5.txt"),
            };

            translationCollection.Add(new Translation(1006L, "Have a good day", "file6.txt"));
            translationCollection.Add(new Translation(1007L, "See you later", "file7.txt"));
            translationCollection.Add(new Translation(1008L, "Take care", "file8.txt"));

            foreach (var translation in translations)
            {
                this.translation.Add(translation);
            }

        }
    }
}
