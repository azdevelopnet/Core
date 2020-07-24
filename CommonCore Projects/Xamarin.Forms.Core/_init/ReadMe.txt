In this document when you see comments inside of /* comment */ 

Android SDK Manager - > Extras
 - Android Support Repository
 - Google Play Services

 Mac IDE - > Add-ins - Gallery - IDE Extensions
 - Nuget Package Explorer
 - Nuget Package Management Extensions
 

Required Nuget Installs
 - ModernHttpClient
 - Newtonsoft.Json
 - Xam.Plugins.Permissions
 - Xamarin.FFImageLoading.Forms
 - Xamarin.FFImageLoading.Transformations
 - Xamarin.Essentials
 - NodaTime

Highly Suggested
 - XF.Material /*https://github.com/contrix09/XF-Material-Library*/

(Optional - if you are using Microsoft Authentication)
 - Microsoft.Identity.Client
 - Microsoft.Graph

(Optional - if you are using Mobile Center)
- Mobile Center Analytics
- Mobile Center Crashes
    -> Is currently moving to the following:
        - Microsoft.AppCenter
        - Microsoft.AppCenter.Analytics
        - Microsoft.AppCenter.Crashes
        - Microsoft.AppCenter.Distribute 

 - Platform Specific Installs:
    - iOS   -> BTProgressHud
            -> TTGSnackbar
    - Droid -> AndHud
            -> Plugin.CurrentActivity

Suggested
- Humanizer /* Displaying strings, enums, dates, times, timespans */
- Plugin.Fingerprint /* https://github.com/smstuebe/xamarin-fingerprint */
- Plugin.Share
- FluentFTP /* if you want to use FTP as a transfer protocol
- AIDatePickerController /* IOS DateTime Picker */
- Microcharts.Forms /* https://github.com/aloisdeniel/Microcharts */
- Xamarin.Plugin.ImageEdit /* https://github.com/muak/Xamarin.Plugin.ImageEdit */
- Android Bottom Tabs /* https://medium.com/naxam-blogs/bottomtabbedpage-bottom-navigation-for-xamarin-forms-on-android-325a1506e216 */

Setup Tasks:

Step 1: 
    Create a folder in your application called Config and copy the config.dev.txt file to it.  Change
    the extension to json. Make the file build action embedded resource. Update the settings as needed. Also 
    create a config.qa.json and config.prod.json file.

    FYI: 
        HTTPSettings Handler Options:  
                IOS -> "Managed", "CFNetwork", "NSURLSession", "ModernHttpClient"
                DROID -> "Managed", "AndroidClientHandler", "ModernHttpClient"

        HttpTimeOut: zero means there is no timeout

Step 2: Embedded Database:
    Sqlite:
        Import Xamarin.Forms.Core.Sqlite and follow instructions in readme.txt.  Example implementation
        in the reference guide project.
    Realm:
        Source: https://realm.io/docs/dotnet/latest/
        Blog article: https://blog.xamarin.com/cross-platform-development-with-xamarin-forms-and-realm/
        Sample: https://github.com/azdevelopnet/Core.Samples/tree/master/database_samples/realmDemo
    LiteDb:
        Source: http://www.litedb.org/
        Sample: https://github.com/azdevelopnet/Core.Samples/tree/master/database_samples/realmDemo
    CouchDb:
        Source: https://www.couchbase.com/
        Sample: https://github.com/azdevelopnet/Core.Samples/tree/master/database_samples/couchdbDemo

Step 3 (enabling push notifications) -> 
     Azure Option:
        Import Xamarin.Forms.Core.AzurePush and follow instructions in readme.txt.  Example implementation
        in the reference guide project.
     OneSignal Option:
        Go to : https://documentation.onesignal.com/docs/xamarin-sdk-setup and follow setup instructions.
     Microsoft App Center Option:
        iOS: go to https://docs.microsoft.com/en-us/appcenter/sdk/push/xamarin-ios and follow setup instructions.
        Android: go to https://docs.microsoft.com/en-us/appcenter/sdk/push/xamarin-android and follow setup instructions.

Step 4 (optional OAuth Setup) -> see readme.authentication.txt nested file under IAuthenticatorService 
in the Services folder. /* CustomTab for Android has issues with Xamarin.Android.Support version 25 */

Step 5 (Setup Fody) ->  Make sure the FodyWeavers.xml file installed from PropertyChanged.Fody nuget has the following:
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
    <PropertyChanged/>
</Weavers>


Step 6 (Optional) -> In order to use differnet configuration files across dev environments, you need to modify build settings.
    * Right click on the solution and selection options
    * In the dialog box under Build select Configuration
    * Out of the box there should be Debug and Release.  You can add QA or any other custom name you want.
    * When finished...open iOS and Android projects and select options.
    * In the dialog box under Build select Compiler. Add appropriate environments names to Define Symbols for each configuration
    * Add the following code:
        * iOS -> AppDelegate -> FinishedLaunching
        * android -> MainApplication -> OnCreate

        #if DEBUG
            CoreSettings.CurrentBuid = "dev";
        #elif QA
            CoreSettings.CurrentBuid = "qa";
        #elif RELEASE
            CoreSettings.CurrentBuid = "prod";
        #endif

