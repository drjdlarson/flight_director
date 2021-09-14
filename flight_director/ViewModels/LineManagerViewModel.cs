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
using Xamarin.Essentials;
using System.IO;
using System.Text;


namespace flight_director.ViewModels
{
    public class LineManagerViewModel : ViewModelBase
    {
        public ObservableRangeCollection<FlightLine> FlightLine { get; set; }
        public AsyncCommand AddFligthLine { get; }
        public AsyncCommand LoadFligthLine { get; }
        public AsyncCommand<FlightLine> RemoveFlightLine { get; }

        public AsyncCommand RefreshLine { get; }

        public AsyncCommand<FlightLine> SelectLine { get; }

        public LineManagerViewModel()
        {
            FlightLine = new ObservableRangeCollection<FlightLine>();  
            AddFligthLine = new AsyncCommand(OnAddLine);
            LoadFligthLine = new AsyncCommand(OnLoadLine);
            RemoveFlightLine = new AsyncCommand<FlightLine>(OnRemove);
            RefreshLine = new AsyncCommand(OnRefresh);
            SelectLine = new AsyncCommand<FlightLine>(OnSelect);
        }

        async Task OnAddLine()
            {
            await Shell.Current.GoToAsync(nameof(EnterNewLine));
            }

        async Task OnLoadLine()
        {
            var result = await FilePicker.PickAsync();   
            if (result == null)
            {
                return;
            }
            string path = result.FullPath;
            string list_content = File.ReadAllText(path);
            char[] line_sep = new char[] { '\r', '\n' };
            string[] line_list = list_content.Split(line_sep,StringSplitOptions.RemoveEmptyEntries);
            int id;
            double startlat, startlon, startalt, endlat, endlon, endalt, avgele;
            string[] line_param;
            await FlightLineService.RemoveAllLine();
            for (int i = 0; i<line_list.Length; i++)
            {
                id = i + 1;
                line_param = line_list[i].Split(',');
                Double.TryParse (line_param[0], out startlat);
                Double.TryParse(line_param[1], out startlon);
                Double.TryParse(line_param[2], out startalt);
                Double.TryParse(line_param[3], out endlat);
                Double.TryParse(line_param[4], out endlon);
                Double.TryParse(line_param[5], out endalt);
                Double.TryParse(line_param[6], out avgele);
                await FlightLineService.AddNewLine(id, startlat, startlon, startalt, endlat, endlon, endalt, avgele);
            }

            await OnRefresh();
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

        async Task OnSelect(FlightLine flightline)
        {
            var route = $"{nameof(LineDetailView)}?LineID={flightline.ID}";
            await Shell.Current.GoToAsync(route);
        }

    }
}
