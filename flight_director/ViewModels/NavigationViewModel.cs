using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using SQLite;
using SQLitePCL;
using flight_director.ViewModels;
using flight_director.Models;
using flight_director.Services;
using Xamarin.Forms;
using static System.Math;
using Command = Xamarin.Forms.Command;
using flight_director.Views;

[assembly: Dependency(typeof(FlightLineService))]

namespace flight_director.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        // UI Bindings
        private string version = AppInfo.VersionString;
        public string Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }

        // Enable or disable the flyto button
        private bool enableflyto = true;
        public bool EnableFlyTo
        {
            get => enableflyto;
            set => SetProperty(ref enableflyto, value);
        }

        // Not sure
        private double deviationoffset = 0;
        public double DeviationOffset
        {
            get => deviationoffset;
            set => SetProperty(ref deviationoffset, value);
        }

        // Abort all line flag
        private bool abortall = false;
        public bool AbortAll
        {
            get => abortall;
            set => SetProperty(ref abortall, value);
        }

        // Abort only current line flag
        private bool abortcur = false;
        public bool AbortCur
        {
            get => abortcur;
            set => SetProperty(ref abortcur, value);
        }

        // Flag that prevline is pressed
        private bool prevline = false;
        public bool PrevLine
        {
            get => prevline;
            set => SetProperty(ref prevline, value);
        }

        // Need checking
        private double heading = 0;
        public double Heading
        {
            get => heading;
            set => SetProperty(ref heading, value);
        }

        // Compass card rotation
        private double headingcomp = 0;
        public double HeadingComp
        {
            get => headingcomp;
            set => SetProperty(ref headingcomp, value);
        }

        // Heading numerical readout
        private int headingdisp = 0;
        public int HeadingDisp
        {
            get => headingdisp;
            set => SetProperty(ref headingdisp, value);
        }
        
        // Need checking
        private int lineid = 0;
        public int LineID
        {
            get => lineid;
            set => SetProperty(ref lineid, value);
        }

        // Status text
        private string status_text = "Stand By";
        public string Status_Text
        {
            get => status_text;
            set => SetProperty(ref status_text, value);
        }

        private string statuscolor = "White";
        public string StatusColor
        {
            get => statuscolor;
            set => SetProperty(ref statuscolor, value);
        }

        private string status_back_color = "Gray";
        public string Status_Back_Color
        {
            get => status_back_color;
            set => SetProperty(ref status_back_color, value);
        }

        private string lineseq = "";
        public string LineSeq
        {
            get => lineseq;
            set => SetProperty(ref lineseq, value);
        }

        private int distrem = 0;
        public int DistRem
        {
            get => distrem;
            set => SetProperty(ref distrem, value);
        }

        private string distremdisp = "Dst Rem: ";
        public string DistRemDisp
        {
            get => distremdisp;
            set => SetProperty(ref distremdisp, value);
        }

        private double course = 0;
        public double Course
        {
            get => course;
            set => SetProperty(ref course, value);
        }

        private double currentlat = 0;
        public double CurrentLat
        {
            get => currentlat;
            set => SetProperty(ref currentlat, value);
        }

        private double currentlon = 0;
        public double CurrentLon
        {
            get => currentlon;
            set => SetProperty(ref currentlon, value);
        }

        private double currentalt = 0;
        public double CurrentAlt
        {
            get => currentalt;
            set => SetProperty(ref currentalt, value);
        }

        private double prevlat = 0;
        public double PrevLat
        {
            get => prevlat;
            set => SetProperty(ref prevlat, value);
        }

        private double prevlon = 0;
        public double PrevLon
        {
            get => prevlon;
            set => SetProperty(ref prevlon, value);
        }

        private double targetlat = 0;
        public double TargetLat
        {
            get => targetlat;
            set => SetProperty(ref targetlat, value);
        }

        private double targetlon = 0;
        public double TargetLon
        {
            get => targetlon;
            set => SetProperty(ref targetlon, value);
        }

        private double targetalt = 0;
        public double TargetAlt
        {
            get => targetalt;
            set => SetProperty(ref targetalt, value);
        }

        private double deviation = 0;
        public double Deviation
        {
            get => deviation;
            set => SetProperty(ref deviation, value);
        }

        private double deviationnum = 0;
        public double DeviationNum
        {
            get => deviationnum;
            set => SetProperty(ref deviationnum, value);
        }

        private string deviationdisp = "Dev:";
        public string DeviationDisp
        {
            get => deviationdisp;
            set => SetProperty(ref deviationdisp, value);
        }

        private double transx = 0;
        public double TransX
        {
            get => transx;
            set => SetProperty(ref transx, value);
        }

        private double transy = 0;
        public double TransY
        {
            get => transy;
            set => SetProperty(ref transy, value);
        }

        private string leg_groundcourse_text = "Crs";
        public string Leg_GroundCourse_Text
        {
            get => leg_groundcourse_text;
            set => SetProperty(ref leg_groundcourse_text, value);
        }

        private string accdisp = "Acc:";
        public string AccDisp
        {
            get => accdisp;
            set => SetProperty(ref accdisp, value);
        }

        private int acc=0;
        public int Acc
        {
            get => acc;
            set => SetProperty(ref acc, value);
        }

        //transparency of dashed line (from current to leadin)
        private int dashcolor = 0;
        public int DashColor
        {
            get => dashcolor;
            set => SetProperty(ref dashcolor, value);
        }

        // transparency of line from leadin to start of leg
        private int leadincolor = 0;
        public int LeadInColor
        {
            get => leadincolor;
            set => SetProperty(ref leadincolor, value);
        }

        private double vertdeviationnum = 0;
        public double VertDeviationNum
        {
            get => vertdeviationnum;
            set => SetProperty(ref vertdeviationnum, value);
        }

        private int vertdeviationdisp = 0;
        public int VertDeviationDisp
        {
            get => vertdeviationdisp;
            set => SetProperty(ref vertdeviationdisp, value);
        }

        private double vertdeviation = 0;
        public double VertDeviation
        {
            get => vertdeviation;
            set => SetProperty(ref vertdeviation, value);
        }

        private double x_start = 0;
        public double X_Start
        {
            get => x_start;
            set => SetProperty(ref x_start, value);
        }

        private double y_start = 0;
        public double Y_Start
        {
            get => y_start;
            set => SetProperty(ref y_start, value);
        }
        private double x_end = 0;
        public double X_End
        {
            get => x_end;
            set => SetProperty(ref x_end, value);
        }
        private double y_end = 0;
        public double Y_End
        {
            get => y_end;
            set => SetProperty(ref y_end, value);
        }

        private double x_leadin = 0;
        public double X_Leadin
        {
            get => x_leadin;
            set => SetProperty(ref x_leadin, value);
        }
        private double y_leadin = 0;
        public double Y_Leadin
        {
            get => y_leadin;
            set => SetProperty(ref y_leadin, value);
        }

        private int coursedes = 0;
        public int CourseDes
        {
            get => coursedes;
            set => SetProperty(ref coursedes, value);
        }

        private double mapscale = 20;
        public double MapScale
        {
            get => mapscale;
            set => SetProperty(ref mapscale, value);
        }

        private double ref_scale = 125 / (10);
        public double Ref_Scale
        {
            get => ref_scale;
            set => SetProperty(ref ref_scale, value);
        }

        private bool isreversed = false;
        public bool IsReversed
        {
            get => isreversed;
            set => SetProperty(ref isreversed, value);
        }

        private bool is_sequence = false;
        public bool Is_Sequenced
        {
            get => is_sequence;
            set => SetProperty(ref is_sequence, value);
        }

        // Variable to debug
        private double debug = 0;
        public double Debug
        {
            get => debug;
            set => SetProperty(ref debug, value);
        }

        private string ref_scale_disp = "NM";
        public string Ref_Scale_Disp
        {
            get => ref_scale_disp;
            set => SetProperty(ref ref_scale_disp, value);
        }

        // General Settings
        public  int WPRadius
        {
            get => Preferences.Get(nameof(WPRadius),1000);
            set
            {
                Preferences.Set(nameof(WPRadius), value);
                OnPropertyChanged(nameof(WPRadius));
            }
        }

        public int FeetperBar
        {
            get => Preferences.Get(nameof(FeetperBar), 600);
            set
            {
                Preferences.Set(nameof(FeetperBar), value);
                OnPropertyChanged(nameof(FeetperBar));
            }
        }
        public int FeetperBarPrecise
        {
            get => Preferences.Get(nameof(FeetperBarPrecise), 10);
            set
            {
                Preferences.Set(nameof(FeetperBarPrecise), value);
                OnPropertyChanged(nameof(FeetperBarPrecise));
            }
        }

        public double HorBarUnit
        {
            get => Preferences.Get(nameof(HorBarUnit), 20.0);
            set
            {
                Preferences.Set(nameof(HorBarUnit), value);
                OnPropertyChanged(nameof(HorBarUnit));
            }
        }

        public double VerBarUnit
        {
            get => Preferences.Get(nameof(VerBarUnit), 30.0);
            set
            {
                Preferences.Set(nameof(VerBarUnit), value);
                OnPropertyChanged(nameof(VerBarUnit));
            }
        }

        public int FeetperVertBar
        {
            get => Preferences.Get(nameof(FeetperVertBar), 200);
            set
            {
                Preferences.Set(nameof(FeetperVertBar), value);
                OnPropertyChanged(nameof(FeetperVertBar));
            }
        }

        public int HeadingThreshold
        {
            get => Preferences.Get(nameof(HeadingThreshold), 3);
            set
            {
                Preferences.Set(nameof(HeadingThreshold), value);
                OnPropertyChanged(nameof(HeadingThreshold));
            }
        }

        public int AntennaAngle
        {
            get => Preferences.Get(nameof(AntennaAngle), 0);
            set
            {
                Preferences.Set(nameof(AntennaAngle), value);
                OnPropertyChanged(nameof(AntennaAngle));
            }
        }
        public int PlaneXOffset
        {
            get => Preferences.Get(nameof(PlaneXOffset), 300);
            set
            {
                Preferences.Set(nameof(PlaneXOffset), value);
                OnPropertyChanged(nameof(PlaneXOffset));
            }
        }
        public int PlaneYOffset
        {
            get => Preferences.Get(nameof(PlaneYOffset), 342);
            set
            {
                Preferences.Set(nameof(PlaneYOffset), value);
                OnPropertyChanged(nameof(PlaneYOffset));
            }
        }

        public int LeadIn
        {
            get => Preferences.Get(nameof(LeadIn), 0);
            set
            {
                Preferences.Set(nameof(LeadIn), value);
                OnPropertyChanged(nameof(LeadIn));
            }
        }


        // Some functions
        public double Rad2Deg(double rad)
        {
            return rad * 180 / Math.PI;
        }

        public double Deg2Rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public double CalcCourse_rad(double start_lat_deg, double start_lon_deg, double end_lat_deg, double end_lon_deg)
        {
            double dL = Deg2Rad(end_lon_deg - start_lon_deg);
            double x = Sin(dL) * Cos(Deg2Rad(end_lat_deg));
            double y = Cos(Deg2Rad(start_lat_deg)) * Sin(Deg2Rad(end_lat_deg)) - Sin(Deg2Rad(start_lat_deg)) * Cos(Deg2Rad(end_lat_deg)) * Cos(dL);
            double bearing_rad = Atan2(x, y);
            return wrapTo2pi(bearing_rad);

        }

        public double wrapTo2pi (double angle_rad)
        {
            return (angle_rad + 2 * PI) % (2 * PI);
        }

        public double translate_lat_rad (double start_lat_deg, double translate_dist_ft, double translate_dir_rad, double R_E)
        {
            double angular_dist_rad = translate_dist_ft / R_E;
            double start_lat_rad = Deg2Rad(start_lat_deg);
            return Asin( Sin(start_lat_rad) * Cos (angular_dist_rad) + Cos(start_lat_rad) * Sin(angular_dist_rad) * Cos(translate_dir_rad));
        }
        public double translate_lon_rad(double start_lat_deg, double start_lon_deg, double end_lat_deg, double translate_dist_ft, double translate_dir_rad, double R_E)
        {
            double angular_dist_rad = translate_dist_ft / R_E;
            double start_lat_rad = Deg2Rad(start_lat_deg);
            double end_lat_rad = Deg2Rad(end_lat_deg);
            double start_lon_rad = Deg2Rad(start_lon_deg);
            return start_lon_rad + Atan2(Sin(translate_dir_rad) * Sin(angular_dist_rad) * Cos(start_lat_rad), Cos(angular_dist_rad) - Sin(start_lat_rad) * Sin(end_lat_rad));
        }



        public NavigationViewModel()
        {
            Flyto = new AsyncCommand(Navigate);
            AbortNav = new Command(CancleNav);
            ToSettings = new AsyncCommand(GotoSettings);
            Skip = new Command(SkipNav);
            Prev = new Command(PrevNav);
            ZoomIn = new Command(zoom_in);
            ZoomOut = new Command(zoom_out);
        }

        public AsyncCommand ToSettings { get; }
        public AsyncCommand Flyto { get; }
        public ICommand AbortNav { get; }
        public ICommand Skip { get; }
        public ICommand Prev { get; }

        public ICommand ZoomIn { get; }
        public ICommand ZoomOut { get; }

        void zoom_in ()
        {
            if (MapScale > 2)
            { MapScale = MapScale - 2; }
            else { MapScale = MapScale; }
            Ref_Scale = 125 * MapScale / 6076.12;
            Ref_Scale_Disp = string.Format("{0:N2} NM",Ref_Scale);
        }

        void zoom_out()
        {
            MapScale = MapScale + 2;
            Ref_Scale = 125 * MapScale /6076.12;
            Ref_Scale_Disp = string.Format("{0:N2} NM", Ref_Scale);
        }

        void SkipNav()
        {
            AbortCur = true;
        }

        void PrevNav()
        {
            AbortCur = true;
            PrevLine = true;
        }

        void CancleNav()
        {
            AbortCur = true;
            AbortAll = true;
        }

        async Task GotoSettings()
        {
            await Shell.Current.GoToAsync(nameof(Settings));
        }

        async Task LineNavigate (string LineName)
        {
            const int R_e = 3963 * 5280; //Earth radius in feet

            // Get Line Number and check reverse command
            int LineNo = 0;
            if (LineName.EndsWith("*"))
            {
                IsReversed = true;
                 LineNo = Int16.Parse(LineName.TrimEnd('*'));
            }
            else
            { 
               LineNo = Int16.Parse(LineName);
               IsReversed = false;

            } 

            // Test GPS 
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1));
            var cur_pos = await Geolocation.GetLocationAsync(request);
            if (cur_pos == null)
            {
                Status_Text = "No GPS";
                StatusColor = "Red";
                EnableFlyTo = true;
                return;
            }

            // Get current line params
            var cur_line = await FlightLineService.GetLine(LineNo);
            if (cur_line == null) //Check if the entered line is valid (id < total number of line in database)
            {
                Status_Text = "Invalid Line";
                StatusColor = "Red";
                EnableFlyTo = true;
                return;
            }
            
            // Calculate leg ground course depending on direction
            double target_groundcourse_rad = CalcCourse_rad(cur_line.StartLat, cur_line.StartLon, cur_line.EndLat, cur_line.EndLon);
            double Start_Lat_deg = cur_line.StartLat;
            double Start_Lon_deg = cur_line.StartLon;
            double End_Lat_deg = cur_line.EndLat;
            double End_Lon_deg = cur_line.EndLon;
            double avg_alt = (cur_line.StartAlt + cur_line.EndAlt) / 2;

            if (IsReversed == true)
            {
                target_groundcourse_rad = CalcCourse_rad(cur_line.EndLat, cur_line.EndLon, cur_line.StartLat, cur_line.StartLon);
                Start_Lat_deg = cur_line.EndLat;
                Start_Lon_deg = cur_line.EndLon;
                End_Lat_deg = cur_line.StartLat;
                End_Lon_deg = cur_line.StartLon;
            }

            // Convert start coordinates to radian 
            double Start_Lat_rad = Deg2Rad(Start_Lat_deg);
            double Start_Lon_rad = Deg2Rad(Start_Lon_deg);
            double End_Lat_rad = Deg2Rad(End_Lat_deg);
            double End_Lon_rad = Deg2Rad(End_Lon_deg);

            // Calculate length of line (Is this neccesary)
            double line_length_mile = 5280 * Location.CalculateDistance(cur_line.StartLat, cur_line.StartLon, cur_line.EndLat, cur_line.EndLon, DistanceUnits.Miles);


            Leg_GroundCourse_Text = $"Crs: {(int)Rad2Deg(target_groundcourse_rad)}";
            
            // Calc offset lat lon with antenna deviation
            double antenna_offset_dist_ft = (double)(avg_alt - cur_line.AvgEle) * Tan(Deg2Rad(AntennaAngle));       //Left offset is negative
            double offset_dir_rad = wrapTo2pi(target_groundcourse_rad + PI / 2);

            double offset_start_lat_rad = translate_lat_rad(Start_Lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_start_lon_rad = translate_lon_rad(Start_Lat_deg, Start_Lon_deg, Rad2Deg(offset_start_lat_rad), antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_end_lat_rad = translate_lat_rad(End_Lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_end_lon_rad = translate_lon_rad(End_Lat_deg, End_Lon_deg, Rad2Deg(offset_end_lat_rad), antenna_offset_dist_ft, offset_dir_rad, R_e);

            // Leadin coordinates calculation
            double leading_dir_rad = wrapTo2pi(target_groundcourse_rad + PI);

            double leadin_lat_rad = translate_lat_rad(Start_Lat_deg, LeadIn, leading_dir_rad, R_e);
            double leadin_lon_rad = translate_lon_rad(Start_Lat_deg, Start_Lon_deg, Rad2Deg(leadin_lat_rad), LeadIn, leading_dir_rad, R_e);

            //Begin navigating in Flyto
            if (IsReversed == true)
            {
                Status_Text = $"Flying to line {cur_line.ID}*";
            }
            else
            {
                Status_Text = $"Flying to line {cur_line.ID}";
            }
            double dist_rem_ft = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, Start_Lat_deg, Start_Lon_deg, DistanceUnits.Miles);
            
            DistRem = (int)dist_rem_ft; // need checking
            Status_Back_Color = "Red";
            DashColor = 100;
            LeadInColor = 100;
            double prevlat_deg = 0;
            double prevlon_deg = 0;

            // Some helpfule things to initiallize before going into loops;
            double bearing_target_to_cur_rad;
            double dist_to_leadin_unit;
            double dist_to_start_unit;
            double dist_to_end_unit;
            double angle_to_start_rad;
            double angle_to_end_rad;
            double angle_to_leadin_rad;

            // FlyTo Loop
            while (dist_rem_ft > WPRadius) //Check if beginning of first waypoint is reached
            {
                // Turn of dashed line if reached leading point
                if ((int)(5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, Rad2Deg(leadin_lat_rad), Rad2Deg(leadin_lon_rad), DistanceUnits.Miles)) < WPRadius)
                {
                    DashColor = 0;
                }
                // Handle if current line is skipped or aborted
                if (AbortCur == true)
                {
                    AbortCur = false;
                    break;
                }
                cur_pos = await Geolocation.GetLocationAsync(request);
                if (cur_pos != null && cur_pos.Accuracy < 30)
                {
                    // Parse current location
                    CurrentLat = cur_pos.Latitude;
                    CurrentLon = cur_pos.Longitude;

                    // Get current altitude
                    if (cur_pos.Altitude != 0)
                    {
                        CurrentAlt = 3.28084 * (double)cur_pos.Altitude;
                        VertDeviationNum = CurrentAlt - avg_alt;
                    }

                    // Update heading when move sufficiently far. Reduce random jumps due to noisy GPS
                    if ((int)(5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, prevlat_deg, prevlon_deg, DistanceUnits.Miles)) > HeadingThreshold)
                    {
                        Heading = (double)Rad2Deg(CalcCourse_rad(prevlat_deg, prevlon_deg, cur_pos.Latitude, cur_pos.Longitude));
                        prevlat_deg = cur_pos.Latitude;
                        prevlon_deg = cur_pos.Longitude;

                        // Change to point direction
                        Course = (double)-Rad2Deg(Deg2Rad(Heading));
                    }



                    // Calculate horizontal deviation
                    dist_rem_ft = (int)(5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, Rad2Deg(offset_start_lat_rad), Rad2Deg(offset_start_lat_rad), DistanceUnits.Miles));
                    bearing_target_to_cur_rad = CalcCourse_rad(Rad2Deg(offset_end_lat_rad), Rad2Deg(offset_end_lon_rad), cur_pos.Latitude, cur_pos.Longitude);
                    DeviationNum = dist_rem_ft * Sin(bearing_target_to_cur_rad - wrapTo2pi(target_groundcourse_rad + PI));
                    Deviation = DeviationNum * (HorBarUnit / FeetperBar);
                    if (Abs(Deviation) > HorBarUnit * 6.1)
                    {
                        Deviation = Sign(Deviation) * HorBarUnit * 6.1;
                    }

                    // Calculate params to draw on moving maps
                    dist_to_start_unit = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, cur_line.StartLat, cur_line.StartLon, DistanceUnits.Miles) / MapScale;
                    dist_to_end_unit = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, cur_line.EndLat, cur_line.EndLon, DistanceUnits.Miles) / MapScale;
                    dist_to_leadin_unit = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, leadin_lat_rad, leadin_lon_rad, DistanceUnits.Miles) / MapScale;

                    angle_to_start_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, cur_line.StartLat, cur_line.StartLon);
                    angle_to_end_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, cur_line.EndLat, cur_line.EndLon);
                    angle_to_leadin_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, leadin_lat_rad, leadin_lon_rad);

                    X_Start = - dist_to_start_unit * Sin(angle_to_start_rad) + PlaneXOffset;
                    Y_Start = - dist_to_start_unit * Cos(angle_to_start_rad) + PlaneYOffset;
                    X_End = - dist_to_end_unit * Sin(angle_to_end_rad) + PlaneXOffset;
                    Y_End = - dist_to_end_unit * Cos(angle_to_end_rad) + PlaneYOffset;

                    X_Leadin = -dist_to_leadin_unit * Sin(angle_to_leadin_rad) + PlaneXOffset; ;
                    Y_Leadin = -dist_to_leadin_unit * Cos(angle_to_leadin_rad) + PlaneYOffset;

                    // Update numerical readouts and graphical elements
                    Acc = (int)(cur_pos.Accuracy * 3.28084);
                    HeadingComp = -Heading;
                    HeadingDisp = (int)Heading;
                    DeviationDisp = $"Dev: {Abs((int)DeviationNum)} ft";
                    DistRemDisp = $"Dst Rem: {DistRem} ft";
                    AccDisp = $"Acc: {Acc} ft";
                    
                }
                await Task.Delay(10);

            }

            // When arrive at starting waypoint of line
            // Set the status text according to flight direction
            if (IsReversed == true)
            {
                Status_Text = $"Flying line {LineID}*";
            }
            else
            {
                Status_Text = $"Flying line {LineID}";
            }

            //Recalculate remaining distance
            dist_rem_ft = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, cur_line.EndLat, cur_line.EndLon, DistanceUnits.Miles);
            DistRem = (int)dist_rem_ft;
            // Update status to Navigate mode (follow the refeence trajectory)
            Status_Back_Color = "Green";
            DashColor = 0;
            LeadInColor = 0;
            X_Leadin = 0;
            Y_Leadin = 0;

            //Line Navigating loop
            while (dist_rem_ft > WPRadius) //Check if beginning of first waypoint is reached
            {
                //Handle cur line abort command
                if (AbortCur == true)
                { 
                    AbortCur = false;
                    break;
                }
                cur_pos = await Geolocation.GetLocationAsync(request);
                if (cur_pos != null && cur_pos.Accuracy < 30)
                {
                    CurrentLat = cur_pos.Latitude;
                    CurrentLon = cur_pos.Longitude;

                    // Heading update
                    if ((int)(5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, prevlat_deg, prevlon_deg, DistanceUnits.Miles)) > HeadingThreshold)
                    {
                        Heading = (double)Rad2Deg(CalcCourse_rad(prevlat_deg, prevlon_deg, cur_pos.Latitude, cur_pos.Longitude));
                        prevlat_deg = cur_pos.Latitude;
                        prevlon_deg = cur_pos.Longitude;
                        Course = (double)-Rad2Deg(Deg2Rad(Heading)); // change to point dir
                    }

                    // Update dist_rem and altitude
                    dist_rem_ft = (int)(5280 * Location.CalculateDistance(CurrentLat, CurrentLon, TargetLat, TargetLon, DistanceUnits.Miles));
                    if (cur_pos.Altitude != 0)
                    {
                        CurrentAlt = 3.28084 * (double)cur_pos.Altitude;
                        VertDeviationNum = CurrentAlt - avg_alt;
                    }

                    bearing_target_to_cur_rad = CalcCourse_rad(cur_line.EndLat, cur_line.EndLon, cur_pos.Latitude, cur_pos.Longitude);

                    // Calculate devieation
                    DeviationNum = dist_rem_ft * Sin(bearing_target_to_cur_rad - wrapTo2pi(target_groundcourse_rad + PI));
                    Deviation = DeviationNum * (HorBarUnit / FeetperBar);
                    // Saturate devviation bug travel
                    if (Abs(Deviation) > HorBarUnit * 6.1)
                    {
                        Deviation = Sign(Deviation) * HorBarUnit * 6.1;
                    }

                    // Calculate params to draw on moving maps
                    dist_to_start_unit = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, cur_line.StartLat, cur_line.StartLon, DistanceUnits.Miles) / MapScale;
                    dist_to_end_unit = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, cur_line.EndLat, cur_line.EndLon, DistanceUnits.Miles) / MapScale;

                    angle_to_start_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, cur_line.StartLat, cur_line.StartLon);
                    angle_to_end_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, cur_line.EndLat, cur_line.EndLon);

                    X_Start = -dist_to_start_unit * Sin(angle_to_start_rad) + PlaneXOffset;
                    Y_Start = -dist_to_start_unit * Cos(angle_to_start_rad) + PlaneYOffset;
                    X_End = -dist_to_end_unit * Sin(angle_to_end_rad) + PlaneXOffset;
                    Y_End = -dist_to_end_unit * Cos(angle_to_end_rad) + PlaneYOffset;

                    // Update numerical readouts and graphical elements
                    Acc = (int)(cur_pos.Accuracy * 3.28084);
                    HeadingComp = -Heading;
                    HeadingDisp = (int)Heading;
                    DeviationDisp = $"Dev: {Abs((int)DeviationNum)} ft";
                    DistRemDisp = $"Dst Rem: {DistRem} ft";
                    AccDisp = $"Acc: {Acc} ft";
                }
                await Task.Delay(10);

            }
            //Finish navigating
            EnableFlyTo = true;
            if (Is_Sequenced == true)
            {
                EnableFlyTo = false;
            }
            Status_Back_Color = "Gray";
            Status_Text = "Stand By";
            HeadingComp = 0;
            HeadingDisp = 0;
            DeviationDisp = "Dev: ";
            Deviation = 0;
            VertDeviationDisp = 0;
            VertDeviation = 0;
            AccDisp = "Acc:";
            DistRemDisp = "Dst Rem:";
            Leg_GroundCourse_Text = "Crs: ";
            Course = 0;
            X_Start = 0;
            Y_Start = 0;
            X_End = 0;
            Y_End = 0;
            CourseDes = 0;
        }

        async Task Navigate()
        {
            
            /* Enter FlyTo Mode
             * flight_director navigate between current position and the start coordinate of the line*/
            //Initialize current position
            StatusColor = "White";
            EnableFlyTo = false;
            Status_Text = "Stand By";


            //Parse selected line info
            var line_seq = LineSeq.Split(',');
            Is_Sequenced = line_seq.Length > 1;
            for (int i = 0; i < line_seq.Length; i++)
            {
                // Run navigation routine
                await LineNavigate(line_seq[i]);
                IsReversed = false;

                // Cancle all lines if AbortAll is true
                if (AbortAll == true)
                {
                    AbortAll = false;
                    return; }

                // Handle previous line command
                if (PrevLine == true)
                {
                    PrevLine = false;
                    if (i != 0)
                    {
                        i = i - 2;
                    }

                    // Force to first line when prev is commanded
                    else
                    {
                        i = -1;
                    }
                    
                }

            }

        }



    }
}
