using Xamarin.Forms;
using Xamarin.Essentials;

namespace flight_director
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            DeviceDisplay.KeepScreenOn = true;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
