﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IPileProperty 
{
	double PileDiameter { get;set; }

	double PileInnerDiameter { get;set; }

	Point3d PileTopPoint { get;set; }

	Point3d PileBottomPoint { get;set; }

	double WaterLevel { get;set; }

	double PileWeight { get;set; }

	double PileUnderWaterWeight { get;set; }

	double GetPilePerimeter();

	double GetPileLength();

	double GetCosAlpha();

	double GetPileCrossSectionArea();

	double GetPileGravity();

}

