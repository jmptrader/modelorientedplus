﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoPlus.ViewModel.Tests.SamplePacks;
using MoPlus.ViewModel.Tests.Staging;

namespace MoPlus.ViewModel.Tests.MSSQLReverseEngineering
{
    [TestClass]
    public class SimpleCreateProjectTest: BaseTest
    {
        /*
         * This test creates a database, and loads that database into an Mo+ solution.
         */

        private string mDatabaseFileName;
        private string mDatabaseLogFileName;
        private string mTemplatesPath;

        protected override void DoExecute(string playground)
        {
            TestLocaldb.Execute("sqllocaldb.exe", "stop v11.0");
            TestLocaldb.Execute("sqllocaldb.exe", "start v11.0");

            var dbName = "Northwind-" + Guid.NewGuid();
            mDatabaseFileName = Path.Combine(playground, dbName + ".mdf");
            mDatabaseLogFileName = Path.Combine(playground, dbName + "_log.ldf");
            mTemplatesPath = Path.Combine(playground, "Templates");

            // setup database
            NorthwindUtility.Create(dbName, mDatabaseFileName, mDatabaseLogFileName);
            var gettingStartedPath = Path.Combine(playground, "Pack");
            Directory.CreateDirectory(gettingStartedPath);

            // unpack sapmle pack to <Playground>\GettingStartedPack
            SamplePacksUtility.ExtractSampleCSharpSQLServerXmlTo(gettingStartedPath);
            var templateBaseDir = Path.Combine(playground, "Pack", "Sample_CSharp_SQLServer_MySQL_Xml", "Templates", "CSharp_VS2010");

            var solutionDesigner = new DesignerViewModel();
            var builder = new BuilderViewModel();

            var solutionVM = ViewModelHelper.NewSolution(builder,
                                                         solutionDesigner,
                                                         "TestSolution",
                                                         "TestNamespace",
                                                         "TestSolution.sln",
                                                         "TestCompany",
                                                         "TestProduct",
                                                         "0.1",
                                                         Path.Combine(playground, "TestSolution.xml"),
                                                         Path.Combine(templateBaseDir, "SolutionFile.mpt"));

            ViewModelHelper.NewDatabaseSource(builder,
                                              solutionDesigner,
                                              solutionVM,
                                              @"(localdb)\v11.0",
                                              mDatabaseFileName,
                                              Path.Combine(gettingStartedPath, @"Sample_CSharp_SQLServer_MySQL_Xml\Specifications\SQLServer\MDLSqlModel.mps"));
            
            Assert.AreEqual(1, solutionVM.Solution.DatabaseSourceList.Count);

            ViewModelHelper.SaveSolution(solutionVM);
            
            ViewModelHelper.CreateNewProject(solutionVM,
                                             solutionDesigner,
                                             "EFBLL",
                                             "EFBLL",
                                             Path.Combine(templateBaseDir, "Project", "EntityFramework.mpt"),
                                             "BLL");

            Assert.AreEqual(1, solutionVM.Solution.DatabaseSourceList.Count);
            Assert.AreEqual(1, solutionVM.Solution.ProjectList.Count); 

            ViewModelHelper.UpdateOutputSolution(solutionVM);
            
            Assert.IsTrue(File.Exists(Path.Combine(playground, "TestSolution.sln")), "Solution file has not been created!");
            Console.WriteLine("Calling MSBuild now");
            MSBuildUtility.Execute(Path.Combine(playground, "TestSolution.sln"),
                                   multiThreaded: true);
            Console.WriteLine("TEst finished for now");
        }
    }
}