using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    public class CrashSeverityData
    {

        public float route { get; set; }
        public float milepoint { get; set; }
        public float lat_utm_y { get; set; }
        public float long_utm_x { get; set; }
        public float pedestrian_involved { get; set; }
        public float motorcycle_involved { get; set; }
        public float intersection_related { get; set; }

        public float overturn_rollover { get; set; }
        public float older_driver_involved { get; set; }
        public float night_dark_condition { get; set; }
        public float distracted_driving { get; set; }

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                route, milepoint, lat_utm_y, long_utm_x, pedestrian_involved, motorcycle_involved,
                intersection_related, overturn_rollover
                , older_driver_involved, night_dark_condition, distracted_driving
            };
            int[] dimensions = new int[] { 1, 11 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
