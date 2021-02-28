namespace Ex03.GarageLogic
{
    public static class VehicleCreator
    {
        public enum eSupportedVehicles
        {
            ElectricMotorcycle = 1,
            FuelMotorcycle,
            ElectricCar,
            FuelCar,
            Truck
        }

        public static Vehicle CreateVehicle(string i_LicenseNumber, string i_ModelName, float i_EnergyPercentageLeft, eSupportedVehicles i_VehicleType)
        {
            Vehicle newVehicle = null;

            switch(i_VehicleType)
            {
                case eSupportedVehicles.ElectricCar:
                    newVehicle = new Car(i_LicenseNumber, i_ModelName, i_EnergyPercentageLeft, Engine.eEngineType.Electric);
                    break;
                case eSupportedVehicles.FuelCar:
                    newVehicle = new Car(i_LicenseNumber, i_ModelName, i_EnergyPercentageLeft, Engine.eEngineType.Fuel);
                    break;

                case eSupportedVehicles.ElectricMotorcycle:
                    newVehicle = new Motorcycle(i_LicenseNumber, i_ModelName, i_EnergyPercentageLeft, Engine.eEngineType.Electric);
                    break;

                case eSupportedVehicles.FuelMotorcycle:
                    newVehicle = new Motorcycle(i_LicenseNumber, i_ModelName, i_EnergyPercentageLeft, Engine.eEngineType.Fuel);
                    break;

                case eSupportedVehicles.Truck:
                    newVehicle = new Truck(i_LicenseNumber, i_ModelName, i_EnergyPercentageLeft, Engine.eEngineType.Fuel);
                    break;
            }

            return newVehicle;
        }
    }
}