using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    [Table("accidenaccidentdatadt")]
    //[Table("Utah_Crash_Data_2020")]
    public class Accident
    {
        [Key]
        [Required]
        public int CRASH_ID {get; set;}
        public DateTime CRASH_DATETIME { get; set; }
        //public string CRASH_DATETIME { get; set; }
        public int ROUTE { get; set; }
        public double MILEPOINT { get; set; }
        public double LAT_UTM_Y { get; set; }
        public double LONG_UTM_X { get; set; }
        public string MAIN_ROAD_NAME { get; set; }
        public string CITY { get; set; }
        public string COUNTY_NAME { get; set; }
        public int CRASH_SEVERITY_ID { get; set; }
        public string WORK_ZONE_RELATED { get; set; }
        public string PEDESTRIAN_INVOLVED { get; set; }
        public string BICYCLIST_INVOLVED { get; set; }
        public string MOTORCYCLE_INVOLVED { get; set; }
        public string IMPROPER_RESTRAINT { get; set; }
        public string UNRESTRAINED { get; set; }
        public string DUI { get; set; }
        public string INTERSECTION_RELATED { get; set; }
        public string WILD_ANIMAL_RELATED { get; set; }
        public string DOMESTIC_ANIMAL_RELATED { get; set; }
        public string OVERTURN_ROLLOVER { get; set; }
        public string COMMERCIAL_MOTOR_VEH_INVOLVED { get; set; }
        public string TEENAGE_DRIVER_INVOLVED { get; set; }
        public string OLDER_DRIVER_INVOLVED { get; set; }
        public string NIGHT_DARK_CONDITION { get; set; }
        public string SINGLE_VEHICLE { get; set; }
        public string DISTRACTED_DRIVING { get; set; }
        public string DROWSY_DRIVING { get; set; }
        public string ROADWAY_DEPARTURE { get; set; }
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