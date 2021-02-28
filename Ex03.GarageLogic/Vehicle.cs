using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        public class Wheel
        {
            private readonly float r_MaxAirPressure;
            private readonly string r_Manufacturer;
            private float m_CurrentAirPressure;

            public enum eMaxAirPressure
            {
                Truck = 28,
                Motorcycle = 30,
                Car = 32
            }

            internal enum eRequiredWheelParams
            {
                ManufacturerName,
                CurrentAirPressure
            }

            // C'tor
            public Wheel(float i_MaxAirPressure, float i_CurrentAirPressure, string i_Manufacturer)
            {
                this.r_MaxAirPressure = i_MaxAirPressure;
                this.r_Manufacturer = i_Manufacturer;
                CurrentAirPressure = i_CurrentAirPressure;
            }

            // Properties
            public float MaxAirPressure
            {
                get
                {
                    return this.r_MaxAirPressure;
                }
            }

            public float CurrentAirPressure
            {
                get
                {
                    return this.m_CurrentAirPressure;
                }

                set
                {
                    if(value < 0 || value > MaxAirPressure)
                    {
                        throw new ValueOutOfRangeException(0, MaxAirPressure);
                    }

                    this.m_CurrentAirPressure = value;
                }
            }

            public string Manufacturer
            {
                get
                {
                    return this.r_Manufacturer;
                }
            }

            internal static bool CheckAirPressureValidation(string i_InputAirPressure, float i_MaxAirPressure)
            {
                bool isNumericInput = float.TryParse(i_InputAirPressure, out float numericAirPressureInput);
                bool isValidInput = false;

                if (isNumericInput)
                {
                    isValidInput = numericAirPressureInput >= 0 && numericAirPressureInput <= i_MaxAirPressure;
                }

                return isValidInput;
            }

            public override string ToString()
            {
                return string.Format(
                    @"Current air pressure: {0}
Manufacturer: {1}", 
                    m_CurrentAirPressure, 
                    r_Manufacturer);
            }

            internal void InflateWheel(float i_AmountOfAirToAdd)
            {
                if (m_CurrentAirPressure + i_AmountOfAirToAdd > r_MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(r_MaxAirPressure, 0);
                }

                CurrentAirPressure += i_AmountOfAirToAdd;
            }
        }

        private readonly string r_LicenseNumber;
        private readonly string r_ModelName;
        private List<Wheel> m_Wheels;
        protected Engine m_Engine = null;

        public enum eNumberOfWheels
        {
            Motorcycle = 2,
            Car = 4,
            Truck = 16
        }

        // C'tor
        protected Vehicle(string i_LicenseNumber, string i_ModelName)
        {
            r_LicenseNumber = i_LicenseNumber;
            r_ModelName = i_ModelName;
        }

        // Operator ==
        public static bool operator ==(Vehicle i_Vehicle1, Vehicle i_Vehicle2)
        {
            return i_Vehicle2 != null && (i_Vehicle1 != null && i_Vehicle1.r_LicenseNumber == i_Vehicle2.r_LicenseNumber);
        }

        // operator !=
        public static bool operator !=(Vehicle i_Vehicle1, Vehicle i_Vehicle2)
        {
            return i_Vehicle2 != null && (i_Vehicle1 != null && i_Vehicle1.r_LicenseNumber != i_Vehicle2.r_LicenseNumber);
        }

        public override bool Equals(object i_Obj)
        {
            bool isEquals = false;

            Vehicle toCompareTo = i_Obj as Vehicle;
            if(toCompareTo != null)
            {
                isEquals = LicenseNumber == toCompareTo.LicenseNumber;
            }

            return isEquals;
        }

        public override int GetHashCode()
        {
            return LicenseNumber.GetHashCode();
        }

        // Properties:
        public List<Wheel> Wheels
        {
            get
            {
                return this.m_Wheels;
            }
        }

        public string LicenseNumber
        {
            get
            {
                return this.r_LicenseNumber;
            }
        }

        public Engine Engine
        {
            get
            {
                return this.m_Engine;
            }
        }

        public string ModelName
        {
            get
            {
                return this.r_ModelName;
            }
        }

        protected void InitializeWheelsList(
            string i_Manufacturer,
            eNumberOfWheels i_NumberOfWheels,
            Wheel.eMaxAirPressure i_MaxAirPressure,
            float i_CurrentAirPressure)
        {
            m_Wheels = new List<Wheel>();

            for(int i = 0; i < (int)i_NumberOfWheels; i++)
            {
                Wheels.Add(new Wheel((float)i_MaxAirPressure, i_CurrentAirPressure, i_Manufacturer));
            }
        }

        public abstract List<string> GetUserQuestions();

        public abstract bool CheckValidInput(string i_CurrentInput, int i_QuestionIndex);

        public abstract void UpdateAdditionalInfo(List<string> i_Answers);

        public override string ToString()
        {
            StringBuilder resultString = new StringBuilder();

            string vehicleInfo = string.Format(
                @"License number: {0} 
Model name: {1}", 
                r_LicenseNumber, 
                r_ModelName);

            resultString.AppendLine(vehicleInfo);
            resultString.AppendLine(m_Wheels[0].ToString());
            resultString.AppendLine(m_Engine.ToString());
            return resultString.ToString();
        }

        internal void InflateAllWheelsToMax()
        {
            foreach(Wheel currentWheel in m_Wheels)
            {
                float airAmountToAdd = currentWheel.MaxAirPressure - currentWheel.CurrentAirPressure;
                if(airAmountToAdd > 0)
                {
                    currentWheel.InflateWheel(airAmountToAdd);
                }
            }
        }
    }
}
