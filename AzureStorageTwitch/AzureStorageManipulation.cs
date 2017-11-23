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
    public class AzureStorageManipulation
    {
        public static CloudTable CreateTable(string tablename, string ConnStr)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(ConnStr));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tablename);

            if (table.CreateIfNotExists())
            {
                return table;
            }
            if (table.Exists())
            {
                return table;
            }
            else
            {
                return null;
            }
        }

        public static List<AccountEntity> AddSampleData()
        {
            return new List<AccountEntity>()
            {
                new AccountEntity("twitchID1")
                {
                    CalendarApp = "GoogleCalendar",
                    TokenCalendar = "1234567890ABC",
                    IDCalendar = "calendar123",
                    MainColor = "#212121",
                    AltColor = "#111",
                    FontColor = "#ff572e",
                    HighlightColor = "#FFFFFF",
                    USFormatTime = false
                },

                 new AccountEntity("twitchID2")
                {
                    CalendarApp = "GoogleCalendar",
                    TokenCalendar = "1234567890ABC",
                    IDCalendar = "calendar123",
                    MainColor = "#212121",
                    AltColor = "#111",
                    FontColor = "#ff572e",
                    HighlightColor = "#FFFFFF",
                    USFormatTime = true
                }
            };
        }

        public static AccountEntity GetTwitchAcc(CloudTable table, string TwitchID)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<AccountEntity>(TwitchID, TwitchID);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult != null)
            {
                return ((AccountEntity)retrievedResult.Result);
            }
            return null;
        }

        public static AccountEntity InsertTwitchAcc(CloudTable table, AccountEntity NewValues)
        {
            TableOperation insertOrReplaceOperation = TableOperation.Insert(NewValues);
            var rtn = table.Execute(insertOrReplaceOperation);

            if (rtn != null)
                return ((AccountEntity)rtn.Result);
            else return null;
        }

        public static AccountEntity ReplaceTwitchAcc(CloudTable table, string TwitchID, AccountEntity NewValues)
        {
            AccountEntity updateEntity = GetTwitchAcc(table, TwitchID);

            if (updateEntity != null)
            {
                updateEntity.CalendarApp = NewValues.CalendarApp;
                updateEntity.TokenCalendar = NewValues.TokenCalendar;
                updateEntity.IDCalendar = NewValues.IDCalendar;
                updateEntity.MainColor = NewValues.MainColor;
                updateEntity.AltColor = NewValues.AltColor;
                updateEntity.FontColor = NewValues.FontColor;
                updateEntity.HighlightColor = NewValues.HighlightColor;
                updateEntity.USFormatTime = NewValues.USFormatTime;

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                var tresult = table.Execute(updateOperation);

                if (tresult != null) return (AccountEntity)tresult.Result;
            }

            return null;
        }

        public static AccountEntity InsertOrReplaceTwitchAcc(CloudTable table, string TwitchID, AccountEntity NewValues)
        {
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(NewValues);
            var rtn = table.Execute(insertOrReplaceOperation);

            if (rtn != null)
                return ((AccountEntity)rtn.Result);
            else return null;
        }

        public static bool DeleteTwitchAcc(CloudTable table, string TwitchID)
        {
            AccountEntity deleteEntity = GetTwitchAcc(table, TwitchID);

            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                var deleted = table.Execute(deleteOperation);
                return true;
            }
            else return false;
        }

        public static List<AccountEntity> GetAllAccount(CloudTable table)
        {
            TableContinuationToken token = null;
            var entities = new List<AccountEntity>();
            do
            {
                var queryResult = table.ExecuteQuerySegmented(new TableQuery<AccountEntity>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        public static bool DeleteTable(string tablename, string ConnStr)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(ConnStr));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tablename);

            if (table == null) return false;
            return table.DeleteIfExists();
        }

    }
}
