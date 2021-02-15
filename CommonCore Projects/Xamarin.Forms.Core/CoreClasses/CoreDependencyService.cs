using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xamarin.Forms.Core
{
    public class CoreDependencyService
    {
        private static List<CoreViewModel> vmContainer = new List<CoreViewModel>();
        private static List<object> srvContainer = new List<object>();
        private static List<CoreBusiness> bllContainer = new List<CoreBusiness>();

        #region ViewModels

        public static bool IsRegistered<T>() where T : CoreViewModel => vmContainer.Count(x => x is T) != 0;

        public static List<CoreViewModel> GetAllViewModels()
        {
            return vmContainer;
        }
        public static T GetViewModel<T>(bool loadResources = false) where T : CoreViewModel
        {
            var vm = (T)vmContainer.FirstOrDefault(x => x is T);

            if (vm == null)
            {
                vm = (T)Activator.CreateInstance(typeof(T));
                vmContainer.Add(vm);

                if (loadResources)
                    vm.OnInit();
                return vm;
            }
            else
            {
                return vm;
            }

        }
        public static CoreViewModel GetViewModel(string vmName, bool loadResources = false)
        {
            if (!string.IsNullOrEmpty(vmName))
            {
                var vm = vmContainer.FirstOrDefault(x => x.GetType().FullName == vmName);

                if (vm == null)
                {
                    var assembly = Assembly.GetAssembly(typeof(ResourceLoader));
                    vm = (CoreViewModel)Activator.CreateInstance(assembly.GetType(vmName));
                    vmContainer.Add(vm);

                    if (loadResources)
                        vm.OnInit();
                    return vm;
                }
                else
                {
                    return vm;
                }
            }
            else
            {
                return null;
            }
        }
        public static void DisposeViewModel<T>() where T : CoreViewModel
        {
            var vm = (T)vmContainer.FirstOrDefault(x => x is T);
            if (vm != null)
            {
                vm.OnRelease(true);
                vmContainer.Remove(vm);
                vm = null;
            }
        }
        public static void SendViewModelMessage(string key, object obj)
        {
            foreach (var vm in vmContainer)
            {
                vm.OnViewMessageReceived(key, obj);
            }
        }
        public static void SendViewModelMessage<T>(string key, object obj) where T : CoreViewModel
        {
            var vm = (T)vmContainer.FirstOrDefault(x => x is T);
            if (vm != null)
                vm.OnViewMessageReceived(key, obj);
        }

        #endregion

        #region Services
        public static T GetService<T, K>(bool isSingleton = false) where K : class, T
        {
            var srv = (K)srvContainer.FirstOrDefault(x => x is K);

            if (srv == null)
            {
                srv = (K)Activator.CreateInstance(typeof(K));
                srvContainer.Add(srv);
                return (T)srv;
            }
            else
            {
                return (T)srv;
            }
        }

        public static void DeRegister<T>() where T : class
        {
            var srv = (T)srvContainer.FirstOrDefault(x => x is T);
            if (srv != null)
            {
                srvContainer.Remove(srv);
            }
        }

        public static void DisposeService<T>() where T : class
        {
            var srv = (T)srvContainer.FirstOrDefault(x => x is T);
            if (srv != null)
            {
                if (srv is IDisposable)
                {
                    ((IDisposable)srv).Dispose();
                }
                srvContainer.Remove(srv);
                srv = null;
            }
        }
        public static void DisposeAllServices()
        {
            for (int x = srvContainer.Count - 1; x > -1; x--)
            {
                var srv = srvContainer[x];
                if (srv is IDisposable)
                {
                    ((IDisposable)srv).Dispose();
                }
                srvContainer.Remove(srv);

            }
        }
        public static void ReleaseViewModelResources()
        {
            foreach (var vm in vmContainer)
            {
                vm.OnRelease(false);
            }
        }
        public static void InitViewModelResources()
        {
            foreach (var vm in vmContainer)
            {
                vm.OnInit();
            }
        }
        #endregion

        #region Business
        public static T GetBusinessLayer<T>() where T : CoreBusiness
        {
            var bll = (T)bllContainer.FirstOrDefault(x => x is T);

            if (bll == null)
            {
                bll = (T)Activator.CreateInstance(typeof(T));
                bllContainer.Add(bll);
                return bll;
            }
            else
            {
                return bll;
            }
        }
        public static void DisposeBusinessLayer<T>() where T : CoreBusiness
        {
            var bll = (T)bllContainer.FirstOrDefault(x => x is T);
            if (bll != null)
            {
                if (bll is IDisposable)
                {
                    ((IDisposable)bll).Dispose();
                }
                bllContainer.Remove(bll);
                bll = null;
            }
        }
        public static void DisposeAllBusinessLayers()
        {
            for (int x = bllContainer.Count - 1; x > -1; x--)
            {
                var bll = bllContainer[x];
                if (bll is IDisposable)
                {
                    ((IDisposable)bll).Dispose();
                }
                bllContainer.Remove(bll);

            }
        }
        #endregion

        #region Dependency
        public static T GetDependency<T>() where T : class
        {
            return DependencyService.Get<T>(DependencyFetchTarget.GlobalInstance);
        }
        #endregion

        public static void InvokeMasterDetailEvent()
        {
            foreach (var vm in vmContainer)
            {
                vm.OnMasterDetailPresented();
            }
        }
    }

}

