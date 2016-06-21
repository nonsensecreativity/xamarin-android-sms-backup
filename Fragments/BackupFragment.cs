using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SmsBackup.Models;
using Newtonsoft.Json;

namespace SmsBackup.Fragments
{
    public class BackupFragment : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Backup, null);
            return view;
        }

        private void BackupSms()
        {

            //get all SMS messages in inbox
            var inboxUri = Android.Net.Uri.Parse(SmsInboxUri);
            var sentUri = Android.Net.Uri.Parse(SmsSentUri);
            var draftUri = Android.Net.Uri.Parse(SmsDraftUri);

            var inboxCurs = Activity.ContentResolver.Query(inboxUri, SmsFields, null, null, null);
            var sentCurs = Activity.ContentResolver.Query(sentUri, SmsFields, null, null, null);
            var draftCurs = Activity.ContentResolver.Query(draftUri, SmsFields, null, null, null);

            var messages = new MessageCollectionModel();

            //loop through recieved messages and pull them all
            while (inboxCurs.MoveToNext())
            {
                try
                {
                    var message = new SmsMessageModel()
                    {
                        Id = inboxCurs.GetInt(0),
                        ThreadId = inboxCurs.GetInt(1),
                        Address = inboxCurs.GetString(2),
                        Person = inboxCurs.GetInt(3),
                        Date = UnixEpoch.AddMilliseconds(inboxCurs.GetLong(4)),
                        Protocol = inboxCurs.GetInt(5),
                        Read = inboxCurs.GetInt(6) == 1,
                        Status = inboxCurs.GetInt(7),
                        Type = inboxCurs.GetInt(8),
                        ReplyPathPresent = inboxCurs.GetInt(9) == 1,
                        Subject = inboxCurs.GetString(10),
                        Body = inboxCurs.GetString(11),
                        ServiceCenter = inboxCurs.GetString(12),
                        Locked = inboxCurs.GetInt(13) == 1
                    };

                    messages.RecievedMessages.Add(message);
                }
                catch
                {
                }
            }

            //now its time for sent messages
            while (sentCurs.MoveToNext())
            {
                try
                {
                    var message = new SmsMessageModel()
                    {
                        Id = sentCurs.GetInt(0),
                        ThreadId = sentCurs.GetInt(1),
                        Address = sentCurs.GetString(2),
                        Person = sentCurs.GetInt(3),
                        Date = UnixEpoch.AddMilliseconds(sentCurs.GetLong(4)),
                        Protocol = sentCurs.GetInt(5),
                        Read = sentCurs.GetInt(6) == 1,
                        Status = sentCurs.GetInt(7),
                        Type = sentCurs.GetInt(8),
                        ReplyPathPresent = sentCurs.GetInt(9) == 1,
                        Subject = sentCurs.GetString(10),
                        Body = sentCurs.GetString(11),
                        ServiceCenter = sentCurs.GetString(12),
                        Locked = sentCurs.GetInt(13) == 1
                    };

                    messages.SentMessages.Add(message);
                }
                catch
                {
                }
            }

            //same for draft messages
            while (draftCurs.MoveToNext())
            {
                try
                {
                    var message = new SmsMessageModel()
                    {
                        Id = draftCurs.GetInt(0),
                        ThreadId = draftCurs.GetInt(1),
                        Address = draftCurs.GetString(2),
                        Person = draftCurs.GetInt(3),
                        Date = UnixEpoch.AddMilliseconds(draftCurs.GetLong(4)),
                        Protocol = draftCurs.GetInt(5),
                        Read = draftCurs.GetInt(6) == 1,
                        Status = draftCurs.GetInt(7),
                        Type = draftCurs.GetInt(8),
                        ReplyPathPresent = draftCurs.GetInt(9) == 1,
                        Subject = draftCurs.GetString(10),
                        Body = draftCurs.GetString(11),
                        ServiceCenter = draftCurs.GetString(12),
                        Locked = draftCurs.GetInt(13) == 1
                    };

                    messages.DraftMessages.Add(message);
                }
                catch
                {
                }
            }

            var ser = JsonConvert.SerializeObject(messages);
        }
    }
}