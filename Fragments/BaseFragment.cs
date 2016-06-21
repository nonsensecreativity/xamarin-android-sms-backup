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

using Fragment = Android.Support.V4.App.Fragment;

namespace SmsBackup.Fragments
{
    public class BaseFragment : Fragment
    {
        public string[] SmsFields
        {
            get
            {
                return Resources.GetStringArray(Resource.Array.SmsFields);
            }
        }

        public string SmsInboxUri
        {
            get
            {
                return Resources.GetString(Resource.String.SmsInboxUri);
            }
        }

        public string SmsSentUri
        {
            get
            {
                return Resources.GetString(Resource.String.SmsSentUri);
            }
        }

        public string SmsDraftUri
        {
            get
            {
                return Resources.GetString(Resource.String.SmsDraftUri);
            }
        }

        public DateTime UnixEpoch
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }
    }
}