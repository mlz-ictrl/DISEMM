﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalScec.Analysis.Stress.Plasticity
{
    public class PlasticityTensor
    {
        #region Old Stuff

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousStiffnessTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousStiffnessTensorError = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousComplianceTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousComplianceTensorError = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousGrainStiffnessTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousGrainStiffnessTensorError = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousGrainComplianceTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _instantaneousGrainComplianceTensorError = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _effectiveStiffnessTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _effectiveStiffnessTensorError = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _effectiveComplianceTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _effectiveComplianceTensorError = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(6, 6, 0.0);

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _hardenningTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _isotropicHardenningTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _independentHardenningTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _kinematicHardenningTensor = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _plasticStrainRate = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        //public List<MathNet.Numerics.LinearAlgebra.Matrix<double>> appliedCrystalStress = new List<MathNet.Numerics.LinearAlgebra.Matrix<double>>();

        //public MathNet.Numerics.LinearAlgebra.Matrix<double> _phaseStrainRate = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        //public double PhaseStrainRate
        //{
        //    get
        //    {
        //        return this._phaseStrainRate[2, 2];
        //    }
        //    set
        //    {
        //        this._phaseStrainRate[2, 2] = value;
        //    }
        //}

        //private double _phaseHardeningRate;
        //public double PhaseHardeningRate
        //{
        //    get
        //    {
        //        return this._phaseHardeningRate;
        //    }
        //    set
        //    {
        //        this._phaseHardeningRate = value;
        //    }
        //}
        //private double _phaseYieldStrength;
        //public double PhaseYieldStrength
        //{
        //    get
        //    {
        //        return this._phaseYieldStrength;
        //    }
        //    set
        //    {
        //        this._phaseYieldStrength = value;
        //        this.PhaseActYieldStrength = value;
        //    }
        //}
        //public double PhaseActYieldStrength = 0.0;

        #endregion

        /// <summary>
        /// Lc
        /// Mc
        /// </summary>
        public Tools.FourthRankTensor _grainStiffness = new Tools.FourthRankTensor();
        public Tools.FourthRankTensor GrainStiffness
        {
            get
            {
                return this._grainStiffness;
            }
            set
            {
                this._grainStiffness = value;
                this._grainCompliance = value.InverseSC();
            }
        }
        public Tools.FourthRankTensor _grainCompliance = new Tools.FourthRankTensor();
        public Tools.FourthRankTensor GrainCompliance
        {
            get
            {
                return this._grainCompliance;
            }
            set
            {
                this._grainCompliance = value;
                this._grainStiffness = value.InverseSC();
            }
        }

        /// <summary>
        /// Ac
        /// Bc
        /// </summary>
        public Tools.FourthRankTensor GrainTransitionStiffness = new Tools.FourthRankTensor();
        public Tools.FourthRankTensor GrainTransitionCompliance = new Tools.FourthRankTensor();

        public MathNet.Numerics.LinearAlgebra.Matrix<double> HardeningMatrix = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        public MathNet.Numerics.LinearAlgebra.Matrix<double> _conditionX = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        public MathNet.Numerics.LinearAlgebra.Matrix<double> ConditionX
        {
            get
            {
                return this._conditionX;
            }
            set
            {
                this._conditionX = value;
                this._conditionY = value.Inverse();
            }
        }
        public MathNet.Numerics.LinearAlgebra.Matrix<double> _conditionY = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(3, 3, 0.0);
        public MathNet.Numerics.LinearAlgebra.Matrix<double> ConditionY
        {
            get
            {
                return this._conditionY;
            }
            set
            {
                this._conditionY = value;
                this._conditionX = value.Inverse();
            }
        }
        //f^i
        public List<MathNet.Numerics.LinearAlgebra.Matrix<double>> InstantStiffnessFactors = new List<MathNet.Numerics.LinearAlgebra.Matrix<double>>();

        //To Be deleted
        public YieldSurface YieldSurfaceData;

        public int _symmetry;
        public string Symmetry
        {
            get
            {
                switch (this._symmetry)
                {
                    case 1:
                        return "cubic";
                    case 2:
                        return "hexagonal";
                    case 3:
                        return "tetragonal type 1";
                    case 4:
                        return "tetragonal type 2";
                    case 5:
                        return "trigonal type 1";
                    case 6:
                        return "trigonal type 2";
                    case 7:
                        return "rhombic";
                    case 8:
                        return "monoclinic";
                    case 9:
                        return "triclinic";
                    default:
                        return "triclinic";
                }
            }
            set
            {
                switch (value)
                {
                    case "cubic":
                        this._symmetry = 1;
                        break;
                    case "hexagonal":
                        this._symmetry = 2;
                        break;
                    case "tetragonal type 1":
                        this._symmetry = 3;
                        break;
                    case "tetragonal type 2":
                        this._symmetry = 4;
                        break;
                    case "trigonal type 1":
                        this._symmetry = 5;
                        break;
                    case "trigonal type 2":
                        this._symmetry = 6;
                        break;
                    case "rhombic":
                        this._symmetry = 7;
                        break;
                    case "monoclinic":
                        this._symmetry = 8;
                        break;
                    case "triclinic":
                        this._symmetry = 9;
                        break;
                    default:
                        this._symmetry = 9;
                        break;
                }
            }
        }

        #region Voigt notation
        


        #endregion

        public Texture.OrientationDistributionFunction ODF;

        //public YieldSurface YieldSurfaceData;
        
        /// <summary>
        /// Alpha, multiplied with the applied stress gets the resolved shear stress
        /// </summary>
        /// <param name="slipPlane"></param>
        /// <param name="slipDirection"></param>
        /// <returns>alpha</returns>
        public MathNet.Numerics.LinearAlgebra.Matrix<double> GetResolvingParameter(DataManagment.CrystalData.HKLReflex slipPlane, DataManagment.CrystalData.HKLReflex slipDirection)
        {
            return Tools.Calculation.GetResolvingParameter(slipPlane, slipDirection);
        }

        #region Old Stuff

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="potentialAktiveSystems"></param>
        ///// <param name="hardenningType">
        ///// 0: perfect plasticity
        ///// 1: isotropic hardening
        ///// 2: independent hardening
        ///// 3: kinematic hardening
        ///// </param>
        ///// <returns>Direction resolved hardening Matrix</returns>
        //public MathNet.Numerics.LinearAlgebra.Matrix<double> GetResolvedHardening(List<ReflexYield> potentialAktiveSystems, Microsopic.ElasticityTensors ET, int hardenningType)
        //{
        //    switch(hardenningType)
        //    {
        //        case 0:
        //            return this.GetResolvedPerfectHardening(potentialAktiveSystems, ET);
        //        case 1:
        //            return this.GetResolvedIsotropicHardening(potentialAktiveSystems, ET);
        //        case 2:
        //            return this.GetResolvedIndependentHardening(potentialAktiveSystems, ET);
        //        case 3:
        //            return this.GetResolvedKinematicHardening(potentialAktiveSystems, ET);
        //        default:
        //            return this.GetResolvedPerfectHardening(potentialAktiveSystems, ET);
        //    }
        //}

        //private MathNet.Numerics.LinearAlgebra.Matrix<double> GetResolvedPerfectHardening(List<ReflexYield> potentialAktiveSystems, Microsopic.ElasticityTensors ET)
        //{
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense(potentialAktiveSystems.Count, potentialAktiveSystems.Count, 0.0);

        //    for(int i = 0; i < potentialAktiveSystems.Count; i++)
        //    {
        //        for(int j = 0; j < potentialAktiveSystems.Count; j++)
        //        {
        //            MathNet.Numerics.LinearAlgebra.Matrix<double> resI = this.GetResolvingParameter(potentialAktiveSystems[i].SlipPlane, potentialAktiveSystems[i].MainSlipDirection);
        //            MathNet.Numerics.LinearAlgebra.Matrix<double> resJ = this.GetResolvingParameter(potentialAktiveSystems[j].SlipPlane, potentialAktiveSystems[j].MainSlipDirection);

        //            Tools.FourthRankTensor ETFourthRank = new Tools.FourthRankTensor(ET._stiffnessTensor);

        //            MathNet.Numerics.LinearAlgebra.Matrix<double> elasticResJ = ETFourthRank * resJ;

        //            for(int m = 0; m < 3; m++)
        //            {
        //                for(int n = 0; n < 3; n++)
        //                {
        //                    ret[i, j] += resI[m, n] * elasticResJ[m, n];
        //                }
        //            }
        //        }
        //    }

        //    return ret;
        //}

        //private MathNet.Numerics.LinearAlgebra.Matrix<double> GetResolvedIsotropicHardening(List<ReflexYield> potentialAktiveSystems, Microsopic.ElasticityTensors ET)
        //{
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ret = this.GetResolvedPerfectHardening(potentialAktiveSystems, ET);

        //    for (int i = 0; i < potentialAktiveSystems.Count; i++)
        //    {
        //        for (int j = 0; j < potentialAktiveSystems.Count; j++)
        //        {
        //            ret[i, j] += this._isotropicHardenningTensor[i, j];
        //        }
        //    }

        //    return ret;
        //}

        //private MathNet.Numerics.LinearAlgebra.Matrix<double> GetResolvedIndependentHardening(List<ReflexYield> potentialAktiveSystems, Microsopic.ElasticityTensors ET)
        //{
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ret = this.GetResolvedPerfectHardening(potentialAktiveSystems, ET);

        //    for (int i = 0; i < potentialAktiveSystems.Count; i++)
        //    {
        //        for (int j = 0; j < potentialAktiveSystems.Count; j++)
        //        {
        //            ret[i, j] += this._independentHardenningTensor[i, j];
        //        }
        //    }

        //    return ret;
        //}

        //private MathNet.Numerics.LinearAlgebra.Matrix<double> GetResolvedKinematicHardening(List<ReflexYield> potentialAktiveSystems, Microsopic.ElasticityTensors ET)
        //{
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ret = this.GetResolvedPerfectHardening(potentialAktiveSystems, ET);

        //    for (int i = 0; i < potentialAktiveSystems.Count; i++)
        //    {
        //        for (int j = 0; j < potentialAktiveSystems.Count; j++)
        //        {
        //            ret[i, j] += this._kinematicHardenningTensor[i, j];
        //        }
        //    }

        //    return ret;
        //}


        //public List<MathNet.Numerics.LinearAlgebra.Matrix<double>> GetShearRateCoefficientF(List<ReflexYield> potentialAktiveSystems, Microsopic.ElasticityTensors ET, int hardenningType)
        //{
        //    List<MathNet.Numerics.LinearAlgebra.Matrix<double>> ret = new List<MathNet.Numerics.LinearAlgebra.Matrix<double>>();

        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ResolvedHardening = this.GetResolvedHardening(potentialAktiveSystems, ET, hardenningType);
        //    MathNet.Numerics.LinearAlgebra.Matrix<double> ResolvedHardeningInverse = ResolvedHardening.Inverse();
        //    for(int i = 0; i < ResolvedHardeningInverse.RowCount; i++)
        //    {
        //        MathNet.Numerics.LinearAlgebra.Matrix<double> retTmp = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0);

        //        for (int j = 0; j < potentialAktiveSystems.Count; j++)
        //        {
        //            MathNet.Numerics.LinearAlgebra.Matrix<double> resJ = this.GetResolvingParameter(potentialAktiveSystems[j].SlipPlane, potentialAktiveSystems[j].MainSlipDirection);

        //            MathNet.Numerics.LinearAlgebra.Vector<double> resJVektor = MathNet.Numerics.LinearAlgebra.CreateVector.Dense<double>(6);

        //            resJVektor[0] = resJ[0, 0];
        //            resJVektor[1] = resJ[1, 1];
        //            resJVektor[2] = resJ[2, 2];
        //            resJVektor[3] = resJ[1, 2];
        //            resJVektor[4] = resJ[0, 2];
        //            resJVektor[5] = resJ[0, 1];

        //            MathNet.Numerics.LinearAlgebra.Vector<double> elasticResJVektor = ET._stiffnessTensor * resJVektor;

        //            MathNet.Numerics.LinearAlgebra.Matrix<double> elasticResJ = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0);

        //            elasticResJ[0, 0] = elasticResJVektor[0];
        //            elasticResJ[1, 1] = elasticResJVektor[1];
        //            elasticResJ[2, 2] = elasticResJVektor[2];

        //            elasticResJ[0, 1] = elasticResJVektor[5];
        //            elasticResJ[1, 0] = elasticResJVektor[5];

        //            elasticResJ[0, 2] = elasticResJVektor[4];
        //            elasticResJ[2, 0] = elasticResJVektor[4];

        //            elasticResJ[1, 2] = elasticResJVektor[3];
        //            elasticResJ[2, 1] = elasticResJVektor[3];

        //            elasticResJ = elasticResJ * ResolvedHardeningInverse[i, j];

        //            retTmp += elasticResJ;
        //        }

        //        ret.Add(retTmp);
        //    }

        //    return ret;
        //}

        #endregion

        public Tools.FourthRankTensor GetInstantanousGrainSiffnessTensor(List<ReflexYield> potentialAktiveSystems, Microsopic.ElasticityTensors ET, List<MathNet.Numerics.LinearAlgebra.Matrix<double>> coefficientF)
        {
            Tools.FourthRankTensor FRT = new Tools.FourthRankTensor();

            for(int n = 0; n < potentialAktiveSystems.Count; n++)
            {
                MathNet.Numerics.LinearAlgebra.Matrix<double> Resolvingparam = this.GetResolvingParameter(potentialAktiveSystems[n].SlipPlane, potentialAktiveSystems[n].MainSlipDirection);

                for(int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                FRT[i, j, k, l] += Resolvingparam[i, j] * coefficientF[n][k, l];
                            }
                        }
                    }
                }
            }

            Tools.FourthRankTensor UnitTensor = Tools.FourthRankTensor.GetUnityTensor();

            FRT = UnitTensor - FRT;

            Tools.FourthRankTensor StiffnessFourthRank = new Tools.FourthRankTensor(ET._stiffnessTensor);

            Tools.FourthRankTensor GrainStiffness = StiffnessFourthRank * FRT;

            return GrainStiffness;
        }

        #region Effective stiffness caluclations

        public Tools.FourthRankTensor GetAc(Tools.FourthRankTensor Ls, Tools.FourthRankTensor Lc, Tools.FourthRankTensor L)
        {
            Tools.FourthRankTensor inv = new Tools.FourthRankTensor();
            Tools.FourthRankTensor fac = new Tools.FourthRankTensor();

            inv = Ls + Lc;
            fac = Ls + L;

            Tools.FourthRankTensor ret = inv.InverseSC() * fac;

            return ret;
        }

        public Tools.FourthRankTensor GetBc(Tools.FourthRankTensor Ac, Tools.FourthRankTensor Lc, Tools.FourthRankTensor L)
        {
            Tools.FourthRankTensor inv = L.Inverse();
            Tools.FourthRankTensor ret = Lc * Ac * inv;

            return ret;
        }

        #region cubic

        private double GetBetaParameter(Microsopic.ElasticityTensors ET, int shearType)
        {
            double shearModulus = ET.ConstantsShearModulusCubicStiffness;
            //if(shearType == 1)
            //{
            //    shearModulus = ET.KroenerShearModulus;
            //}
            //else if(shearType == 2)
            //{
            //    shearModulus = ET.DeWittShearModulus;
            //}

            double Z = 6 * (ET.KappaCubicStiffness + (2 * shearModulus));
            double N = 5 * ((3 * ET.KappaCubicStiffness) + (4 * shearModulus));

            return Z / N;
        }

        public Tools.FourthRankTensor GetConstraintStiffnessCubicIsotropic(Microsopic.ElasticityTensors ET, int shearType)
        {
            double shearModulus = ET.ConstantsShearModulusCubicStiffness;
            //if (ET.Symmetry == "cubic")
            //{
            //    if (shearType == 1 && ET.KroenerShearModulus != 0)
            //    {
            //        shearModulus = ET.KroenerShearModulus;
            //    }
            //    else if (shearType == 2 && ET.DeWittShearModulus != 0)
            //    {
            //        shearModulus = ET.DeWittShearModulus;
            //    }
            //}
            //else if (ET.Symmetry == "hexagonal")
            //{
            //    shearModulus = ET.ConstantsShearModulusHexagonalStiffness;
            //}
            //double shearModulus = ET.ConstantsShearModulusHexagonalStiffness;

            Tools.FourthRankTensor ret = new Tools.FourthRankTensor();

            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            double IJ = 0;
                            double KL = 0;
                            double IK = 0;
                            double JL = 0;
                            double IL = 0;
                            double JK = 0;
                            if (i == j)
                            {
                                IJ = 1;
                            }
                            if (k == l)
                            {
                                KL = 1;
                            }
                            if (i == k)
                            {
                                IK = 1;
                            }
                            if (l == j)
                            {
                                JL = 1;
                            }
                            if (i == l)
                            {
                                IL = 1;
                            }
                            if (k == j)
                            {
                                JK = 1;
                            }

                            double FirstTmp = 4.0 / 3.0;
                            FirstTmp *= shearModulus * IJ * KL;

                            double SecondTmp = (IK * JL) + (IL * JK) + (IJ * KL);
                            SecondTmp *= shearModulus;
                            double beta = this.GetBetaParameter(ET, shearType);
                            SecondTmp *= (1 - beta) / beta;

                            ret[i, j, k, l] = FirstTmp + SecondTmp;
                        }
                    }
                }
            }

            return ret;
        }

        #endregion

        #region hexagonal

        private double hA0Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            return usedStiffness[0, 0] * usedStiffness[3, 3] * usedStiffness[5, 5];
        }

        private double hA1Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            double ret = -3.0 * usedStiffness[0, 0] * usedStiffness[3, 3] * usedStiffness[5, 5];
            ret += usedStiffness[0, 0] * usedStiffness[2, 2] * usedStiffness[5, 5];
            ret -= Math.Pow(usedStiffness[0, 2], 2) * usedStiffness[5, 5];
            ret -= 2.0 * usedStiffness[0, 2] * usedStiffness[3, 3] * usedStiffness[5, 5];
            ret += usedStiffness[0, 0] * Math.Pow(usedStiffness[3, 3], 2);

            return ret;
        }

        private double hA2Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            double ret = 3.0 * usedStiffness[0, 0] * usedStiffness[3, 3] * usedStiffness[5, 5];
            ret += usedStiffness[0, 0] * usedStiffness[2, 2] * usedStiffness[3, 3];
            ret += usedStiffness[2, 2] * usedStiffness[3, 3] * usedStiffness[5, 5];
            ret -= Math.Pow(usedStiffness[0, 2], 2) * usedStiffness[3, 3];
            ret -= 2.0 * Math.Pow(usedStiffness[3, 3], 2) * usedStiffness[0, 0];
            ret -= 2.0 * Math.Pow(usedStiffness[0, 2], 2) * usedStiffness[5, 5];
            ret += 4.0 * usedStiffness[0, 2] * usedStiffness[3, 3] * usedStiffness[5, 5];

            return ret;
        }

        private double hA3Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            double ret = -1.0 * usedStiffness[0, 0] * usedStiffness[3, 3] * usedStiffness[5, 5];
            ret += Math.Pow(usedStiffness[3, 3], 2) * usedStiffness[2, 2];
            ret -= usedStiffness[0, 0] * usedStiffness[2, 2] * usedStiffness[3, 3];
            ret -= usedStiffness[2, 2] * usedStiffness[3, 3] * usedStiffness[5, 5];
            ret += Math.Pow(usedStiffness[0, 2], 2) * usedStiffness[3, 3];
            ret += 2.0 * Math.Pow(usedStiffness[3, 3], 2) * usedStiffness[0, 2];
            ret += usedStiffness[0, 0] * usedStiffness[2, 2] * usedStiffness[5, 5];
            ret += Math.Pow(usedStiffness[3, 3], 2) * usedStiffness[0, 0];
            ret -= Math.Pow(usedStiffness[0, 2], 2) * usedStiffness[5, 5];
            ret -= 2.0 * usedStiffness[0, 2] * usedStiffness[0, 1] * usedStiffness[5, 5];

            return ret;
        }

        private MathNet.Numerics.LinearAlgebra.Matrix<double> GetLambda0Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0);

            ret[0, 0] = usedStiffness[3, 3] * (usedStiffness[0, 0] + (3.0 * usedStiffness[5, 5]));
            ret[1, 1] = ret[0, 0];

            ret[0, 1] = -1.0 * usedStiffness[3, 3] * (usedStiffness[0, 0] + usedStiffness[5, 5]);
            ret[1, 0] = ret[0, 1];

            ret[0, 2] = 0;
            ret[2, 0] = ret[0, 2];
            ret[1, 2] = ret[0, 2];
            ret[2, 1] = ret[0, 2];

            ret[2, 2] = 0;
            
            ret[3, 3] = usedStiffness[0, 0] * usedStiffness[5, 5];
            ret[4, 4] = ret[3, 3];
            
            ret[5, 5] = 0.5 * (ret[0, 0] - ret[0, 1]);

            return ret;
        }

        private MathNet.Numerics.LinearAlgebra.Matrix<double> GetLambda1Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0);

            ret[0, 0] = (usedStiffness[0, 0] + (3.0 * usedStiffness[5, 5])) * (usedStiffness[2, 2] - (3.0 * usedStiffness[3, 3]));
            ret[0, 0] -= Math.Pow(usedStiffness[0, 2] + usedStiffness[3, 3], 2);
            ret[0, 0] += 4.0 * Math.Pow(usedStiffness[3, 3], 2);
            ret[1, 1] = ret[0, 0];

            ret[0, 1] = ((3.0 * usedStiffness[3, 3]) - usedStiffness[2, 2]) * (usedStiffness[0, 0] - usedStiffness[5, 5]);
            ret[0, 1] += Math.Pow(usedStiffness[0, 2] - usedStiffness[3, 3], 2);
            ret[1, 0] = ret[0, 1];

            ret[0, 2] = -4.0 * usedStiffness[5, 5] * (usedStiffness[0, 2] + usedStiffness[3, 3]);
            ret[2, 0] = ret[0, 2];
            ret[1, 2] = ret[0, 2];
            ret[2, 1] = ret[0, 2];

            ret[2, 2] = 8.0 * usedStiffness[0, 0] * usedStiffness[5, 5];

            ret[3, 3] = 2.0 * usedStiffness[0, 0] * usedStiffness[3, 3];
            ret[3, 3] -= 2.0 * usedStiffness[0, 2] * usedStiffness[5, 5];
            ret[3, 3] -= 3.0 * usedStiffness[0, 0] * usedStiffness[5, 5];
            ret[4, 4] = ret[3, 3];

            ret[5, 5] = 0.5 * (ret[0, 0] - ret[0, 1]);

            return ret;
        }

        private MathNet.Numerics.LinearAlgebra.Matrix<double> GetLambda2Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0);

            ret[0, 0] = ((3.0 * usedStiffness[3, 3]) - (2.0 * usedStiffness[2, 2])) * (usedStiffness[0, 0] + (3.0 * usedStiffness[5, 5]));
            ret[0, 0] += 4.0 * usedStiffness[2, 2] * usedStiffness[3, 3];
            ret[0, 0] -= 8.0 * Math.Pow(usedStiffness[3, 3], 2);
            ret[0, 0] += 2.0 * Math.Pow(usedStiffness[0, 2] + usedStiffness[3, 3], 2);
            ret[1, 1] = ret[0, 0];

            ret[0, 1] = ((2.0 * usedStiffness[2, 2]) - (3.0 * usedStiffness[3, 3])) * (usedStiffness[0, 0] - usedStiffness[5, 5]);
            ret[0, 1] -= 2.0 * Math.Pow(usedStiffness[0, 2] - usedStiffness[3, 3], 2);
            ret[1, 0] = ret[0, 1];

            ret[0, 2] = -4.0 * ((2.0 * usedStiffness[5, 5]) - usedStiffness[3, 3]) * (usedStiffness[0, 2] + usedStiffness[3, 3]);
            ret[2, 0] = ret[0, 2];
            ret[1, 2] = ret[0, 2];
            ret[2, 1] = ret[0, 2];

            ret[2, 2] = -16.0 * usedStiffness[0, 0] * usedStiffness[5, 5];
            ret[2, 2] += 8.0 * usedStiffness[3, 3] * (usedStiffness[0, 2] + usedStiffness[5, 5]);

            ret[3, 3] = usedStiffness[0, 0] * usedStiffness[0, 2];
            ret[3, 3] += usedStiffness[2, 2] * usedStiffness[5, 5];
            ret[3, 3] -= 4.0 * usedStiffness[0, 0] * usedStiffness[3, 3];
            ret[3, 3] += Math.Pow(usedStiffness[3, 3], 2);
            ret[3, 3] -= Math.Pow(usedStiffness[0, 2] + usedStiffness[3, 3], 2);
            ret[3, 3] += 4.0 * usedStiffness[0, 2] * usedStiffness[5, 5];
            ret[3, 3] -= 2.0 * usedStiffness[0, 2] * usedStiffness[3, 3];
            ret[3, 3] += 3.0 * usedStiffness[0, 0] * usedStiffness[5, 5];
            ret[4, 4] = ret[3, 3];

            ret[5, 5] = 0.5 * (ret[0, 0] - ret[0, 1]);

            return ret;
        }

        private MathNet.Numerics.LinearAlgebra.Matrix<double> GetLambda3Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> ret = MathNet.Numerics.LinearAlgebra.CreateMatrix.Dense<double>(6, 6, 0);

            ret[0, 0] = (usedStiffness[2, 2] - usedStiffness[3, 3]) * (usedStiffness[0, 0] + (3.0 * usedStiffness[5, 5]) - (4.0 * usedStiffness[3, 3]));
            ret[0, 0] -= Math.Pow(usedStiffness[0, 2] + usedStiffness[3, 3], 2);
            ret[1, 1] = ret[0, 0];

            ret[0, 1] = (usedStiffness[3, 3] - usedStiffness[2, 2]) * (usedStiffness[0, 0] - usedStiffness[5, 5]);
            ret[0, 1] += Math.Pow(usedStiffness[0, 2] + usedStiffness[3, 3], 2);
            ret[1, 0] = ret[0, 1];

            ret[0, 2] = -4.0 * (usedStiffness[3, 3] - usedStiffness[5, 5]) * (usedStiffness[0, 2] + usedStiffness[3, 3]);
            ret[2, 0] = ret[0, 2];
            ret[1, 2] = ret[0, 2];
            ret[2, 1] = ret[0, 2];

            ret[2, 2] = 8.0 * (usedStiffness[0, 0] - usedStiffness[3, 3]) * (usedStiffness[5, 5] - usedStiffness[3, 3]);

            ret[3, 3] = -1.0 * usedStiffness[0, 0] * usedStiffness[2, 2];
            ret[3, 3] -= usedStiffness[2, 2] * usedStiffness[5, 5];
            ret[3, 3] += 2.0 * usedStiffness[2, 2] * usedStiffness[3, 3];
            ret[3, 3] += 2.0 * usedStiffness[0, 0] * usedStiffness[3, 3];
            ret[3, 3] -= Math.Pow(usedStiffness[3, 3], 2);
            ret[3, 3] += Math.Pow(usedStiffness[0, 2] + usedStiffness[3, 3], 2);
            ret[3, 3] -= 2.0 * usedStiffness[0, 2] * usedStiffness[5, 5];
            ret[3, 3] += 2.0 * usedStiffness[0, 2] * usedStiffness[3, 3];
            ret[3, 3] -= usedStiffness[0, 0] * usedStiffness[5, 5];
            ret[4, 4] = ret[3, 3];

            ret[5, 5] = 0.5 * (ret[0, 0] - ret[0, 1]);

            return ret;
        }

        //TODO: Implement this one
        private Tools.FourthRankTensor GetLambdaPrime0Parameter(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            Tools.FourthRankTensor ret = new Tools.FourthRankTensor();

            ret[0, 0, 0, 0] = usedStiffness[3, 3] * (usedStiffness[0, 0] + (3.0 * usedStiffness[5, 5]));
            ret[1, 1, 1, 1] = ret[0, 0, 0, 0];

            return ret;
        }

        public Tools.FourthRankTensor GetElastoPlasticEshelbyTensor(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            Tools.FourthRankTensor LambdaPrime = this.GetLambdaPrime0Parameter(usedStiffness);
            
            Tools.FourthRankTensor StiffnessTensor = new Tools.FourthRankTensor(usedStiffness);

            return LambdaPrime * StiffnessTensor;
        }

        public Tools.FourthRankTensor GetEffectiveStiffnessHexagonal(MathNet.Numerics.LinearAlgebra.Matrix<double> usedStiffness)
        {
            Tools.FourthRankTensor EshelbyTensor = this.GetElastoPlasticEshelbyTensor(usedStiffness);
            MathNet.Numerics.LinearAlgebra.Matrix<double> EshelbyVoigt = EshelbyTensor.GetVoigtTensor();
            MathNet.Numerics.LinearAlgebra.Matrix<double> InverseEshelbyVoigt = EshelbyVoigt.Inverse();
            Tools.FourthRankTensor InverseEshelbyTensor = new Tools.FourthRankTensor(InverseEshelbyVoigt);

            Tools.FourthRankTensor UnitTensor = Tools.FourthRankTensor.GetUnityTensor();
            Tools.FourthRankTensor StiffnessTensor = new Tools.FourthRankTensor(usedStiffness);

            Tools.FourthRankTensor Ret = InverseEshelbyTensor - UnitTensor;
            Ret = StiffnessTensor * Ret;

            return Ret;
        }

        #endregion

        #endregion
    }
}
