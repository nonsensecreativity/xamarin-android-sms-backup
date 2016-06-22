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
using System.Xml.Serialization;
using System.IO;

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
            var backupButton = view.FindViewById<Button>(Resource.Id.Backup_BackupButton);

            backupButton.Click += BackupButton_Click;
            backupButton.LongClick += BackupButton_LongClick;

            return view;
        }

        private void BackupButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            var inboxUri = Android.Net.Uri.Parse(SmsInboxUri);

            var vals = new ContentValues();

            vals.Put("thread_id", 122);
            vals.Put("address", "+17067265006");
            vals.Put("person", 0);
            vals.Put("date", 1000000);
            vals.Put("protocol", 0);
            vals.Put("read", 1);
            vals.Put("status", -1);
            vals.Put("type", 1);
            vals.Put("reply_path_present", 0);
            vals.Put("body", "test insertion");
            vals.Put("service_center", "+12063130055");
            vals.Put("locked", 0);

            var inboxCurs = Activity.ContentResolver.Insert(inboxUri, vals);
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {
            var includeCont = View.FindViewById<CheckBox>(Resource.Id.Backup_IncludeContacts);
            var discardSubj = View.FindViewById<CheckBox>(Resource.Id.Backup_DiscardSubjects);
            var discardShort = View.FindViewById<CheckBox>(Resource.Id.Backup_DiscardShortNumbers);
            var incImg = View.FindViewById<CheckBox>(Resource.Id.Backup_IncludeImages);
            var selFormat = View.FindViewById<RadioGroup>(Resource.Id.Backup_FormatRadioGroup);

            var settings = new BackupSettingsModel()
            {
                IncludeContacts = includeCont.Checked,
                DiscardSubjects = discardSubj.Checked,
                DiscardShortNumbers = discardShort.Checked,
                IncludeImages = incImg.Checked
            };

            switch (selFormat.CheckedRadioButtonId)
            {
                case Resource.Id.Backup_Format_Json:
                    settings.FileFormat = BackupFileFormat.JSON;
                    break;
                case Resource.Id.Backup_Format_Xml:
                    settings.FileFormat = BackupFileFormat.XML;
                    break;
                case Resource.Id.Backup_Format_Csv:
                    settings.FileFormat = BackupFileFormat.CSV;
                    break;
            }

            Toast.MakeText(Context, "Starting backup", ToastLength.Short).Show();
            StartBackup(settings);
        }

        private bool StartBackup(BackupSettingsModel settings)
        {
            var list = CreateBackupList(settings);
            if (list == null || list.Count == 0)
                return false;

            var serialized = SerializeBackup(list, settings);
            if (string.IsNullOrWhiteSpace(serialized))
                return false;

            return true;
        }

        private List<SmsMessageModel> CreateBackupList(BackupSettingsModel settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            //get all SMS messages in inbox
            var inboxUri = Android.Net.Uri.Parse(SmsInboxUri);
            var sentUri = Android.Net.Uri.Parse(SmsSentUri);
            var draftUri = Android.Net.Uri.Parse(SmsDraftUri);

            var inboxCurs = Activity.ContentResolver.Query(inboxUri, SmsFields, null, null, null);
            var sentCurs = Activity.ContentResolver.Query(sentUri, SmsFields, null, null, null);
            var draftCurs = Activity.ContentResolver.Query(draftUri, SmsFields, null, null, null);

            var messlist = new List<SmsMessageModel>();

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
                        Locked = inboxCurs.GetInt(13) == 1,
                        BackupMessageType = MessageType.Inbox
                    };

                    messlist.Add(message);
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
                        Locked = sentCurs.GetInt(13) == 1,
                        BackupMessageType = MessageType.Sent
                    };

                    messlist.Add(message);
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
                        Locked = draftCurs.GetInt(13) == 1,
                        BackupMessageType = MessageType.Draft
                    };

                    messlist.Add(message);
                }
                catch
                {
                }
            }

            //we now have all of our messages, we can apply our settings
            if (settings.DiscardShortNumbers)
                messlist.RemoveAll(x => x.Address.Length < 10);

            if (settings.DiscardSubjects)
                messlist.ForEach(x => x.Subject = string.Empty);

            return messlist;
        }

        private string SerializeBackup(List<SmsMessageModel> messages, BackupSettingsModel settings)
        {
            var retVal = string.Empty;

            switch (settings.FileFormat)
            {
                case BackupFileFormat.JSON:
                    retVal = JsonConvert.SerializeObject(messages);
                    break;
                case BackupFileFormat.XML:
                    using (var sw = new StringWriter())
                    {
                        var xmlSer = new XmlSerializer(typeof(List<SmsMessageModel>));
                        xmlSer.Serialize(sw, messages);
                        sw.Flush();
                        retVal = sw.ToString();
                    }
                    break;
                case BackupFileFormat.CSV:
                    break;
            }

            return retVal;
        }
    }
}