using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SmsBackup.Models;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Android.Support.V7.App;
using Android.Support.V4.View;
using SmsBackup.Adapters;

using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Linq;
using Android.Support.Design.Widget;
using SmsBackup.Fragments;
using Android.Support.V4.Content;

namespace SmsBackup
{
    [Activity(Label = "SMS Backup", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var fragments = new List<Fragment>() { new BackupFragment(), new RestoreFragment() };
            var tabTitles = Resources.GetStringArray(Resource.Array.MainTabTitles).ToList();

            var toolbar = FindViewById<Toolbar>(Resource.Id.Main_Toolbar);

            ((AppBarLayout.LayoutParams)toolbar.LayoutParameters).ScrollFlags = 0;
            SetSupportActionBar(toolbar);

            var viewPager = FindViewById<ViewPager>(Resource.Id.Main_ViewPager);
            viewPager.Adapter = new TabFragmentPagerAdapter(SupportFragmentManager, fragments, tabTitles);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.Main_Tabs);
            tabLayout.SetupWithViewPager(viewPager);
        }
    }
}

