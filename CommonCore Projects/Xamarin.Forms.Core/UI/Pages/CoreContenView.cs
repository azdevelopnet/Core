using System;
namespace Xamarin.Forms.Core
{


    public class CoreContenView : ContentView
    {
        string viewModelName;

        /// <summary>
        /// Gets or sets the binding context based on the fully qualified name of the viewmodel.
        /// </summary>
        /// <value>The name of the view model.</value>
        public string ViewModelName
        {
            get
            {
                return viewModelName;
            }

            set
            {
                viewModelName = value;
                this.BindingContext = CoreDependencyService.GetViewModel(viewModelName);
            }
        }

        public object VM
        {
            get { return CoreDependencyService.GetViewModel(viewModelName); }
        }

    }

    public class CoreContentView<T> : ContentView
        where T : CoreViewModel
    {
        public T VM
        {
            get { return CoreDependencyService.GetViewModel<T>(true); }
        }

        public CoreContentView()
        {
            this.BindingContext = VM;
        }

    }
}
