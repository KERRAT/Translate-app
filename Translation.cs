using System;

namespace Translate_app
{
    public enum TranslationStatus
    {
        Untranslated,
        Translated,
        OnReview
    }

    public class Translation
    {
        public long ID { get; }
        public string UntranslatedText { get; }
        private string _translatedText;
        private TranslationStatus _translateStatus;
        public DateTime ChangedDate { get; private set; }
        public string FileName { get; }

        public string TranslatedText
        {
            get => _translatedText;
            set
            {
                _translatedText = value;
                ChangedDate = DateTime.UtcNow;
                _translateStatus = TranslationStatus.OnReview;
            }
        }

        public string TranslateStatus
        {
            get => _translateStatus.ToString();
            private set
            {
                if (!Enum.TryParse(value, out TranslationStatus status))
                    throw new ArgumentException($"Invalid translation status: {value}");

                _translateStatus = status;
            }
        }

        public Translation(long id, string untranslatedText, string translatedText, string translateStatus, string changedDate, string fileName)
        {
            ID = id;
            UntranslatedText = untranslatedText;
            _translatedText = translatedText;
            TranslateStatus = translateStatus;  // This will trigger the set of TranslateStatus property

            if (!DateTime.TryParse(changedDate, out DateTime date))
                throw new ArgumentException($"Invalid date format: {changedDate}");
            ChangedDate = date;

            FileName = fileName;
        }

        public Translation(long id, string untranslatedText, string fileName)
        {
            ID = id;
            UntranslatedText = untranslatedText;
            FileName = fileName;
            ChangedDate = DateTime.UtcNow;
        }

        public void SetStatusOnReview()
        {
            _translateStatus = TranslationStatus.OnReview;
        }

        public void SetStatusTranslated()
        {
            _translateStatus = TranslationStatus.Translated;
        }

        public override string ToString()
        {
            return $"Translation: {UntranslatedText} -> {TranslatedText}, Status: {TranslateStatus}, Date: {ChangedDate}, File: {FileName}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Translation other)
            {
                return ID == other.ID
                       && string.Equals(UntranslatedText, other.UntranslatedText)
                       && string.Equals(TranslatedText, other.TranslatedText)
                       && string.Equals(TranslateStatus, other.TranslateStatus)
                       && ChangedDate.Equals(other.ChangedDate)
                       && string.Equals(FileName, other.FileName);
            }

            return false;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(ID, UntranslatedText, TranslatedText ?? "", TranslateStatus, ChangedDate, FileName);
        }

    }
}