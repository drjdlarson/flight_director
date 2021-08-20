using MvvmHelpers.Commands;
using flight_director.Services;
using flight_director.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace flight_director.ViewModels
{

    public class AddLineViewModel : LineManagerViewModel
    {
        private int id;
        public int ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private double startlat, startlon, startalt, endlat, endlon, endalt, avgele;
        public double StartLat
        {
            get => startlat;
            set => SetProperty(ref startlat, value);
        }
        public double StartLon
        {
            get => startlon;
            set => SetProperty(ref startlon, value);
        }
        public double StartAlt
        {
            get => startalt;
            set => SetProperty(ref startalt, value);
        }
        public double EndLat
        {
            get => endlat;
            set => SetProperty(ref endlat, value);
        }
        public double EndLon
        {
            get => endlon;
            set => SetProperty(ref endlon, value);
        }
        public double EndAlt
        {
            get => endalt;
            set => SetProperty(ref endalt, value);
        }
        public double AvgEle
        {
            get => avgele;
            set => SetProperty(ref avgele, value);
        }

        public AsyncCommand SaveLine { get; }

        public AddLineViewModel()
        {
            SaveLine = new AsyncCommand(OnSave);
        }

        async Task OnSave()
        {
            if (id == 0 || startlat == 0 || startlon == 0 || endlat == 0 || endlon == 0 || startalt == 0 || endalt == 0 || avgele ==0)
            {
                return;
            }

            await FlightLineService.AddNewLine(id, startlat, startlon, startalt, endlat, endlon, endalt, avgele);
            await Shell.Current.GoToAsync ("..");

        }

    }
}

