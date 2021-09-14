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
    public class LineDetailViewModel : LineManagerViewModel
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

        public LineDetailViewModel()
        {
            
        }

    }
}

