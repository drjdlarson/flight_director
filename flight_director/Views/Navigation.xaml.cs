using flight_director.ViewModels;
using Xamarin.Forms;

namespace flight_director.Views
{
    public partial class Navigation : ContentPage
    {
        public Navigation()
        {
            InitializeComponent();
            BindingContext = new NavigationViewModel();

        }

        

    }
}