﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;
using BM = Bentley.MstnPlatformNET;

namespace PDIWT_MS
{
    internal class KeyinCommands
    {
        #region (PDIWT_MS Test) command
        public static void Test(string unparsed)
        {
            PDIWT_MS.Test.KeyinTest.TestDimsionElement();
            //PDIWT_MS.Test.KeyinTest.TestPileQuery();
        }

        public static void TestWPF(string unparsed)
        {
            PDIWT_MS.Test.wpfUC.ShowWindow(Program.Addin);
        }

        public static void TestWindowWPF(string unparsed)
        {
            PDIWT_MS.Test.WindowHost.ShowWindow();
        }
        public static void CreateSolid(string unparsed)
        {
            //    PDIWT_MS.Test.SolidTest.CreateDgnBox();
            //    PDIWT_MS.Test.SolidTest.CreateSphere();
            PDIWT_MS.Test.SolidTest.CreateExtrudedSolid();
        }
        #endregion


        #region (PDIWT_MS Tools) command
        public static void QuickInsert(string unparsed)
        {
            PDIWT_MS.Tools.View.WindowHost.ShowWindow();
        }

        public static void GetQuantities(string unparsed)
        {
            Tools.GetQuantitiesUC.ShowWindow(Program.Addin);
        }
        #endregion
    }
}
