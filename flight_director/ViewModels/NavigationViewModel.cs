using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
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

        // Current aircraft ground track
        private double heading = 0;
        public double Heading
        {
            get => heading;
            set => SetProperty(ref heading, value);
        }

        // Compass card rotation ( = - Heading). Need to be included since can't bind to negative variable
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

        // Status text
        private string status_text = "Stand By";
        public string Status_Text
        {
            get => status_text;
            set => SetProperty(ref status_text, value);
        }

        // Status text font color
        private string statuscolor = "White";
        public string StatusColor
        {
            get => statuscolor;
            set => SetProperty(ref statuscolor, value);
        }

        // Status button background color
        private string status_back_color = "Gray";
        public string Status_Back_Color
        {
            get => status_back_color;
            set => SetProperty(ref status_back_color, value);
        }

        // Status button background color
        private string flyto_back_color = "DeepSkyBlue";
        public string Flyto_Back_Color
        {
            get => flyto_back_color;
            set => SetProperty(ref flyto_back_color, value);
        }

        // Line sequence input text box
        private string lineseq = "";
        public string LineSeq
        {
            get => lineseq;
            set => SetProperty(ref lineseq, value);
        }

        // Distance remaining text 
        private string etadisp = "ETA: ";
        public string ETADisp
        {
            get => etadisp;
            set => SetProperty(ref etadisp, value);
        }

        // Heading bug on compas card to show which direction to turn
        private double course_bug = 0;
        public double Course_Bug
        {
            get => course_bug;
            set => SetProperty(ref course_bug, value);
        }

        // Current Lat - keep maybe for future use
        private double currentlat = 0;
        public double CurrentLat
        {
            get => currentlat;
            set => SetProperty(ref currentlat, value);
        }

        // Current Lon - keep maybe for future use
        private double currentlon = 0;
        public double CurrentLon
        {
            get => currentlon;
            set => SetProperty(ref currentlon, value);
        }

        // Numerical reading of altitude
        private double currentalt = 0;
        public double CurrentAlt
        {
            get => currentalt;
            set => SetProperty(ref currentalt, value);
        }

        // Deviation tick movement distance
        private double deviation = 0;
        public double Deviation
        {
            get => deviation;
            set => SetProperty(ref deviation, value);
        }

        // Deviation text
        private string altdisp = "MSL_Alt:";
        public string AltDisp
        {
            get => altdisp;
            set => SetProperty(ref altdisp, value);
        }

        // Leg course text
        private string leg_groundcourse_text = "Crs";
        public string Leg_GroundCourse_Text
        {
            get => leg_groundcourse_text;
            set => SetProperty(ref leg_groundcourse_text, value);
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

        // X location on screen of line start point
        private double x_start = 0;
        public double X_Start
        {
            get => x_start;
            set => SetProperty(ref x_start, value);
        }

        // Y location on screen of line start point
        private double y_start = 0;
        public double Y_Start
        {
            get => y_start;
            set => SetProperty(ref y_start, value);
        }

        // X location on screen of line end point
        private double x_end = 0;
        public double X_End
        {
            get => x_end;
            set => SetProperty(ref x_end, value);
        }

        // Y location on screen of line end point
        private double y_end = 0;
        public double Y_End
        {
            get => y_end;
            set => SetProperty(ref y_end, value);
        }

        // X location on screen of leadin point
        private double x_leadin = 0;
        public double X_Leadin
        {
            get => x_leadin;
            set => SetProperty(ref x_leadin, value);
        }

        // Y location on screen of leadin point
        private double y_leadin = 0;
        public double Y_Leadin
        {
            get => y_leadin;
            set => SetProperty(ref y_leadin, value);
        }

        // Current Map scaling factor
        private double mapscale = 20;
        public double MapScale
        {
            get => mapscale;
            set => SetProperty(ref mapscale, value);
        }

        // Dist of first reference mark
        private double ref_scale_1 = 166 / (20);
        public double Ref_Scale_1
        {
            get => ref_scale_1;
            set => SetProperty(ref ref_scale_1, value);
        }

        private string ref_scale_1_disp = "NM";
        public string Ref_Scale_1_Disp
        {
            get => ref_scale_1_disp;
            set => SetProperty(ref ref_scale_1_disp, value);
        }

        // Dist of second reference mark
        private double ref_scale_2 = 166 / (20);
        public double Ref_Scale_2
        {
            get => ref_scale_2;
            set => SetProperty(ref ref_scale_2, value);
        }

        private string ref_scale_2_disp = "NM";
        public string Ref_Scale_2_Disp
        {
            get => ref_scale_2_disp;
            set => SetProperty(ref ref_scale_2_disp, value);
        }


        // Reversed flag
        private bool isreversed = false;
        public bool IsReversed
        {
            get => isreversed;
            set => SetProperty(ref isreversed, value);
        }

        // Sequence flag
        private bool is_sequence = false;
        public bool Is_Sequence
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


        // General Settings

        // WP radiius - how close too switch wp
        public  int WPRadius
        {
            get => Preferences.Get(nameof(WPRadius),500);
            set
            {
                Preferences.Set(nameof(WPRadius), value);
                OnPropertyChanged(nameof(WPRadius));
            }
        }

        // Deviation distance per horizontal tick for flyto
        public int FeetperBar
        {
            get => Preferences.Get(nameof(FeetperBar), 100);
            set
            {
                Preferences.Set(nameof(FeetperBar), value);
                OnPropertyChanged(nameof(FeetperBar));
            }
        }

        // Deviation distance per horizontal tick for precise line navigation
        public int FeetperBarPrecise
        {
            get => Preferences.Get(nameof(FeetperBarPrecise), 10);
            set
            {
                Preferences.Set(nameof(FeetperBarPrecise), value);
                OnPropertyChanged(nameof(FeetperBarPrecise));
            }
        }

        // Distance on screen between horizontal ticks
        public double HorBarUnit
        {
            get => Preferences.Get(nameof(HorBarUnit), 25.0);
            set
            {
                Preferences.Set(nameof(HorBarUnit), value);
                OnPropertyChanged(nameof(HorBarUnit));
            }
        }

        // Threshold for calculating heading
        public int HeadingThreshold
        {
            get => Preferences.Get(nameof(HeadingThreshold), 3);
            set
            {
                Preferences.Set(nameof(HeadingThreshold), value);
                OnPropertyChanged(nameof(HeadingThreshold));
            }
        }

        // Antenna angle for lateral offset calculation
        public int AntennaAngle
        {
            get => Preferences.Get(nameof(AntennaAngle), 0);
            set
            {
                Preferences.Set(nameof(AntennaAngle), value);
                OnPropertyChanged(nameof(AntennaAngle));
            }
        }

        // Screen X location of simulated plane
        public int PlaneXOffset
        {
            get => Preferences.Get(nameof(PlaneXOffset), 376);
            set
            {
                Preferences.Set(nameof(PlaneXOffset), value);
                OnPropertyChanged(nameof(PlaneXOffset));
            }
        }

        // Screen Y location of simulated plane
        public int PlaneYOffset
        {
            get => Preferences.Get(nameof(PlaneYOffset), 515);
            set
            {
                Preferences.Set(nameof(PlaneYOffset), value);
                OnPropertyChanged(nameof(PlaneYOffset));
            }
        }

        // Leadin distance
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

        // Convert radian to degree
        public double Rad2Deg(double rad)
        {
            return rad * 180 / Math.PI;
        }

        // convert degree to radian
        public double Deg2Rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        // Calculate bearing between 2 coordinates
        public double CalcCourse_rad(double start_lat_deg, double start_lon_deg, double end_lat_deg, double end_lon_deg)
        {
            double dL = Deg2Rad(end_lon_deg - start_lon_deg);
            double x = Sin(dL) * Cos(Deg2Rad(end_lat_deg));
            double y = Cos(Deg2Rad(start_lat_deg)) * Sin(Deg2Rad(end_lat_deg)) - Sin(Deg2Rad(start_lat_deg)) * Cos(Deg2Rad(end_lat_deg)) * Cos(dL);
            double bearing_rad = Atan2(x, y);
            return wrapTo2pi(bearing_rad);

        }

        // Wrap angle to [0 - 2pi]
        public double wrapTo2pi (double angle_rad)
        {
            return (angle_rad + 2 * PI) % (2 * PI);
        }

        // Translate latitude by distance and bearing
        public double translate_lat_deg (double start_lat_deg, double translate_dist_ft, double translate_dir_rad, double R_E)
        {
            double angular_dist_rad = translate_dist_ft / R_E;
            double start_lat_rad = Deg2Rad(start_lat_deg);
            double translated_lat_rad = Asin( Sin(start_lat_rad) * Cos (angular_dist_rad) + Cos(start_lat_rad) * Sin(angular_dist_rad) * Cos(translate_dir_rad));
            return Rad2Deg(translated_lat_rad);
        }

        // Translate longitude by distance and bearing
        public double translate_lon_deg(double start_lat_deg, double start_lon_deg, double end_lat_deg, double translate_dist_ft, double translate_dir_rad, double R_E)
        {
            double angular_dist_rad = translate_dist_ft / R_E;
            double start_lat_rad = Deg2Rad(start_lat_deg);
            double end_lat_rad = Deg2Rad(end_lat_deg);
            double start_lon_rad = Deg2Rad(start_lon_deg);
            double translated_lon_rad = start_lon_rad + Atan2(Sin(translate_dir_rad) * Sin(angular_dist_rad) * Cos(start_lat_rad), Cos(angular_dist_rad) - Sin(start_lat_rad) * Sin(end_lat_rad));
            return Rad2Deg(translated_lon_rad);
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
            { MapScale = MapScale - 3; }
            else { MapScale = MapScale; }
            Ref_Scale_1 = 166 * MapScale / 6076.12;
            Ref_Scale_1_Disp = string.Format("{0:N2} NM",Ref_Scale_1);
            Ref_Scale_2 = 2 * Ref_Scale_1;
            Ref_Scale_2_Disp = string.Format("{0:N2} NM", Ref_Scale_2);
        }

        void zoom_out()
        {
            MapScale = MapScale + 3;
            Ref_Scale_1 = 166 * MapScale /6076.12;
            Ref_Scale_1_Disp = string.Format("{0:N2} NM", Ref_Scale_1);
            Ref_Scale_2 = 2 * Ref_Scale_1;
            Ref_Scale_2_Disp = string.Format("{0:N2} NM", Ref_Scale_2);
        }

        // Skip current navigation process (skip flyto or line navigation step)
        void SkipNav()
        {
            AbortCur = true;
        }

        // Go to previous line
        void PrevNav()
        {
            AbortCur = true;
            PrevLine = true;
        }

        // Cancel all line
        void CancleNav()
        {
            AbortCur = true;
            AbortAll = true;
        }

        // Go to setting page
        async Task GotoSettings()
        {
            await Shell.Current.GoToAsync(nameof(Settings));
        }

        // Line navigation routine
        async Task LineNavigate (string LineName)
        {
            const int R_e = 3963 * 5280; //Earth radius in feet
            // Get Line Number and check reverse command
            int LineNo = 0;

            string temp;
            temp = LineName.Remove(LineName.Length - 1);
            if (!int.TryParse(LineName, out LineNo) && !int.TryParse(temp, out LineNo))
            {
                EnableFlyTo = true;
                Flyto_Back_Color = "DeepSkyBlue";
                Status_Text = $"Invalid Line";
                StatusColor = "Red";
                return;
            }
            
            IsReversed = LineName.EndsWith("*");
            
            // Test GPS 
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1));
            var cur_pos = await Geolocation.GetLocationAsync(request);

            if (cur_pos == null)
            {
                Status_Text = "No GPS";
                StatusColor = "Red";
                EnableFlyTo = true;
                Flyto_Back_Color = "DeepSkyBlue";
                return;
            }

            // Get current line params
            
            var cur_line = await FlightLineService.GetLine(LineNo);
            if (cur_line == null) //Check if the entered line is valid (id < total number of line in database)
            {
                Status_Text = "Invalid Line";
                StatusColor = "Red";
                EnableFlyTo = true;
                Flyto_Back_Color = "DeepSkyBlue";
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


            Leg_GroundCourse_Text = $"Crs: {(int)Rad2Deg(target_groundcourse_rad)}";
            
            // Calc offset lat lon with antenna deviation
            double antenna_offset_dist_ft = (double)(avg_alt - cur_line.AvgEle) * Tan(Deg2Rad(AntennaAngle));       //Left offset is negative
            double offset_dir_rad = wrapTo2pi(target_groundcourse_rad + PI / 2);

            double offset_start_lat_deg = translate_lat_deg(Start_Lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_start_lon_deg = translate_lon_deg(Start_Lat_deg, Start_Lon_deg, offset_start_lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);

            double offset_end_lat_deg = translate_lat_deg(End_Lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);
            double offset_end_lon_deg = translate_lon_deg(End_Lat_deg, End_Lon_deg, offset_end_lat_deg, antenna_offset_dist_ft, offset_dir_rad, R_e);

            // Leadin coordinates calculation
            double leading_dir_rad = wrapTo2pi(target_groundcourse_rad + PI);

            double leadin_lat_deg = translate_lat_deg(offset_start_lat_deg, LeadIn, leading_dir_rad, R_e);
            double leadin_lon_deg = translate_lon_deg(offset_start_lat_deg, offset_start_lon_deg, leadin_lat_deg, LeadIn, leading_dir_rad, R_e);

            //Begin navigating in Flyto
            if (IsReversed == true)
            {
                Status_Text = $"Flying to line {cur_line.ID}*";
            }
            else
            {
                Status_Text = $"Flying to line {cur_line.ID}";
            }
            double dist_rem_ft = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, offset_start_lat_deg, offset_start_lon_deg, DistanceUnits.Miles);

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
            double deviation_num;
            double dist_to_start_ft = dist_rem_ft;
            double speed_fps;
            double eta;

            // FlyTo Loop
            while (dist_to_start_ft > WPRadius && dist_to_start_ft < 100000000) //Check if beginning of first waypoint is reached
            {
                // Turn of dashed line if reached leading point
                if ((int)(5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, leadin_lat_deg, leadin_lon_deg, DistanceUnits.Miles)) < WPRadius)
                {
                    DashColor = 0;
                }
                // Handle if current line is skipped or aborted
                if (AbortCur == true)
                {
                    if (AbortAll != true)
                    {
                        AbortCur = false;
                    }
                    break;
                }
                cur_pos = await Geolocation.GetLocationAsync(request);
                if (cur_pos != null)
                {
                    // Parse current location
                    CurrentLat = cur_pos.Latitude;
                    CurrentLon = cur_pos.Longitude;

                    // Get current altitude
                    if (cur_pos.Altitude != 0)
                    {
                        CurrentAlt = (int)(3.28084 * (double)cur_pos.Altitude);
                    }

                    // Update heading when move sufficiently far. Reduce random jumps due to noisy GPS
                    if ((int)(5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, prevlat_deg, prevlon_deg, DistanceUnits.Miles)) > HeadingThreshold)
                    {
                        Heading = (double)Rad2Deg(CalcCourse_rad(prevlat_deg, prevlon_deg, cur_pos.Latitude, cur_pos.Longitude));
                        prevlat_deg = cur_pos.Latitude;
                        prevlon_deg = cur_pos.Longitude;
                    }

                    // Calculate horizontal deviation
                    dist_rem_ft = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, offset_end_lat_deg, offset_end_lon_deg, DistanceUnits.Miles);
                    dist_to_start_ft = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, offset_start_lat_deg, offset_start_lon_deg, DistanceUnits.Miles);
                    bearing_target_to_cur_rad = CalcCourse_rad(offset_end_lat_deg, offset_end_lon_deg, cur_pos.Latitude, cur_pos.Longitude);
                    deviation_num = dist_rem_ft * Sin(bearing_target_to_cur_rad - wrapTo2pi(target_groundcourse_rad + PI));
                    Deviation = deviation_num * (HorBarUnit / FeetperBar);
                    if (Abs(Deviation) > HorBarUnit * 6.1)
                    {
                        Deviation = Sign(Deviation) * HorBarUnit * 6.1;
                    }

                    // Calculate params to draw on moving maps
                    dist_to_start_unit = dist_to_start_ft / MapScale;
                    dist_to_end_unit = dist_rem_ft / MapScale;
                    dist_to_leadin_unit = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, leadin_lat_deg, leadin_lon_deg, DistanceUnits.Miles) / MapScale;

                    angle_to_start_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, offset_start_lat_deg, offset_start_lon_deg);
                    angle_to_end_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, offset_end_lat_deg, offset_end_lon_deg);
                    angle_to_leadin_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, leadin_lat_deg, leadin_lon_deg);

                    X_Start = - dist_to_start_unit * Sin(angle_to_start_rad) + PlaneXOffset;
                    Y_Start = - dist_to_start_unit * Cos(angle_to_start_rad) + PlaneYOffset;
                    X_End = - dist_to_end_unit * Sin(angle_to_end_rad) + PlaneXOffset;
                    Y_End = - dist_to_end_unit * Cos(angle_to_end_rad) + PlaneYOffset;

                    X_Leadin = -dist_to_leadin_unit * Sin(angle_to_leadin_rad) + PlaneXOffset; ;
                    Y_Leadin = -dist_to_leadin_unit * Cos(angle_to_leadin_rad) + PlaneYOffset;

                    // Update numerical readouts and graphical elements
                    Course_Bug = angle_to_start_rad;
                    HeadingComp = -Heading;
                    HeadingDisp = (int)Heading;
                    AltDisp = $"MSL_Alt: {(int)CurrentAlt} ft";

                    speed_fps = (double)cur_pos.Speed * 3.28084;
                    eta = dist_to_start_ft / speed_fps;
                    if (eta > 60)
                    {
                        ETADisp = $"ETA: {(int)(eta / 60)} min";
                    }
                    else
                    {
                        ETADisp = $"ETA: {(int)(eta)} sec";
                    }


                }
                await Task.Delay(15);

            }

            // When arrive at starting waypoint of line
            // Set the status text according to flight direction
            if (IsReversed == true)
            {
                Status_Text = $"Flying line {cur_line.ID}*";
            }
            else
            {
                Status_Text = $"Flying line {cur_line.ID}";
            }

            //Recalculate remaining distance
            dist_rem_ft = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, offset_end_lat_deg, offset_end_lon_deg, DistanceUnits.Miles);

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
                if (cur_pos != null)
                {

                    // Heading update
                    if ((int)(5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, prevlat_deg, prevlon_deg, DistanceUnits.Miles)) > HeadingThreshold)
                    {
                        Heading = (double)Rad2Deg(CalcCourse_rad(prevlat_deg, prevlon_deg, cur_pos.Latitude, cur_pos.Longitude));
                        prevlat_deg = cur_pos.Latitude;
                        prevlon_deg = cur_pos.Longitude;
                    }

                    // Update dist_rem and altitude
                    dist_rem_ft = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, offset_end_lat_deg, offset_end_lon_deg, DistanceUnits.Miles);
                    if (cur_pos.Altitude != 0)
                    {
                        CurrentAlt = (int)(3.28084 * (double)cur_pos.Altitude);
                    }

                    bearing_target_to_cur_rad = CalcCourse_rad(offset_end_lat_deg, offset_end_lon_deg, cur_pos.Latitude, cur_pos.Longitude);

                    // Calculate devieation
                    deviation_num = dist_rem_ft * Sin(bearing_target_to_cur_rad - wrapTo2pi(target_groundcourse_rad + PI));
                    Deviation = deviation_num * (HorBarUnit / FeetperBarPrecise);
                    // Saturate devviation bug travel
                    if (Abs(Deviation) > HorBarUnit * 6.1)
                    {
                        Deviation = Sign(Deviation) * HorBarUnit * 6.1;
                    }

                    // Calculate params to draw on moving maps
                    dist_to_start_unit = 5280 * Location.CalculateDistance(cur_pos.Latitude, cur_pos.Longitude, offset_start_lat_deg, offset_start_lon_deg, DistanceUnits.Miles) / MapScale;
                    dist_to_end_unit = dist_rem_ft / MapScale;

                    angle_to_start_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, offset_start_lat_deg, offset_start_lon_deg);
                    angle_to_end_rad = Deg2Rad(Heading) - (double)CalcCourse_rad(cur_pos.Latitude, cur_pos.Longitude, offset_end_lat_deg, offset_end_lon_deg);

                    X_Start = -dist_to_start_unit * Sin(angle_to_start_rad) + PlaneXOffset;
                    Y_Start = -dist_to_start_unit * Cos(angle_to_start_rad) + PlaneYOffset;
                    X_End = -dist_to_end_unit * Sin(angle_to_end_rad) + PlaneXOffset;
                    Y_End = -dist_to_end_unit * Cos(angle_to_end_rad) + PlaneYOffset;

                    // Update numerical readouts and graphical elements
                    Course_Bug = angle_to_end_rad;
                    HeadingComp = -Heading;
                    HeadingDisp = (int)Heading;
                    AltDisp = $"MSL_Alt: {(int)CurrentAlt} ft";

                    speed_fps = (double)cur_pos.Speed * 3.28084;
                    eta = dist_to_start_ft / speed_fps;
                    if (eta > 60)
                    {
                        ETADisp = $"ETA: {(int)(eta/60)} min";
                    }
                    else
                    {
                        ETADisp = $"ETA: {(int)(eta)} sec";
                    }
                    
                }
                await Task.Delay(15);

            }
            //Finish navigating
            EnableFlyTo = true;
            if (Is_Sequence == true)
            {
                EnableFlyTo = false;
            }
            Flyto_Back_Color = "DeepSkyBlue";
            Status_Back_Color = "Gray";
            Status_Text = "Stand By";
            HeadingComp = 0;
            HeadingDisp = 0;
            AltDisp = "MSL_Alt:";
            Deviation = 0;
            ETADisp = "ETA:";
            Leg_GroundCourse_Text = "Crs: ";
            Course_Bug = 0;
            X_Start = 0;
            Y_Start = 0;
            X_End = 0;
            Y_End = 0;
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
            await Task.Delay(1000);
            if (LineSeq == null || LineSeq.Length < 1)
            {
                EnableFlyTo = true;
                Flyto_Back_Color = "DeepSkyBlue";
                return;
            }
            

            var line_seq = LineSeq.Split(',');
            
            Is_Sequence = line_seq.Length > 1;
            for (int i = 0; i < line_seq.Length; i++)
            {
                if (line_seq[i].Length < 1)
                {
                    EnableFlyTo = true;
                    Flyto_Back_Color = "DeepSkyBlue";
                    StatusColor = "Red";
                    Status_Text = "Invalid Line";
                    return;
                }
                // Run navigation routine
                await LineNavigate(line_seq[i]);
                IsReversed = false;

                // Cancle all lines if AbortAll is true
                if (AbortAll == true)
                {
                    AbortAll = false;
                    break; 
                }

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
            EnableFlyTo = true;
            Flyto_Back_Color = "DeepSkyBlue";
        }



    }
}
