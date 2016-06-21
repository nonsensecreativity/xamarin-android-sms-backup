using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Java.Lang;

namespace SmsBackup.Adapters
{
    public class TabFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Fragment[] fragments;

        private readonly ICharSequence[] titles;

        public TabFragmentPagerAdapter(FragmentManager fm, Fragment[] fragments, ICharSequence[] titles) : base(fm)
        {
            this.fragments = fragments;
            this.titles = titles;
        }
        public TabFragmentPagerAdapter(FragmentManager fm, List<Fragment> fragments, List<string> titles) : base(fm)
        {
            this.fragments = fragments.ToArray();
            this.titles = titles.Select(x => new Java.Lang.String(x)).ToArray();
        }
        public override int Count
        {
            get
            {
                return fragments.Length;
            }
        }

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return titles[position];
        }
    }
}