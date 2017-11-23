﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_PiledWharf.Models
{
    public class DrawAxisFromExcel
    {

    }

    public class PileAxisInfo
    {
        public string GridHorizontal { get; set; }
        public string GridVertical { get; set; }
        public double TopX { get; set; }
        public double TopY { get; set; }
        public double TopZ { get; set; }
        public double Skewness { get; set; }
        public double RotationDegree { get; set; }
        public double Length { get; set; } //unit mm
        
        public string Type { get; set; }
        public string CrossSectionType { get; set; }

        public double SideLength { get; set; }
        public double InnerDiameter { get; set; }
        public double Weight { get; set; }
        public double UnderWaterWeight { get; set; }
    }
}
