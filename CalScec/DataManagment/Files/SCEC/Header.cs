﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalScec.DataManagment.Files.SCEC
{
    [Serializable]
    public class Header
    {
        #region Version data

        private int _sampleVersion = CalScec.Properties.Settings.Default.SampleVersion;
        public int SampleVersion
        {
            get
            {
                return this._sampleVersion;
            }
        }

        #endregion

        #region General settings

        public int ActPatternId;

        private double _usedWaveLength;
        public double UsedWaveLength
        {
            get
            {
                return this._usedWaveLength;
            }
        }

        #endregion

        #region Sample

        public string SampleName;
        public double SampleArea;

        public List<DataManagment.CrystalData.CODData> CrystalData = new List<DataManagment.CrystalData.CODData>();

        #endregion

        #region Peaks and regions

        public List<PatternInformation> ContainingPatterns = new List<PatternInformation>();

        #endregion

        #region MacroElasticity

        public List<Analysis.Stress.Macroskopic.TensileTest> TensileTests = new List<Analysis.Stress.Macroskopic.TensileTest>();

        public List<MacroElasticInformation> MacroElasticData = new List<MacroElasticInformation>();

        #endregion

        #region MicroElasticity

        public List<List<REKInformation>> DiffractionConstants = new List<List<REKInformation>>();

        public List<ElasticityTensorInformation> VoigtTensorInformation = new List<ElasticityTensorInformation>();
        public List<ElasticityTensorInformation> ReussTensorInformation = new List<ElasticityTensorInformation>();
        public List<ElasticityTensorInformation> HillTensorInformation = new List<ElasticityTensorInformation>();
        public List<ElasticityTensorInformation> KroenerTensorInformation = new List<ElasticityTensorInformation>();
        public List<ElasticityTensorInformation> DeWittTensorInformation = new List<ElasticityTensorInformation>();
        public List<ElasticityTensorInformation> GeometricHillTensorInformation = new List<ElasticityTensorInformation>();

        #endregion

        #region Texture
        //Altes Zeug (kann eigentlich weg)
        List<ODFDataInformation> ODFData = new List<ODFDataInformation>();

        public List<ODFInformation> ODFList = new List<ODFInformation>();
        #endregion

        #region EPSC

        public List<Analysis.Stress.Plasticity.ElastoPlasticExperiment> SimulationData = new List<Analysis.Stress.Plasticity.ElastoPlasticExperiment>();
        public List<SimulatedExperimentInformation> SimulationInformation = new List<SimulatedExperimentInformation>();

        public List<PlasticityTensorInformation> PlasticTensor = new List<PlasticityTensorInformation>();

        #endregion

        public List<MathNet.Numerics.LinearAlgebra.Matrix<double>> StressTransitionFactors = new List<MathNet.Numerics.LinearAlgebra.Matrix<double>>();

        public Header(Analysis.Sample sample)
        {
            this.ActPatternId = sample.ActualPatterId;
            this.SampleArea = sample.Area;
            this.SampleName = sample.Name;
            this._usedWaveLength = CalScec.Properties.Settings.Default.UsedWaveLength;
            this.TensileTests = sample.TensileTests;
            
            for (int n = 0; n < sample.CrystalData.Count; n++)
            {
                this.CrystalData.Add(new DataManagment.CrystalData.CODData(sample.CrystalData[n]));

                this.DiffractionConstants.Add(new List<REKInformation>());
                for(int i = 0; i < sample.DiffractionConstants[n].Count; i++)
                {
                    this.DiffractionConstants[n].Add(new REKInformation(sample.DiffractionConstants[n][i]));
                }

                this.VoigtTensorInformation.Add(new ElasticityTensorInformation(sample.VoigtTensorData[n]));
                this.ReussTensorInformation.Add(new ElasticityTensorInformation(sample.ReussTensorData[n]));
                this.HillTensorInformation.Add(new ElasticityTensorInformation(sample.HillTensorData[n]));
                this.KroenerTensorInformation.Add(new ElasticityTensorInformation(sample.KroenerTensorData[n]));
                this.DeWittTensorInformation.Add(new ElasticityTensorInformation(sample.DeWittTensorData[n]));
                this.GeometricHillTensorInformation.Add(new ElasticityTensorInformation(sample.DeWittTensorData[n]));

                try
                {
                    this.ODFList.Add(new ODFInformation(sample.ODFList[n]));
                }
                catch
                {

                }

                //if (sample.HillTensorData[n].ODF != null)
                //{
                //    ODFDataInformation ODFDataTmp = new ODFDataInformation();
                //    ODFDataTmp.TDData = sample.HillTensorData[n].ODF.TDData;
                //    ODFDataTmp.CrystalData = sample.CrystalData[n];

                //    this.ODFData.Add(ODFDataTmp);
                //}

                if (sample.PlasticTensor.Count != 0)
                {
                    this.PlasticTensor.Add(new PlasticityTensorInformation(sample.PlasticTensor[n]));
                }
                else
                {
                    this.PlasticTensor = null;
                }
            }

            //this.SimulationData = sample.SimulationData;
            //for(int n = 0; n < sample.SimulationData.Count; n++)
            //{
            //    this.SimulationInformation.Add(new SimulatedExperimentInformation(sample.SimulationData[n]));
            //}

            for (int n = 0; n < sample.MacroElasticData.Count; n++)
            {
                this.MacroElasticData.Add(new MacroElasticInformation(sample.MacroElasticData[n]));
            }

            for (int n = 0; n < sample.DiffractionPatterns.Count; n++)
            {
                this.ContainingPatterns.Add(new PatternInformation(sample.DiffractionPatterns[n]));
            }

            this.StressTransitionFactors = sample.StressTransitionFactors;
        }

        public Analysis.Sample GetSample()
        {
            Analysis.Sample Ret = new Analysis.Sample(this.ActPatternId);

            Ret.Name = this.SampleName;
            Ret.Area = this.SampleArea;

            if(this.TensileTests != null)
            {
                Ret.TensileTests = this.TensileTests;
            }

            CalScec.Properties.Settings.Default.UsedWaveLength = this._usedWaveLength;
            //Ret.SimulationData = this.SimulationData;
            if (this.SimulationInformation != null)
            {
                for (int n = 0; n < this.SimulationInformation.Count; n++)
                {
                    Ret.SimulationData.Add(this.SimulationInformation[n].GetElastoPlasticExperiment());
                }
            }
            if (this.StressTransitionFactors == null)
            {
                for (int n = 0; n < this.CrystalData.Count; n++)
                {
                    MathNet.Numerics.LinearAlgebra.Matrix<double> tmp = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0.0);
                    Ret.StressTransitionFactors.Add(tmp);
                }
            }
            else if(this.StressTransitionFactors.Count != this.CrystalData.Count)
            {
                for (int n = 0; n < this.CrystalData.Count; n++)
                {
                    MathNet.Numerics.LinearAlgebra.Matrix<double> tmp = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0.0);
                    Ret.StressTransitionFactors.Add(tmp);
                }
            }
            else
            {
                Ret.StressTransitionFactors = this.StressTransitionFactors;
            }

            for (int n = 0; n < Ret.SimulationData.Count; n++)
            {
                if(Ret.SimulationData[n].YieldInformation == null)
                {
                    Ret.SimulationData[n].YieldInformation = new List<Analysis.Stress.Plasticity.YieldSurface>();
                }

                if (Ret.SimulationData[n].YieldInformation.Count == 0)
                {
                    for (int i = 0; i < this.CrystalData.Count; i++)
                    {
                        Ret.SimulationData[n].YieldInformation.Add(new Analysis.Stress.Plasticity.YieldSurface(this.CrystalData[i]));
                    }
                }
                if(Ret.SimulationData[n]._hardenningTensor == null)
                {
                    Ret.SimulationData[n]._hardenningTensor = new List<MathNet.Numerics.LinearAlgebra.Matrix<double>>();
                }
                if(Ret.SimulationData[n]._hardenningTensor.Count == 0)
                {
                    for (int i = 0; i < this.CrystalData.Count; i++)
                    {
                        MathNet.Numerics.LinearAlgebra.Matrix<double> tmp = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(3, 3, 0.0);
                        tmp[0, 0] = 1;
                        tmp[1, 1] = 1;
                        tmp[2, 2] = 1;
                        Ret.SimulationData[n]._hardenningTensor.Add(tmp);
                    }
                }
            }

            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                Ret.CrystalData.Add(new DataManagment.CrystalData.CODData(CrystalData[n]));

                if(Ret.CrystalData[n].inclusionDimension == null)
                {
                    double[] tmp = { 0.0, 0.0, 0.0 };
                    Ret.CrystalData[n].inclusionDimension = tmp;
                    Ret.CrystalData[n].Matrix = true;
                }
                string SymString = "";
                try
                {
                    Ret.PlasticTensor.Add(this.PlasticTensor[n].GetPlastticityTensor());
                }
                catch
                {
                    Ret.PlasticTensor.Add(new Analysis.Stress.Plasticity.PlasticityTensor());
                    Ret.PlasticTensor[n].YieldSurfaceData = new Analysis.Stress.Plasticity.YieldSurface(this.CrystalData[n]);
                }
                try
                {
                    Ret.VoigtTensorData.Add(this.VoigtTensorInformation[n].GetElasticityTensor());
                    SymString = this.VoigtTensorInformation[n]._symmetry;
                }
                catch
                {
                    Ret.VoigtTensorData.Add(new Analysis.Stress.Microsopic.ElasticityTensors());
                    Ret.VoigtTensorData[n].Symmetry = SymString;
                }
                try
                {
                    Ret.ReussTensorData.Add(this.ReussTensorInformation[n].GetElasticityTensor());
                    SymString = this.ReussTensorInformation[n]._symmetry;
                }
                catch
                {
                    Ret.ReussTensorData.Add(new Analysis.Stress.Microsopic.ElasticityTensors());
                    Ret.ReussTensorData[n].Symmetry = SymString;
                }
                try
                {
                    Ret.HillTensorData.Add(this.HillTensorInformation[n].GetElasticityTensor());
                    SymString = this.HillTensorInformation[n]._symmetry;
                }
                catch
                {
                    Ret.HillTensorData.Add(new Analysis.Stress.Microsopic.ElasticityTensors());
                    Ret.HillTensorData[n].Symmetry = SymString;
                }
                try
                {
                    Ret.KroenerTensorData.Add(this.KroenerTensorInformation[n].GetElasticityTensor());
                    SymString = this.KroenerTensorInformation[n]._symmetry;
                }
                catch
                {
                    Ret.KroenerTensorData.Add(new Analysis.Stress.Microsopic.ElasticityTensors());
                    Ret.KroenerTensorData[n].Symmetry = SymString;
                }
                try
                {
                    Ret.DeWittTensorData.Add(this.DeWittTensorInformation[n].GetElasticityTensor());
                    SymString = this.DeWittTensorInformation[n]._symmetry;
                }
                catch
                {
                    Ret.DeWittTensorData.Add(new Analysis.Stress.Microsopic.ElasticityTensors());
                    Ret.DeWittTensorData[n].Symmetry = SymString;
                }
                try
                {
                    Ret.GeometricHillTensorData.Add(this.GeometricHillTensorInformation[n].GetElasticityTensor());
                    SymString = this.GeometricHillTensorInformation[n]._symmetry;
                }
                catch
                {
                    Ret.GeometricHillTensorData.Add(new Analysis.Stress.Microsopic.ElasticityTensors());
                    Ret.GeometricHillTensorData[n].Symmetry = SymString;
                }

                Ret.DiffractionConstants.Add(new List<Analysis.Stress.Microsopic.REK>());
                try
                {
                    for (int i = 0; i < this.DiffractionConstants[n].Count; i++)
                    {
                        Ret.DiffractionConstants[n].Add(this.DiffractionConstants[n][i].GetREK(Ret.CrystalData[n]));
                    }
                }
                catch
                {

                }
                Ret.DiffractionConstantsTexture.Add(new List<Analysis.Stress.Microsopic.REK>());
            }

            if(this.ODFList != null && this.ODFList.Count != 0)
            {
                for (int n = 0; n < this.ODFList.Count; n++)
                {
                    Ret.ODFList.Add(this.ODFList[n].GetODF());

                    Ret.ODFList[n].SetStepSizes();

                    Ret.VoigtTensorData[n].ODF = Ret.ODFList[n];
                    Ret.ReussTensorData[n].ODF = Ret.ODFList[n];
                    Ret.HillTensorData[n].ODF = Ret.ODFList[n];
                    Ret.GeometricHillTensorData[n].ODF = Ret.ODFList[n];
                    Ret.KroenerTensorData[n].ODF = Ret.ODFList[n];
                    Ret.DeWittTensorData[n].ODF = Ret.ODFList[n];
                }
            }

            //if (this.ODFData != null && this.ODFData.Count != 0)
            //{
            //    for (int n = 0; n < this.CrystalData.Count; n++)
            //    {
            //        for(int i = 0; i < this.ODFData.Count; i++)
            //        {
            //            if (this.CrystalData[n].SymmetryGroupID != -1)
            //            {
            //                if (this.CrystalData[n].SymmetryGroupID == this.ODFData[i].CrystalData.SymmetryGroupID)
            //                {
            //                    Ret.VoigtTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.ReussTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.HillTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.GeometricHillTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.KroenerTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.DeWittTensorData[n].ODF.TDData = this.ODFData[i].TDData;

            //                    Ret.VoigtTensorData[n].ODF.SetStepSizes();
            //                    Ret.ReussTensorData[n].ODF.SetStepSizes();
            //                    Ret.HillTensorData[n].ODF.SetStepSizes();
            //                    Ret.GeometricHillTensorData[n].ODF.SetStepSizes();
            //                    Ret.KroenerTensorData[n].ODF.SetStepSizes();
            //                    Ret.DeWittTensorData[n].ODF.SetStepSizes();
            //                }
            //            }
            //            else
            //            {
            //                if (this.CrystalData[n].SymmetryGroup == this.ODFData[i].CrystalData.SymmetryGroup)
            //                {
            //                    Ret.VoigtTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.ReussTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.HillTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.GeometricHillTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.KroenerTensorData[n].ODF.TDData = this.ODFData[i].TDData;
            //                    Ret.DeWittTensorData[n].ODF.TDData = this.ODFData[i].TDData;

            //                    Ret.VoigtTensorData[n].ODF.SetStepSizes();
            //                    Ret.ReussTensorData[n].ODF.SetStepSizes();
            //                    Ret.HillTensorData[n].ODF.SetStepSizes();
            //                    Ret.GeometricHillTensorData[n].ODF.SetStepSizes();
            //                    Ret.KroenerTensorData[n].ODF.SetStepSizes();
            //                    Ret.DeWittTensorData[n].ODF.SetStepSizes();
            //                }
            //            }
            //        }
            //    }
            //}

            if (this.ContainingPatterns != null)
            {
                for (int n = 0; n < this.ContainingPatterns.Count; n++)
                {
                    Ret.DiffractionPatterns.Add(ContainingPatterns[n].GetDiffractionPattern());
                }
            }

            if (this.MacroElasticData != null)
            {
                for (int n = 0; n < this.MacroElasticData.Count; n++)
                {
                    Analysis.Stress.Macroskopic.Elasticity ElTmp = new Analysis.Stress.Macroskopic.Elasticity(this.MacroElasticData[n]);
                    ElTmp.Clear();

                    for (int i = 0; i < this.MacroElasticData[n].Count; i++)
                    {
                        for (int j = 0; j < Ret.DiffractionPatterns.Count; j++)
                        {
                            bool PeakNewlyAssociated = false;
                            for (int k = 0; k < Ret.DiffractionPatterns[j].FoundPeaks.Count; k++)
                            {
                                if (this.MacroElasticData[n][i].DPeak.PeakId == Ret.DiffractionPatterns[j].FoundPeaks[k].PeakId)
                                {
                                    ElTmp.Add(new Analysis.Stress.Macroskopic.PeakStressAssociation(this.MacroElasticData[n][i].Stress, this.MacroElasticData[n][i].PsiAngle, Ret.DiffractionPatterns[j].FoundPeaks[k], this.MacroElasticData[n][i].PsiAngle));
                                    PeakNewlyAssociated = true;
                                    break;
                                }
                            }
                            if (PeakNewlyAssociated)
                            {
                                break;
                            }
                        }
                    }

                    Ret.MacroElasticData.Add(ElTmp);
                }
            }

            return Ret;
        }
        
    }
}
