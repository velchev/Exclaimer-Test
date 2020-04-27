namespace DeveloperTest
{
    using System.IO;
    using DeveloperTestInterfaces;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DeveloperTest
    {
        private readonly WordsDictionary _dictionary = new WordsDictionary();
        private readonly object _locker = new object();
        private bool _finished;

        public bool Finished
        {
            get => _finished;
            set
            {
                lock (_locker)
                {
                    _finished = value;
                }
            }
        }

        public async Task ProcessReaderAsync(ICharacterReader reader)
        {
            var buffer = new StringBuilder();
            try
            {
                do
                {
                    var res = reader.GetNextChar();
                    if (res.IsLetter() || res.IsAcceptedSymbol())
                    {
                        if (buffer.Length > 0 &&
                            (buffer.ToString()[buffer.ToString().Length - 1].IsAcceptedSymbol() && res.IsAcceptedSymbol())
                        )
                        {
                            buffer.Remove(buffer.Length - 1, 1);

                            _dictionary.Add(buffer.ToString());
                            buffer.Clear();
                            continue;
                        }
                        buffer.Append(res);
                    }
                    else
                    {
                        _dictionary.Add(buffer.ToString());
                        buffer.Clear();
                    }
                } while (true);

            }
            catch (EndOfStreamException)
            {
                _dictionary.Add(buffer.ToString());
            }
        }

        public Task Print(IOutputResult output)
        {
            foreach (var keyValuePair in _dictionary.Dictionary
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key))
            {
                {
                    output.AddResult($"{keyValuePair.Key} - {keyValuePair.Value}");
                }
            }

            return Task.FromResult(0);
        }

        public async Task DelayedPrint(IOutputResult output)
        {
            while (true)
            {
                if (!Finished)
                {
                    //every 10 seconds should print. 
                    //at least one print even if the execution is less than 10 seconds
                    //as this starts in paralel with the processing
                    Task.Delay(10 * 1000).Wait();
                    await Print(output);
                }
                else
                {
#if DEBUG
                    Console.WriteLine("Finished with printing");
#endif
                    break;
                }
            }
        }
    }
}
