
using flight_director.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace flight_director.Views
{
    public partial class FligthLineManager : ContentPage
    {
        public FligthLineManager()
        {
            InitializeComponent();
            BindingContext = new LineManagerViewModel();

        }
    }
}