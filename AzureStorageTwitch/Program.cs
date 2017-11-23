using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AzureStorageTwitch.Table;

namespace AzureStorageTwitch
{
    class Program
    {
        static string TABLENAME = "twitchcal";
        static string KEYSTORAGE = "StorageConnectionString";

        static void Main(string[] args)
        {
            Console.WriteLine("Clean datas ...");
            Console.WriteLine((AzureStorageManipulation.DeleteTable(TABLENAME, KEYSTORAGE)) 
                ? "Deleted" : "Failed or nothing to delete");

            Console.WriteLine("Creation Table ...");
            CloudTable table = AzureStorageManipulation.CreateTable(TABLENAME, KEYSTORAGE);
            Console.WriteLine((table != null) ? "Succeed" : "Failed");

            Console.WriteLine("\n");
            Console.WriteLine("Getting Sample ...");
            var samplelist = AzureStorageManipulation.AddSampleData();
            DisplayAccounts(samplelist);
            
            Console.Read();

            Console.WriteLine("Adding first ...");
            var firstAcc = AzureStorageManipulation.InsertTwitchAcc(table, samplelist.ElementAt(0));
            Console.WriteLine((firstAcc != null) ? "Succeed" : "Failed");
            
            Console.Read();

            Console.WriteLine("Adding second ...");
            var secondAcc = AzureStorageManipulation.InsertTwitchAcc(table, samplelist.ElementAt(1));
            Console.WriteLine((secondAcc != null) ? "Succeed" : "Failed");
            Console.WriteLine("\n");
            DisplayAccounts(AzureStorageManipulation.GetAllAccount(table));
            
            Console.Read();

            Console.WriteLine("Modifying second acc ...");
            secondAcc.CalendarApp = "Microsoft";
            AzureStorageManipulation.ReplaceTwitchAcc(table, secondAcc.RowKey, secondAcc);
            Console.WriteLine("done");
            Console.WriteLine("\n");
            Console.WriteLine("Getting second acc ...");
            var modifyedacc = AzureStorageManipulation.GetTwitchAcc(table, secondAcc.RowKey);
            Console.WriteLine("\n");
            DisplayAccounts(new List<AccountEntity>() { modifyedacc });

            Console.WriteLine("deleting second acc ...");
            var deleted = AzureStorageManipulation.DeleteTwitchAcc(table, modifyedacc.RowKey);
            Console.WriteLine((deleted) ? "Deleted" : "Failed");
            Console.WriteLine("\n");
            DisplayAccounts(AzureStorageManipulation.GetAllAccount(table));
            
            Console.Read();
        }
        
        public static void DisplayAccounts(List<AccountEntity> accs)
        {
            if (accs.Count == 0) Console.WriteLine("Nothing");

            foreach (AccountEntity acc in accs)
            {
                if (acc == null)
                    Console.WriteLine("Echec");
                else
                    Console.WriteLine($"{acc.RowKey} - Token : {acc.TokenCalendar} ({acc.CalendarApp}) IDCal : {acc.IDCalendar}\n" +
                        $"Colors : {acc.MainColor}, {acc.AltColor}, {acc.FontColor}, {acc.HighlightColor} and format time : {FormatShortTime(acc.USFormatTime)}");
            }
        }

        public static string FormatShortTime(bool usFormat) => (usFormat) ? "12h" : "24h";
    }
}