using System;
namespace Xamarin.Forms.Core
{
    public class CoreMasterDetailPage<T> : MasterDetailPage
        where T : CoreViewModel
    {
        public T VM { get; set; }
        public CoreMasterDetailPage()
        {
            VM = CoreDependencyService.GetViewModel<T>(true);
            this.BindingContext = VM;
        }
    }


    public class CoreMasterDetailPage : MasterDetailPage
    {
        string viewModel;

        /// <summary>
        /// Gets or sets the binding context based on the fully qualified name of the viewmodel.
        /// </summary>
        /// <value>The name of the view model.</value>
        public string ViewModel
        {
            get
            {
                return viewModel;
            }

            set
            {
                viewModel = value;
                if (!string.IsNullOrEmpty(value))
                {
                    this.BindingContext = CoreDependencyService.GetViewModel(viewModel);
                }
            }
        }

        public CoreViewModel VM
        {
            get { return (CoreViewModel)CoreDependencyService.GetViewModel(viewModel); }
        }

    }
}
