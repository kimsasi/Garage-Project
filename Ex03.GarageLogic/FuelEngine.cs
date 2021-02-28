namespace Ex03.GarageLogic
{
    public class FuelEngine : Engine
    {
        private readonly eFuelType r_FuelType;

        public enum eFuelType
        {
            Octan95 = 1,
            Octan96,
            Octan98,
            Soler
        }

        public enum eFuelTankCapacity
        {
            Car = 60,
            Motorcycle = 7,
            Truck = 120
        }

        // C'tor
        public FuelEngine(eFuelTankCapacity i_MaxFuelCapacity, float i_CurrentFuelPercentage, eFuelType i_FuelType)
            : base((float)i_MaxFuelCapacity, i_CurrentFuelPercentage)
        {
            this.r_FuelType = i_FuelType;
        }

        // Properties:
        public eFuelType FuelType
        {
            get
            {
                return this.r_FuelType;
            }
        }

        public override string ToString()
        {
            return string.Format(
                @"Engine Type: Fuel 
Max fuel tank capacity: {0}
Current fuel amount in percentage: {1}%
Fuel Type: {2}", 
                MaxEnergyCapacity, 
                CurrentEnergy, 
                FuelType);
        }
    }
}
