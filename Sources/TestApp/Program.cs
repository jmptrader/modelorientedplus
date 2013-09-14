﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using MoPlus.Interpreter.BLL.Solutions;
using MoPlus.ViewModel;
using MoPlus.ViewModel.Entities;
using MoPlus.ViewModel.Events;
using MoPlus.ViewModel.Interpreter;
using MoPlus.ViewModel.Messaging;
using MoPlus.ViewModel.Solutions;
using MoPlus.ViewModel.Tests;
using MoPlus.ViewModel.Tests.MSSQLReverseEngineering;

namespace TestApp
{
    public class Program
    {
        private const string Playground = @"c:\OpenSource\MoPlusPlayground";
        static void Main(string[] args)
        {
            new Program().Execute();
        }

        private void Execute()
        {
            BaseTest.EventWaitTimeout = -1;
            var test = new SimpleCreateProjectVS2012Test();
            test.Execute(Playground);
        }
    }
}