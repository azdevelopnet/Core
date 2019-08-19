using System;
namespace Xamarin.Forms.Core
{
    public abstract class CorePage<T> : BasePages
		where T : CoreViewModel
	{
        
        public T VM
        {
            get { return CoreDependencyService.GetViewModel<T>(true); }
        }

		public CorePage()
		{
            
			this.BindingContext = VM;
            if (VM != null)
            {
                if (string.IsNullOrEmpty(VM.PageTitle))
                    VM.PageTitle = this.Title;
            }
            this.SetBinding(ContentPage.TitleProperty, "PageTitle");

		}

	}


    public abstract class CorePage : BasePages
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
                    if (string.IsNullOrEmpty(VM.PageTitle))
                        VM.PageTitle = this.Title;
                }
            }
        }

        public CoreViewModel VM
        {
            get { return (CoreViewModel)CoreDependencyService.GetViewModel(viewModel); }
        }

        public CorePage()
        {
            this.SetBinding(ContentPage.TitleProperty, "PageTitle");
        }


    }
}

