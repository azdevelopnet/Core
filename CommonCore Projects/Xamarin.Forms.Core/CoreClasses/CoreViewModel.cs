using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xamarin.Forms.Core
{
    /// <summary>
    /// Observable view model.
    /// </summary>
    public abstract partial class CoreViewModel : BaseNotify
    {
        private bool isLoadingHUD;
        private bool isLoadingOverlay;

        public string LoadingMessageOverlay { get; set; }
        public string LoadingMessageHUD { get; set; }

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
        /// Backgrounding event timer that fires an event specified in the future on a repeating basis.
        /// </summary>
        /// <value>The background timer.</value>
        [JsonIgnore]
        protected IBackgroundTimer BackgroundTimer
        {
            get
            {
                return (IBackgroundTimer)CoreDependencyService.GetService<IBackgroundTimer, BackgroundTimer>(true);
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
        /// <value>The audio player.</value>
        [JsonIgnore]
        public IAudioPlayer AudioPlayer
        {
            get { return DependencyService.Get<IAudioPlayer>(); }
        }

        /// <summary>
        /// DependencyService for IBlurOverlay.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public IBlurOverlay BlurOverlay
        {
            get { return DependencyService.Get<IBlurOverlay>(); }
        }
        /// <summary>
        /// DependencyService for ICalendarEvent.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public ICalendarEvent CalendarEvent
        {
            get { return DependencyService.Get<ICalendarEvent>(); }
        }
        /// <summary>
        /// DependencyService for ICommunication.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public ITelephony Communication
        {
            get { return DependencyService.Get<ITelephony>(); }
        }

        /// <summary>
        /// DependencyService for IDialogPrompt.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public IDialogPrompt DialogPrompt
        {
            get { return DependencyService.Get<IDialogPrompt>(); }
        }
        /// <summary>
        /// DependencyService for ILocalNotify.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public ILocalNotify LocalNotify
        {
            get { return DependencyService.Get<ILocalNotify>(); }
        }
        /// <summary>
        /// DependencyService for IMapNavigate.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public IMapNavigate MapNavigate
        {
            get { return DependencyService.Get<IMapNavigate>(); }
        }
        /// <summary>
        /// DependencyService for IOverlayDependency.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public IOverlayDependency OverlayDependency
        {
            get { return DependencyService.Get<IOverlayDependency>(); }
        }
        /// <summary>
        /// DependencyService for IProgressIndicator.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public IProgressIndicator ProgressIndicator
        {
            get { return DependencyService.Get<IProgressIndicator>(); }
        }
        /// <summary>
        /// DependencyService for ISnackBar.
        /// </summary>
        /// <value>The audio player.</value>
        [JsonIgnore]
        public ISnackBar SnackBar
        {
            get { return DependencyService.Get<ISnackBar>(); }
        }
        /// <summary>
        /// DependencyService for IViewStack.
        /// </summary>
        /// <value>The audio player.</value>
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
            get { return CoreSettings.AppNav; }
            set { CoreSettings.AppNav = value; }
        }

        [JsonIgnore]
        public bool IsLoadingOverlay
        {
            get
            {
                return isLoadingOverlay;
            }

            set
            {

                isLoadingOverlay = value;

                //Ensure that this action is performed on the UI thread
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (value)
                    {
                        var color = Color.FromHex(CoreStyles.OverlayColor);
                        OverlayDependency.ShowOverlay(LoadingMessageOverlay, color, CoreStyles.OverlayOpacity);
                    }
                    else
                    {
                        OverlayDependency.HideOverlay();
                    }
                });


            }
        }

        [JsonIgnore]
        public bool IsLoadingHUD
        {
            get
            {
                return isLoadingHUD;
            }

            set
            {
                if (isLoadingHUD != value)
                {
                    isLoadingHUD = value;

                    //Ensure that this action is performed on the UI thread
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (value)
                        {
                            ProgressIndicator.ShowProgress(LoadingMessageHUD);
                        }
                        else
                        {
                            ProgressIndicator.Dismiss();
                        }
                    });
                }
            }
        }


        protected void ShowNotification(LocalNotification notification)
        {
            LocalNotify.RequestPermission((permit) =>
            {
                if (permit)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LocalNotify.Show(notification);
                    });

                }
            });

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

