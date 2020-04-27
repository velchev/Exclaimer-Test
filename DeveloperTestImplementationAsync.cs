﻿#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
using System;
using System.Threading.Tasks;

using DeveloperTestInterfaces;

namespace DeveloperTest
{
    public sealed class DeveloperTestImplementationAsync : IDeveloperTestAsync
    {
        public Task RunQuestionOne(ICharacterReader reader, IOutputResult output)
        {
            throw new NotImplementedException();
        }

        public Task RunQuestionTwo(ICharacterReader[] readers, IOutputResult output)
        {
            throw new NotImplementedException();
        }
    }
}