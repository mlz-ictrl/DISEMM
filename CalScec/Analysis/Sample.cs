﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalScec.Analysis
{
    public class Sample : ICloneable
    {
        private string _name;
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        private double _area;
        public double Area
        {
            get
            {
                return this._area;
            }
            set
            {
                this._area = value;
            }
        }

        private int _actualPatternId;
        public int ActualPatterId
        {
            get
            {
                this._actualPatternId++;
                return this._actualPatternId;
            }
        }

        public List<DataManagment.CrystalData.CODData> CrystalData = new List<DataManagment.CrystalData.CODData>();

        public List<Pattern.DiffractionPattern> DiffractionPatterns = new List<Pattern.DiffractionPattern>();

        #region Macro elastic calculations

        public List<Stress.Macroskopic.TensileTest> TensileTests = new List<Stress.Macroskopic.TensileTest>();

        public List<Stress.Macroskopic.Elasticity> MacroElasticData = new List<Stress.Macroskopic.Elasticity>();

        public List<Tools.BulkElasticPhaseEvaluations> AveragedEModulStandard()
        {
            List<Tools.BulkElasticPhaseEvaluations> Ret = new List<Tools.BulkElasticPhaseEvaluations>();

            for (int i = 0; i < this.CrystalData.Count; i++)
            {
                Tools.BulkElasticPhaseEvaluations Tmp = new Tools.BulkElasticPhaseEvaluations();
                Tmp.HKLPase = this.CrystalData[i].SymmetryGroup;

                int count = 0;
                double ret = 0.0;

                for (int n = 0; n < MacroElasticData.Count; n++)
                {
                    if (MacroElasticData[n][0].DPeak.AssociatedCrystalData.SymmetryGroup == this.CrystalData[i].SymmetryGroup)
                    {
                        if (MacroElasticData[n][0].PsiAngle == 0)
                        {
                            count++;
                            ret += MacroElasticData[n].EModul;
                        }
                    }
                }
                if (count == 0)
                {
                    count = 1;
                }
            
                Tmp.PsiAngle = 0;
                Tmp.BulkElasticity = ret / count;

                ret = 0.0;
                for (int n = 0; n < MacroElasticData.Count; n++)
                {
                    if (MacroElasticData[n][0].DPeak.AssociatedCrystalData.SymmetryGroup == this.CrystalData[i].SymmetryGroup)
                    {
                        if (MacroElasticData[n][0].PsiAngle == 0)
                        {
                            ret += Math.Pow(Tmp.BulkElasticity - MacroElasticData[n].EModul, 2);

                        }
                    }
                }

                Tmp.BulkElasticityError = Math.Sqrt(ret / MacroElasticData.Count);

                Ret.Add(Tmp);
            }

            return Ret;
        }

        public List<Tools.BulkElasticPhaseEvaluations> AveragedPossionNumberStandard()
        {
            List<Tools.BulkElasticPhaseEvaluations> Ret = new List<Tools.BulkElasticPhaseEvaluations>();

            for (int i = 0; i < this.CrystalData.Count; i++)
            {
                Tools.BulkElasticPhaseEvaluations Tmp = new Tools.BulkElasticPhaseEvaluations();
                Tmp.HKLPase = this.CrystalData[i].SymmetryGroup;

                int count = 0;
                double ret = 0.0;

                for (int n = 0; n < MacroElasticData.Count; n++)
                {
                    if (MacroElasticData[n][0].DPeak.AssociatedCrystalData.SymmetryGroup == this.CrystalData[i].SymmetryGroup)
                    {
                        if (MacroElasticData[n][0].PsiAngle == 90)
                        {
                            count++;
                            ret += MacroElasticData[n].EModul;
                        }
                    }
                }
                if (count == 0)
                {
                    count = 1;
                }

                Tmp.PsiAngle = 90;
                Tmp.BulkElasticity = ret / count;

                ret = 0.0;

                for (int n = 0; n < MacroElasticData.Count; n++)
                {
                    if (MacroElasticData[n][0].DPeak.AssociatedCrystalData.SymmetryGroup == this.CrystalData[i].SymmetryGroup)
                    {
                        if (MacroElasticData[n][0].PsiAngle == 90)
                        {
                            ret += Math.Pow(Tmp.BulkElasticity - MacroElasticData[n].EModul, 2);

                        }
                    }
                }

                Tmp.BulkElasticityError = Math.Sqrt(ret / MacroElasticData.Count);

                Ret.Add(Tmp);
            }

            return Ret;
        }

        public List<Tools.BulkElasticPhaseEvaluations> AveragedEModulWeighted()
        {
            List<Tools.BulkElasticPhaseEvaluations> Ret = new List<Tools.BulkElasticPhaseEvaluations>();

            for (int i = 0; i < this.CrystalData.Count; i++)
            {
                Tools.BulkElasticPhaseEvaluations Tmp = new Tools.BulkElasticPhaseEvaluations();
                Tmp.HKLPase = this.CrystalData[i].SymmetryGroup;

                double TotalWeight = 0;
                double ret = 0.0;

                for (int n = 0; n < MacroElasticData.Count; n++)
                {
                    if (MacroElasticData[n][0].DPeak.AssociatedCrystalData.SymmetryGroup == this.CrystalData[i].SymmetryGroup)
                    {
                        if (MacroElasticData[n][0].PsiAngle == 0)
                        {
                            TotalWeight += MacroElasticData[n].EModulError;
                            ret += MacroElasticData[n].EModul / MacroElasticData[n].EModulError;
                        }
                    }
                }

                Tmp.PsiAngle = 0;
                Tmp.BulkElasticity = ret / TotalWeight;
                Tmp.BulkElasticityError = 0.0;

                Ret.Add(Tmp);
            }

            return Ret;
        }

        public List<Tools.BulkElasticPhaseEvaluations> AveragedPossionNumberWeighted()
        {
            List<Tools.BulkElasticPhaseEvaluations> Ret = new List<Tools.BulkElasticPhaseEvaluations>();

            for (int i = 0; i < this.CrystalData.Count; i++)
            {
                Tools.BulkElasticPhaseEvaluations Tmp = new Tools.BulkElasticPhaseEvaluations();
                Tmp.HKLPase = this.CrystalData[i].SymmetryGroup;

                double TotalWeight = 0;
                double ret = 0.0;

                for (int n = 0; n < MacroElasticData.Count; n++)
                {
                    if (MacroElasticData[n][0].DPeak.AssociatedCrystalData.SymmetryGroup == this.CrystalData[i].SymmetryGroup)
                    {
                        if (MacroElasticData[n][0].PsiAngle == 90)
                        {
                            TotalWeight += MacroElasticData[n].EModulError;
                            ret += MacroElasticData[n].EModul / MacroElasticData[n].EModulError;
                        }
                    }
                }

                Tmp.PsiAngle = 90;
                Tmp.BulkElasticity = ret / TotalWeight;
                Tmp.BulkElasticityError = 0.0;

                Ret.Add(Tmp);
            }

            return Ret;
        }

        public List<Analysis.Stress.Macroskopic.PeakStressAssociation> GetSinPsyData()
        {
            List<Analysis.Stress.Macroskopic.PeakStressAssociation> ret = new List<Stress.Macroskopic.PeakStressAssociation>();

            for(int n = 0; n < this.DiffractionPatterns.Count; n++)
            {
                for(int i = 0; i < this.DiffractionPatterns[n].FoundPeaks.Count; i++)
                {
                    ret.Add(new Stress.Macroskopic.PeakStressAssociation(this.DiffractionPatterns[n].Stress, this.DiffractionPatterns[n].PsiAngle(this.DiffractionPatterns[n].FoundPeaks[i].Angle), this.DiffractionPatterns[n].FoundPeaks[i], this.DiffractionPatterns[n].PsiAngle(this.DiffractionPatterns[n].FoundPeaks[i].Angle)));
                }
            }

            return ret;
        }

        #endregion

        #region Micro elastic calculation

        public List<List<Stress.Microsopic.REK>> DiffractionConstants = new List<List<Stress.Microsopic.REK>>();
        public List<List<Stress.Microsopic.REK>> DiffractionConstantsTexture = new List<List<Stress.Microsopic.REK>>();

        public List<Stress.Microsopic.ElasticityTensors> VoigtTensorData = new List<Stress.Microsopic.ElasticityTensors>();
        public List<Stress.Microsopic.ElasticityTensors> ReussTensorData = new List<Stress.Microsopic.ElasticityTensors>();
        public List<Stress.Microsopic.ElasticityTensors> HillTensorData = new List<Stress.Microsopic.ElasticityTensors>();
        public List<Stress.Microsopic.ElasticityTensors> KroenerTensorData = new List<Stress.Microsopic.ElasticityTensors>();
        public List<Stress.Microsopic.ElasticityTensors> DeWittTensorData = new List<Stress.Microsopic.ElasticityTensors>();
        public List<Stress.Microsopic.ElasticityTensors> GeometricHillTensorData = new List<Stress.Microsopic.ElasticityTensors>();

        public void SetHillTensorData(int phase)
        {
            if (VoigtTensorData[phase].Symmetry == ReussTensorData[phase].Symmetry)
            {
                this.HillTensorData[phase] = new Stress.Microsopic.ElasticityTensors();
                this.HillTensorData[phase].Symmetry = VoigtTensorData[phase].Symmetry;
                this.HillTensorData[phase].IsIsotropic = VoigtTensorData[phase].IsIsotropic;
                this.HillTensorData[phase].FitConverged = VoigtTensorData[phase].FitConverged;

                switch (this.VoigtTensorData[phase].Symmetry)
                {
                    case "cubic":
                        if (VoigtTensorData[phase].IsIsotropic)
                        {
                            this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                            this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        }
                        else
                        {
                            this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                            this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                            this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        }
                        break;
                    case "hexagonal":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        break;
                    case "tetragonal type 1":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        this.HillTensorData[phase].C66 = (this.VoigtTensorData[phase].C66 + this.ReussTensorData[phase].C66) / 2;
                        break;
                    case "tetragonal type 2":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C16 = (this.VoigtTensorData[phase].C16 + this.ReussTensorData[phase].C16) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        this.HillTensorData[phase].C66 = (this.VoigtTensorData[phase].C66 + this.ReussTensorData[phase].C66) / 2;
                        break;
                    case "trigonal type 1":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C14 = (this.VoigtTensorData[phase].C14 + this.ReussTensorData[phase].C14) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        break;
                    case "trigonal type 2":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C14 = (this.VoigtTensorData[phase].C14 + this.ReussTensorData[phase].C14) / 2;
                        this.HillTensorData[phase].C15 = (this.VoigtTensorData[phase].C15 + this.ReussTensorData[phase].C15) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        break;
                    case "rhombic":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C22 = (this.VoigtTensorData[phase].C22 + this.ReussTensorData[phase].C22) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C23 = (this.VoigtTensorData[phase].C23 + this.ReussTensorData[phase].C23) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        this.HillTensorData[phase].C55 = (this.VoigtTensorData[phase].C55 + this.ReussTensorData[phase].C55) / 2;
                        this.HillTensorData[phase].C66 = (this.VoigtTensorData[phase].C66 + this.ReussTensorData[phase].C66) / 2;
                        break;
                    case "monoclinic":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C22 = (this.VoigtTensorData[phase].C22 + this.ReussTensorData[phase].C22) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C16 = (this.VoigtTensorData[phase].C16 + this.ReussTensorData[phase].C16) / 2;
                        this.HillTensorData[phase].C23 = (this.VoigtTensorData[phase].C23 + this.ReussTensorData[phase].C23) / 2;
                        this.HillTensorData[phase].C26 = (this.VoigtTensorData[phase].C26 + this.ReussTensorData[phase].C26) / 2;
                        this.HillTensorData[phase].C36 = (this.VoigtTensorData[phase].C36 + this.ReussTensorData[phase].C36) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        this.HillTensorData[phase].C45 = (this.VoigtTensorData[phase].C45 + this.ReussTensorData[phase].C45) / 2;
                        this.HillTensorData[phase].C55 = (this.VoigtTensorData[phase].C55 + this.ReussTensorData[phase].C55) / 2;
                        this.HillTensorData[phase].C66 = (this.VoigtTensorData[phase].C66 + this.ReussTensorData[phase].C66) / 2;
                        break;
                    case "triclinic":
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C14 = (this.VoigtTensorData[phase].C14 + this.ReussTensorData[phase].C14) / 2;
                        this.HillTensorData[phase].C15 = (this.VoigtTensorData[phase].C15 + this.ReussTensorData[phase].C15) / 2;
                        this.HillTensorData[phase].C16 = (this.VoigtTensorData[phase].C16 + this.ReussTensorData[phase].C16) / 2;
                        this.HillTensorData[phase].C22 = (this.VoigtTensorData[phase].C22 + this.ReussTensorData[phase].C22) / 2;
                        this.HillTensorData[phase].C23 = (this.VoigtTensorData[phase].C23 + this.ReussTensorData[phase].C23) / 2;
                        this.HillTensorData[phase].C24 = (this.VoigtTensorData[phase].C24 + this.ReussTensorData[phase].C24) / 2;
                        this.HillTensorData[phase].C25 = (this.VoigtTensorData[phase].C25 + this.ReussTensorData[phase].C25) / 2;
                        this.HillTensorData[phase].C26 = (this.VoigtTensorData[phase].C26 + this.ReussTensorData[phase].C26) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C34 = (this.VoigtTensorData[phase].C34 + this.ReussTensorData[phase].C34) / 2;
                        this.HillTensorData[phase].C35 = (this.VoigtTensorData[phase].C35 + this.ReussTensorData[phase].C35) / 2;
                        this.HillTensorData[phase].C36 = (this.VoigtTensorData[phase].C36 + this.ReussTensorData[phase].C36) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        this.HillTensorData[phase].C45 = (this.VoigtTensorData[phase].C45 + this.ReussTensorData[phase].C45) / 2;
                        this.HillTensorData[phase].C46 = (this.VoigtTensorData[phase].C46 + this.ReussTensorData[phase].C46) / 2;
                        this.HillTensorData[phase].C55 = (this.VoigtTensorData[phase].C55 + this.ReussTensorData[phase].C55) / 2;
                        this.HillTensorData[phase].C56 = (this.VoigtTensorData[phase].C56 + this.ReussTensorData[phase].C56) / 2;
                        this.HillTensorData[phase].C66 = (this.VoigtTensorData[phase].C66 + this.ReussTensorData[phase].C66) / 2;
                        break;
                    default:
                        this.HillTensorData[phase].C11 = (this.VoigtTensorData[phase].C11 + this.ReussTensorData[phase].C11) / 2;
                        this.HillTensorData[phase].C12 = (this.VoigtTensorData[phase].C12 + this.ReussTensorData[phase].C12) / 2;
                        this.HillTensorData[phase].C13 = (this.VoigtTensorData[phase].C13 + this.ReussTensorData[phase].C13) / 2;
                        this.HillTensorData[phase].C14 = (this.VoigtTensorData[phase].C14 + this.ReussTensorData[phase].C14) / 2;
                        this.HillTensorData[phase].C15 = (this.VoigtTensorData[phase].C15 + this.ReussTensorData[phase].C15) / 2;
                        this.HillTensorData[phase].C16 = (this.VoigtTensorData[phase].C16 + this.ReussTensorData[phase].C16) / 2;
                        this.HillTensorData[phase].C22 = (this.VoigtTensorData[phase].C22 + this.ReussTensorData[phase].C22) / 2;
                        this.HillTensorData[phase].C23 = (this.VoigtTensorData[phase].C23 + this.ReussTensorData[phase].C23) / 2;
                        this.HillTensorData[phase].C24 = (this.VoigtTensorData[phase].C24 + this.ReussTensorData[phase].C24) / 2;
                        this.HillTensorData[phase].C25 = (this.VoigtTensorData[phase].C25 + this.ReussTensorData[phase].C25) / 2;
                        this.HillTensorData[phase].C26 = (this.VoigtTensorData[phase].C26 + this.ReussTensorData[phase].C26) / 2;
                        this.HillTensorData[phase].C33 = (this.VoigtTensorData[phase].C33 + this.ReussTensorData[phase].C33) / 2;
                        this.HillTensorData[phase].C34 = (this.VoigtTensorData[phase].C34 + this.ReussTensorData[phase].C34) / 2;
                        this.HillTensorData[phase].C35 = (this.VoigtTensorData[phase].C35 + this.ReussTensorData[phase].C35) / 2;
                        this.HillTensorData[phase].C36 = (this.VoigtTensorData[phase].C36 + this.ReussTensorData[phase].C36) / 2;
                        this.HillTensorData[phase].C44 = (this.VoigtTensorData[phase].C44 + this.ReussTensorData[phase].C44) / 2;
                        this.HillTensorData[phase].C45 = (this.VoigtTensorData[phase].C45 + this.ReussTensorData[phase].C45) / 2;
                        this.HillTensorData[phase].C46 = (this.VoigtTensorData[phase].C46 + this.ReussTensorData[phase].C46) / 2;
                        this.HillTensorData[phase].C55 = (this.VoigtTensorData[phase].C55 + this.ReussTensorData[phase].C55) / 2;
                        this.HillTensorData[phase].C56 = (this.VoigtTensorData[phase].C56 + this.ReussTensorData[phase].C56) / 2;
                        this.HillTensorData[phase].C66 = (this.VoigtTensorData[phase].C66 + this.ReussTensorData[phase].C66) / 2;
                        break;
                }

                this.HillTensorData[phase].CalculateCompliances();
            }
        }

        #region Multiphase analysis

        public MathNet.Numerics.LinearAlgebra.Matrix<double> GetOverallCompliances()
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                ret[0, 0] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[0, 0];
                ret[0, 1] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[0, 1];
                ret[0, 2] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[0, 2];
                ret[0, 3] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[0, 3];
                ret[0, 4] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[0, 4];
                ret[0, 5] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[0, 5];

                ret[1, 0] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[1, 0];
                ret[1, 1] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[1, 1];
                ret[1, 2] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[1, 2];
                ret[1, 3] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[1, 3];
                ret[1, 4] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[1, 4];
                ret[1, 5] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[1, 5];

                ret[2, 0] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[2, 0];
                ret[2, 1] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[2, 1];
                ret[2, 2] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[2, 2];
                ret[2, 3] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[2, 3];
                ret[2, 4] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[2, 4];
                ret[2, 5] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[2, 5];

                ret[3, 0] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[3, 0];
                ret[3, 1] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[3, 1];
                ret[3, 2] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[3, 2];
                ret[3, 3] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[3, 3];
                ret[3, 4] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[3, 4];
                ret[3, 5] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[3, 5];

                ret[4, 0] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[4, 0];
                ret[4, 1] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[4, 1];
                ret[4, 2] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[4, 2];
                ret[4, 3] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[4, 3];
                ret[4, 4] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[4, 4];
                ret[4, 5] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[4, 5];

                ret[5, 0] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[5, 0];
                ret[5, 1] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[5, 1];
                ret[5, 2] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[5, 2];
                ret[5, 3] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[5, 3];
                ret[5, 4] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[5, 4];
                ret[5, 5] += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n]._complianceTensor[5, 5];

            }

            return ret;
        }
        public MathNet.Numerics.LinearAlgebra.Matrix<double> GetOverallCompliances(int sECIndex)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                Stress.Microsopic.ElasticityTensors eTPhase = this.GetSECByIndex(sECIndex, n);

                ret[0, 0] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[0, 0];
                ret[0, 1] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[0, 1];
                ret[0, 2] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[0, 2];
                ret[0, 3] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[0, 3];
                ret[0, 4] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[0, 4];
                ret[0, 5] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[0, 5];

                ret[1, 0] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[1, 0];
                ret[1, 1] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[1, 1];
                ret[1, 2] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[1, 2];
                ret[1, 3] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[1, 3];
                ret[1, 4] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[1, 4];
                ret[1, 5] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[1, 5];

                ret[2, 0] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[2, 0];
                ret[2, 1] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[2, 1];
                ret[2, 2] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[2, 2];
                ret[2, 3] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[2, 3];
                ret[2, 4] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[2, 4];
                ret[2, 5] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[2, 5];

                ret[3, 0] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[3, 0];
                ret[3, 1] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[3, 1];
                ret[3, 2] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[3, 2];
                ret[3, 3] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[3, 3];
                ret[3, 4] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[3, 4];
                ret[3, 5] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[3, 5];

                ret[4, 0] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[4, 0];
                ret[4, 1] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[4, 1];
                ret[4, 2] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[4, 2];
                ret[4, 3] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[4, 3];
                ret[4, 4] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[4, 4];
                ret[4, 5] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[4, 5];

                ret[5, 0] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[5, 0];
                ret[5, 1] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[5, 1];
                ret[5, 2] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[5, 2];
                ret[5, 3] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[5, 3];
                ret[5, 4] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[5, 4];
                ret[5, 5] += this.CrystalData[n].PhaseFraction * eTPhase._complianceTensor[5, 5];

            }

            return ret;
        }

        public void SetPhaseStresses()
        {
            for(int n = 0; n < this.DiffractionConstants.Count; n++)
            {
                for (int i = 0; i < this.DiffractionConstants[n].Count; i++)
                {
                    for (int j = 0; j < this.DiffractionConstants[n][i].ElasticStressData.Count; j++)
                    {
                        this.DiffractionConstants[n][i].ElasticStressData[j].PhaseFractionStress = this.DiffractionConstants[n][i].ElasticStressData[j].Stress * StressTransitionFactors[n][2, 2];
                    }
                }
            }

            for (int n = 0; n < this.DiffractionConstantsTexture.Count; n++)
            {
                for (int i = 0; i < this.DiffractionConstantsTexture[n].Count; i++)
                {
                    for (int j = 0; j < this.DiffractionConstantsTexture[n][i].ElasticStressData.Count; j++)
                    {
                        this.DiffractionConstantsTexture[n][i].ElasticStressData[j].PhaseFractionStress = this.DiffractionConstantsTexture[n][i].ElasticStressData[j].Stress * StressTransitionFactors[n][2, 2];
                    }
                }
            }
        }

        public void RefitAllDECStressCorrected()
        {
            for (int n = 0; n < this.DiffractionConstants.Count; n++)
            {
                for (int i = 0; i < this.DiffractionConstants[n].Count; i++)
                {
                    this.DiffractionConstants[n][i].FitClassicPhaseREKFunction();
                }
            }

            for (int n = 0; n < this.DiffractionConstants.Count; n++)
            {
                for (int i = 0; i < this.DiffractionConstants[n].Count; i++)
                {
                    this.DiffractionConstants[n][i].FitClassicPhaseREKFunction();
                }
            }
        }

        public List<MathNet.Numerics.LinearAlgebra.Matrix<double>> StressTransitionFactors = new List<MathNet.Numerics.LinearAlgebra.Matrix<double>>();

        #region old code
        //public void SetStressTransitionFactors(int matrix, int inclusion, bool sphere)
        //{
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> eshelbyTensor = this.GetEshelbyTensorEllipsoid(matrix, inclusion, 2);
        //    if(sphere)
        //    {
        //        eshelbyTensor = this.GetEshelbyTensorSpherical(matrix, 2);
        //    }

        //    MathNet.Numerics.LinearAlgebra.Matrix<double> sampleCompliances = this.GetOverallCompliances();
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> sampleStiffnesses = sampleCompliances.Inverse();
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> unity = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
        //    unity[0, 0] = 1.0;
        //    unity[1, 1] = 1.0;
        //    unity[2, 2] = 1.0;
        //    unity[3, 3] = 1.0;
        //    unity[4, 4] = 1.0;
        //    unity[5, 5] = 1.0;

        //    MathNet.Numerics.LinearAlgebra.Matrix<double> first = -1 * sampleStiffnesses * (eshelbyTensor - unity);
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> second = (this.ReussTensorData[inclusion]._stiffnessTensor - sampleStiffnesses) * eshelbyTensor;
        //    second += sampleStiffnesses;
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> third = (this.ReussTensorData[inclusion]._stiffnessTensor - sampleStiffnesses) * sampleCompliances;

        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ret1 = first * second.Inverse() * third;
        //    ret1 += unity;

        //    Tools.FourthRankTensor eshelbyTensorFR = new Tools.FourthRankTensor(eshelbyTensor);
        //    Tools.FourthRankTensor sampleCompliancesFR = new Tools.FourthRankTensor(sampleCompliances);
        //    Tools.FourthRankTensor sampleStiffnessesFR = new Tools.FourthRankTensor(sampleStiffnesses);
        //    Tools.FourthRankTensor unityFR = Tools.FourthRankTensor.GetUnityTensor();
        //    Tools.FourthRankTensor phaseStiffnessesFR = new Tools.FourthRankTensor(this.ReussTensorData[inclusion]._stiffnessTensor);

        //    Tools.FourthRankTensor firstFR = -1.0 * sampleStiffnessesFR * (eshelbyTensorFR - unityFR);
        //    Tools.FourthRankTensor secondFR = (phaseStiffnessesFR - sampleStiffnessesFR) * eshelbyTensorFR;
        //    secondFR += sampleStiffnessesFR;
        //    Tools.FourthRankTensor thirdFR = (phaseStiffnessesFR - sampleStiffnessesFR) * sampleCompliancesFR;

        //    MathNet.Numerics.LinearAlgebra.Matrix<double> firstComp = firstFR.GetVoigtTensor();
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> secondComp = secondFR.GetVoigtTensor();
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> thirdComp = thirdFR.GetVoigtTensor();

        //    Tools.FourthRankTensor secondInverseFR = new Tools.FourthRankTensor(secondComp.Inverse());

        //    Tools.FourthRankTensor ret1FR = firstFR * secondInverseFR * thirdFR;
        //    ret1FR += unityFR;

        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ret1Comp = ret1FR.GetVoigtTensor();

        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ret2 = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

        //    ret2[0, 0] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[0, 0]);
        //    ret2[0, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 1]);
        //    ret2[0, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 2]);
        //    ret2[0, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 3]);
        //    ret2[0, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 4]);
        //    ret2[0, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 5]);

        //    ret2[1, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 0]);
        //    ret2[1, 1] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[1, 1]);
        //    ret2[1, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 2]);
        //    ret2[1, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 3]);
        //    ret2[1, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 4]);
        //    ret2[1, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 5]);

        //    ret2[2, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 0]);
        //    ret2[2, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 1]);
        //    ret2[2, 2] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[2, 2]);
        //    ret2[2, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 3]);
        //    ret2[2, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 4]);
        //    ret2[2, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 5]);

        //    ret2[3, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 0]);
        //    ret2[3, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 1]);
        //    ret2[3, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 2]);
        //    ret2[3, 3] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[3, 3]);
        //    ret2[3, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 4]);
        //    ret2[3, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 5]);

        //    ret2[4, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 0]);
        //    ret2[4, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 1]);
        //    ret2[4, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 2]);
        //    ret2[4, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 3]);
        //    ret2[4, 4] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[4, 4]);
        //    ret2[4, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 5]);

        //    ret2[5, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 0]);
        //    ret2[5, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 1]);
        //    ret2[5, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 2]);
        //    ret2[5, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 3]);
        //    ret2[5, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 4]);
        //    ret2[5, 5] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[5, 5]);

        //    ret2 /= this.CrystalData[matrix].PhaseFraction;
        //    this.StressTransitionFactors.Clear();

        //    if(inclusion < matrix)
        //    {
        //        this.StressTransitionFactors.Add(ret1);
        //        this.StressTransitionFactors.Add(ret2);
        //    }
        //    else
        //    {
        //        this.StressTransitionFactors.Add(ret2);
        //        this.StressTransitionFactors.Add(ret1);
        //    }
        //}
        #endregion

        public void SetStressTransitionFactors(int matrix, int inclusion, bool sphere, int sECIndex)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> eshelbyTensor = this.GetEshelbyTensorEllipsoid(matrix, inclusion, sECIndex);
            if (sphere)
            {
                eshelbyTensor = this.GetEshelbyTensorSpherical(matrix, sECIndex);
            }

            MathNet.Numerics.LinearAlgebra.Matrix<double> sampleCompliances = this.GetOverallCompliances();
            MathNet.Numerics.LinearAlgebra.Matrix<double> sampleStiffnesses = sampleCompliances.Inverse();
            MathNet.Numerics.LinearAlgebra.Matrix<double> unity = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
            unity[0, 0] = 1.0;
            unity[1, 1] = 1.0;
            unity[2, 2] = 1.0;
            unity[3, 3] = 1.0;
            unity[4, 4] = 1.0;
            unity[5, 5] = 1.0;

            Stress.Microsopic.ElasticityTensors eTInclusion = this.GetSECByIndex(sECIndex, inclusion);

            MathNet.Numerics.LinearAlgebra.Matrix<double> first = -1 * sampleStiffnesses * (eshelbyTensor - unity);
            MathNet.Numerics.LinearAlgebra.Matrix<double> second = (eTInclusion._stiffnessTensor - sampleStiffnesses) * eshelbyTensor;
            second += sampleStiffnesses;
            MathNet.Numerics.LinearAlgebra.Matrix<double> third = (eTInclusion._stiffnessTensor - sampleStiffnesses) * sampleCompliances;

            MathNet.Numerics.LinearAlgebra.Matrix<double> ret1 = first * second.Inverse() * third;
            ret1 += unity;

            Tools.FourthRankTensor eshelbyTensorFR = new Tools.FourthRankTensor(eshelbyTensor);
            Tools.FourthRankTensor sampleCompliancesFR = new Tools.FourthRankTensor(sampleCompliances);
            Tools.FourthRankTensor sampleStiffnessesFR = new Tools.FourthRankTensor(sampleStiffnesses);
            Tools.FourthRankTensor unityFR = Tools.FourthRankTensor.GetUnityTensor();
            Tools.FourthRankTensor phaseStiffnessesFR = new Tools.FourthRankTensor(eTInclusion._stiffnessTensor);

            Tools.FourthRankTensor firstFR = -1.0 * sampleStiffnessesFR * (eshelbyTensorFR - unityFR);
            Tools.FourthRankTensor secondFR = (phaseStiffnessesFR - sampleStiffnessesFR) * eshelbyTensorFR;
            secondFR += sampleStiffnessesFR;
            Tools.FourthRankTensor thirdFR = (phaseStiffnessesFR - sampleStiffnessesFR) * sampleCompliancesFR;

            MathNet.Numerics.LinearAlgebra.Matrix<double> firstComp = firstFR.GetVoigtTensor();
            MathNet.Numerics.LinearAlgebra.Matrix<double> secondComp = secondFR.GetVoigtTensor();
            MathNet.Numerics.LinearAlgebra.Matrix<double> thirdComp = thirdFR.GetVoigtTensor();

            Tools.FourthRankTensor secondInverseFR = new Tools.FourthRankTensor(secondComp.Inverse());

            Tools.FourthRankTensor ret1FR = firstFR * secondInverseFR * thirdFR;
            ret1FR += unityFR;

            MathNet.Numerics.LinearAlgebra.Matrix<double> ret1Comp = ret1FR.GetVoigtTensor();

            MathNet.Numerics.LinearAlgebra.Matrix<double> ret2 = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

            ret2[0, 0] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[0, 0]);
            ret2[0, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 1]);
            ret2[0, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 2]);
            ret2[0, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 3]);
            ret2[0, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 4]);
            ret2[0, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[0, 5]);

            ret2[1, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 0]);
            ret2[1, 1] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[1, 1]);
            ret2[1, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 2]);
            ret2[1, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 3]);
            ret2[1, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 4]);
            ret2[1, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[1, 5]);

            ret2[2, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 0]);
            ret2[2, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 1]);
            ret2[2, 2] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[2, 2]);
            ret2[2, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 3]);
            ret2[2, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 4]);
            ret2[2, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[2, 5]);

            ret2[3, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 0]);
            ret2[3, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 1]);
            ret2[3, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 2]);
            ret2[3, 3] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[3, 3]);
            ret2[3, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 4]);
            ret2[3, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[3, 5]);

            ret2[4, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 0]);
            ret2[4, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 1]);
            ret2[4, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 2]);
            ret2[4, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 3]);
            ret2[4, 4] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[4, 4]);
            ret2[4, 5] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[4, 5]);

            ret2[5, 0] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 0]);
            ret2[5, 1] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 1]);
            ret2[5, 2] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 2]);
            ret2[5, 3] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 3]);
            ret2[5, 4] = -1 * (this.CrystalData[inclusion].PhaseFraction * ret1[5, 4]);
            ret2[5, 5] = 1 - (this.CrystalData[inclusion].PhaseFraction * ret1[5, 5]);

            ret2 /= this.CrystalData[matrix].PhaseFraction;
            this.StressTransitionFactors.Clear();

            if (inclusion < matrix)
            {
                this.StressTransitionFactors.Add(ret1);
                this.StressTransitionFactors.Add(ret2);
            }
            else
            {
                this.StressTransitionFactors.Add(ret2);
                this.StressTransitionFactors.Add(ret1);
            }
        }

        private Stress.Microsopic.ElasticityTensors GetSECByIndex(int index, int phase)
        {
            switch(index)
            {
                case 0:
                    return this.VoigtTensorData[phase];
                case 1:
                    return this.ReussTensorData[phase];
                case 2:
                    return this.HillTensorData[phase];
                case 3:
                    return this.KroenerTensorData[phase];
                case 4:
                    return this.DeWittTensorData[phase];
                case 5:
                    return this.GeometricHillTensorData[phase];
                default:
                    return HillTensorData[phase];
            }
        }

        #region Eshelby tensor

        public MathNet.Numerics.LinearAlgebra.Matrix<double> GetEshelbyTensorSpherical(int matrix, int sECIndex)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
            Stress.Microsopic.ElasticityTensors eTPhase = this.GetSECByIndex(sECIndex, matrix);

            double e11 = 7 - (5 * eTPhase.AveragedNu);
            e11 /= 15 * (1 - eTPhase.AveragedNu);

            double e12 = -1 + (5 * eTPhase.AveragedNu);
            e12 /= 15 * (1 - eTPhase.AveragedNu);

            double e66 = 4 - (5 * eTPhase.AveragedNu);
            e66 /= 15 * (1 - eTPhase.AveragedNu);

            ret[0, 0] = e11;
            ret[1, 1] = e11;
            ret[2, 2] = e11;

            ret[0, 1] = e12;
            ret[0, 2] = e12;
            ret[1, 2] = e12;
            ret[1, 0] = e12;
            ret[2, 0] = e12;
            ret[2, 1] = e12;

            ret[3, 3] = e66;
            ret[4, 4] = e66;
            ret[5, 5] = e66;

            return ret;
        }

        public MathNet.Numerics.LinearAlgebra.Matrix<double> GetEshelbyTensorEllipsoid(int matrix, int inclusion, int sECIndex)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
            Stress.Microsopic.ElasticityTensors eTMatrix = this.GetSECByIndex(sECIndex, matrix);

            double q = this.GetEshelbyQ(eTMatrix);
            double r = this.GetEshelbyR(eTMatrix);
            double i = this.GetEshelbyI(inclusion);
            double l = this.GetEshelbyL(inclusion);

            double e11 = Math.PI + (Math.Pow(this.CrystalData[inclusion].InclusionA, 2) * l / 4.0);
            e11 *= q;
            e11 += r * i;

            double e33 = -2 * (Math.Pow(this.CrystalData[inclusion].InclusionC, 2) * l / 3.0);
            e33 += 4.0 * Math.PI / 3.0;
            e33 *= q;
            e33 += r * ((4.0 * Math.PI) - (2 * i));

            double e12 = -1 * (Math.Pow(this.CrystalData[inclusion].InclusionA, 2) * l / 12.0);
            e12 += Math.PI / 3.0;
            e12 *= q;
            e12 -= r * i;

            double e13 = q * (Math.Pow(this.CrystalData[inclusion].InclusionC, 2) * l / 3.0);
            e13 -= r * i;

            double e31 = q * (Math.Pow(this.CrystalData[inclusion].InclusionA, 2) * l / 3.0);
            e31 -= r * ((4 * Math.PI) - (2 * i));

            double e44 = q * l / 6.0;
            e44 *= Math.Pow(this.CrystalData[inclusion].InclusionA, 2) + Math.Pow(this.CrystalData[inclusion].InclusionC, 2);
            e44 -= (r / 2.0) * ((4.0 * Math.PI) - i);

            double e66 = -1.0 * Math.Pow(this.CrystalData[inclusion].InclusionA, 2) * l / 12.0;
            e66 += Math.PI / 3.0;
            e66 *= q;
            e66 += r * i;

            ret[0, 0] = e11;
            ret[1, 1] = e11;

            ret[2, 2] = e33;

            ret[0, 1] = e12;
            ret[1, 0] = e12;

            ret[0, 2] = e13;
            ret[1, 2] = e13;

            ret[2, 0] = e31;
            ret[2, 1] = e31;

            ret[3, 3] = e44;
            ret[4, 4] = e44;

            ret[5, 5] = e66;

            return ret;
        }

        double GetEshelbyQ(Stress.Microsopic.ElasticityTensors eTMatrix)
        {
            double ret = 3.0 / (8.0 * Math.PI);
            ret /= (1 - eTMatrix.AveragedNu);

            return ret;
        }
        double GetEshelbyR(Stress.Microsopic.ElasticityTensors eTMatrix)
        {
            double ret = (1 - (2 * eTMatrix.AveragedNu));
            ret /= (8.0 * Math.PI) * (1 - eTMatrix.AveragedNu);

            return ret;
        }
        double GetEshelbyI(int inclusion)
        {
            double caNenner = Math.Sqrt(Math.Pow(this.CrystalData[inclusion].InclusionC, 2) - Math.Pow(this.CrystalData[inclusion].InclusionA, 2));

            double right = (this.CrystalData[inclusion].InclusionC / Math.Pow(this.CrystalData[inclusion].InclusionA, 2)) * caNenner;
            right -= MathNet.Numerics.Trig.Acosh(this.CrystalData[inclusion].InclusionC / this.CrystalData[inclusion].InclusionA);

            double left = 2 * Math.PI * Math.Pow(this.CrystalData[inclusion].InclusionA, 2) * this.CrystalData[inclusion].InclusionC;
            left /= Math.Pow(caNenner, 3);

            return left * right;

        }
        double GetEshelbyL(int inclusion)
        {
            double caNenner = Math.Pow(this.CrystalData[inclusion].InclusionC, 2) - Math.Pow(this.CrystalData[inclusion].InclusionA, 2);

            double ret = 4 * Math.PI;

            ret -= 3 * this.GetEshelbyI(inclusion);
            ret /= caNenner;

            return ret;
        }

        #endregion

        #region Reuss
        
        public double S1ReussFraction(DataManagment.CrystalData.HKLReflex hKL)
        {
            double ret = 0;

            for(int n = 0; n < this.CrystalData.Count; n++)
            {
                if(this.CrystalData[n].SymmetryGroupID == 194)
                {
                    ret += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n].S1ReussHexagonal(hKL);
                }
                else
                {
                    ret += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n].S1ReussCubic(hKL);
                }
            }

            return ret;
        }

        public double HS2ReussFraction(DataManagment.CrystalData.HKLReflex hKL)
        {
            double ret = 0;

            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                if (this.CrystalData[n].SymmetryGroupID == 194)
                {
                    ret += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n].HS2ReussHexagonal(hKL);
                }
                else
                {
                    ret += this.CrystalData[n].PhaseFraction * this.ReussTensorData[n].HS2ReussCubic(hKL);
                }
            }

            return ret;
        }

        #region Derivatives

        /// <summary>
        /// Calculates the Hessian matrix and the solution vecor
        /// </summary>
        /// <returns>
        /// The Deltas for the paramaeters:
        ///[0] C11
        ///[1] C12
        ///[2] C44
        /// </returns>
        public MathNet.Numerics.LinearAlgebra.Vector<double> ParameterDeltaVektorReussFraction(double Lambda)
        {

            int parameterDimension = 0;

            for(int n = 0; n < this.CrystalData.Count; n++)
            {
                if(this.CrystalData[n].SymmetryGroupID == 194)
                {
                    parameterDimension += 5;
                }
                else
                {
                    parameterDimension += 3;
                }
            }
            //[0][0] C11
            //[1][1] C12
            MathNet.Numerics.LinearAlgebra.Matrix<double> HessianMatrix = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(parameterDimension, parameterDimension, 0.0);

            //[0] Aclivity
            //[1] Constant
            MathNet.Numerics.LinearAlgebra.Vector<double> SolutionVector = MathNet.Numerics.LinearAlgebra.CreateVector.Dense<double>(parameterDimension);
            int usedIndices = 0;
            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                int usedIndicesP = 0;
                for (int j = 0; j < this.CrystalData.Count; j++)
                {
                    if (n == j)
                    {
                        if (this.CrystalData[n].SymmetryGroupID == 194)
                        {
                            for (int i = 0; i < this.DiffractionConstants.Count; i++)
                            {
                                #region Matrix Build

                                HessianMatrix[usedIndices + 0, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndices + 1, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndices + 2, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndices + 3, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndices + 4, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndices + 4] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndices + 3] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                #endregion

                                #region Vector build

                                SolutionVector[usedIndices + 0] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 0] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 1] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 1] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 2] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 2] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 3] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 3] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 4] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 4] += ((this.ReussTensorData[n].DiffractionConstants[n].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex);

                                #endregion
                            }

                            usedIndices += 5;
                            usedIndicesP += 5;
                        }
                        else
                        {
                            for (int i = 0; i < this.DiffractionConstants[n].Count; i++)
                            {
                                #region Matrix Build

                                HessianMatrix[usedIndices + 0, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndices + 1, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndices + 2, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 0] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndices + 2] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndices + 1] += (Math.Pow(this.CrystalData[n].PhaseFraction, 2) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                #endregion

                                #region Vector build

                                SolutionVector[usedIndices + 0] += ((this.DiffractionConstants[n][i].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 0] += ((this.DiffractionConstants[n][i].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 1] += ((this.DiffractionConstants[n][i].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 1] += ((this.DiffractionConstants[n][i].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 2] += ((this.DiffractionConstants[n][i].ClassicS1 - this.S1ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2)) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex);
                                SolutionVector[usedIndices + 2] += ((this.DiffractionConstants[n][i].ClassicHS2 - this.HS2ReussFraction(this.DiffractionConstants[n][i].UsedReflex)) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2)) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex);

                                #endregion
                            }

                            usedIndices += 3;
                            usedIndicesP += 3;
                        }
                    }
                    else if(j > n)
                    {
                        if (this.CrystalData[n].SymmetryGroupID == 194)
                        {
                            for (int i = 0; i < this.DiffractionConstants.Count; i++)
                            {
                                #region Matrix Build

                                HessianMatrix[usedIndicesP + usedIndices + 0, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 0, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 3, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 4, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS11HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS33HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS12HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 3, usedIndicesP + usedIndices + 4] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13S1ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 4, usedIndices + 3] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[j].FirstDerivativeS13HS2ReussHexagonal(this.DiffractionConstants[n][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                #endregion
                            }

                            usedIndicesP += 5;
                        }
                        else
                        {
                            for (int i = 0; i < this.DiffractionConstants[n].Count; i++)
                            {
                                #region Matrix Build

                                HessianMatrix[usedIndicesP + usedIndices + 0, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 0, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 1, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));

                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 2, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 0, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 0] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS11HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndices + 1, usedIndicesP + usedIndices + 2] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44S1ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12S1ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicS1Error, 2));
                                HessianMatrix[usedIndicesP + usedIndices + 2, usedIndices + 1] += (this.CrystalData[n].PhaseFraction * this.CrystalData[j].PhaseFraction * this.ReussTensorData[n].FirstDerivativeS44HS2ReussCubic(this.DiffractionConstants[n][i].UsedReflex) * this.ReussTensorData[n].FirstDerivativeS12HS2ReussCubic(this.DiffractionConstants[j][i].UsedReflex) / Math.Pow(this.DiffractionConstants[n][i].ClassicHS2Error, 2));
                                #endregion
                            }

                            usedIndicesP += 3;
                        }
                    }
                }
            }

            for (int n = 0; n < 3; n++)
            {
                HessianMatrix[n, n] *= (1 + Lambda);
            }

            MathNet.Numerics.LinearAlgebra.Vector<double> ParamDelta = MathNet.Numerics.LinearAlgebra.CreateVector.Dense<double>(3);

            HessianMatrix.Solve(SolutionVector, ParamDelta);

            return ParamDelta;
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Texture

        public List<Texture.OrientationDistributionFunction> ODFList = new List<Texture.OrientationDistributionFunction>();

        public void SetMRDForExperiment(int phaseIndex, int experimentIndex)
        {
            ODFList[phaseIndex].SetStepSizes();

            double grainOrientationStepPhi1 = this.SimulationData[experimentIndex].SymStepPhi1(phaseIndex);
            grainOrientationStepPhi1 /= 2;

            double grainOrientationStepPsi = this.SimulationData[experimentIndex].SymStepPsi(phaseIndex);
            grainOrientationStepPsi /= 2;

            double grainOrientationStepPhi2 = this.SimulationData[experimentIndex].SymStepPhi2(phaseIndex);
            grainOrientationStepPhi2 /= 2;

            for (int i = 0; i < this.SimulationData[experimentIndex].GrainOrientations[phaseIndex].Count; i++)
            {
                this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].MRD = 0;
            }

            for (int n = 0; n < ODFList[phaseIndex].TDData.Count; n++)
            {
                for(int i = 0; i < this.SimulationData[experimentIndex].GrainOrientations[phaseIndex].Count; i++)
                {
                    bool phi1 = false;
                    bool psi = false;
                    bool phi2 = false;

                    if(this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].Phi1 - grainOrientationStepPhi1 < ODFList[phaseIndex].TDData[n][0] && this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].Phi1 + grainOrientationStepPhi1 > ODFList[phaseIndex].TDData[n][0])
                    {
                        phi1 = true;
                    }
                    if (this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].Psi - grainOrientationStepPsi < ODFList[phaseIndex].TDData[n][1] && this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].Psi + grainOrientationStepPsi > ODFList[phaseIndex].TDData[n][1])
                    {
                        psi = true;
                    }
                    if (this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].Phi2 - grainOrientationStepPhi2 < ODFList[phaseIndex].TDData[n][2] && this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].Phi2 + grainOrientationStepPhi2 > ODFList[phaseIndex].TDData[n][2])
                    {
                        phi2 = true;
                    }

                    if(phi1 && psi && phi2)
                    {
                        this.SimulationData[experimentIndex].GrainOrientations[phaseIndex][i].MRD += ODFList[phaseIndex].TDData[n][3];
                        break;
                    }
                }
            }
        }

        public void InterpolateMRDForExperiment(int phaseIndex, int experimentIndex)
        {

        }

        #endregion

        #region Elasto plastic self consistent

        #region Simulation Multi threading

        #region Simulation using Multi Threading

        public System.Threading.ManualResetEvent _doneEvent;

        public event System.ComponentModel.PropertyChangedEventHandler FitFinished;
        public event System.ComponentModel.PropertyChangedEventHandler FitStarted;

        protected void OnElastoPlasticSimulationStarted()
        {
            System.ComponentModel.PropertyChangedEventHandler handler = FitStarted;
            if (handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs("Simulation Started"));
            }
        }

        protected void OnElastoPlasticSimulationFinished()
        {
            this._doneEvent.Set();

            System.ComponentModel.PropertyChangedEventHandler handler = FitFinished;
            if (handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs("Simulation Finished"));
            }
        }

        // Wrapper method for use with thread pool. 
        public void FitRegionCallback(Object threadContext)
        {
            OnElastoPlasticSimulationStarted();



            OnElastoPlasticSimulationFinished();
        }

        public void SetResetEvent(System.Threading.ManualResetEvent DoneEvent)
        {
            this._doneEvent = DoneEvent;
        }


        #endregion

        #endregion

        #region Parameter

        /// <summary>
        /// Alle experimentellen Reflexe sortiert nach [Phase][Orientierung (z.B.100)][Psi Orientierung][gemessener Reflex]
        /// </summary>
        public List<List<List<List<Stress.Macroskopic.PeakStressAssociation>>>> SortedExperimentalPeakData = new List<List<List<List<Stress.Macroskopic.PeakStressAssociation>>>>();

        public void SetExperimentalData()
        {
            List<List<Stress.Microsopic.REK>> preDEC = new List<List<Stress.Microsopic.REK>>();
            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                List<Stress.Microsopic.REK> phaseDEC = new List<Stress.Microsopic.REK>();

                for (int i = 0; i < this.CrystalData[n].HKLList.Count; i++)
                {
                    Stress.Microsopic.REK ActualREK = new Stress.Microsopic.REK(this.CrystalData[n], this.CrystalData[n].HKLList[i]);

                    for (int j = 0; j < this.DiffractionPatterns.Count; j++)
                    {
                        for (int k = 0; k < this.DiffractionPatterns[j].FoundPeaks.Count; k++)
                        {
                            if (this.DiffractionPatterns[j].FoundPeaks[k].AssociatedCrystalData.SymmetryGroupID == this.CrystalData[n].SymmetryGroupID)
                            {
                                if (this.DiffractionPatterns[j].FoundPeaks[k].AssociatedHKLReflex.HKLString == this.CrystalData[n].HKLList[i].HKLString)
                                {
                                    Stress.Macroskopic.PeakStressAssociation NewAssociation = new Stress.Macroskopic.PeakStressAssociation(this.DiffractionPatterns[j].Stress, this.DiffractionPatterns[j].PsiAngle(this.DiffractionPatterns[j].FoundPeaks[k].Angle), this.DiffractionPatterns[j].FoundPeaks[k], this.DiffractionPatterns[j].PhiAngle(this.DiffractionPatterns[j].FoundPeaks[k].Angle));
                                    NewAssociation._macroskopicStrain = this.DiffractionPatterns[j].MacroStrain;
                                    ActualREK.ElasticStressData.Add(NewAssociation);
                                }
                            }
                        }
                    }
                    if (ActualREK.ElasticStressData.Count != 0)
                    {
                        phaseDEC.Add(ActualREK);
                    }
                }

                preDEC.Add(phaseDEC);
            }

            SortedExperimentalPeakData.Clear();

            for (int phase = 0; phase < CrystalData.Count; phase++)
            {
                List<List<List<Stress.Macroskopic.PeakStressAssociation>>> sortedData = new List<List<List<Stress.Macroskopic.PeakStressAssociation>>>();

                for (int n = 0; n < preDEC[phase].Count; n++)
                {
                    List<List<Stress.Macroskopic.PeakStressAssociation>> orientationPre = new List<List<Stress.Macroskopic.PeakStressAssociation>>();

                    for (int i = 0; i < preDEC[phase][n].ElasticStressData.Count; i++)
                    {
                        bool newPsyAngle = true;
                        for (int j = 0; j < orientationPre.Count; j++)
                        {
                            if (Math.Abs(orientationPre[j][0].PsiAngle - preDEC[phase][n].ElasticStressData[i].PsiAngle) < CalScec.Properties.Settings.Default.PsyAcceptanceAngle)
                            {
                                orientationPre[j].Add(preDEC[phase][n].ElasticStressData[i]);
                                newPsyAngle = false;
                                break;
                            }
                        }

                        if (newPsyAngle)
                        {
                            List<Stress.Macroskopic.PeakStressAssociation> tmp = new List<Stress.Macroskopic.PeakStressAssociation>();
                            tmp.Add(preDEC[phase][n].ElasticStressData[i]);
                            orientationPre.Add(tmp);
                        }
                    }

                    sortedData.Add(orientationPre);

                }

                SortedExperimentalPeakData.Add(sortedData);
            }

            #region OldStuff
            //for (int phase = 0; phase < CrystalData.Count; phase++)
            //{
            //    List<List<List<Stress.Macroskopic.PeakStressAssociation>>> sortedData = new List<List<List<Stress.Macroskopic.PeakStressAssociation>>>();

            //    for(int n = 0; n < this.DiffractionConstants[phase].Count; n++)
            //    {
            //        List<List<Stress.Macroskopic.PeakStressAssociation>> orientationPre = new List<List<Stress.Macroskopic.PeakStressAssociation>>();

            //        for(int i = 0; i < this.DiffractionConstants[phase][n].ElasticStressData.Count; i++)
            //        {
            //            bool newPsyAngle = true;
            //            for(int j = 0; j < orientationPre.Count; j++)
            //            {
            //                if(Math.Abs(orientationPre[j][0].PsiAngle - this.DiffractionConstants[phase][n].ElasticStressData[i].PsiAngle) < CalScec.Properties.Settings.Default.PsyAcceptanceAngle)
            //                {
            //                    orientationPre[j].Add(this.DiffractionConstants[phase][n].ElasticStressData[i]);
            //                    newPsyAngle = false;
            //                    break;
            //                }
            //            }

            //            if(newPsyAngle)
            //            {
            //                List<Stress.Macroskopic.PeakStressAssociation> tmp = new List<Stress.Macroskopic.PeakStressAssociation>();
            //                tmp.Add(this.DiffractionConstants[phase][n].ElasticStressData[i]);
            //                orientationPre.Add(tmp);
            //            }
            //        }

            //        sortedData.Add(orientationPre);

            //    }

            //    SortedExperimentalPeakData.Add(sortedData);
            //}

            #endregion
        }

        public void SetExperimentalStrainData()
        {
            for (int phase = 0; phase < CrystalData.Count; phase++)
            {
                for(int n = 0; n < SortedExperimentalPeakData[phase].Count; n++)
                {
                    for (int i = 0; i < SortedExperimentalPeakData[phase][n].Count; i++)
                    {
                        for (int j = 0; j < SortedExperimentalPeakData[phase][n][i].Count; j++)
                        {
                            SortedExperimentalPeakData[phase][n][i][j].Strain = (SortedExperimentalPeakData[phase][n][i][j].DifPeak.LatticeDistance - SortedExperimentalPeakData[phase][n][i][0].DifPeak.LatticeDistance) / SortedExperimentalPeakData[phase][n][i][0].DifPeak.LatticeDistance;
                        }
                    }
                }
            }
        }

        public List<Stress.Plasticity.ElastoPlasticExperiment> SimulationData = new List<Stress.Plasticity.ElastoPlasticExperiment>();

        //public List<MathNet.Numerics.LinearAlgebra.Matrix<double>> appliedSampleStressHistory = new List<MathNet.Numerics.LinearAlgebra.Matrix<double>>();
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> StrainRate = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        
        public List<Stress.Plasticity.PlasticityTensor> PlasticTensor = new List<Stress.Plasticity.PlasticityTensor>();

        public void SetYieldExperimentalData()
        {
            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                for (int i = 0; i < this.PlasticTensor[n].YieldSurfaceData.ReflexYieldData.Count; i++)
                {
                    for (int j = 0; j < this.DiffractionPatterns.Count; j++)
                    {
                        for (int k = 0; k < this.DiffractionPatterns[j].FoundPeaks.Count; k++)
                        {
                            if (this.DiffractionPatterns[j].FoundPeaks[k].AssociatedCrystalData.SymmetryGroupID == this.CrystalData[n].SymmetryGroupID)
                            {
                                if (this.DiffractionPatterns[j].FoundPeaks[k].AssociatedHKLReflex.HKLString == this.PlasticTensor[n].YieldSurfaceData.ReflexYieldData[i].HKLString)
                                {
                                    Stress.Macroskopic.PeakStressAssociation NewAssociation = new Stress.Macroskopic.PeakStressAssociation(this.DiffractionPatterns[j].Stress, this.DiffractionPatterns[j].PsiAngle(this.DiffractionPatterns[j].FoundPeaks[k].Angle), this.DiffractionPatterns[j].FoundPeaks[k], this.DiffractionPatterns[j].PhiAngle(this.DiffractionPatterns[j].FoundPeaks[k].Angle));
                                    NewAssociation._macroskopicStrain = this.DiffractionPatterns[j].MacroStrain;
                                    this.PlasticTensor[n].YieldSurfaceData.ReflexYieldData[i].StressStrainData.Add(NewAssociation);
                                }
                            }
                        }
                    }
                }
            }
        }

        public Tools.FourthRankTensor GetSampleCompliances(bool textureActive)
        {
            List<Tools.FourthRankTensor> phaseOCompliances = new List<Tools.FourthRankTensor>();

            for(int n = 0; n < this.CrystalData.Count; n++)
            {
                if (textureActive)
                {
                    phaseOCompliances.Add(this.SetTextureTensor(this.HillTensorData[n], n).GetFourtRankStiffnesses());
                }
                else
                {
                    phaseOCompliances.Add(this.SetTextureTensorIso(this.HillTensorData[n]).GetFourtRankStiffnesses());
                }
            }

            Tools.FourthRankTensor ret = new Tools.FourthRankTensor();

            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                ret += this.CrystalData[n].PhaseFraction * phaseOCompliances[n];
            }

            //ret /= phaseOCompliances.Count;

            return ret;
        }

        public Tools.FourthRankTensor GetSampleCompliances(int grainModel)
        {
            List<Tools.FourthRankTensor> phaseCompliances = new List<Tools.FourthRankTensor>();
            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                switch (grainModel)
                {
                    case 0:
                        phaseCompliances.Add(this.SetTextureTensorIso(this.ReussTensorData[n]).GetFourtRankCompliances());
                        break;
                    case 1:
                        phaseCompliances.Add(this.SetTextureTensorIso(this.HillTensorData[n]).GetFourtRankCompliances());
                        break;
                    case 2:
                        phaseCompliances.Add(this.SetTextureTensorIso(this.KroenerTensorData[n]).GetFourtRankCompliances());
                        break;
                    case 3:
                        phaseCompliances.Add(this.SetTextureTensorIso(this.DeWittTensorData[n]).GetFourtRankCompliances());
                        break;
                    case 4:
                        phaseCompliances.Add(this.SetTextureTensorIso(this.GeometricHillTensorData[n]).GetFourtRankCompliances());
                        break;
                    default:
                        phaseCompliances.Add(this.SetTextureTensorIso(this.HillTensorData[n]).GetFourtRankCompliances());
                        break;
                }
            }
            Tools.FourthRankTensor ret = new Tools.FourthRankTensor();
            for (int n = 0; n < this.CrystalData.Count; n++)
            {
                ret += this.CrystalData[n].PhaseFraction * phaseCompliances[n];
            }

            return ret;
        }

        public Tools.FourthRankTensor GetSampleStiffnesses(bool textureActive)
        {
            List<Tools.FourthRankTensor> phaseOStiffnesses = new List<Tools.FourthRankTensor>();
            
            for (int n = 0; n < this.HillTensorData.Count; n++)
            {
                if (textureActive)
                {
                    phaseOStiffnesses.Add(this.SetTextureTensor(this.HillTensorData[n], n).GetFourtRankStiffnesses());
                }
                else
                {
                    phaseOStiffnesses.Add(this.SetTextureTensorIso(this.HillTensorData[n]).GetFourtRankStiffnesses());
                }
            }
            
            Tools.FourthRankTensor ret = new Tools.FourthRankTensor();

            for (int n = 0; n < phaseOStiffnesses.Count; n++)
            {
                ret += this.CrystalData[n].PhaseFraction * phaseOStiffnesses[n];
            }

            ret /= phaseOStiffnesses.Count;

            return ret;
        }
        public Tools.FourthRankTensor GetSampleStiffnesses(bool textureActive, List<Analysis.Stress.Microsopic.ElasticityTensors> tensorData)
        {
            List<Tools.FourthRankTensor> phaseOStiffnesses = new List<Tools.FourthRankTensor>();

            for (int n = 0; n < tensorData.Count; n++)
            {
                if (textureActive)
                {
                    phaseOStiffnesses.Add(this.SetTextureTensor(tensorData[n], n).GetFourtRankStiffnesses());
                }
                else
                {
                    phaseOStiffnesses.Add(this.SetTextureTensorIso(tensorData[n]).GetFourtRankStiffnesses());
                }
            }

            Tools.FourthRankTensor ret = new Tools.FourthRankTensor();

            for (int n = 0; n < phaseOStiffnesses.Count; n++)
            {
                ret += phaseOStiffnesses[n];
            }

            ret /= phaseOStiffnesses.Count;

            return ret;
        }

        #region Averaging

        public Stress.Microsopic.ElasticityTensors SetTextureTensorIso(Stress.Microsopic.ElasticityTensors averagingTensor)
        {
            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            switch (averagingTensor.Symmetry)
            {
                case "cubic":
                    ret = this.SetTextureTensorCubicIso(averagingTensor);
                    break;
                case "hexagonal":
                    ret = this.SetTextureTensorHexagonalIso(averagingTensor);
                    break;
                case "tetragonal type 1":
                    ret = this.SetTextureTensorTetragonalType1Iso(averagingTensor);
                    break;
                case "tetragonal type 2":

                    break;
                case "trigonal type 1":

                    break;
                case "trigonal type 2":

                    break;
                case "rhombic":
                    ret = this.SetTextureTensorRhombicIso(averagingTensor);
                    break;
                case "monoclinic":

                    break;
                case "triclinic":

                    break;
                default:

                    break;
            }

            ret.CalculateCompliances();

            return ret;
        }
        
        public Stress.Microsopic.ElasticityTensors SetTextureTensorCubicIso(Stress.Microsopic.ElasticityTensors averagingTensor)
        {
            double TC11 = 0;
            double TC12 = 0;
            double TC44 = 0;

            double normFactor = 0.0;

            for (double phi1 = 0.0; phi1 < 360.0; phi1 += 5.0)
            {
                for (double psi1 = 0.0; psi1 < 360.0; psi1 += 5.0)
                {
                    for (double phi2 = 0.0; phi2 < 360.0; phi2 += 5.0)
                    {
                        normFactor++;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC12 += Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;
                        TC12 += Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C55;
                        TC12 += Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C66;
                        //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;
                        //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C55;
                        //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C66;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C12;
                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C13;
                        TC44 += Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C23;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C12;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C13;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C23;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC44 += Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C44;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C55;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C55;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C66;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C66;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C66;

                    }
                }
            }
            
            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = (TC11 / normFactor);
            ret.C12 = (TC12 / normFactor);
            ret.C44 = (TC44 / normFactor);

            return ret;
        }

        private Stress.Microsopic.ElasticityTensors SetTextureTensorHexagonalIso(Stress.Microsopic.ElasticityTensors averagingTensor)
        {
            double TC11 = 0;
            double TC33 = 0;
            double TC12 = 0;
            double TC13 = 0;
            double TC44 = 0;

            double normFactor = 0.0;

            for (double phi1 = 0.0; phi1 < 360.0; phi1 += 5.0)
            {
                for (double psi1 = 0.0; psi1 < 360.0; psi1 += 5.0)
                {
                    for (double phi2 = 0.0; phi2 < 360.0; phi2 += 5.0)
                    {
                        normFactor++;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        //TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        //TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        //TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        //TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        //TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        //TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC12 += Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;
                        TC12 += Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C55;
                        TC12 += Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C66;
                        //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;
                        //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C55;
                        //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C66;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC13 += Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;
                        TC13 += Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C55;
                        TC13 += Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C66;
                        //TC13 += 4 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;
                        //TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C55;
                        //TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C66;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C22;

                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C12;
                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C13;
                        TC44 += Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C23;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC44 += Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C55;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        TC44 += Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C66;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C12;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C13;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C23;

                        //TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;

                        //TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C55;

                        //TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C66;
                    }
                }
            }

            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = TC11 / normFactor;
            ret.C33 = TC33 / normFactor;
            ret.C12 = TC12 / normFactor;
            ret.C13 = TC13 / normFactor;
            ret.C44 = TC44 / normFactor;

            return ret;
        }

        private Stress.Microsopic.ElasticityTensors SetTextureTensorTetragonalType1Iso(Stress.Microsopic.ElasticityTensors averagingTensor)
        {
            double TC11 = 0;
            double TC33 = 0;
            double TC12 = 0;
            double TC13 = 0;
            double TC44 = 0;
            double TC66 = 0;

            double normFactor = 0.0;

            for (double phi1 = 0.0; phi1 < 360.0; phi1 += 5.0)
            {
                for (double psi1 = 0.0; psi1 < 360.0; psi1 += 5.0)
                {
                    for (double phi2 = 0.0; phi2 < 360.0; phi2 += 5.0)
                    {
                        normFactor++;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC12 += 4 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;
                        TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C55;
                        TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C66;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC13 += 4 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;
                        TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C55;
                        TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C66;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C22;

                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C12;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C13;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C23;

                        TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;

                        TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C55;

                        TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C66;

                        TC66 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC66 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC66 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C22;

                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C12;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C13;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C23;

                        TC66 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;

                        TC66 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * averagingTensor.C55;

                        TC66 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * averagingTensor.C66;
                    }
                }
            }

            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = TC11 / normFactor;
            ret.C33 = TC33 / normFactor;
            ret.C12 = TC12 / normFactor;
            ret.C13 = TC13 / normFactor;
            ret.C44 = TC44 / normFactor;
            ret.C66 = TC66 / normFactor;

            return ret;
        }

        private Stress.Microsopic.ElasticityTensors SetTextureTensorRhombicIso(Stress.Microsopic.ElasticityTensors averagingTensor)
        {
            double TC11 = 0;
            double TC22 = 0;
            double TC33 = 0;
            double TC12 = 0;
            double TC13 = 0;
            double TC23 = 0;
            double TC44 = 0;
            double TC55 = 0;
            double TC66 = 0;

            double normFactor = 0.0;

            for (double phi1 = 0.0; phi1 < 360.0; phi1 += 5.0)
            {
                for (double psi1 = 0.0; psi1 < 360.0; psi1 += 5.0)
                {
                    for (double phi2 = 0.0; phi2 < 360.0; phi2 += 5.0)
                    {
                        normFactor++;

                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC11 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC22 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC22 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC22 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC22 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC22 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC22 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC22 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC22 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC22 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 4) * averagingTensor.C11;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 4) * averagingTensor.C22;
                        TC33 += Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 4) * averagingTensor.C33;

                        TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC12 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC12 += 4 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;
                        TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C55;
                        TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C66;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC13 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC13 += 4 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;
                        TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C55;
                        TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C66;

                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C33;

                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C12;
                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C12;

                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C13;
                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C13;

                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C23;
                        TC23 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C23;

                        TC23 += 4 * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;
                        TC23 += 4 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C55;
                        TC23 += 4 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C66;

                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C22;

                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C12;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C13;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C23;

                        TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;

                        TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C55;

                        TC44 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C66;

                        TC55 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC55 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC55 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C22;

                        TC55 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * averagingTensor.C12;
                        TC55 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C13;
                        TC55 += 2 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C23;

                        TC55 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC55 += 2 * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * averagingTensor.C44;

                        TC55 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC55 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM33(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C55;

                        TC55 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        TC55 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM32(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM31(phi1, psi1, phi2) * averagingTensor.C66;

                        TC66 += Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2), 2) * averagingTensor.C11;
                        TC66 += Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C22;
                        TC66 += Math.Pow(Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C22;

                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * averagingTensor.C12;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C13;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C23;

                        TC66 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C44;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * averagingTensor.C44;

                        TC66 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2), 2) * averagingTensor.C55;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM23(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM13(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * averagingTensor.C55;

                        TC66 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2), 2) * averagingTensor.C66;
                        TC66 += 2 * Texture.OrientationDistributionFunction.SCTM11(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM22(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM12(phi1, psi1, phi2) * Texture.OrientationDistributionFunction.SCTM21(phi1, psi1, phi2) * averagingTensor.C66;
                    }
                }
            }

            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = TC11 / normFactor;
            ret.C22 = TC22 / normFactor;
            ret.C33 = TC33 / normFactor;
            ret.C12 = TC12 / normFactor;
            ret.C13 = TC13 / normFactor;
            ret.C23 = TC23 / normFactor;
            ret.C44 = TC44 / normFactor;
            ret.C55 = TC55 / normFactor;
            ret.C66 = TC66 / normFactor;

            return ret;
        }

        public Stress.Microsopic.ElasticityTensors SetTextureTensor(Stress.Microsopic.ElasticityTensors averagingTensor, int phaseIndex)
        {
            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            switch (averagingTensor.Symmetry)
            {
                case "cubic":
                    ret = this.SetTextureTensorCubic(averagingTensor, phaseIndex);
                    break;
                case "hexagonal":
                    ret = this.SetTextureTensorHexagonal(averagingTensor, phaseIndex);
                    break;
                case "tetragonal type 1":
                    ret = this.SetTextureTensorTetragonalType1(averagingTensor, phaseIndex);
                    break;
                case "tetragonal type 2":

                    break;
                case "trigonal type 1":

                    break;
                case "trigonal type 2":

                    break;
                case "rhombic":
                    ret = this.SetTextureTensorRhombic(averagingTensor, phaseIndex);
                    break;
                case "monoclinic":

                    break;
                case "triclinic":

                    break;
                default:

                    break;
            }

            ret.CalculateCompliances();

            return ret;
        }

        public Stress.Microsopic.ElasticityTensors SetTextureTensorCubic(Stress.Microsopic.ElasticityTensors averagingTensor, int phaseIndex)
        {
            double TC11 = 0;
            double TC12 = 0;
            double TC44 = 0;

            double normFactor = 0.0;

            for(int n = 0; n < this.ODFList[phaseIndex].TDData.Count; n++)
            {
                normFactor += this.ODFList[phaseIndex].TDData[n][3];

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                
                //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
                //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;
                //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC44 += Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
                //TC44 += 2 * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
            }

            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = (TC11 / normFactor);
            ret.C12 = (TC12 / normFactor);
            ret.C44 = (TC44 / normFactor);

            return ret;
        }

        private Stress.Microsopic.ElasticityTensors SetTextureTensorHexagonal(Stress.Microsopic.ElasticityTensors averagingTensor, int phaseIndex)
        {
            double TC11 = 0;
            double TC33 = 0;
            double TC12 = 0;
            double TC13 = 0;
            double TC44 = 0;

            double normFactor = 0.0;


            for (int n = 0; n < this.ODFList[phaseIndex].TDData.Count; n++)
            {
                normFactor += this.ODFList[phaseIndex].TDData[n][3];

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                //TC11 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                //TC11 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                //TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                //TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                //TC33 += 2 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                //TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                //TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                //TC33 += 4 * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
                //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                //TC12 += 4 * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
                //TC13 += 4 * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                //TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                //TC13 += 4 * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
            }
            

            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = TC11 / normFactor;
            ret.C33 = TC33 / normFactor;
            ret.C12 = TC12 / normFactor;
            ret.C13 = TC13 / normFactor;
            ret.C44 = TC44 / normFactor;

            return ret;
        }

        private Stress.Microsopic.ElasticityTensors SetTextureTensorTetragonalType1(Stress.Microsopic.ElasticityTensors averagingTensor, int phaseIndex)
        {
            double TC11 = 0;
            double TC33 = 0;
            double TC12 = 0;
            double TC13 = 0;
            double TC44 = 0;
            double TC66 = 0;

            double normFactor = 0.0;

            for (int n = 0; n < this.ODFList[phaseIndex].TDData.Count; n++)
            {
                normFactor += this.ODFList[phaseIndex].TDData[n][3];

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
            }
            

            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = TC11 / normFactor;
            ret.C33 = TC33 / normFactor;
            ret.C12 = TC12 / normFactor;
            ret.C13 = TC13 / normFactor;
            ret.C44 = TC44 / normFactor;
            ret.C66 = TC66 / normFactor;

            return ret;
        }

        private Stress.Microsopic.ElasticityTensors SetTextureTensorRhombic(Stress.Microsopic.ElasticityTensors averagingTensor, int phaseIndex)
        {
            double TC11 = 0;
            double TC22 = 0;
            double TC33 = 0;
            double TC12 = 0;
            double TC13 = 0;
            double TC23 = 0;
            double TC44 = 0;
            double TC55 = 0;
            double TC66 = 0;

            double normFactor = 0.0;

            for (int n = 0; n < this.ODFList[phaseIndex].TDData.Count; n++)
            {
                normFactor += this.ODFList[phaseIndex].TDData[n][3];

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC11 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC22 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C11;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C22;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 4) * averagingTensor.C33;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC33 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC12 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC13 += this.ODFList[phaseIndex].TDData[n][3] * this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC13 += this.ODFList[phaseIndex].TDData[n][3] * this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C33;

                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;
                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C12;

                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;
                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C13;

                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;
                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C23;

                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;
                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;
                TC23 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;

                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                TC44 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;

                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;

                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;

                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM33(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;

                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                TC55 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM32(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM31(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C11;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C22;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C12;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C13;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C23;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C44;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C44;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C55;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM23(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM13(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C55;

                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Math.Pow(Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * Math.Pow(Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]), 2) * averagingTensor.C66;
                TC66 += this.ODFList[phaseIndex].TDData[n][3] * Texture.OrientationDistributionFunction.SCTM11(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM22(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM12(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * Texture.OrientationDistributionFunction.SCTM21(this.ODFList[phaseIndex].TDData[n][0], this.ODFList[phaseIndex].TDData[n][1], this.ODFList[phaseIndex].TDData[n][2]) * averagingTensor.C66;
            }

            Stress.Microsopic.ElasticityTensors ret = averagingTensor.Clone() as Stress.Microsopic.ElasticityTensors;

            ret.C11 = TC11 / normFactor;
            ret.C22 = TC22 / normFactor;
            ret.C33 = TC33 / normFactor;
            ret.C12 = TC12 / normFactor;
            ret.C13 = TC13 / normFactor;
            ret.C23 = TC23 / normFactor;
            ret.C44 = TC44 / normFactor;
            ret.C55 = TC55 / normFactor;
            ret.C66 = TC66 / normFactor;

            return ret;
        }
        #endregion

        #endregion

        #region Data display

        public Pattern.Counts SimulationDataDisplay(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> xData, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> yData)
        {
            Pattern.Counts ret = new Pattern.Counts();

            if(xData.Count == yData.Count)
            {
                for (int n = 0; n < yData.Count; n++)
                {
                    MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = xData[n] * direction1.Direction;
                    MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = yData[n] * direction2.Direction;

                    double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
                    ret.Add(tmp);
                }
            }
            else
            {
                if (xData.Count > yData.Count)
                {
                    for (int n = 0; n < yData.Count; n++)
                    {
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = xData[n] * direction1.Direction;
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = yData[n] * direction2.Direction;

                        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
                        ret.Add(tmp);
                    }
                }
                else
                {
                    for (int n = 0; n < xData.Count; n++)
                    {
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = xData[n] * direction1.Direction;
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = yData[n] * direction2.Direction;

                        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
                        ret.Add(tmp);
                    }
                }
            }

            return ret;
        }

        public Pattern.Counts SimulationDataDisplay(int[] xIndex, int[] yIndex, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> xData, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> yData)
        {
            Pattern.Counts ret = new Pattern.Counts();

            if (xData.Count == yData.Count)
            {
                for (int n = 0; n < yData.Count; n++)
                {
                    double[] tmp = { xData[n][xIndex[0], xIndex[1]], yData[n][yIndex[0], yIndex[1]] };
                    ret.Add(tmp);
                }
            }
            else
            {
                if(xData.Count > yData.Count)
                {
                    for (int n = 0; n < yData.Count; n++)
                    {
                        double[] tmp = { xData[n][xIndex[0], xIndex[1]], yData[n][yIndex[0], yIndex[1]] };
                        ret.Add(tmp);
                    }
                }
                else
                {
                    for (int n = 0; n < xData.Count; n++)
                    {
                        double[] tmp = { xData[n][xIndex[0], xIndex[1]], yData[n][yIndex[0], yIndex[1]] };
                        ret.Add(tmp);
                    }
                }
            }

            return ret;
        }

        public Pattern.Counts SimulationDataDisplay(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> xData, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> yData, bool xSum, bool ySum)
        {
            Pattern.Counts ret = new Pattern.Counts();

            if (xData.Count == yData.Count)
            {
                double sumXValue = 0.0;
                double sumYValue = 0.0;
                for (int n = 0; n < yData.Count; n++)
                {
                    MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = xData[n] * direction1.DirectionNorm;
                    MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = yData[n] * direction2.DirectionNorm;
                    if(xSum)
                    {
                        sumXValue += direction1.DirectionNorm * directionValueX;
                        if (ySum)
                        {
                            sumYValue += direction2.DirectionNorm * directionValueY;
                            double[] tmp = { sumXValue, sumYValue };
                            ret.Add(tmp);
                        }
                        else
                        {
                            double[] tmp = { sumXValue, direction2.DirectionNorm * directionValueY };
                            ret.Add(tmp);
                        }
                    }
                    else
                    {
                        if (ySum)
                        {
                            sumYValue += direction2.DirectionNorm * directionValueY;
                            double[] tmp = { direction1.DirectionNorm * directionValueX, sumYValue };
                            ret.Add(tmp);
                        }
                        else
                        {
                            double[] tmp = { direction1.DirectionNorm * directionValueX, direction2.DirectionNorm * directionValueY };
                            ret.Add(tmp);
                        }
                    }
                }
            }
            else
            {
                if (xData.Count > yData.Count)
                {
                    double sumXValue = 0.0;
                    double sumYValue = 0.0;
                    for (int n = 0; n < yData.Count; n++)
                    {
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = xData[n] * direction1.DirectionNorm;
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = yData[n] * direction2.DirectionNorm;
                        if (xSum)
                        {
                            sumXValue += direction1.DirectionNorm * directionValueX;
                            if (ySum)
                            {
                                sumYValue += direction2.DirectionNorm * directionValueY;
                                double[] tmp = { sumXValue, sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { sumXValue, direction2.DirectionNorm * directionValueY };
                                ret.Add(tmp);
                            }
                        }
                        else
                        {
                            if (ySum)
                            {
                                sumYValue += direction2.DirectionNorm * directionValueY;
                                double[] tmp = { direction1.DirectionNorm * directionValueX, sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { direction1.DirectionNorm * directionValueX, direction2.DirectionNorm * directionValueY };
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                else
                {
                    double sumXValue = 0.0;
                    double sumYValue = 0.0;
                    for (int n = 0; n < xData.Count; n++)
                    {
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = xData[n] * direction1.DirectionNorm;
                        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = yData[n] * direction2.DirectionNorm;
                        if (xSum)
                        {
                            sumXValue += direction1.DirectionNorm * directionValueX;
                            if (ySum)
                            {
                                sumYValue += direction2.DirectionNorm * directionValueY;
                                double[] tmp = { sumXValue, sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { sumXValue, direction2.DirectionNorm * directionValueY };
                                ret.Add(tmp);
                            }
                        }
                        else
                        {
                            if (ySum)
                            {
                                sumYValue += direction2.DirectionNorm * directionValueY;
                                double[] tmp = { direction1.DirectionNorm * directionValueX, sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { direction1.DirectionNorm * directionValueX, direction2.DirectionNorm * directionValueY };
                                ret.Add(tmp);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        public Pattern.Counts SimulationDataDisplay(int[] xIndex, int[] yIndex, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> xData, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> yData, bool xSum, bool ySum)
        {
            Pattern.Counts ret = new Pattern.Counts();

            if (xData.Count == yData.Count)
            {
                double sumXValue = 0.0;
                double sumYValue = 0.0;
                for (int n = 0; n < yData.Count; n++)
                {
                    if (xSum)
                    {
                        sumXValue += xData[n][xIndex[0], xIndex[1]];
                        if (ySum)
                        {
                            sumYValue += yData[n][yIndex[0], yIndex[1]];
                            double[] tmp = { sumXValue, sumYValue };
                            ret.Add(tmp);
                        }
                        else
                        {
                            double[] tmp = { sumXValue, yData[n][yIndex[0], yIndex[1]] };
                            ret.Add(tmp);
                        }
                    }
                    else
                    {
                        if (ySum)
                        {
                            sumYValue += yData[n][yIndex[0], yIndex[1]];
                            double[] tmp = { xData[n][xIndex[0], xIndex[1]], sumYValue };
                            ret.Add(tmp);
                        }
                        else
                        {
                            double[] tmp = { xData[n][xIndex[0], xIndex[1]], yData[n][yIndex[0], yIndex[1]] };
                            ret.Add(tmp);
                        }
                    }
                }
            }
            else
            {
                if (xData.Count > yData.Count)
                {
                    double sumXValue = 0.0;
                    double sumYValue = 0.0;
                    for (int n = 0; n < yData.Count; n++)
                    {
                        if (xSum)
                        {
                            sumXValue += xData[n][xIndex[0], xIndex[1]];
                            if (ySum)
                            {
                                sumYValue += yData[n][yIndex[0], yIndex[1]];
                                double[] tmp = { sumXValue, sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { sumXValue, yData[n][yIndex[0], yIndex[1]] };
                                ret.Add(tmp);
                            }
                        }
                        else
                        {
                            if (ySum)
                            {
                                sumYValue += yData[n][yIndex[0], yIndex[1]];
                                double[] tmp = { xData[n][xIndex[0], xIndex[1]], sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { xData[n][xIndex[0], xIndex[1]], yData[n][yIndex[0], yIndex[1]] };
                                ret.Add(tmp);
                            }
                        }
                    }
                }
                else
                {
                    double sumXValue = 0.0;
                    double sumYValue = 0.0;
                    for (int n = 0; n < xData.Count; n++)
                    {
                        if (xSum)
                        {
                            sumXValue += xData[n][xIndex[0], xIndex[1]];
                            if (ySum)
                            {
                                sumYValue += yData[n][yIndex[0], yIndex[1]];
                                double[] tmp = { sumXValue, sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { sumXValue, yData[n][yIndex[0], yIndex[1]] };
                                ret.Add(tmp);
                            }
                        }
                        else
                        {
                            if (ySum)
                            {
                                sumYValue += yData[n][yIndex[0], yIndex[1]];
                                double[] tmp = { xData[n][xIndex[0], xIndex[1]], sumYValue };
                                ret.Add(tmp);
                            }
                            else
                            {
                                double[] tmp = { xData[n][xIndex[0], xIndex[1]], yData[n][yIndex[0], yIndex[1]] };
                                ret.Add(tmp);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        #region OldStuff

        //public Pattern.Counts SampleStrainOverSampleStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainOverGrainStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainOverSampleStressData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainOverGrainStressData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainOverGrainShearStressData(DataManagment.CrystalData.HKLReflex direction1, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction1.Direction;

        //        double[] tmp = { directionValueX, direction1.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainOverSampleShearStresData(DataManagment.CrystalData.HKLReflex direction1, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        double directionValueX = this.PlasticTensor[0].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction1.Direction;

        //        double[] tmp = { directionValueX, direction1.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts GrainStrainOverSampleStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainOverGrainStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainCFHistory[phase][n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainOverSampleStressData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainOverGrainStressData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainCFHistory[phase][n] * direction2.Direction;

        //        double[] tmp = { direction1.Direction * directionValueX, direction2.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainOverGrainShearStressData(DataManagment.CrystalData.HKLReflex direction1, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValueX, direction1.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainOverSampleShearStressData(DataManagment.CrystalData.HKLReflex direction1, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValueX, direction1.Direction * directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * direction2.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts SampleStressOverSampleStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * direction2.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * direction1.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStressOverGrainStrainData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStressOverSampleStressData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * direction2.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStressOverGrainStressData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStressOverGrainShearStressData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStressOverSampleShearStressData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStressOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex direction, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * direction.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * direction.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStressOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts GrainStressOverSampleStrainData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStressOverGrainStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * direction2.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStressOverSampleStressData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStressOverGrainStressData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * direction2.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStressOverGrainShearStressData(DataManagment.CrystalData.HKLReflex direction1, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStressOverSampleShearStressData(DataManagment.CrystalData.HKLReflex direction1, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStressOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStressOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StressCFHistory[phase][n] * microDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts GrainShearStressOverSampleStrainData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainShearStressOverGrainStrainData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * microDirection.Direction;
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainShearStressOverSampleStressData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainShearStressOverGrainStressData(DataManagment.CrystalData.HKLReflex direction1, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainShearStressOverGrainShearStressData(int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        double directionValue = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValue, directionValue };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainShearStressOverSampleShearStressData(int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValueX, directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainShearStressOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainShearStressOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts SampleShearStressOverSampleStrainData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        double directionValueY = this.PlasticTensor[0].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleShearStressOverGrainStrainData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * microDirection.Direction;
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleShearStressOverSampleStressData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        double directionValueY = this.PlasticTensor[0].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleShearStressOverGrainStressData(DataManagment.CrystalData.HKLReflex direction1, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * direction1.Direction;
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleShearStressOverGrainShearStressData(int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        double directionValue = this.PlasticTensor[0].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValue, directionValue };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleShearStressOverSampleShearStressData(int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        double directionValue = this.PlasticTensor[0].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValue, directionValue };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleShearStressOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        double directionValueY = this.PlasticTensor[0].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleShearStressOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StressSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        double directionValueY = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts SampleStrainRateOverSampleStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * direction2.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainRateOverGrainStrainData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainRateOverSampleStressData(DataManagment.CrystalData.HKLReflex direction, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * direction.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * direction.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainRateOverGrainStressData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainRateOverGrainShearStressData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainRateOverSampleShearStressData(DataManagment.CrystalData.HKLReflex macroDirection, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        double directionValueX = this.PlasticTensor[0].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainRateOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * direction2.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts SampleStrainRateOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateSFHistory.Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts GrainStrainRateOverSampleStrainData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainRateOverGrainStrainData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * direction2.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainRateOverSampleStressData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainRateOverGrainStressData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StressCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainRateOverGrainShearStressData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressCFHistory[phase][n]);

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainRateOverSampleShearStressData(DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        double directionValueX = this.PlasticTensor[phase].YieldSurfaceData.Shearforce(SimulationData[experiment].StressSFHistory[n]);

        //        double[] tmp = { directionValueX, directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainRateOverSampleStrainRateData(DataManagment.CrystalData.HKLReflex macroDirection, DataManagment.CrystalData.HKLReflex microDirection, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * microDirection.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateSFHistory[n] * macroDirection.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts GrainStrainRateOverGrainStrainRateData(DataManagment.CrystalData.HKLReflex direction1, DataManagment.CrystalData.HKLReflex direction2, int experiment, int phase)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    for (int n = 0; n < SimulationData[experiment].StrainRateCFHistory[phase].Count; n++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueX = SimulationData[experiment].StrainRateCFHistory[phase][n] * direction1.Direction;
        //        MathNet.Numerics.LinearAlgebra.Vector<double> directionValueY = SimulationData[experiment].StrainRateCFHistory[phase][n] * direction2.Direction;

        //        double[] tmp = { directionValueX.L2Norm(), directionValueY.L2Norm() };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}



        //public Pattern.Counts MacroStrainOverMacroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData = Stress.Plasticity.EPModeling.GetPhaseStrainLD(eT, pT, this.appliedSampleStressHistory);

        //    for(int n = 0; n < plottingData.Count; n++)
        //    {
        //        double[] tmp = { plottingData[n][1], plottingData[n][1] };
        //        ret.Add(plottingData[n]);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts MacroStrainOverMicroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);
        //    List<double[]> plottingData2 = Stress.Plasticity.EPModeling.GetPhaseStrainLD(eT, pT, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData1[n][1], plottingData2[n][1] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts MacroStrainDataOverStressRD(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData = Stress.Plasticity.EPModeling.GetPhaseStrainLD(eT, pT, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData.Count; n++)
        //    {
        //        double[] tmp = { this.appliedSampleStressHistory[n][2, 2], plottingData[n][1] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts MacroStrainDataOverPlainAdjustedStress(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData.Count; n++)
        //    {
        //        ret.Add(plottingData[n]);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts MicroStrainOverMacroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);
        //    List<double[]> plottingData2 = Stress.Plasticity.EPModeling.GetPhaseStrainLD(eT, pT, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData2[n][1], plottingData1[n][1] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts MicroStrainOverMicroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData1[n][1], plottingData1[n][1] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts MicroStrainDataOverStressRD(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { this.appliedSampleStressHistory[n][2, 2], plottingData1[n][1] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts MicroStrainDataOverPlainAdjustedStress(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData1[n][0], plottingData1[n][1] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts StressRDOverMacroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData2 = Stress.Plasticity.EPModeling.GetPhaseStrainLD(eT, pT, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData2.Count; n++)
        //    {
        //        double[] tmp = { plottingData2[n][1], this.appliedSampleStressHistory[n][2, 2] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts StressRDOverMicroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData1[n][1], this.appliedSampleStressHistory[n][2, 2] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts StressRDOverStressData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { this.appliedSampleStressHistory[n][2, 2], this.appliedSampleStressHistory[n][2, 2] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts StressRDOverPlainAdjustedStressData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData1[n][0], this.appliedSampleStressHistory[n][2, 2] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        //public Pattern.Counts PlainAdjustedStressOverMacroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);
        //    List<double[]> plottingData2 = Stress.Plasticity.EPModeling.GetPhaseStrainLD(eT, pT, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData2.Count; n++)
        //    {
        //        double[] tmp = { plottingData2[n][1], plottingData1[n][0] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts PlainAdjustedStressOverMicroStrainData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData1[n][1], plottingData1[n][0] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts PlainAdjustedStressOverStressRDData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { this.appliedSampleStressHistory[n][2, 2], plottingData1[n][0] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}
        //public Pattern.Counts PlainAdjustedStressOverPlainAdjustedStressData(Stress.Microsopic.ElasticityTensors eT, Stress.Plasticity.PlasticityTensor pT, Stress.Plasticity.ReflexYield rY)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();

        //    List<double[]> plottingData1 = Stress.Plasticity.EPModeling.GetLatticeStrains(eT, pT, rY, this.appliedSampleStressHistory);

        //    for (int n = 0; n < plottingData1.Count; n++)
        //    {
        //        double[] tmp = { plottingData1[n][0], plottingData1[n][0] };
        //        ret.Add(tmp);
        //    }

        //    return ret;
        //}

        #endregion

        #region To Experiment

        ///// <summary>
        ///// return the macroscopic stress strain curve in LOAD DIRECTION
        ///// </summary>
        ///// <param name="elasticModel"></param>
        ///// <param name="phase"></param>
        ///// <returns></returns>
        //public Pattern.Counts GetMacroStressStrainCurveLD(int elasticModel, int phase, double totalYieldStrength, List<Stress.Macroskopic.PeakStressAssociation> stressStrainData)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();
        //    double eMod = 0.0;
        //    stressStrainData.Sort((a, b) => a.MacroskopicStrain.CompareTo(b.MacroskopicStrain));

        //    switch (elasticModel)
        //    {
        //        case 0:
        //            eMod = this.VoigtTensorData[phase].AveragedEModul;
        //            break;
        //        case 1:
        //            eMod = this.ReussTensorData[phase].AveragedEModul;
        //            break;
        //        case 2:
        //            eMod = this.HillTensorData[phase].AveragedEModul;
        //            break;
        //        case 3:
        //            eMod = this.KroenerTensorData[phase].AveragedEModul;
        //            break;
        //        case 4:
        //            eMod = this.DeWittTensorData[phase].AveragedEModul;
        //            break;
        //        case 5:
        //            eMod = this.GeometricHillTensorData[phase].AveragedEModul;
        //            break;
        //        default:
        //            eMod = this.HillTensorData[phase].AveragedEModul;
        //            break;
        //    }

        //    double plasticStrainRate = 0.0;
        //    int rateCounter = 0;

        //    for (int n = 0; n < stressStrainData.Count; n++)
        //    {
        //        if (stressStrainData[n].Stress > totalYieldStrength)
        //        {
        //            plasticStrainRate += stressStrainData[n].MacroskopicStrain - stressStrainData[n - 1].MacroskopicStrain;
        //            rateCounter++;
        //        }
        //    }

        //    plasticStrainRate /= rateCounter;

        //    double plasticStrain = 0.0;

        //    for (int n = 0; n < stressStrainData.Count; n++)
        //    {
        //        if (stressStrainData[n].Stress > totalYieldStrength)
        //        {
        //            plasticStrain += plasticStrainRate;
        //        }

        //        double[] dataTmp = { stressStrainData[n].Stress, ((1 / eMod) * stressStrainData[n].Stress) + plasticStrain };

        //        ret.Add(dataTmp);
        //    }

        //    return ret;
        //}

        ///// <summary>
        ///// return the macroscopic stress strain curve in LOAD DIRECTION
        ///// </summary>
        ///// <param name="elasticModel"></param>
        ///// <param name="phase"></param>
        ///// <returns></returns>
        //public Pattern.Counts GetMacroStrainLatticeStrainCurveLD(int elasticModel, int phase, double totalYieldStrength, List<Stress.Macroskopic.PeakStressAssociation> stressStrainData)
        //{
        //    Pattern.Counts ret = new Pattern.Counts();
        //    double eMod = 0.0;
        //    stressStrainData.Sort((a, b) => a.MacroskopicStrain.CompareTo(b.MacroskopicStrain));

        //    switch (elasticModel)
        //    {
        //        case 0:
        //            eMod = this.VoigtTensorData[phase].AveragedEModul;
        //            break;
        //        case 1:
        //            eMod = this.ReussTensorData[phase].AveragedEModul;
        //            break;
        //        case 2:
        //            eMod = this.HillTensorData[phase].AveragedEModul;
        //            break;
        //        case 3:
        //            eMod = this.KroenerTensorData[phase].AveragedEModul;
        //            break;
        //        case 4:
        //            eMod = this.DeWittTensorData[phase].AveragedEModul;
        //            break;
        //        case 5:
        //            eMod = this.GeometricHillTensorData[phase].AveragedEModul;
        //            break;
        //        default:
        //            eMod = this.HillTensorData[phase].AveragedEModul;
        //            break;
        //    }

        //    double plasticStrainRate = 0.0;
        //    int rateCounter = 0;

        //    for (int n = 0; n < stressStrainData.Count; n++)
        //    {
        //        if (stressStrainData[n].Stress > totalYieldStrength)
        //        {
        //            plasticStrainRate += stressStrainData[n].MacroskopicStrain - stressStrainData[n - 1].MacroskopicStrain;
        //            rateCounter++;
        //        }
        //    }

        //    plasticStrainRate /= rateCounter;

        //    double plasticStrain = 0.0;

        //    for (int n = 0; n < stressStrainData.Count; n++)
        //    {
        //        if (stressStrainData[n].Stress > totalYieldStrength)
        //        {
        //            plasticStrain += plasticStrainRate;
        //        }

        //        double[] dataTmp = { stressStrainData[n].Strain, ((1 / eMod) * stressStrainData[n].Stress) + plasticStrain };

        //        ret.Add(dataTmp);
        //    }

        //    return ret;
        //}

        #endregion

        #endregion

        #endregion

        #region Anisotropy

        #region Cubic

        public double GetLogEukAnisitropyCubic(int phase)
        {
            double VRShear = this.VoigtTensorData[phase].ConstantsShearModulusCubicStiffness / this.ReussTensorData[phase].ConstantsShearModulusCubicCompliance;
            double VRBulk = this.VoigtTensorData[phase].KappaCubicStiffness / this.ReussTensorData[phase].KappaCubicCompliance;

            VRShear = Math.Log(VRShear);
            VRBulk = Math.Log(VRBulk);

            double Ret = Math.Sqrt((5.0 * Math.Pow(VRShear, 2)) + Math.Pow(VRBulk, 2));

            return Ret;
        }

        public double GetUniversalAnisotropy(int phase)
        {
            double VRShear = this.VoigtTensorData[phase].ConstantsShearModulusCubicStiffness / this.ReussTensorData[phase].ConstantsShearModulusCubicCompliance;
            double VRBulk = this.VoigtTensorData[phase].KappaCubicStiffness / this.ReussTensorData[phase].KappaCubicCompliance;

            double Ret = VRBulk + (5.0 * VRShear) - 6;

            return Ret;
        }

        public double GetChungBuessemAnistropy(int phase)
        {
            double VRShearNeg = this.VoigtTensorData[phase].ConstantsShearModulusCubicStiffness - this.ReussTensorData[phase].ConstantsShearModulusCubicCompliance;
            double VRShearPos = this.VoigtTensorData[phase].ConstantsShearModulusCubicStiffness + this.ReussTensorData[phase].ConstantsShearModulusCubicCompliance;

            return VRShearNeg / VRShearPos;
        }

        #endregion

        #region Hexagonal

        public double GetLogEukAnisitropyHexagonal(int phase)
        {
            double VRShear = this.VoigtTensorData[phase].ConstantsShearModulusHexagonalStiffness / this.ReussTensorData[phase].ConstantsShearModulusHexagonalCompliance;
            double VRBulk = this.VoigtTensorData[phase].BulkModulusHexagonalStiffness / this.ReussTensorData[phase].BulkModulusHexagonalCompliance;

            VRShear = Math.Log(VRShear);
            VRBulk = Math.Log(VRBulk);

            double Ret = Math.Sqrt((5.0 * Math.Pow(VRShear, 2)) + Math.Pow(VRBulk, 2));

            return Ret;
        }

        public double GetUniversalAnisotropyHexagonal(int phase)
        {
            double VRShear = this.VoigtTensorData[phase].ConstantsShearModulusHexagonalStiffness / this.ReussTensorData[phase].ConstantsShearModulusHexagonalCompliance;
            double VRBulk = this.VoigtTensorData[phase].BulkModulusHexagonalStiffness / this.ReussTensorData[phase].BulkModulusHexagonalCompliance;

            double Ret = VRBulk + (5.0 * VRShear) - 6;

            return Ret;
        }

        public double GetChungBuessemAnistropyHexagonal(int phase)
        {
            double VRShearNeg = this.VoigtTensorData[phase].ConstantsShearModulusHexagonalStiffness - this.ReussTensorData[phase].ConstantsShearModulusHexagonalCompliance;
            double VRShearPos = this.VoigtTensorData[phase].ConstantsShearModulusHexagonalStiffness + this.ReussTensorData[phase].ConstantsShearModulusHexagonalCompliance;

            return VRShearNeg / VRShearPos;
        }

        #endregion

        #endregion

        public Sample()
        {
            this._name = "Default";
            this._actualPatternId = 0;
        }

        public Sample(int PatternId)
        {
            this._name = "Default";
            this._actualPatternId = PatternId;
        }



        #region IClonable

        public object Clone()
        {
            Sample Ret = new Sample();

            Ret.Name = this.Name;
            Ret._area = this._area;

            foreach(Pattern.DiffractionPattern DP in this.DiffractionPatterns)
            {
                Ret.DiffractionPatterns.Add((Pattern.DiffractionPattern)DP.Clone());
            }
            foreach(DataManagment.CrystalData.CODData CD in this.CrystalData)
            {
                Ret.CrystalData.Add(new DataManagment.CrystalData.CODData(CD));
            }
            foreach (Stress.Macroskopic.Elasticity ME in this.MacroElasticData)
            {
                Ret.MacroElasticData.Add(ME.Clone() as Stress.Macroskopic.Elasticity);
            }

            for(int n = 0; n < this.HillTensorData.Count; n++ )
            {
                Ret.VoigtTensorData.Add(this.VoigtTensorData[n].Clone() as Stress.Microsopic.ElasticityTensors);
                Ret.ReussTensorData.Add(this.ReussTensorData[n].Clone() as Stress.Microsopic.ElasticityTensors);
                Ret.HillTensorData.Add(this.HillTensorData[n].Clone() as Stress.Microsopic.ElasticityTensors);
                Ret.KroenerTensorData.Add(this.KroenerTensorData[n].Clone() as Stress.Microsopic.ElasticityTensors);
                Ret.DeWittTensorData.Add(this.DeWittTensorData[n].Clone() as Stress.Microsopic.ElasticityTensors);
                Ret.GeometricHillTensorData.Add(this.GeometricHillTensorData[n].Clone() as Stress.Microsopic.ElasticityTensors);
            }

            return Ret;
        }

        #endregion
    }
}
