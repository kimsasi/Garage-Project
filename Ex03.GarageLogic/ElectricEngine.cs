namespace Ex03.GarageLogic
{
    public class ElectricEngine : Engine
    {
        public enum eFullChargeMinutesCapacity
        {
            Motorcycle = 72,
            Car = 126
        }

        // C'tor
        public ElectricEngine(eFullChargeMinutesCapacity i_MaxBatteryCapacity, float i_CurrentBatteryTimePercentage)
            : base((float)i_MaxBatteryCapacity / 60, i_CurrentBatteryTimePercentage)
        {
        }

        public override string ToString()
        {
            return string.Format(
                @"Engine Type: Electric 
Max battery capacity (in hours): {0}
Current battery percentage: {1}%", 
                MaxEnergyCapacity, 
                CurrentEnergy);
        }
    }
}