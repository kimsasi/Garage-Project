using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class Car : Vehicle
    {
        private const byte k_MaxNumberOfDoors = 5;
        private const byte k_MinNumberOfDoors = 2;
        private byte m_NumberOfDoors;
        private eColor m_Color;

        internal enum eColor
        {
            Red = 1,
            White,
            Black,
            Silver
        }

        internal enum eRequiredCarParams
        {
            Color = 2,
            NumberOfDoors
        }

        // C'tor
        public Car(string i_LicenseNumber, string i_ModelName, float i_EnergyPercentageLeft, Engine.eEngineType i_EngineType)
            : base(i_LicenseNumber, i_ModelName)
        {
            initializeEngine(i_EngineType, i_EnergyPercentageLeft);
        }

        // Properties
        internal byte NumberOfDoors
        {
            get
            {
                return this.m_NumberOfDoors;
            }

            set
            {
                if(value < k_MinNumberOfDoors || value > k_MaxNumberOfDoors)
                {
                    throw new ValueOutOfRangeException(k_MinNumberOfDoors, k_MaxNumberOfDoors);
                }

                this.m_NumberOfDoors = value;
            }
        }

        internal eColor Color
        {
            get
            {
                return this.m_Color;
            }

            set
            {
                if(!Enum.IsDefined(typeof(eColor), value))
                {
                    throw new ValueOutOfRangeException((float)eColor.Red, (float)eColor.Silver);
                }

                this.m_Color = value;
            }
        }

        public override void UpdateAdditionalInfo(List<string> i_Answers)
        {
            try
            {
                string wheelsManufacturer = i_Answers[(int)Wheel.eRequiredWheelParams.ManufacturerName];
                float currentAirPressure = float.Parse(i_Answers[(int)Wheel.eRequiredWheelParams.CurrentAirPressure]);
                InitializeWheelsList(wheelsManufacturer, eNumberOfWheels.Car, Wheel.eMaxAirPressure.Car, currentAirPressure);
                Color = (eColor)Enum.Parse(typeof(eColor), i_Answers[(int)eRequiredCarParams.Color]);
                NumberOfDoors = byte.Parse(i_Answers[(int)eRequiredCarParams.NumberOfDoors]);
            }
            catch(ArgumentException)
            {
                // User friendly message
                throw new FormatException("Input was not in the correct format");
            }
        }

        public override List<string> GetUserQuestions()
        {
            List<string> userQuestions = new List<string>();
            
            string wheelCurrentAirPressure = string.Format(
                "Please enter your wheels current air pressure (not more than {0})",
                (int)Wheel.eMaxAirPressure.Car);

            string colorQuestion = string.Format(
                @"Please choose the color of your car:

Press {0} to choose {1}
Press {2} to choose {3}
Press {4} to choose {5}
Press {6} to choose {7}",
                            (int)eColor.Red,
                            eColor.Red,
                            (int)eColor.White,
                            eColor.White,
                            (int)eColor.Black,
                            eColor.Black,
                            (int)eColor.Silver,
                            eColor.Silver);

            string numOfDoorsQuestion = string.Format(
                "Please enter your car's number of doors (a number between {0}-{1})",
                k_MinNumberOfDoors,
                k_MaxNumberOfDoors);

            userQuestions.Add("Please enter your wheels manufacturer: ");
            userQuestions.Add(wheelCurrentAirPressure);
            userQuestions.Add(colorQuestion);
            userQuestions.Add(numOfDoorsQuestion);

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
                    isValidInput = Wheel.CheckAirPressureValidation(i_CurrentInput, (float)Wheel.eMaxAirPressure.Car);
                    break;

                case (int)eRequiredCarParams.Color:
                    isValidInput = Enum.TryParse(i_CurrentInput, true, out eColor carColor)
                                   && Enum.IsDefined(typeof(eColor), carColor);
                    break;

                case (int)eRequiredCarParams.NumberOfDoors:
                    bool isNumericInput = byte.TryParse(i_CurrentInput, out byte numericInput);
                    if(isNumericInput)
                    {
                        isValidInput = numericInput >= k_MinNumberOfDoors && numericInput <= k_MaxNumberOfDoors;
                    }

                    break;
            }

            return isValidInput;
        }

        public override string ToString()
        {
            StringBuilder stringResult = new StringBuilder();
            string carInfo = string.Format(
                @"Color: {0}
Number of doors: {1}",
                Color,
                NumberOfDoors);
            stringResult.Append(base.ToString());
            stringResult.Append(carInfo);
            return stringResult.ToString();
        }

        private void initializeEngine(Engine.eEngineType i_EngineType, float i_CurrentEnergyAmount)
        {
            if (i_EngineType == Engine.eEngineType.Fuel)
            {
                this.m_Engine = new FuelEngine(FuelEngine.eFuelTankCapacity.Car, i_CurrentEnergyAmount, FuelEngine.eFuelType.Octan96);
            }
            else
            {
                this.m_Engine = new ElectricEngine(ElectricEngine.eFullChargeMinutesCapacity.Car, i_CurrentEnergyAmount);
            }
        }
    }
}