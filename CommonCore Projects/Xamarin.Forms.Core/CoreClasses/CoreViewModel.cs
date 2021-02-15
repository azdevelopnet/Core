using System.Collections.Generic;
using Newtonsoft.Json;
using PropertyChanged;

namespace Xamarin.Forms.Core
{
    /// <summary>
    /// Observable view model.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public abstract partial class CoreViewModel
    {

        #region ReadOnly AppData Settings
        [JsonIgnore]
        public string AESEncryptionKey { get { return CoreSettings.Config.AESEncryptionKey; } }
        [JsonIgnore]
        public Dictionary<string, string> WebApis { get { return CoreSettings.Config?.WebApi; } }
        [JsonIgnore]
        public Dictionary<string, string> CustomSettings { get { return CoreSettings.Config?.CustomSettings; } }
        #endregion

        #region Injection Services


        /// <summary>
        /// The Image manger service to get image sizes
        /// </summary>
        [JsonIgnore]
        protected IImageManager ImageManager
        {
            get
            {
                return (IImageManager)CoreDependencyService.GetService<IImageManager, ImageManager>(true);
            }
        }

        /// <summary>
        /// Service that provides network calls over http.
        /// </summary>
        /// <value>The http service.</value>
        [JsonIgnore]
        protected IHttpService HttpService
        {
            get
            {
                return (IHttpService)CoreDependencyService.GetService<IHttpService, HttpService>(true);
            }
        }

        /// <summary>
        /// Embedded file store that allow objects to be json serialized.
        /// </summary>
        /// <value>The file store.</value>
        [JsonIgnore]
        protected IFileStore FileStore
        {
            get
            {
                return (IFileStore)CoreDependencyService.GetService<IFileStore, FileStore>(true);
            }
        }

        /// <summary>
        /// Service that uses the OS account store to retrieve dictionary data
        /// </summary>
        /// <value>The account service.</value>
        [JsonIgnore]
        protected ISecureDataService SecureDataService
        {
            get
            {
                return (ISecureDataService)CoreDependencyService.GetService<ISecureDataService, SecureDataService>(true);
            }
        }

        /// <summary>
        /// AES encryption and Hash service.
        /// </summary>
        /// <value>The encryption service.</value>
        [JsonIgnore]
        protected IEncryptionService EncryptionService
        {
            get
            {
                return (IEncryptionService)CoreDependencyService.GetService<IEncryptionService, EncryptionService>(true);
            }
        }
        #endregion

        #region Dependencies

        /// <summary>
        /// DependencyService for IAudioPlayer.
        /// </summary>
        [JsonIgnore]
        public IAudioPlayer AudioPlayer
        {
            get { return DependencyService.Get<IAudioPlayer>(); }
        }

        /// <summary>
        /// DependencyService for IBlurOverlay.
        /// </summary>
        [JsonIgnore]
        public IBlurOverlay BlurOverlay
        {
            get { return DependencyService.Get<IBlurOverlay>(); }
        }
        /// <summary>
        /// DependencyService for ICalendarEvent.
        /// </summary>
        [JsonIgnore]
        public ICalendarEvent CalendarEvent
        {
            get { return DependencyService.Get<ICalendarEvent>(); }
        }

        /// <summary>
        /// DependencyService for ICalendarEvent.
        /// </summary>
        public IFileViewer FileViewer
        {
            get { return DependencyService.Get<IFileViewer>(); }
        }

        /// <summary>
        /// DependencyService for ICommunication.
        /// </summary>
        [JsonIgnore]
        public ITelephony Communication
        {
            get { return DependencyService.Get<ITelephony>(); }
        }

        /// <summary>
        /// DependencyService for IDialogPrompt.
        /// </summary>
        [JsonIgnore]
        public IDialogPrompt DialogPrompt
        {
            get { return DependencyService.Get<IDialogPrompt>(); }
        }

        /// <summary>
        /// DependencyService for INotificationManager.
        /// </summary>
        [JsonIgnore]
        public INotificationManager NotificationManager
        {
            get { return DependencyService.Get<INotificationManager>(); }
        }

        /// <summary>
        /// DependencyService for IMapNavigate.
        /// </summary>
        [JsonIgnore]
        public IMapNavigate MapNavigate
        {
            get { return DependencyService.Get<IMapNavigate>(); }
        }

        /// <summary>
        /// DependencyService for IOverlayDependency.
        /// </summary>
        [JsonIgnore]
        public IOverlayService OverlayService
        {
            get { return DependencyService.Get<IOverlayService>(); }
        }

        /// <summary>
        /// DependencyService for IViewStack.
        /// </summary>
        [JsonIgnore]
        public IViewStack ViewStack
        {
            get { return DependencyService.Get<IViewStack>(); }
        }

        #endregion

        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the navigation.
        /// </summary>
        /// <value>The navigation.</value>
        [JsonIgnore]
        public INavigation Navigation
        {
            get
            {
                if (Application.Current.MainPage is NavigationPage)
                {
                    return ((NavigationPage)Application.Current.MainPage).Navigation;
                }
                if(Application.Current.MainPage is TabbedPage)
                {
                    var tab = (TabbedPage)Application.Current.MainPage;
                    if (tab.CurrentPage is INavigation)
                        return ((NavigationPage)tab.CurrentPage).Navigation;
                    else
                        return null;
                }
                if(Application.Current.MainPage is FlyoutPage)
                {
                    var md = (FlyoutPage)Application.Current.MainPage;
                    if (md.Detail is NavigationPage)
                        return ((NavigationPage)md.Detail).Navigation;
                    else
                        return null;
                }

                return null;
            }
        }


        /// <summary>
        /// Broadcast message to all view model instances
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="obj">Object.</param>
        protected void SendViewMessage(string key, object obj)
        {
            CoreDependencyService.SendViewModelMessage(key, obj);
        }
        /// <summary>
        /// Broadcast message to a particular view model instance
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected void SendViewMessage<T>(string key, object obj) where T : CoreViewModel
        {
            CoreDependencyService.SendViewModelMessage<T>(key, obj);
        }

        protected bool IsEmtpyOrNull(params string[] properties)
        {
            foreach (var prop in properties)
            {
                if (string.IsNullOrEmpty(prop))
                    return true;
            }
            return false;
        }



        /// <summary>
        /// Method to receive viewmodel messages.  (NO BASE IMPLEMENTATION)
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="obj">Object.</param>
        public abstract void OnViewMessageReceived(string key, object obj);

        /// <summary>
        /// //false is default value when system call back press
        /// </summary>
        /// <returns></returns>
        public virtual bool OnBackButtonPressed()
        {
            //false is default value when system call back press
            return false;
        }

        /// <summary>
        /// called when page need override soft back button.  (NO BASE IMPLEMENTATION)
        /// </summary>
        public virtual void OnSoftBackButtonPressed() { }


        public virtual void OnInit() { }
        public virtual void OnRelease(bool includeEvents) { }
        /// <summary>
        /// Method to indicate that an event has been trigger show sliding panel if page is master detail
        /// </summary>
        public virtual void OnMasterDetailPresented() { }


    }

}

