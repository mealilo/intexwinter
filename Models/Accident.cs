using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    public class Accident
    {
        [Key]
        [Required]
        public int CRASH_ID {get; set;}
        public DateTime CRASH_DATETIME { get; set; } 
        public int ROUTE { get; set; }
        public double MILEPOINT { get; set; }
        public double LAT_UTM_Y { get; set; }
        public double LONG_UTM_X { get; set; }
        public string MAIN_ROAD_NAME { get; set; }
        public string CITY { get; set; }
        public string COUNTY_NAME { get; set; }
        public int CRASH_SEVERITY_ID { get; set; }
        public bool WORK_ZONE_RELATED { get; set; }
        public bool PEDESTRIAN_INVOLVED { get; set; }
        public bool BICYCLIST_INVOLVED { get; set; }
        public bool MOTORCYCLE_INVOLVED { get; set; }
        public bool IMPROPER_RESTRAINT { get; set; }
        public bool UNRESTRAINED { get; set; }
        public bool DUI { get; set; }
        public bool INTERSECTION_RELATED { get; set; }
        public bool WILD_ANIMAL_RELATED { get; set; }
        public bool DOMESTIC_ANIMAL_RELATED { get; set; }
        public bool OVERTURN_ROLLOVER { get; set; }
        public bool COMMERCIAL_MOTOR_VEH_INVOLVED { get; set; }
        public bool TEENAGE_DRIVER_INVOLVED { get; set; }
        public bool OLDER_DRIVER_INVOLVED { get; set; }
        public bool NIGHT_DARK_CONDITION { get; set; }
        public bool SINGLE_VEHICLE { get; set; }
        public bool DISTRACTED_DRIVING { get; set; }
        public bool DROWSY_DRIVING { get; set; }
        public bool ROADWAY_DEPARTURE { get; set; }
    }
}


//Table: Utah_Crash_Data_2020
//Columns:
//CRASH_ID int
//CRASH_DATETIME text 
//ROUTE int 
//MILEPOINT double 
//LAT_UTM_Y double 
//LONG_UTM_X double 
//MAIN_ROAD_NAME text 
//CITY text 
//COUNTY_NAME text 
//CRASH_SEVERITY_ID int 
//WORK_ZONE_RELATED text 
//PEDESTRIAN_INVOLVED text 
//BICYCLIST_INVOLVED text 
//MOTORCYCLE_INVOLVED text 
//IMPROPER_RESTRAINT text 
//UNRESTRAINED text 
//DUI text 
//INTERSECTION_RELATED text 
//WILD_ANIMAL_RELATED text 
//DOMESTIC_ANIMAL_RELATED text 
//OVERTURN_ROLLOVER text 
//COMMERCIAL_MOTOR_VEH_INVOLVED text 
//TEENAGE_DRIVER_INVOLVED text 
//OLDER_DRIVER_INVOLVED text 
//NIGHT_DARK_CONDITION text 
//SINGLE_VEHICLE text 
//DISTRACTED_DRIVING text 
//DROWSY_DRIVING text 
//ROADWAY_DEPARTURE text