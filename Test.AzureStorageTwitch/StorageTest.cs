using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureStorageTwitch;
using System.Diagnostics;

namespace Test.AzureStorageTwitch
{
    [TestClass]
    public class StorageTest
    {
        static string TABLENAME = "user_test";
        static string KEYSTORAGE = "StorageConnectionString";

        [TestInitialize]
        public void Initialize()
        {
            var process = Process.Start(@"C:\Program Files\Microsoft SDKs\Azure\Emulator\csrun","/devstore");

            if(process != null)
                process.WaitForExit();
            else
                throw new Exception("Unable to start storage emulator");
        }

        [TestMethod]
        public void TestCreationTable()
        {
            var createdTable = AzureStorageManipulation.CreateTable(TABLENAME, KEYSTORAGE);
            Assert.IsNotNull(createdTable);
            Assert.IsTrue("usertest" == createdTable.Name);
        }


    }
}