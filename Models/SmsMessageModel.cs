using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmsBackup.Models
{
    public class SmsMessageModel
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Address { get; set; }
        public int Person { get; set; }
        public DateTime Date { get; set; }
        public int Protocol { get; set; }
        public bool Read { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public bool ReplyPathPresent { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ServiceCenter { get; set; }
        public bool Locked { get; set; }
        public MessageType BackupMessageType { get; set; }
    }

    public enum MessageType
    {
        Inbox = 0,
        Sent = 1,
        Draft = 2
    }
}