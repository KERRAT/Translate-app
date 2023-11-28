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
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (!IsItemExists(item.ID))
            {
                translationCollection.Add(item);
            }
        }

        // Метод для додавання кількох елементів
        public void AddRange(IEnumerable<Translation> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                Add(item); // Use the Add method to add each item (this will ensure uniqueness)
            }
        }

        // Допоміжний метод для перевірки існування елемента в колекції за ID
        private bool IsItemExists(long id)
        {
            return translationCollection.Any(t => t.ID == id);
        }
    }
}
