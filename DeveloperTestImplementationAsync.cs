#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion


namespace DeveloperTest
{
    using System.Linq;
    using System.Threading.Tasks;
    using DeveloperTestInterfaces;

    public sealed class DeveloperTestImplementationAsync : IDeveloperTestAsync
    {
        public async Task RunQuestionOne(ICharacterReader reader, IOutputResult output)
        {
            var dt = new DeveloperTest();
            await dt.ProcessReaderAsync(reader);
            await dt.Print(output);
        }

        public async Task RunQuestionTwo(ICharacterReader[] readers, IOutputResult output)
        {
            var dt = new DeveloperTest();
            var tasks = readers.Select(dt.ProcessReaderAsync).ToList();
            var printCounterTask = new Task(() => dt.DelayedPrint(output));
            printCounterTask.Start();

            Task.WhenAll(tasks).ContinueWith(x => dt.Print(output).ContinueWith(_ =>
            {
                lock (dt._locker)
                {
                    //mark flag to true to signal that the delayed printing need to stop
                    dt._finished = true;
                }
            })).Wait();

            printCounterTask.Wait();
        }
    }
}