Step 7 (XAML projects only)
    * Projects Options -> Output-> Assembly Name (make the same for both Android and iOS projects)
    * Add the following xmlns to your pages:
        xmlns:core="clr-namespace:Xamarin.Forms.Core;assembly=yourassemblyname" 
    * Define your pages with CorePage and Set ViewModel property to fully qualified ViewModel name (example):

    <?xml version="1.0" encoding="utf-8"?>
    <core:CorePage xmlns="http://xamarin.com/schemas/2014/forms" 
        xmlns:core="clr-namespace:Xamarin.Forms.Core;assembly=Core.XamlReferenceGuide" 
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
        xmlns:local="clr-namespace:Core.XamlReferenceGuide" 
        xmlns:ffimageloading="clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms" 
        x:Class="Core.XamlReferenceGuide.Core_XamlReferenceGuidePage" 
        ViewModel="Core.XamlReferenceGuide.AppViewModel">
        <ScrollView>
            <StackLayout Padding="20">
                <ffimageloading:CachedImage Margin="20,60,60,20" HeightRequest="200" WidthRequest="200" CacheDuration="30" RetryCount="3" RetryDelay="250" Source="angrymonkey300.png">
                </ffimageloading:CachedImage>
                <core:CoreButton Margin="20,0,20,5" Text="Core List View" Style="{StaticResource LightOrange}" Command="{Binding ViewListControl}">
                </core:CoreButton>
                <core:CoreButton Margin="20,0,20,5" Text="Behaviors" Style="{StaticResource LightOrange}" Command="{Binding ViewBehaviors}">
                </core:CoreButton>
            </StackLayout>
        </ScrollView>
    </core:CorePage>

    * Copy Application.Resources from the _init/Styles/AppStyles.xaml.txt file into your App.xaml as a starter for styles.

*** Search View ***
IOS uses the Search bar as a control in the UI but Android generally expects it in the Actionbar.
In Order to accomplish this, add the following to the Toolbar.xml file and implement the ISearchProvider on the ViewModel.
    <SearchView
        android:minWidth="25px"
        android:minHeight="25px"
        android:layout_width="wrap_content"
        android:layout_height="match_parent"
        android:layout_gravity="right"
        android:visibility="gone"
        android:id="@+id/searchView" />

In the MainActivity in the OnCreate method set:
     CoreSettings.SearchView = Resource.Id.searchView;

*** Calendar Dependency ***
IOS requires you add in the info.plist the following for Usages:
    <key>NSCalendarsUsageDescription</key>
    <string>Calendars App needs Calendar Access</string>
    <key>NSRemindersUsageDescription</key>
    <string>Calendars App needs Reminder Access</string>

Xamarin Github Examples: https://github.com/xamarin/ios-samples/tree/master/Calendars


Android requires permission in the AndroidManifest.xml
    <uses-permission android:name="android.permission.READ_CALENDAR" />
    <uses-permission android:name="android.permission.WRITE_CALENDAR" />

Xamarin WebSite: https://developer.xamarin.com/guides/android/user_interface/controls/calendar/#Calendar_API
Xamarin Github Code Sample: https://github.com/xamarin/monodroid-samples/tree/master/CalendarDemo


*** Generial Permissions ***
Grant Access In Android & IOS to Access Resource Like Calendar, Contacts (Internet)

Additional References: 


TLS explanation : https://blog.xamarin.com/securing-web-requests-with-tls-1-2/
Icon tool: http://apetools.webprofusion.com/tools/imagegorilla


*** MAC Helps ***
Configure Mac 127.0.0.1 to use a custom name so simulators/emulators will see the service. 
i.e
Alter Host File
127.0.0.1   TestDev
and then save....Open Terminal use the following commands in the terminal:

sudo nano /etc/hosts
*****************

*** ANDROID Helps  ***
Use 10.0.2.2 for Android Simulator
*****************


****************************** TOOLS ******************************

*** Background Page Image / Splash Screen ***
http://apetools.webprofusion.com/tools/imagegorilla  -> create icons and screens
https://apetools.webprofusion.com/app/#/tools/imagegorilla

*** Android Asset Studio ***
https://romannurik.github.io/AndroidAssetStudio/index.html

*** JSON to C# ***
https://quicktype.io/


***************************** STYLES IN XAML ******************************

see -> https://blog.xamarin.com/easy-app-theming-with-xamarin-forms-styles/

<Application
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="namespace.App">
    <Application.Resources>
        <ResourceDictionary>
            <Color x:Key="backgroundColor">#33302E</Color>
            <Color x:Key="textColor">White</Color>
 
            <Style x:Key="labelStyle" TargetType="Label">
                <Setter Property="TextColor" Value="{DynamicResource textColor}" />
            </Style>
            <Style x:Key="backgroundStyle" TargetType="VisualElement">
                <Setter Property="BackgroundColor" Value="{DynamicResource backgroundColor}" />
            </Style>
        </ResourceDictionary>
    </Application.Resources>
</Application>




