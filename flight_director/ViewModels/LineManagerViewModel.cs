using MvvmHelpers;
using MvvmHelpers.Commands;
using flight_director.Services;
using flight_director.Models;
using flight_director.Views;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using SQLite;
using Command = MvvmHelpers.Commands.Command;

namespace flight_director.ViewModels
{
    public class LineManagerViewModel : ViewModelBase
    {
        public ObservableRangeCollection<FlightLine> FlightLine { get; set; }
        public AsyncCommand AddFligthLine { get; }
        public AsyncCommand<FlightLine> RemoveFlightLine { get; }

        public AsyncCommand RefreshLine { get; }

        public LineManagerViewModel()
        {
            FlightLine = new ObservableRangeCollection<FlightLine>();  
            AddFligthLine = new AsyncCommand(OnAddLine);
            RemoveFlightLine = new AsyncCommand<FlightLine>(OnRemove);
            RefreshLine = new AsyncCommand(OnRefresh);
        }

        async Task OnAddLine()
            {
            await Shell.Current.GoToAsync(nameof(EnterNewLine));
            }

        async Task OnRemove(FlightLine flightline)
        {
            await FlightLineService.RemoveLine(flightline.ID);
            await OnRefresh();
        }

        async Task OnRefresh()
        {
            IsBusy = true;
            await Task.Delay(500);
            FlightLine.Clear();
            var flightline = await FlightLineService.GetLine();
            FlightLine.AddRange(flightline);
            IsBusy = false;
        }

    }
}
