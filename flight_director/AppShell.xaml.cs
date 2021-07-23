using Xamarin.Forms;
using flight_director.Views;

namespace flight_director
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EnterNewLine), typeof(EnterNewLine));
        }

    }
}
