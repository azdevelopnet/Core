using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Forms.Core
{
    public class CoreDependencyService
    {
        private static List<string> vmContainer = new List<string>();
        private static List<string> srvContainer = new List<string>();
        private static List<string> cvtrContainer = new List<string>();
        private static List<string> bllContainer = new List<string>();
        private static List<string> singletonContainer = new List<string>();
        /// <summary>
        /// InjectionManager has view models
        /// </summary>
        /// <value><c>true</c> if has view models; otherwise, <c>false</c>.</value>
        public static bool HasViewModels => vmContainer.Count() > 0 ? true : false;
        /// <summary>
        /// ViewModel has been registered
        /// </summary>
        /// <returns><c>true</c>, if registered was ised, <c>false</c> otherwise.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool IsRegistered<T>() where T : CoreViewModel => vmContainer.Count(x => x == typeof(T).FullName) != 0;

        /// <summary>
        /// Get all view model instances
        /// </summary>
        /// <returns>The all view models.</returns>
        public static List<CoreViewModel> GetAllViewModels()
        {
            var lst = new List<CoreViewModel>();
            foreach (var name in vmContainer)
            {
                lst.Add((CoreViewModel)GetObjectByName(name));
            }
            return lst;
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns>The view model.</returns>
        /// <param name="loadResources">If set to <c>true</c> load resources.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetViewModel<T>(bool loadResources = false) where T : CoreViewModel
        {

            if (!vmContainer.Any(x => x == typeof(T).FullName))
            {
                DependencyService.Register<T>();
                vmContainer.Add(typeof(T).FullName);
                var vm = DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);
                if (loadResources)
                    vm.OnInit();
                return vm;
            }
            else
            {
                return DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);
            }

        }


        public static T GetDependency<T>() where T: class
        {
            return DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);
        }


        public static T GetBusinessLayer<T>() where T : CoreBusiness
        {
            if (!bllContainer.Any(x => x == typeof(T).FullName))
            {
                DependencyService.Register<T>();
                bllContainer.Add(typeof(T).FullName);
            }
            return DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);
        }

        public static T GetSingletonObject<T>() where T : class
        {
            if (!singletonContainer.Any(x => x == typeof(T).FullName))
            {
                DependencyService.Register<T>();
                singletonContainer.Add(typeof(T).FullName);
            }
            return DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);
        }

        /// <summary>
        /// Gets the view model by fully qualified name.
        /// </summary>
        /// <returns>The view model.</returns>
        /// <param name="vmName">Vm name.</param>
        /// <param name="loadResources">If set to <c>true</c> load resources.</param>
        public static CoreViewModel GetViewModel(string vmName, bool loadResources = false)
        {
            if (!string.IsNullOrEmpty(vmName))
            {
                if (!vmContainer.Any(x => x == vmName))
                {
                    RegisterObjectByName(vmName);
                    vmContainer.Add(vmName);
                }
                var vm = (CoreViewModel)GetObjectByName(vmName);

                if (loadResources)
                    vm.OnInit();
                return vm;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Invoke ReleaseResources method on all ViewModels
        /// </summary>
        /// <param name="ignoreCaller">If set to <c>true</c> ignoreCaller caller.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ReleaseViewModelResource<T>(bool ignoreCaller = false) where T : CoreViewModel
        {
            var caller = typeof(T).FullName;
            foreach (var name in vmContainer)
            {
                var vm = ((CoreViewModel)GetObjectByName(name));
                if (!ignoreCaller && !name.Equals(caller))
                {
                    vm.OnRelease(false);
                }

            }
        }

        /// <summary>
        /// Disposes the view model, makes it null and removes it from the registration.
        /// </summary>
        public static void DisposeViewModel<T>() where T: CoreViewModel
        {
            var vmName = typeof(T).FullName;
            if (vmContainer.Any(x=>x == vmName)) 
            {
                var vm = (CoreViewModel)GetObjectByName(vmName);
                vm.OnRelease(true);
                vm = null;
                vmContainer.Remove(vmName);
            }
        }

        /// <summary>
        /// Releases all resources on all VM instances.
        /// </summary>
        public static void ReleaseViewModelResources()
        {
            foreach (var name in vmContainer)
            {
                var vm = ((CoreViewModel)GetObjectByName(name));
                vm.OnRelease(false);
            }
        }

        /// <summary>
        /// Reloads the
        /// </summary>
        public static void InitViewModelResources()
        {
            foreach (var name in vmContainer)
            {
                var vm = ((CoreViewModel)GetObjectByName(name));
                vm.OnInit();
            }
        }


        /// <summary>
        /// Reloads all resources on all VM instances.
        /// </summary>
        public static void InitViewModelsResource<T>() where T : CoreViewModel
        {
            var vmName = typeof(T).FullName;
            if (!vmContainer.Any(x => x == vmName))
            {
                RegisterObjectByName(vmName);
                vmContainer.Add(vmName);
            }
            var vm = (CoreViewModel)GetObjectByName(vmName);
            vm.OnInit();
        }

        /// <summary>
        /// Calls the dispose method on all services
        /// </summary>
        public static void DisposeServices()
        {
            foreach (var name in srvContainer)
            {
                var obj = GetObjectByName(name);
                if (obj is IDisposable)
                {
                    ((IDisposable)obj).Dispose();
                }
            }
        }

        /// <summary>
        /// Sends the view model message.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="obj">Object.</param>
        public static void SendViewModelMessage(string key, object obj)
        {
            foreach (var name in vmContainer)
            {
                ((CoreViewModel)GetObjectByName(name)).OnViewMessageReceived(key, obj);
            }
        }

        public static void InvokeMasterDetailEvent() 
        {
            foreach (var name in vmContainer)
            {
                ((CoreViewModel)GetObjectByName(name)).OnMasterDetailPresented();
            }
        }

        /// <summary>
        /// Sends a message to a specific view model.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SendViewModelMessage<T>(string key, object obj) where T : CoreViewModel
        {
            if (vmContainer.Any(x => x == typeof(T).FullName))
                DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance).OnViewMessageReceived(key, obj);
        }

        /// <summary>
        /// Gets and registers a service as a singleton.
        /// </summary>
        /// <returns>The service.</returns>
        /// <param name="isSingleton">If set to <c>true</c> is singleton.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="K">The 2nd type parameter.</typeparam>
        public static T GetService<T, K>(bool isSingleton = false) where K : class, T
        {
            if (!srvContainer.Any(x => x == typeof(K).FullName))
            {
                DependencyService.Register<K>();
                srvContainer.Add(typeof(T).FullName);
            }

            var iSrv = default(T);
            if (isSingleton)
            {
                iSrv = (T)DependencyService.Get<K>(DependencyFetchTarget.GlobalInstance);
            }
            else
            {
                iSrv = (T)DependencyService.Get<K>(DependencyFetchTarget.NewInstance);
            }
            return iSrv;
        }

        /// <summary>
        /// Gets and registers a converter as a singleton.
        /// </summary>
        /// <returns>The converter.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetConverter<T>() where T : class, IValueConverter
        {
            if (!cvtrContainer.Any(x => x == typeof(T).FullName))
            {
                DependencyService.Register<T>();
                cvtrContainer.Add(typeof(T).FullName);
            }
            var converter = (T)DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);

            return converter;
        }

        public static void RegisterObjectByName(string typeName)
        {
            var method = typeof(DependencyClarifier).GetMethod("Register");
            var t = Type.GetType(typeName);
            var genericMethod = method.MakeGenericMethod(t);
            genericMethod.Invoke(null, null);
        }

        public static object GetObjectByName(string typeName)
        {
            var method = typeof(DependencyClarifier).GetMethod("Get");
            var t = Type.GetType(typeName);
            var genericMethod = method.MakeGenericMethod(t);
            return genericMethod.Invoke(null, null);
        }

    }

    /// <summary>
    /// Dependency clarifier helps resolve ambiguous match exceptions on static calls to the DependencyService.
    /// </summary>
    public class DependencyClarifier
    {
        public static void Register<T>() where T : class
        {
            DependencyService.Register<T>();
        }
        public static object Get<T>() where T : class
        {
            return DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);
        }
    }
}

