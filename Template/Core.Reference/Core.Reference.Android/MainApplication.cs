﻿using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Xamarin.Forms.Core;

namespace Core.Reference.Droid
{

    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public static Activity AppContext
        {
            get { return CrossCurrentActivity.Current.Activity; }
        }

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            InitBuildSettings();
            InitGlobalLibraries();
        }

        public override void OnTerminate()
        {

            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        private void InitGlobalLibraries()
        {
            //CoreSettings.AppIcon = Resource.Drawable.icon;
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
        }

        private void InitBuildSettings()
        {

//#if DEBUG
//            CoreSettings.CurrentBuild = BuildEnv.Dev;
//#elif QA
//            CoreSettings.CurrentBuild = BuildEnv.Dev;
//#elif UAT
//            CoreSettings.CurrentBuild = BuildEnv.UAT;
//#elif PROD
//		    CoreSettings.CurrentBuild = BuildEnv.PROD;
//#elif RELEASE

//			CoreSettings.CurrentBuild = BuildEnv.PROD;
//#endif

        }
    }
}