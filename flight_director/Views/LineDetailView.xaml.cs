using flight_director.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flight_director.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace flight_director.Views
{
    [QueryProperty(nameof(LineID),nameof(LineID))]
    public partial class LineDetailView : ContentPage
    {
        public string LineID { get; set; }
        public LineDetailView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing();
            int.TryParse(LineID, out var result);
            BindingContext = await FlightLineService.GetLine(result);
        }
    }
}