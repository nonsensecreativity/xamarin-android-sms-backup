using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmsBackup.Models
{
    class MessageCollectionModel
    {
        public List<SmsMessageModel> RecievedMessages { get; set; }
        public List<SmsMessageModel> SentMessages { get; set; }
        public List<SmsMessageModel> DraftMessages { get; set; }

        public MessageCollectionModel()
        {
            RecievedMessages = new List<SmsMessageModel>();
            SentMessages = new List<SmsMessageModel>();
            DraftMessages = new List<SmsMessageModel>();
        }
    }
}