clear;
close all;
clc;
wmclose all;

R = 6378137;

%% Mission parameters
start_wp_deg = [38.9807628, -107.9094315];
cross_track_dist_m = 300; 
line_length_m = 1000;
line_course = 274;
num_lines = 15;

%% Define starting conditions
avg_elevation_ft = 11000;
flight_alt_agl_m = 500;
flight_alt_agl_ft = flight_alt_agl_m * 3.281;

%% Pre calculations

line_course_deg = wrapTo360(90-line_course);

lat = start_wp_deg (1);

dkm_dlat = 111.13209 - 0.56605*cosd(2*lat) +0.0012*cosd(4*lat);

dkm_dlon = 111.41513 * cosd(lat) - 0.09455 * cosd (3*lat) + 0.00012 * cos (5*lat);

dm_dlat = dkm_dlat * 1000;

dm_dlon = dkm_dlon * 1000;

dlat_dm = 1/dm_dlat;

dlon_dm = 1/dm_dlon;



%% Offset from line length

len_offset_ns_m = sind(line_course_deg) * line_length_m;

len_offset_lat_deg = len_offset_ns_m * dlat_dm;

len_offset_ew_m = cosd(line_course_deg) * line_length_m;

len_offset_lon_deg = len_offset_ew_m * dlon_dm;



%% Offset from cross track distance

offset_dir = wrapTo360(-line_course);

x_offset_ns_m = sind(offset_dir) * cross_track_dist_m;

x_offset_lat_deg = x_offset_ns_m * dlat_dm;

x_offset_ew_m = cosd(offset_dir) * cross_track_dist_m;

x_offset_lon_deg = x_offset_ew_m * dlon_dm;



%% Create lines

result = zeros (num_lines,5);

result (1,1:2) = start_wp_deg;



for i = 2:num_lines

   result(i,1) = result(i-1,1) + x_offset_lat_deg; 

   result(i,2) = result(i-1,2) + x_offset_lon_deg;

end



result (:,3) = result(:,1) + len_offset_lat_deg;

result (:,4) = result(:,2) + len_offset_lon_deg;

result (:,5) = avg_elevation_ft + flight_alt_agl_ft;

%% Plot the plan for verification

wm = webmap('World Imagery');
for i = 1:num_lines
    s = geoshape([result(i,1),result(i,3)],[result(i,2),result(i,4)]);
    wmline(s,'Color', 'red', 'Width', 1);
end


%% Confirmation for file generation
answer = questdlg ('Create files?', 'Flight plan confirmation',...
    'Yes','Cancel','Cancel');
switch answer
    case 'Yes'
        file_name = inputdlg ('Filename:');
        file_name = file_name{:};
        file_name_kml = strcat (file_name,'.kml');
        file_name_mat = strcat (file_name,'.mat');
        
        % Generate KML file for google maps
        % Add header
        fid = fopen(file_name_kml,'w');
        fprintf(fid,'<kml>\n<Document>\n');
        fid = fclose(fid);
        for j = 1:num_lines
        fid = fopen(file_name_kml,'a');
        fprintf(fid,'<Placemark>\n<LineString>\n<coordinates>\n%f,%f,0,%f,%f,0\n</coordinates>\n</LineString>\n</Placemark>\n',...
            result(j,2),result(j,1),result(j,4),result(j,3));
        fid = fclose(fid);   
        end
        % Add footer
        fid = fopen(file_name_kml,'a');
        fprintf(fid,'</Document>\n</kml>');
        fid = fclose(fid);

        %Save mat with cordinates for line creation tools
        save (file_name_mat,'result');
        wmclose all;

    case 'Cancel'
        wmclose all;
end



