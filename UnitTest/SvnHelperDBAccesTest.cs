using LibSvnChangeSet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ChangesetSvnTestProject
{
    
    
    /// <summary>
    ///This is a test class for SvnHelperTestConfigHandler and is intended
    ///to contain all SvnHelperTestConfigHandler Unit Tests
    ///</summary>
    [TestClass()]
    public class SvnHelperTestConfigHandler
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        

        /// <summary>
        ///A test for getModifiedFilePaths
        ///</summary>
        [TestMethod()]
        public void getModifiedFilePathsTest()
        {
            SvnHelper target = new SvnHelper(); // TODO: Initialize to an appropriate value
            string localPath = "";
            List<string> expected = null;
            List<string> actual;
            actual = target.getModifiedFilePaths(localPath);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getFile
        ///</summary>
        [TestMethod()]
        public void getFileRevisionTest()
        {
            SvnHelper target = new SvnHelper();
            string fileWorkingCopyPath = string.Empty;
            string filePathToWrite = string.Empty;
            string localPath = "";
            List<string> files = target.getModifiedFilePaths(localPath);
            
        }

        /// <summary>
        ///A test for createChangeList
        ///</summary>
        [TestMethod()]
        public void createChangeListTest()
        {
            SvnHelper target = new SvnHelper(); // TODO: Initialize to an appropriate value
            string localPath = "";
            List<string> modifiedFileList;
            modifiedFileList = target.getModifiedFilePaths(localPath);

            string dirPath = @"C:\temp\cstest";
            string localArchivePath = localPath;
            bool expected = true;
            bool actual;
            actual = target.createChangeList(modifiedFileList, localArchivePath, dirPath);
            Assert.AreEqual(expected, actual);
        }
    }
}
namespace UnitTest
{
    
    
    /// <summary>
    ///This is a test class for SvnHelperTestConfigHandler and is intended
    ///to contain all SvnHelperTestConfigHandler Unit Tests
    ///</summary>
    [TestClass()]
    public class SvnHelperTestConfigHandler
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for getModifiedListAsync
        ///</summary>
        [TestMethod()]
        public void getModifiedListAsyncTest()
        {
            SvnHelper target = new SvnHelper(); // TODO: Initialize to an appropriate value
            string localPath = "";
            EventHandler<ProgressEventArgs> progressCallback = cb_Progress;
            EventHandler<CompletedEventArgs> completedCallback = cb_completed;
            bool expected = true;
            bool actual;

            actual = target.getModifiedListAsync(localPath, progressCallback, completedCallback);
            Assert.AreEqual(expected, actual);
        }

        void cb_Progress(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.FileName);
            
        }

        void cb_completed(object sender, CompletedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
