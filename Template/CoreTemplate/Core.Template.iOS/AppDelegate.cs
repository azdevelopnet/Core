﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Template.Core;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Core.Template.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("Brush_Experimental", "Markup_Experimental", "SwipeView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            InitBuildSettings();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }



        private void InitBuildSettings()
        {


//#if DEBUG
//            CoreSettings.CurrentBuild = BuildEnv.Dev;
//#else
//            CoreSettings.CurrentBuild = BuildEnv.PROD;
//#endif


        }

    }
}
