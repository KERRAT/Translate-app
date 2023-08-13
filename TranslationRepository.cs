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
        private static TranslationRepository _instance;

        private ObservableCollection<Translation> translation;

        public ObservableCollection<Translation> translationCollection
        {
            get { return translation; }
            private set { this.translation = value; }
        }

        private TranslationRepository()
        {
            translation = new ObservableCollection<Translation>();
        }

        public static TranslationRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TranslationRepository();
                }
                return _instance;
            }
        }

        // Метод для додавання одного елемента
        public void Add(Translation item)
        {
            translationCollection.Add(item);
        }

        // Метод для додавання кількох елементів
        public void AddRange(IEnumerable<Translation> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            var itemsList = items.ToList(); // Convert to List first

            foreach (var item in itemsList)
            {
                translationCollection.Add(item);
            }
        }
    }
}
