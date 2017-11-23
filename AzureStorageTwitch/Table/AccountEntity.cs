using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageTwitch.Table
{
    public class AccountEntity : TableEntity
    {
        public AccountEntity(string twitchID)
        {
            this.PartitionKey = twitchID;
            this.RowKey = twitchID;
        }

        public AccountEntity() { }

        public string CalendarApp { get; set; }
        public string TokenCalendar { get; set; }
        public string IDCalendar { get; set; }
        public string MainColor { get; set; }
        public string AltColor { get; set; }
        public string FontColor { get; set; }
        public string HighlightColor { get; set; }
        public bool USFormatTime { get; set; }
    }
}