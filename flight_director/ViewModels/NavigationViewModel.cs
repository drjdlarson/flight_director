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

        private bool enableflyto = true;
        public bool EnableFlyTo
        {
            get => enableflyto;
            set => SetProperty(ref enableflyto, value);
        }

        private double deviationoffset = 0;
        public double DeviationOffset
        {
            get => deviationoffset;
            set => SetProperty(ref deviationoffset, value);
        }

        private bool abortall = false;
        public bool AbortAll
        {
            get => abortall;
            set => SetProperty(ref abortall, value);
        }

        private bool abortcur = false;
        public bool AbortCur
        {
            get => abortcur;
            set => SetProperty(ref abortcur, value);
        }

        private bool prevline = false;
        public bool PrevLine
        {
            get => prevline;
            set => SetProperty(ref prevline, value);
        }

        private double heading = 0;
        public double Heading
        {
            get => heading;
            set => SetProperty(ref heading, value);
        }

        private double headingcomp = 0;
        public double HeadingComp
        {
            get => headingcomp;
            set => SetProperty(ref headingcomp, value);
        }

        private int headingdisp = 0;
        public int HeadingDisp
        {
            get => headingdisp;
            set => SetProperty(ref headingdisp, value);
        }

        private int lineid = 0;
        public int LineID
        {
            get => lineid;
            set => SetProperty(ref lineid, value);
        }

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

        private int dashcolor = 0;
        public int DashColor
        {
            get => dashcolor;
            set => SetProperty(ref dashcolor, value);
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
            double bearing_cur_pos;
            // Calc offset lat lon with antenna deviation
            double antenna_offset_dist_ft = (double)(avg_alt - cur_line.AvgEle) * Tan(Deg2Rad(AntennaAngle));       //Left offset is negative
            double offset_dir_rad = wrapTo2pi(target_groundcourse_rad + PI / 2);

            double offset_start_lat_rad = translate_lat_rad(Start_Lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_start_lon_rad = translate_lon_rad(Start_Lat_deg, Start_Lon_deg, Rad2Deg(offset_start_lat_rad), antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_end_lat_rad = translate_lat_rad(End_Lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_end_lon_rad = translate_lon_rad(End_Lat_deg, End_Lon_deg, Rad2Deg(offset_end_lat_rad), antenna_offset_dist_ft, offset_dir_rad, R_e);

            // Leadin coordinates calculation
            double leading_dir_rad = wrapTo2pi(target_groundcourse_rad + PI);

            double leadin_start_lat_rad = translate_lat_rad(Start_Lat_deg, LeadIn, leading_dir_rad, R_e);
            double leadin_start_lon_rad = translate_lon_rad(Start_Lat_deg, Start_Lon_deg, Rad2Deg(leadin_start_lat_rad), LeadIn, leading_dir_rad, R_e);

            //Begin navigating in Flyto
            if (IsReversed == true)
            {
                Status_Text = $"Flying to line {cur_line.ID}*";
            }
            else
            {
                Status_Text = $"Flying to line {cur_line.ID}";
            }
            double rem_miles = Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, Start_Lat_deg, Start_Lon_deg, DistanceUnits.Miles);
            double rem_ft = rem_miles * 5280;
            DistRem = (int)rem_ft;
            Status_Back_Color = "Red";
            DashColor = 100;
            

            // FlyTo Loop
            while (DistRem > WPRadius) //Check if beginning of first waypoint is reached
            {
                if (AbortCur == true)
                {
                    EnableFlyTo = true;
                    Status_Back_Color = "Gray";
                    Status_Text = "Stand By";
                    HeadingComp = 0;
                    HeadingDisp = 0;
                    DeviationDisp = "Dev: ";
                    Deviation = 0;
                    AccDisp = "Acc:";
                    DistRemDisp = "Dst Rem:";
                    Leg_GroundCourse_Text = "Crs: ";
                    Course = 0;
                    TransX = 0;
                    TransY = 0;
                    AbortCur = false;
                    X_Start = 0;
                    Y_Start = 0;
                    X_End = 0;
                    Y_End = 0;
                    CourseDes = 0;
                    X_Leadin = 0;
                    Y_Leadin = 0;
                    DashColor = 0;
                    break;
                }
                cur_pos = await Geolocation.GetLocationAsync(request);
                if (cur_pos != null && cur_pos.Accuracy < 30)
                {
                    CurrentLat = cur_pos.Latitude;
                    CurrentLon = cur_pos.Longitude;
                    if (cur_pos.Altitude != 0)
                    {
                        CurrentAlt = 3.28084 * (double)cur_pos.Altitude;
                        VertDeviationNum = CurrentAlt - TargetAlt;
                        VertDeviationDisp = (int)VertDeviationNum;
                        VertDeviation = VertDeviationNum * (VerBarUnit / FeetperVertBar);
                        if (Abs(VertDeviation) > VerBarUnit * 2.2)
                        {
                            VertDeviation = Sign(VertDeviation) * VerBarUnit * 2.2;
                        }
                    }
                    if ((int)(5280 * Location.CalculateDistance(CurrentLat, CurrentLon, PrevLat, PrevLon, DistanceUnits.Miles)) > HeadingThreshold)
                    {
                        Heading = (double)Rad2Deg(CalcCourse_rad(PrevLat, PrevLon, CurrentLat, CurrentLon));
                        PrevLat = CurrentLat;
                        PrevLon = CurrentLon;
                        Course = (double)-Rad2Deg(Deg2Rad(Heading) - track_course);
                    }
                    DistRem = (int)(5280 * Location.CalculateDistance(CurrentLat, CurrentLon, TargetLat, TargetLon, DistanceUnits.Miles));
                    bearing_cur_pos = CalcCourse_rad(TargetLat, TargetLon, CurrentLat, CurrentLon);
                    DeviationNum = (DistRem * Sin(bearing_cur_pos - bearing_track));
                    Deviation = DeviationNum * (HorBarUnit / FeetperBar);
                    if (Abs(Deviation) > HorBarUnit * 6.1)
                    {
                        Deviation = Sign(Deviation) * HorBarUnit * 6.1;
                    }

                    double dist_to_start_unit = ((5280 * Location.CalculateDistance(CurrentLat, CurrentLon, cur_line.StartLat, cur_line.StartLon, DistanceUnits.Miles))/MapScale);
                    double dist_to_end_unit = ((5280 * Location.CalculateDistance(CurrentLat, CurrentLon, cur_line.EndLat, cur_line.EndLon, DistanceUnits.Miles))/MapScale);
                    double angle_to_start_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(CurrentLat, CurrentLon, cur_line.StartLat, cur_line.StartLon);
                    double angle_to_end_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(CurrentLat, CurrentLon, cur_line.EndLat, cur_line.EndLon);
                    double temp_x = 0;
                    double temp_y = 0;
                    double map_offset = -DeviationOffset / MapScale;
                    double deviation_offset_angle = Deg2Rad(Heading) - (trackcourse + PI / 2);
                    X_Start = - dist_to_start_unit * Sin(angle_to_start_rad) + PlaneXOffset + Sin(deviation_offset_angle) * map_offset;
                    Y_Start = - dist_to_start_unit * Cos(angle_to_start_rad) + PlaneYOffset + Cos(deviation_offset_angle) * map_offset;
                    X_End = - dist_to_end_unit * Sin(angle_to_end_rad) + PlaneXOffset + Sin(deviation_offset_angle) * map_offset;
                    Y_End = - dist_to_end_unit * Cos(angle_to_end_rad) + PlaneYOffset + Cos(deviation_offset_angle) * map_offset;
                    if (IsReversed == true)
                    {
                        temp_x = X_Start;
                        temp_y = Y_Start;
                        X_Start = X_End;
                        Y_Start = Y_End;
                        X_End = temp_x;
                        Y_End = temp_y;
                    }
                    X_Marker = X_Start + 2;
                    Y_Marker = Y_Start + 2;
                    Acc = (int)(cur_pos.Accuracy * 3.28084);
                    HeadingComp = -Heading;
                    HeadingDisp = (int)Heading;
                    DeviationDisp = $"Dev: {Abs((int)DeviationNum)} ft";
                    DistRemDisp = $"Dst Rem: {DistRem} ft";
                    AccDisp = $"Acc: {Acc} ft";
                    
                }
                await Task.Delay(15);

            }

            // When arrive at starting waypoint of line
            // Set the start and end coordinate to the lines coordinates
            if (IsReversed == true)
            {
                CurrentLat = cur_line.EndLat;
                CurrentLon = cur_line.EndLon;
                TargetLat = cur_line.StartLat;
                TargetLon = cur_line.StartLon;
                Status = $"Flying line {LineID}*";
            }
            else
            {
                CurrentLat = cur_line.StartLat;
                CurrentLon = cur_line.StartLon;
                TargetLat = cur_line.EndLat;
                TargetLon = cur_line.EndLon;
                Status = $"Flying line {LineID}";
            }
            bearing_track = CalcCourse_rad(TargetLat, TargetLon, CurrentLat, CurrentLon);
            // Calc offset lat lon with antenna deviation
            DeviationOffset = (double)(TargetAlt - cur_line.AvgEle) * Tan(Deg2Rad(AntennaAngle));       //Left offset is negative
            offset_lat = TargetLat + Rad2Deg(Cos(lat_offset_angle) * DeviationOffset / R_e);
            offset_lon = TargetLon + Rad2Deg(Sin(lat_offset_angle) * DeviationOffset / (R_e * Cos(Deg2Rad(TargetLat))));
            TargetLat = offset_lat;
            TargetLon = offset_lon;
            //Recalculate remaining distance
            rem_miles = Location.CalculateDistance(CurrentLat, CurrentLon, TargetLat, TargetLon, DistanceUnits.Miles);
            rem_ft = rem_miles * 5280;
            DistRem = (int)rem_ft;
            // Update status to Navigate mode (follow the refeence trajectory)
            ButtonColor = "Green";
            CourseDes = (int)Rad2Deg(trackcourse);
            DashColor = 0;

            //Line Navigating loop
            while (DistRem > WPRadius) //Check if beginning of first waypoint is reached
            {
                if (AbortCur == true)
                {
                    EnableFlyTo = true;
                    Status_Back_Color = "Gray";
                    Status_Text = "Stand By";
                    HeadingComp = 0;
                    HeadingDisp = 0;
                    DeviationDisp = "Dev: ";
                    Deviation = 0;
                    AccDisp = "Acc:";
                    DistRemDisp = "Dst Rem:";
                    Leg_GroundCourse_Text = "Crs: ";
                    Course = 0;
                    TransX = 0;
                    TransY = 0;
                    AbortCur = false;
                    X_Start = 0;
                    Y_Start = 0;
                    X_End = 0;
                    Y_End = 0;
                    CourseDes = 0;
                    X_Leadin = 0;
                    Y_Leadin = 0;
                    DashColor = 0;
                    break;
                }
                cur_pos = await Geolocation.GetLocationAsync(request);
                if (cur_pos != null && cur_pos.Accuracy < 30)
                {
                    CurrentLat = cur_pos.Latitude;
                    CurrentLon = cur_pos.Longitude;
                    if ((int)(5280 * Location.CalculateDistance(CurrentLat, CurrentLon, PrevLat, PrevLon, DistanceUnits.Miles)) > HeadingThreshold)
                    {
                        Heading = (double)Rad2Deg(CalcCourse_rad(PrevLat, PrevLon, CurrentLat, CurrentLon));
                        PrevLat = CurrentLat;
                        PrevLon = CurrentLon;
                        Course = (double)-Rad2Deg(Deg2Rad(Heading) - trackcourse);
                    }
                    DistRem = (int)(5280 * Location.CalculateDistance(CurrentLat, CurrentLon, TargetLat, TargetLon, DistanceUnits.Miles));
                    if (cur_pos.Altitude != 0)
                    {
                        CurrentAlt = 3.28084 * (double)cur_pos.Altitude;
                        TargetAlt = cur_line.EndAlt + Tan(gs_angle) * DistRem;
                        if (IsReversed == true)
                        {
                            TargetAlt = cur_line.StartAlt + Tan(gs_angle) * DistRem;
                        }
                        VertDeviationNum = CurrentAlt - TargetAlt;
                        VertDeviationDisp = (int)VertDeviationNum;
                        VertDeviation = VertDeviationNum * (VerBarUnit / FeetperVertBar);
                        if (Abs(VertDeviation) > VerBarUnit * 2.2)
                        {
                            VertDeviation = Sign(VertDeviation) * VerBarUnit * 2.2;
                        }
                    }
                    DeviationOffset = (double)(TargetAlt - cur_line.AvgEle) * Tan(Deg2Rad(AntennaAngle));
                    bearing_cur_pos = CalcCourse_rad(TargetLat, TargetLon, CurrentLat, CurrentLon);
                    DeviationNum = (DistRem * Sin(bearing_cur_pos - bearing_track));
                    Deviation = DeviationNum * (HorBarUnit / FeetperBarPrecise);
                    if (Abs(Deviation) > HorBarUnit * 6.1)
                    {
                        Deviation = Sign(Deviation) * HorBarUnit * 6.1;
                    }
                    double dist_to_start_unit = ((5280 * Location.CalculateDistance(CurrentLat, CurrentLon, cur_line.StartLat, cur_line.StartLon, DistanceUnits.Miles)) / MapScale);
                    double dist_to_end_unit = ((5280 * Location.CalculateDistance(CurrentLat, CurrentLon, cur_line.EndLat, cur_line.EndLon, DistanceUnits.Miles)) / MapScale);
                    double angle_to_start_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(CurrentLat, CurrentLon, cur_line.StartLat, cur_line.StartLon);
                    double angle_to_end_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(CurrentLat, CurrentLon, cur_line.EndLat, cur_line.EndLon);
                    double temp_x = 0;
                    double temp_y = 0;
                    double map_offset = -DeviationOffset / MapScale;
                    double deviation_offset_angle = Deg2Rad(Heading) - (trackcourse + PI / 2 );
                    X_Start = -dist_to_start_unit * Sin(angle_to_start_rad) + PlaneXOffset + Sin(deviation_offset_angle) * map_offset;
                    Y_Start = -dist_to_start_unit * Cos(angle_to_start_rad) + PlaneYOffset + Cos(deviation_offset_angle) * map_offset;
                    X_End = -dist_to_end_unit * Sin(angle_to_end_rad) + PlaneXOffset + Sin(deviation_offset_angle) * map_offset;
                    Y_End = -dist_to_end_unit * Cos(angle_to_end_rad) + PlaneYOffset + Cos(deviation_offset_angle) * map_offset;
                    if (IsReversed == true)
                    {
                        temp_x = X_Start;
                        temp_y = Y_Start;
                        X_Start = X_End;
                        Y_Start = Y_End;
                        X_End = temp_x;
                        Y_End = temp_y;
                    }
                    X_Marker = X_Start + 2;
                    Y_Marker = Y_Start + 2;
                    Acc = (int)(cur_pos.Accuracy * 3.28084);
                    HeadingComp = -Heading;
                    HeadingDisp = (int)Heading;
                    DeviationDisp = $"Dev: {Abs((int)(DeviationNum))} ft";
                    DistRemDisp = $"Dst Rem: {DistRem} ft";
                    AccDisp = $"Acc: {Acc} ft";
                }
                await Task.Delay(15);

            }
            //Finish navigating
            EnableFlyTo = true;
            if (Is_Sequenced == true)
            {
                EnableFlyTo = false;
            }
            ButtonColor = "Gray";
            Status = "Stand By";
            HeadingComp = 0;
            HeadingDisp = 0;
            DeviationDisp = "Dev: ";
            Deviation = 0;
            VertDeviationDisp = 0;
            VertDeviation = 0;
            AccDisp = "Acc:";
            DistRemDisp = "Dst Rem:";
            TrackCourse = "Crs: ";
            Course = 0;
            X_Start = 0;
            Y_Start = 0;
            X_End = 0;
            Y_End = 0;
            CourseDes = 0;
            X_Marker = 0;
            Y_Marker = 0;
            DashColor = 0;
        }

        async Task Navigate()
        {
            
            /* Enter FlyTo Mode
             * flight_director navigate between current position and the start coordinate of the line*/
            //Initialize current position
            StatusColor = "White";
            EnableFlyTo = false;
            Status = "Stand By";


            //Parse selected line info
            var line_seq = LineSeq.Split(',');
            Is_Sequenced = line_seq.Length > 1;
            for (int i = 0; i < line_seq.Length; i++)
            {
                
                await LineNavigate(line_seq[i]);
                IsReversed = false;
                if (AbortAll == true)
                {
                    AbortAll = false;
                    return; }
                if (PrevLine == true)
                {
                    PrevLine = false;
                    if (i != 0)
                    {
                        i = i - 2;
                    }
                    else
                    {
                        i = -1;
                    }
                    
                }

            }

        }



    }
}
