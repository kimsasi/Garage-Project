using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        public class CostumerVehicle
        {
            private readonly string r_OwnerName;
            private readonly int r_OwnerPhoneNumber;
            private eRepairStatus m_VehicleStatus;

            private readonly Vehicle r_Vehicle;

            // C'tor
            public CostumerVehicle(string i_OwnerName, int i_OwnerPhoneNumber, Vehicle i_NewVehicle)
            {
                this.r_OwnerName = i_OwnerName;
                this.r_OwnerPhoneNumber = i_OwnerPhoneNumber;
                VehicleStatus = eRepairStatus.InRepair;
                this.r_Vehicle = i_NewVehicle;
            }

            // Properties
            public string OwnerName
            {
                get
                {
                    return this.r_OwnerName;
                }
            }

            public int OwnerPhoneNumber
            {
                get
                {
                    return this.r_OwnerPhoneNumber;
                }
            }

            public eRepairStatus VehicleStatus
            {
                get
                {
                    return this.m_VehicleStatus;
                }

                set
                {
                    if(!Enum.IsDefined(typeof(eRepairStatus), value))
                    {
                        throw new ValueOutOfRangeException((float)eRepairStatus.InRepair, (float)eRepairStatus.Paid);
                    }

                    this.m_VehicleStatus = value;
                }
            }

            public Vehicle Vehicle
            {
                get
                {
                    return this.r_Vehicle;
                }
            }

            public string LicenseNumber
            {
                get
                {
                    return this.r_Vehicle.LicenseNumber;
                }
            }

            public override string ToString()
            {
                StringBuilder stringResult = new StringBuilder();
                
                string ownerInfo = string.Format(
                    @"Owner name: {0}
Owner phone number: {1}
Repairing status: {2}",
                    OwnerName,
                    OwnerPhoneNumber, 
                    VehicleStatus);

                stringResult.AppendLine(ownerInfo);
                stringResult.AppendLine(Vehicle.ToString());
                return stringResult.ToString();
            }
        }

        public enum eRepairStatus
        {
            InRepair = 1,
            Repaired,
            Paid
        }

        private readonly Dictionary<string, CostumerVehicle> r_AllVehiclesInGarage;

        // C'tor
        public Garage()
        {
            this.r_AllVehiclesInGarage = new Dictionary<string, CostumerVehicle>();
        }

        // Properties:
        public Dictionary<string, CostumerVehicle> AllVehicles
        {
            get
            {
                return this.r_AllVehiclesInGarage;
            }
        }

        public void UpdateVehicleRepairStatus(string i_LicenseNumber, eRepairStatus i_NewStatus)
        {
            AllVehicles[i_LicenseNumber].VehicleStatus = i_NewStatus;
        }

        public void ChargeVehicleElectricEngine(string i_LicenseNumber, float i_ChargingAddition)
        {
            Engine vehicleEngine = AllVehicles[i_LicenseNumber].Vehicle.Engine;

            bool isElectricEngine = vehicleEngine is ElectricEngine;

            if(isElectricEngine)
            {
                vehicleEngine.FillEnergy(i_ChargingAddition);
            }
            else
            {
                throw new ArgumentException("This vehicle is not powered by an electric engine.\n");
            }
        }

        public void FillVehicleFuelTank(string i_LicenseNumber, FuelEngine.eFuelType i_FuelType, float i_FuelAddition)
        {
            Engine vehicleEngine = AllVehicles[i_LicenseNumber].Vehicle.Engine;

            bool isFuelEngine = vehicleEngine is FuelEngine;

            if(isFuelEngine)
            {
                bool isSuitableFuelType = i_FuelType == (vehicleEngine as FuelEngine).FuelType;

                if(isSuitableFuelType)
                {
                    vehicleEngine.FillEnergy(i_FuelAddition);
                }
                else
                {
                    throw new ArgumentException("The fuel type is not suitable to the vehicle's engine.\n");
                }
            }
            else
            {
                throw new ArgumentException("This vehicle is not powered by a fuel engine.\n");
            }
        }

        public void FillMaxAirPressure(string i_LicenseNumber)
        {
            AllVehicles[i_LicenseNumber].Vehicle.InflateAllWheelsToMax();
        }
    }
}