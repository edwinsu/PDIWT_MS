﻿using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.GeometryNET;

using BDDE = Bentley.DgnPlatformNET.DgnEC;
using BES = Bentley.ECObjects.Schema;
using BEI = Bentley.ECObjects.Instance;
using BDEPQ = Bentley.EC.Persistence.Query;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace PDIWT_MS_PiledWharf.ViewModels
{
    using Interface;
    using Models;
    using Models.Soil;
    using Models.Piles;
    using Models.Piles.CrossSection;
    using Bentley.MstnPlatformNET;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            //_Piles = new ObservableCollection<PileBase>()
            //{
            //    new SolidPile()
            //    {
            //        TopPoint = new Bentley.GeometryNET.DPoint3d(0, 0, 0),
            //        BottomPoint = new DPoint3d(0, 0, -100),
            //        Code = "Pile1",
            //        ID = 2120,
            //        ICrossSection = new SquareCrossSection(0.5)
            //    },
            //    new SolidPile()
            //    {
            //        TopPoint = new DPoint3d(10, 15, 0),
            //        BottomPoint = new Bentley.GeometryNET.DPoint3d(10, 10, -80),
            //        Code = "Pile2",
            //        ID = 333,
            //        ICrossSection = new SquareCrossSection(0.6)
            //    },
            //};
            //if (_Piles.Count > 0)
            //    _SelectedPile = _Piles.First();
            _GammaR = 1.2;
            _Eta = 0;
            _WaterLevel = 0;
        }

        #region Properties
        private ObservableCollection<PileBase> _Piles = new ObservableCollection<PileBase>();
        public ObservableCollection<PileBase> Piles
        {
            get { return _Piles; }
            set { Set(ref _Piles, value); }
        }

        private PileBase _SelectedPile;
        public PileBase SelectedPile
        {
            get { return _SelectedPile; }
            set
            {
                Set(ref _SelectedPile, value);
                RaisePropertyChanged(() => SelectedPile_pilepiecesoilinfo);
                RaisePropertyChanged(() => SelectedPile_Qr);
                RaisePropertyChanged(() => SelectedPile_Qd);
                RaisePropertyChanged(() => SelectedPile_Td);
            }
        }

        public ObservableCollection<PilePieceInSoilLayerInfo> SelectedPile_pilepiecesoilinfo
        {
            get { return _SelectedPile?.GetPilePieceInEachSoilLayerInfos(); }
        }

        public double? SelectedPile_Qr
        {
            get
            {
                return _SelectedPile?.GetPilePieceInEachSoilLayerInfos().Last().CurrentSoilLayerInfo.Qri;
            }
        }
        public double? SelectedPile_Qd
        {
            get
            {
                return _SelectedPile?.CalculateQd(_GammaR, _Eta);
            }
        }

        public double? SelectedPile_Td
        {
            get
            {
                return _SelectedPile?.CalculateTd(_GammaR, _WaterLevel);
            }
        }

        private double _GammaR;
        public double GammaR
        {
            get { return _GammaR; }
            set
            {
                Set(ref _GammaR, value);
                RaisePropertyChanged(() => SelectedPile_Qd);
                RaisePropertyChanged(() => SelectedPile_Td);
            }
        }
        private double _Eta;
        public double Eta
        {
            get { return _Eta; }
            set { Set(ref _Eta, value); }
        }



        private double _WaterLevel;
        public double WaterLevel
        {
            get { return _WaterLevel; }
            set
            {
                Set(ref _WaterLevel, value);
                RaisePropertyChanged(() => SelectedPile_Qd);
                RaisePropertyChanged(() => SelectedPile_Td);
            }
        }
        #endregion

        #region Commands
        private RelayCommand _GetPilesInfoFromDgnCommand;
        public RelayCommand GetPilesInfoFromDgnCommand => _GetPilesInfoFromDgnCommand ?? (_GetPilesInfoFromDgnCommand = new RelayCommand(ExecuteGetPilesInfoFromDgnCommand));
        public void ExecuteGetPilesInfoFromDgnCommand()
        {
            BDDE.FindInstancesScope scope = BDDE.FindInstancesScope.CreateScope(Program.GetActiveDgnFile(), new BDDE.FindInstancesScopeOption(BDDE.DgnECHostType.All));
            BES.IECSchema schema = BDDE.DgnECManager.Manager.LocateSchemaInScope(scope, "PDIWT_Wharf", 1, 0, BES.SchemaMatchType.Exact);
            if (schema == null)
            {
                MessageCenter.Instance.ShowErrorMessage("无法定位\"PDIWT_Wharfschema\"", "", false);
                return;
            }
            BES.IECClass pileecclass = schema.GetClass("Pile");
            if (pileecclass == null)
            {
                MessageCenter.Instance.ShowErrorMessage("无法定位\"Pile\"Class", "", false);
                return;
            }

            ScanCriteria sc = new ScanCriteria();
            BitMask bmtype = new BitMask(false);
            bmtype.Set((uint)MSElementType.Line - 1);
            bmtype.EnsureCapacity(112);
            sc.SetElementTypeTest(bmtype);
            sc.SetModelRef(Program.GetActiveDgnModelRef());
            sc.SetModelSections(DgnModelSections.GraphicElements);

            int _count = 0;
            sc.Scan((ele, model) =>
            {
                BDDE.FindInstancesScope ele_scope = BDDE.FindInstancesScope.CreateScope(ele, new BDDE.FindInstancesScopeOption(BDDE.DgnECHostType.All));
                BDEPQ.ECQuery query = new BDEPQ.ECQuery(pileecclass);
                query.SelectClause.SelectAllProperties = true;
                using (BDDE.DgnECInstanceCollection ecInstances = BDDE.DgnECManager.Manager.FindInstances(ele_scope, query))
                {
                    if (ecInstances != null && ecInstances.Count() != 0)
                    {
                        BDDE.IDgnECInstance _pileecinstance = ecInstances.First();
                        LineElement _line = ele as LineElement;
                        CurveVector _cv = _line.GetCurveVector();
                        DPoint3d _top, _bottom;
                        _cv.GetStartEnd(out _top, out _bottom);
                        PileFactory _pilefactory = new PileFactory(_top, _bottom, _line.ElementId, _pileecinstance);
                        _Piles.Add(_pilefactory.Create());
                        _count++;
                    }
                }
                return StatusInt.Success;
            });
            if (_count > 0)
                MessageBox.Show($"共提取{_count}根桩", "提取成功", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("无法提取当前dgn模型中的桩轴线\n或模型中不存在桩轴线", "提取失败", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private RelayCommand _RemovePileCommand;
        public RelayCommand RemovePileCommand => _RemovePileCommand ?? (_RemovePileCommand = new RelayCommand(() => _Piles.Remove(_SelectedPile), () => _SelectedPile != null && _Piles.Count > 0));

        private RelayCommand _RemoveAllPileCommand;
        public RelayCommand RemoveAllPileCommand => _RemoveAllPileCommand ?? (_RemoveAllPileCommand = new RelayCommand(() => _Piles.Clear(), () => _Piles.Count > 0));

        private RelayCommand _OutputCalculationSheetCommand;
        public RelayCommand OutputCalculationSheetCommand => _OutputCalculationSheetCommand ?? (_OutputCalculationSheetCommand = new RelayCommand(ExecuteOutputCalculationSheetCommand));
        public void ExecuteOutputCalculationSheetCommand()
        {
            SaveFileDialog svdlg = new SaveFileDialog()
            {
                Title = "输出计算书",
                Filter = Properties.Resources.ExcelFilter                
            };
            try
            {
                if(svdlg.ShowDialog() == true)
                {
                    if (File.Exists(svdlg.FileName))
                        File.Delete(svdlg.FileName);
                    OutputCalculationSheet outputsheet = new OutputCalculationSheet(_Piles, _GammaR, _Eta, _WaterLevel);
                    outputsheet.Export(svdlg.FileName);
                    MessageBox.Show($"计算书保存至{svdlg.FileName}", "计算书生成成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private RelayCommand _DrawPilePositionMapCommand;
        public RelayCommand DrawPilePositionMapCommand => _DrawPilePositionMapCommand ?? (_DrawPilePositionMapCommand = new RelayCommand(ExecuteDrawPilePositionMapCommand));
        public void ExecuteDrawPilePositionMapCommand()
        {
            PilePositionMap posMap = new PilePositionMap(_Piles);
            posMap.CreateMap();
        }

        private RelayCommand _Test;
        public RelayCommand Test => _Test ?? (_Test = new RelayCommand(ExecuteTest));
        public void ExecuteTest()
        {
            //Models.IntersectionPointQuery _test = new Models.IntersectionPointQuery();
            //var _list = _test.FindBSElement();
            //var _pointquery = new  PDIWT_MS_PiledWharf_CPP.PointQuery();
            //var _pileinfo = _pointquery.GetPilePieceSoilLayerInfos(new Bentley.GeometryNET.DPoint3d(0, 0, 0), new Bentley.GeometryNET.DPoint3d(0, 0, -1e8));
            //var _res = _pointquery.QueryResult;
            //MessageBox.Show(Enum.GetName(typeof(PDIWT_MS_PiledWharf_CPP.QueryResultStatus), _res));
            var _pileinfo = _SelectedPile.GetPilePieceInEachSoilLayerInfos();
            StringBuilder _sb = new StringBuilder();
            foreach (var _pile in _pileinfo)
            {
                _sb.Append(String.Format("Name:{0} topz:{1} bottomz:{2} length:{3}\n", _pile.CurrentSoilLayerInfo.SoilLayerNum, _pile.PileTopZ_InCurrentSoilLayer, _pile.PileBottomZ_InCurrentSoilLayer, _pile.PilePieceLength));
            }
            MessageBox.Show(_sb.ToString());
        }
        #endregion


    }
}