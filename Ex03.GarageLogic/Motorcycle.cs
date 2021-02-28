using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class Motorcycle : Vehicle
    {
        private eLicenseType m_LicenseType;
        private int m_EngineCapacity;

        public enum eLicenseType
        {
            A = 1,
            A1,
            AA,
            B
        }

        internal enum eRequiredMotorcycleParams
        {
            LicenseType = 2,
            EngineCapacity
        }

        // C'tor
        public Motorcycle(string i_LicenseNumber, string i_ModelName, float i_EnergyPercentageLeft, Engine.eEngineType i_EngineType)
            : base(i_LicenseNumber, i_ModelName)
        {
            initializeEngine(i_EngineType, i_EnergyPercentageLeft);
        }

        // Properties
        public eLicenseType LicenseType
        {
            get
            {
                return this.m_LicenseType;
            }

            set
            {
                if(!Enum.IsDefined(typeof(eLicenseType), value))
                {
                    throw new ValueOutOfRangeException((float)eLicenseType.A, (float)eLicenseType.B);
                }

                this.m_LicenseType = value;
            }
        }

        public int EngineCapacity
        {
            get
            {
                return this.m_EngineCapacity;
            }

            set
            {
                if(value <= 0)
                {
                    // No max value, therefore no use for ValueOutOfRangeException class
                    throw new ArgumentOutOfRangeException("Engine Capacity", "Engine capacity must be a positive number");
                }

                this.m_EngineCapacity = value;
            }
        }

        public override List<string> GetUserQuestions()
        {
            List<string> userQuestions = new List<string>();

            string wheelCurrentAirPressure = string.Format(
                "Please enter your wheels current air pressure (not more than {0})",
                (int)Wheel.eMaxAirPressure.Motorcycle);

            string licenseTypeQuestion = string.Format(
                @"Please enter license type:
Press {0} to choose {1}
Press {2} to choose {3}
Press {4} to choose {5}
Press {6} to choose {7}",
                (int)eLicenseType.A,
                eLicenseType.A,
                (int)eLicenseType.A1,
                eLicenseType.A1,
                (int)eLicenseType.AA,
                eLicenseType.AA,
                (int)eLicenseType.B,
                eLicenseType.B);

            userQuestions.Add("Please enter your wheels manufacturer: ");
            userQuestions.Add(wheelCurrentAirPressure);
            userQuestions.Add(licenseTypeQuestion);
            userQuestions.Add("Please enter your motorcycle engine capacity: ");

            return userQuestions;
        }

        public override void UpdateAdditionalInfo(List<string> i_Answers)
        {
            try
            {
                string wheelsManufacturer = i_Answers[(int)Wheel.eRequiredWheelParams.ManufacturerName];
                float currentAirPressure = float.Parse(i_Answers[(int)Wheel.eRequiredWheelParams.CurrentAirPressure]);
                InitializeWheelsList(
                    wheelsManufacturer,
                    eNumberOfWheels.Motorcycle,
                    Wheel.eMaxAirPressure.Motorcycle,
                    currentAirPressure);
                LicenseType = (eLicenseType)Enum.Parse(
                    typeof(eLicenseType),
                    i_Answers[(int)eRequiredMotorcycleParams.LicenseType]);
                EngineCapacity = int.Parse(i_Answers[(int)eRequiredMotorcycleParams.EngineCapacity]);
            }
            catch(ArgumentException errorException)
            {
                if(errorException is ArgumentOutOfRangeException)
                {
                    throw;
                }

                // User friendly message
                throw new FormatException("Input was not in the correct format");
            }
        }

        public override bool CheckValidInput(string i_CurrentInput, int i_QuestionIndex)
        {
            bool isValidInput = false;

            switch (i_QuestionIndex)
            {
                case (int)Wheel.eRequiredWheelParams.ManufacturerName:
                    isValidInput = !string.IsNullOrEmpty(i_CurrentInput);
                    break;

                case (int)Wheel.eRequiredWheelParams.CurrentAirPressure:
                    isValidInput = Wheel.CheckAirPressureValidation(i_CurrentInput, (float)Wheel.eMaxAirPressure.Motorcycle);
                    break;

                case (int)eRequiredMotorcycleParams.LicenseType:
                    isValidInput = Enum.TryParse(i_CurrentInput, true, out eLicenseType licenseType) && Enum.IsDefined(typeof(eLicenseType), licenseType);
                    break;

                case (int)eRequiredMotorcycleParams.EngineCapacity:
                    isValidInput = int.TryParse(i_CurrentInput, out int result) && result >= 0;
                    break;
            }

            return isValidInput;
        }

        public override string ToString()
        {
            StringBuilder stringResult = new StringBuilder();
            string motorCycleInfo = string.Format(
                @"License type: {0}
Engine capacity: {1}",
                LicenseType,
                EngineCapacity);

            stringResult.Append(base.ToString());
            stringResult.Append(motorCycleInfo);
            return stringResult.ToString();
        }

        private void initializeEngine(Engine.eEngineType i_EngineType, float i_CurrentEnergyAmount)
        {
            if(i_EngineType == Engine.eEngineType.Fuel)
            {
                this.m_Engine = new FuelEngine(FuelEngine.eFuelTankCapacity.Motorcycle, i_CurrentEnergyAmount, FuelEngine.eFuelType.Octan95);
            }
            else
            {
                this.m_Engine = new ElectricEngine(ElectricEngine.eFullChargeMinutesCapacity.Motorcycle, i_CurrentEnergyAmount);
            }
        }
    }
}
