using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    
    internal class Truck : Vehicle
    {
        internal enum eRequiredTruckParams
        {
            HazardousMaterials = 2,
            CargoVolume
        }

        private float m_CargoVolume;
        private bool m_IsCarryingHazardousMaterials;

        // C'tor
        public Truck(string i_LicenseNumber, string i_ModelName, float i_EnergyPercentageLeft, Engine.eEngineType i_EngineType)
            : base(i_LicenseNumber, i_ModelName)
        {
            InitializeEngine(i_EngineType, i_EnergyPercentageLeft);
        }

        // Properties
        public float CargoVolume
        {
            get
            {
                return this.m_CargoVolume;
            }

            set
            {
                if(value <= 0)
                {
                    // No max value, therefore no use for ValueOutOfRangeException class
                    throw new ArgumentOutOfRangeException("Cargo volume", "Cargo volume must be a positive number");
                }

                this.m_CargoVolume = value;
            }
        }

        public bool IsCarryingHazardousMaterials
        {
            get
            {
                return this.m_IsCarryingHazardousMaterials;
            }

            set
            {
                this.m_IsCarryingHazardousMaterials = value;
            }
        }
        

        public override List<string> GetUserQuestions()
        {
            List<string> userQuestions = new List<string>();

            string wheelCurrentAirPressure = string.Format(
                "Please enter your wheels current air pressure (not more than {0})",
                (int)Wheel.eMaxAirPressure.Truck);

            userQuestions.Add("Please enter your wheels manufacturer: ");
            userQuestions.Add(wheelCurrentAirPressure);
            userQuestions.Add("Do you carry hazardous materials? (Y/N) ");
            userQuestions.Add("Please enter your cargo volume: ");

            return userQuestions;
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
                    isValidInput = Wheel.CheckAirPressureValidation(i_CurrentInput, (float)Wheel.eMaxAirPressure.Truck);
                    break;

                case (int)eRequiredTruckParams.HazardousMaterials:
                    bool isBoolean = char.TryParse(i_CurrentInput, out char choiceInputResult);
                    if(isBoolean)
                    {
                        isValidInput = choiceInputResult == 'y' | choiceInputResult == 'Y' | choiceInputResult == 'n'
                                       | choiceInputResult == 'N';
                    }

                    break;

                case (int)eRequiredTruckParams.CargoVolume:
                    bool isNumeric = float.TryParse(i_CurrentInput, out float floatNumericInput);
                    if (isNumeric)
                    {
                        isValidInput = floatNumericInput >= 0;
                    }

                    break;
            }

            return isValidInput;
        }

        public override void UpdateAdditionalInfo(List<string> i_Answers)
        {
            try
            {
                string wheelsManufacturer = i_Answers[(int)Wheel.eRequiredWheelParams.ManufacturerName];
                float currentAirPressure = float.Parse(i_Answers[(int)Wheel.eRequiredWheelParams.CurrentAirPressure]);
                InitializeWheelsList(
                    wheelsManufacturer,
                    eNumberOfWheels.Truck,
                    Wheel.eMaxAirPressure.Truck,
                    currentAirPressure);
                char hazardousMaterialsInput = char.Parse(i_Answers[(int)eRequiredTruckParams.HazardousMaterials]);
                string hazardousMaterialsBooleanAnswer = checkHazardousMaterialInput(hazardousMaterialsInput);
                IsCarryingHazardousMaterials = bool.Parse(hazardousMaterialsBooleanAnswer);
                CargoVolume = float.Parse(i_Answers[(int)eRequiredTruckParams.CargoVolume]);
            }
            catch(ArgumentOutOfRangeException)
            {
                throw;
            }
            catch(FormatException)
            {
                // User friendly message
                throw new FormatException("Invalid input, the input must be Y/N");
            }
            catch(Exception)
            {
                // User friendly message
                throw new FormatException("Input was not in the correct format");
            }
        }

        private static string checkHazardousMaterialInput(char i_Input)
        {
            string booleanAnswer = i_Input.ToString();

            if(i_Input == 'y' | i_Input == 'Y')
            {
                booleanAnswer = "True";     // Boolean.TrueString Field
            }
            else if(i_Input == 'n' | i_Input == 'N')
            {
                booleanAnswer = "False";    // Boolean.FalseString Field
            }

            return booleanAnswer;
        }

        public override string ToString()
        {
            StringBuilder stringResult = new StringBuilder();
            string truckInfo = string.Format(
                @"Carrying hazardous materials? {0}
Cargo volume: {1}",
                IsCarryingHazardousMaterials ? "Yes" : "No",
                CargoVolume);

            stringResult.Append(base.ToString());
            stringResult.Append(truckInfo);
            return stringResult.ToString();
        }

        internal void InitializeEngine(Engine.eEngineType i_EngineType, float i_CurrentEnergyAmount)
        {
            this.m_Engine = new FuelEngine(FuelEngine.eFuelTankCapacity.Truck, i_CurrentEnergyAmount, FuelEngine.eFuelType.Soler);
        }
    }
}