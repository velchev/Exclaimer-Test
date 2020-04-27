namespace DeveloperTest
{
    using System.Collections.Generic;

    public class WordsDictionary
    {
        public void Add(string word)
        {
            var key = word.ToLower().Trim();
            if (key.Length > 0)
            {
                if (Dictionary.ContainsKey(key))
                {
                    Dictionary[key] = Dictionary[key] + 1;
                }
                else
                {
                    Dictionary.Add(key, 1);
                }
            }
        }

        public void Clean()
        {
            Dictionary = new Dictionary<string, int>();
        }
        public Dictionary<string, int> Dictionary { get; private set; } = new Dictionary<string, int>();
    }
}