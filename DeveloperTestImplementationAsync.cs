#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion


namespace DeveloperTest
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System;
    using System.Threading.Tasks;
    using DeveloperTestInterfaces;

    public sealed class DeveloperTestImplementationAsync : IDeveloperTestAsync
    {
        private readonly WordsDictionary _dictionary = new WordsDictionary();
        private readonly object _locker = new object();
        private bool _finished;
        public async Task RunQuestionOne(ICharacterReader reader, IOutputResult output)
        {
            // need fresh dictionary
            _dictionary.Clean();
            await ProcessReaderAsync(reader);
            await Print(output);
        }

        public async Task RunQuestionTwo(ICharacterReader[] readers, IOutputResult output)
        {
            // need fresh dictionary
            _dictionary.Clean();

            var tasks = readers.Select(ProcessReaderAsync).ToList();
            var printCounterTask = new Task(() => DelayedPrint(output));
            printCounterTask.Start();

            Task.WhenAll(tasks).ContinueWith(x => Print(output).ContinueWith(_ =>
            {
                lock (_locker)
                {
                    //mark flag to true to signal that the delayed printing need to stop
                    _finished = true;
                }
            })).Wait();

            printCounterTask.Wait();
        }

        private async Task ProcessReaderAsync(ICharacterReader reader)
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

        private Task Print(IOutputResult output)
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

        private async Task DelayedPrint(IOutputResult output)
        {
            while (true)
            {
                if (!_finished)
                {
                    //every 10 seconds should print. 
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