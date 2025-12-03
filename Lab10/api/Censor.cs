namespace api
{
    public class Censor
    {
        public List<string> Blacklist { get; set; } = new List<string>();

        public string CensorWord(string word)
        {
            bool isBlacklisted = Blacklist.Any(b => b.Equals(word, StringComparison.OrdinalIgnoreCase));

            if (isBlacklisted)
            {
                if (word.Length <= 1) return word;

                return word[0] + new string('*', word.Length - 1);
            }

            return word;
        }

        public string CensorText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var words = text.Split(' ');
            var censoredWords = new List<string>();

            foreach (var w in words)
            {
                censoredWords.Add(CensorWord(w));
            }

            return string.Join(" ", censoredWords);
        }
    }
}