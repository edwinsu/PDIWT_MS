﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using HCHXCodeQueryLib;
using PDIWT_MS_ZJCZL.Models;
using System.Linq;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class ViewModelCalculateByRay : ViewModelBase
    {        
        public ObservableCollection<PileInfoClass> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }

        public PileInfoClass CurrentPile
        {
            get { return GetProperty(() => CurrentPile); }
            set { SetProperty(() => CurrentPile, value); }
        }


        //[Command]
        //public void Calculate()
        //{
        //    CurrentPile.Result = CalculatePileCapacity.Calculate(CurrentPile.PileDiameter, CurrentPile.SoilInfo, CurrentPile.CalParameter);
        //    RaisePropertiesChanged("CurrentPile");
        //}
        [Command]
        public void AddPile()
        {
            var AddPileViewModel = new ViewAddPileViewModel();
            AddPileViewModel.Piles = Piles;
            var addpileview = new Views.ViewAddPile();
            addpileview.DataContext = AddPileViewModel;
            addpileview.ShowDialog();
        }

        [Command]
        public void RemovePile(PileInfoClass pileinfo)
        {
            Piles.Remove(pileinfo);
        }
        public bool CanRemovePile(PileInfoClass pileinfo)
        {
            return Piles.Count > 0;
        }
                
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            Piles = new ObservableCollection<PileInfoClass>
            {
                new PileInfoClass
                {
                    PileId = 924,
                    PileCode = "Test1",
                    PileDiameter = 10,
                    PileTypeInfo = PileType.Filling,
                    SoilInfo = new ObservableCollection<SoilInfoClass>()
                    {
                        new SoilInfoClass() { SoilLayerName="layer1", SoilLayerNum="0-0" },
                        new SoilInfoClass() { SoilLayerName="layer2", SoilLayerNum="0-1" }
                    },
                    CalParameter = new CalculateParameter { GammaR = 1.2 },
                    Result = new CalculateResult { UltimateBearingCapacity= 100, UltimatePullingCapacity =100 }
                },
                new PileInfoClass
                {
                    PileId = 100,
                    PileCode = "Test2",
                    PileDiameter = 12,
                    PileTypeInfo = PileType.PostgroutingFilling,
                    SoilInfo = new ObservableCollection<SoilInfoClass>()
                    {
                        new SoilInfoClass() { SoilLayerName="layer3", SoilLayerNum="1-0" },
                        new SoilInfoClass() { SoilLayerName="layer4", SoilLayerNum="1-1" }
                    },
                    CalParameter = new CalculateParameter { GammaR = 1.3 },
                    Result = new CalculateResult { UltimateBearingCapacity = 200, UltimatePullingCapacity = 200 }
                }
            };
            CurrentPile = Piles[0];
            //AddPileViewModel = new ViewAddPileViewModel();
            //((ISupportParameter)AddPileViewModel).Parameter = Piles;
        }
    }
}