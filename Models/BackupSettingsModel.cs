using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SmsBackup.Models
{
    public class BackupSettingsModel
    {
        public bool IncludeContacts { get; set; }
        public bool DiscardSubjects { get; set; }
        public bool DiscardShortNumbers { get; set; }
        public bool IncludeImages { get; set; }
        public BackupFileFormat FileFormat { get; set; }
    }

    public enum BackupFileFormat
    {
        JSON = 0,
        XML = 1,
        CSV = 2
    }
}