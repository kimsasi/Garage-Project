using System;
using System.Collections.Generic;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    internal class UserInterface
    {
        private enum eMenuChoice
        {
            AddNewVehicle = 1,
            ShowVehicles,
            UpdateRepairingStatus,
            PumpAirPressure,
            FillFuelTank,
            ChargeElectricEngine,
            ShowSpecificVehicleDetails,
            Exit
        }

        private enum eFilterVehiclesOptions
        {
            Unfiltered = 1,
            Filtered
        }

        private readonly Garage r_Garage;

        // C'tor
        public UserInterface()
        {
            this.r_Garage = new Garage();
        }

        // Properties
        internal Garage Garage
        {
            get
            {
                return this.r_Garage;
            }
        }

        private static void printExceptionErrors(Exception i_Exception)
        {
            Console.Clear();
            Console.WriteLine(i_Exception.Message);

            Exception innerException = i_Exception.InnerException;

            while(innerException != null)
            {
                Console.WriteLine(innerException.Message);
                innerException = innerException.InnerException;
            }
        }

        private static eMenuChoice getChosenOption()
        {
            printGarageMenu();

            string chosenOptionInput = Console.ReadLine();

            while(!checkEnumValidation(typeof(eMenuChoice), chosenOptionInput))
            {
                Console.WriteLine("Invalid option, please try again");
                chosenOptionInput = Console.ReadLine();
            }

            eMenuChoice chosenOption = (eMenuChoice)Enum.Parse(typeof(eMenuChoice), chosenOptionInput); 
            return chosenOption;
        }

        private static bool checkEnumValidation(Type i_EnumType, string i_Input)
        {
            bool isValid = false;

            if (int.TryParse(i_Input, out int inputResult))
            {
                isValid = Enum.IsDefined(i_EnumType, inputResult);
            }

            return isValid;
        }

        private static void printGarageMenu()
        {
            string options = string.Format(
                @"Please choose an option (a number between {0} - {1}):
{0}. Add a new vehicle to the garage
{2}. Show all vehicles that are currently at our garage (License Plates)
{3}. Update vehicle's repairing status
{4}. Pump vehicle wheel's air pressure to maximum 
{5}. Fill up a vehicle's fuel tank
{6}. Charge an electric vehicle engine
{7}. Show all details for a specific vehicle
{1}. Exit our garage", 
            (int)eMenuChoice.AddNewVehicle,
            (int)eMenuChoice.Exit,
            (int)eMenuChoice.ShowVehicles,
            (int)eMenuChoice.UpdateRepairingStatus,
            (int)eMenuChoice.PumpAirPressure,
            (int)eMenuChoice.FillFuelTank,
            (int)eMenuChoice.ChargeElectricEngine,
            (int)eMenuChoice.ShowSpecificVehicleDetails);

            Console.WriteLine(options);
            Console.WriteLine();
        }

        
        private static int getPhoneNumber()
        {
            Console.WriteLine("\nPlease enter your phone number (maximum 9 digits):");
            string ownerPhoneNumber = Console.ReadLine();
            bool isValidPhoneNumber = int.TryParse(ownerPhoneNumber, out int phoneNumber);

            while (!isValidPhoneNumber)
            {
                Console.WriteLine("Invalid phone number, please try again");
                ownerPhoneNumber = Console.ReadLine();
                isValidPhoneNumber = int.TryParse(ownerPhoneNumber, out phoneNumber);
            }

            return phoneNumber;
        }


        private static float getEnergyLeftPercentage()
        {
            Console.WriteLine("\nPlease enter your vehicle's remaining energy percentage:");
            string energyLeftInput = Console.ReadLine();

            while(!checkValidPercentage(energyLeftInput))
            {
                Console.WriteLine("Invalid percentage, please enter a number between 0-100 :");
                energyLeftInput = Console.ReadLine();
            }

            return float.Parse(energyLeftInput);
        }

        private static bool checkValidPercentage(string i_EnergyLeftPercentage)
        {
            bool isNumeric = float.TryParse(i_EnergyLeftPercentage, out float energyLeft);
            bool isValidPercentage = false;

            if (isNumeric)
            {
                isValidPercentage = energyLeft >= 0 && energyLeft <= 100;
            }

            return isValidPercentage;
        }

        private static void printListOfEnum(string i_PrintMessage, Type i_TypeOfEnum)
        {
            string[] types = Enum.GetNames(i_TypeOfEnum);
            Console.WriteLine(i_PrintMessage);

            for(int i = 0; i < types.Length; ++i)
            {
                Console.WriteLine("{0}. {1}", i + 1, types[i]);
            }

            Console.WriteLine();
        }

        private static string getLicenseNumber()
        {
            Console.WriteLine("\nPlease enter the vehicle's license number (only digits and letters): ");
            string licenseNumberInput = Console.ReadLine();

            while(string.IsNullOrEmpty(licenseNumberInput) || !checkOnlyLettersAndDigits(licenseNumberInput))
            {
                Console.WriteLine("Invalid input, please try again. ");
                licenseNumberInput = Console.ReadLine();
            }

            return licenseNumberInput;
        }

        private static Garage.eRepairStatus getTypeOfRepairStatus()
        {
            string repairStatusRequest = "Please choose your desired vehicle status: ";
            printListOfEnum(repairStatusRequest, typeof(Garage.eRepairStatus));

            string typeOfRepairStatusInput = Console.ReadLine();

            while(!checkEnumValidation(typeof(Garage.eRepairStatus), typeOfRepairStatusInput))
            {
                Console.WriteLine("Invalid repairing status, please try again");
                typeOfRepairStatusInput = Console.ReadLine();
            }

            Garage.eRepairStatus typeORepairStatus = (Garage.eRepairStatus)Enum.Parse(
                typeof(Garage.eRepairStatus),
                typeOfRepairStatusInput);

            return typeORepairStatus;
        }

        private static VehicleCreator.eSupportedVehicles getVehicleType()
        {
            printListOfEnum("\nPlease choose a vehicle", typeof(VehicleCreator.eSupportedVehicles));
            string vehicleTypeInput = Console.ReadLine();

            while (!checkEnumValidation(typeof(VehicleCreator.eSupportedVehicles), vehicleTypeInput))
            {
                Console.WriteLine("Invalid vehicle type input, please try again");
                vehicleTypeInput = Console.ReadLine();
            }

            VehicleCreator.eSupportedVehicles vehicleType = (VehicleCreator.eSupportedVehicles)Enum.Parse(
                typeof(VehicleCreator.eSupportedVehicles),
                vehicleTypeInput);

            return vehicleType;
        }

        private static bool checkOnlyLettersAndDigits(string i_Input)
        {
            bool isValid = true;

            foreach(char currentCharacter in i_Input)
            {
                if (char.IsLetterOrDigit(currentCharacter))
                {
                    continue;
                }

                isValid = false;
                break;
            }

            return isValid;
        }

        private static List<string> getAdditionalSpecificDetails(Vehicle i_NewVehicle)
        {
            List<string> requestedParams = i_NewVehicle.GetUserQuestions();
            List<string> requiredParams = new List<string>();

            for(int i = 0; i < requestedParams.Count; i++)
            {
                bool isValidInput = false;
                Console.WriteLine();
                Console.WriteLine(requestedParams[i]);
                do
                {
                    string currentInput = Console.ReadLine();
                    if (i_NewVehicle.CheckValidInput(currentInput, i))
                    {
                        isValidInput = true;
                        requiredParams.Add(currentInput);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please try again");
                    }
                }
                while (!isValidInput);
            }

            return requiredParams;
        }

        private static FuelEngine.eFuelType getFuelType()
        {
            string fuelTypeRequestMsg = "Please choose fuel type: ";
            printListOfEnum(fuelTypeRequestMsg, typeof(FuelEngine.eFuelType));

            string fuelTypeInput = Console.ReadLine();

            while(!checkEnumValidation(typeof(FuelEngine.eFuelType), fuelTypeInput))
            {
                Console.WriteLine("Invalid fuel type option, please try again");
                fuelTypeInput = Console.ReadLine();
            }

            FuelEngine.eFuelType fuelType =
                (FuelEngine.eFuelType)Enum.Parse(typeof(FuelEngine.eFuelType), fuelTypeInput);

            return fuelType;
        }
        

        private static float getAmountOfEnergyForAddition(Engine.eEngineType i_EngineType)
        {
            string energyAdditionMsgFormat = string.Format("Please enter the amount of {0} that you would like to add: ", i_EngineType == Engine.eEngineType.Fuel ? "fuel" : "minutes");
            Console.WriteLine(energyAdditionMsgFormat);

            string energyAdditionInput = Console.ReadLine();

            while(!checkPositiveFloatValueValidation(energyAdditionInput))
            {
                Console.WriteLine("Invalid input, please enter a positive number");
                energyAdditionInput = Console.ReadLine();
            }

            float energyAddition = float.Parse(energyAdditionInput);

            return energyAddition;
        }

        private static bool checkPositiveFloatValueValidation(string i_Input)
        {
            bool isNumeric = float.TryParse(i_Input, out float numericInput);
            bool isValid = false;

            if (isNumeric)
            {
                isValid = numericInput > 0;
            }

            return isValid;
        }

        private static void exitGarage()
        {
            Console.Clear();
            Console.WriteLine(
                @"
                          ____               __
   ____ _____  ____  ____/ / /_  __  _____  / /
  / __ `/ __ \/ __ \/ __  / __ \/ / / / _ \/ / 
 / /_/ / /_/ / /_/ / /_/ / /_/ / /_/ /  __/_/  
 \__, /\____/\____/\__,_/_.___/\__, /\___(_)   
/____/                        /____/    
");
            System.Threading.Thread.Sleep(2000);
            Environment.Exit(1);
        }

        internal void ManageGarage()
        {
            printWelcomeMessage();

            while(true)
            {
                try
                {
                    eMenuChoice chosenAction = getChosenOption();
                    completeNewTask(chosenAction);
                }
                catch (Exception error)
                {
                    printExceptionErrors(error);
                    System.Threading.Thread.Sleep(3000);
                    Console.Clear();
                }
            }
        }

        private static void printWelcomeMessage()
        {
            Console.WriteLine(@"  ___   __  ____  __   ___ ____ 
 / __) / _\(  _ \/ _\ / __|  __)
( (_ \/    \)   /    ( (_ \) _) 
 \___/\_/\_(__\_)_/\_/\___(____)
");

            string welcomeMessage = @"Hello! welcome to our garage!
How can we help you today?";
            Console.WriteLine(welcomeMessage);
        }

        private void completeNewTask(eMenuChoice i_ChosenTask)
        {
            bool startOverMenu = false;
            string licenseNumber = string.Empty;

            if (i_ChosenTask != eMenuChoice.AddNewVehicle && i_ChosenTask != eMenuChoice.ShowVehicles && i_ChosenTask != eMenuChoice.Exit)
            {
                licenseNumber = getLicenseNumber();
                startOverMenu = !Garage.AllVehicles.ContainsKey(licenseNumber);
            }

            if (!startOverMenu)
            {
                switch (i_ChosenTask)
                {
                    case eMenuChoice.AddNewVehicle:
                        addNewVehicle();
                        break;
                    case eMenuChoice.ShowVehicles:
                        showVehiclesLicenseNumber();
                        break;
                    case eMenuChoice.UpdateRepairingStatus:
                        updateVehicleRepairingStatus(licenseNumber);
                        break;
                    case eMenuChoice.PumpAirPressure:
                        pumpAirPressureToMax(licenseNumber);
                        break;
                    case eMenuChoice.FillFuelTank:
                        if (Garage.AllVehicles[licenseNumber].Vehicle.Engine is FuelEngine chosenFuelEngine)
                        {
                            fillFuelTank(licenseNumber, chosenFuelEngine);
                        }
                        else
                        {
                            Console.WriteLine("This vehicle is not powered by a fuel engine.");
                        }

                        break;
                    case eMenuChoice.ChargeElectricEngine:
                        if (Garage.AllVehicles[licenseNumber].Vehicle.Engine is ElectricEngine chosenElectricEngine)
                        {
                            chargeElectricEngine(licenseNumber, chosenElectricEngine);
                        }
                        else
                        {
                            Console.WriteLine("This vehicle is not powered by an electric engine.");
                        }

                        break;
                    case eMenuChoice.ShowSpecificVehicleDetails:
                        showVehicleData(licenseNumber);
                        break;
                    case eMenuChoice.Exit:
                        exitGarage();
                        break;
                }
            }
            else
            {
                Console.WriteLine("The license number you entered was not found");
            }
        }

        private void addNewVehicle()
        {
            string licenseNumber = getLicenseNumber();

            if (Garage.AllVehicles.ContainsKey(licenseNumber))
            {
                Garage.UpdateVehicleRepairStatus(licenseNumber, Garage.eRepairStatus.InRepair);
                Console.WriteLine("Welcome back, your vehicle is in repair!");
            }
            else
            {
                Console.WriteLine("\nPlease enter your name:");
                string ownerName = Console.ReadLine();
                bool isValidName = !string.IsNullOrEmpty(ownerName);

                while(!isValidName)
                {
                    Console.WriteLine("The name must contain at least one letter, please try again");
                    ownerName = Console.ReadLine();
                    isValidName = !string.IsNullOrEmpty(ownerName);
                }

                int ownerPhoneNumber = getPhoneNumber();
                VehicleCreator.eSupportedVehicles vehicleType = getVehicleType();
                Vehicle newVehicle = createNewVehicle(vehicleType, licenseNumber);
                Garage.CostumerVehicle newCostumerVehicle = new Garage.CostumerVehicle(ownerName, ownerPhoneNumber, newVehicle);
                r_Garage.AllVehicles.Add(licenseNumber, newCostumerVehicle);
                Console.WriteLine("Thank you for joining our family!");
                System.Threading.Thread.Sleep(1500);
                Console.Clear();
            }
        }

        private Vehicle createNewVehicle(VehicleCreator.eSupportedVehicles i_VehicleType, string i_LicenseNumber)
        {
            Console.WriteLine("\nPlease enter your vehicle's model name:");
            string modelName = Console.ReadLine();

            while(string.IsNullOrEmpty(modelName))
            {
                Console.WriteLine("The model name must contain at least one letter, please try again");
                modelName = Console.ReadLine();
            }

            float energyLeft = getEnergyLeftPercentage();

            Vehicle newVehicle = VehicleCreator.CreateVehicle(i_LicenseNumber, modelName, energyLeft, i_VehicleType);
            List<string> answers = getAdditionalSpecificDetails(newVehicle);
            newVehicle.UpdateAdditionalInfo(answers);

            return newVehicle;
        }

        private void showVehiclesLicenseNumber()
        {
            string showVehiclesFormat = "Would you like to watch the vehicles license numbers filtered by their repairing status?";
            printListOfEnum(showVehiclesFormat, typeof(eFilterVehiclesOptions));
            
            string filterChoiceInput = Console.ReadLine();

            while(!checkEnumValidation(typeof(eFilterVehiclesOptions), filterChoiceInput))
            {
                Console.WriteLine("Invalid option, please try again");
                Console.WriteLine(showVehiclesFormat);
            }

            eFilterVehiclesOptions filterChoice = (eFilterVehiclesOptions)Enum.Parse(typeof(eFilterVehiclesOptions), filterChoiceInput);

            if (filterChoice == eFilterVehiclesOptions.Unfiltered)
            {
                printAllVehiclesAtGarage();
            }
            else
            {
                printVehiclesFilteredByRepairStatus();
            }
        }

        private void printAllVehiclesAtGarage()
        {
            Console.WriteLine("Our Vehicles license numbers:");

            foreach(KeyValuePair<string, Garage.CostumerVehicle> currentVehicle in r_Garage.AllVehicles)
            {
                Console.WriteLine(currentVehicle.Key);
            }

            Console.WriteLine(Environment.NewLine);
        }

        private void printVehiclesFilteredByRepairStatus()
        {
            Garage.eRepairStatus repairStatusFilter = getTypeOfRepairStatus();

            foreach(KeyValuePair<string, Garage.CostumerVehicle> currentVehicle in r_Garage.AllVehicles)
            {
                if (currentVehicle.Value.VehicleStatus == repairStatusFilter)
                {
                    Console.WriteLine(currentVehicle.Key);
                }
            }
        }

        private void updateVehicleRepairingStatus(string i_LicenseNumber)
        {
            Garage.eRepairStatus newRepairStatus = getTypeOfRepairStatus();
            Garage.UpdateVehicleRepairStatus(i_LicenseNumber, newRepairStatus);
            string statusMsg = string.Format("Your vehicle status is -{0}-", newRepairStatus);
            Console.WriteLine(statusMsg);
        }

        private void pumpAirPressureToMax(string i_LicenseNumber)
        {
            Vehicle chosenVehicle = Garage.AllVehicles[i_LicenseNumber].Vehicle;

            if(!chosenVehicle.Wheels[0].CurrentAirPressure.Equals(chosenVehicle.Wheels[0].MaxAirPressure))
            {
                float previousAirPressure = chosenVehicle.Wheels[0].CurrentAirPressure;
                Garage.FillMaxAirPressure(i_LicenseNumber);
                printAirPressurePumpingMessage(previousAirPressure, chosenVehicle.Wheels[0].CurrentAirPressure);
            }
            else
            {
                Console.WriteLine("Vehicle's wheels already have maximum air pressure\n");
            }
        }

        private void printAirPressurePumpingMessage(float i_PreviousAirPressure, float i_NewAirPressure)
        {
            string airPressureMsg = string.Format(
                @"Current air pressure: {0}
PUMPING........... ",
                i_PreviousAirPressure);

            Console.WriteLine(airPressureMsg);
            System.Threading.Thread.Sleep(2000);
            airPressureMsg = string.Format(
                @"Current air pressure after pump: {0}!
",
                i_NewAirPressure);
            Console.WriteLine(airPressureMsg);
            System.Threading.Thread.Sleep(700);
        }

        private void fillFuelTank(string i_LicenseNumber, FuelEngine i_FuelEngine)
        {
            FuelEngine.eFuelType fuelType = getFuelType();

            bool isSuitableFuelType = i_FuelEngine.FuelType == fuelType;

            if(isSuitableFuelType)
            {
                float fuelAddition = getAmountOfEnergyForAddition(Engine.eEngineType.Fuel);
                float currentFuelInPercentage = i_FuelEngine.CurrentEnergy;
                float currentFuelLiters = currentFuelInPercentage * i_FuelEngine.MaxEnergyCapacity / 100;

                bool isValidEnergyAddition = currentFuelLiters + fuelAddition < i_FuelEngine.MaxEnergyCapacity;

                if(isValidEnergyAddition)
                {
                    Garage.FillVehicleFuelTank(i_LicenseNumber, fuelType, fuelAddition);

                    printEnergyLoadingMessage(Engine.eEngineType.Fuel, currentFuelInPercentage, i_FuelEngine.CurrentEnergy);
                }
                else
                {
                    Console.WriteLine("The fuel amount you entered is out of range.");
                }
            }
            else
            {
                Console.WriteLine("The fuel type is not suitable to the vehicle's engine.");
            }
        }

        private void printEnergyLoadingMessage(Engine.eEngineType i_EngineType, float i_PreviousEnergyAmount, float i_NewEnergyAmount)
        {
            string fuelTankMsg = string.Format(
                @"Current {0} amount in percentage: {1}%
LOADING........... ",
                i_EngineType == Engine.eEngineType.Fuel ? "fuel" : "electricity",
                i_PreviousEnergyAmount);
            Console.WriteLine(fuelTankMsg);

            System.Threading.Thread.Sleep(2000);

            fuelTankMsg = string.Format(
                @"Current {0} amount in percentage after refueling: {1}%
",
                i_EngineType == Engine.eEngineType.Fuel ? "fuel" : "electricity",
                i_NewEnergyAmount);
            Console.WriteLine(fuelTankMsg);
            System.Threading.Thread.Sleep(1000);
        }

        private void chargeElectricEngine(string i_LicenseNumber, ElectricEngine i_ElectricEngine)
        {
            float hoursAddition = getAmountOfEnergyForAddition(Engine.eEngineType.Electric) / 60;
            float currentLeftEnergyInPercentage = i_ElectricEngine.CurrentEnergy;
            float currentLeftEnergyInHours = currentLeftEnergyInPercentage * i_ElectricEngine.MaxEnergyCapacity / 100;
            bool isValidEnergyAddition = currentLeftEnergyInHours + hoursAddition < i_ElectricEngine.MaxEnergyCapacity;

            if (isValidEnergyAddition)
            {
                Garage.ChargeVehicleElectricEngine(i_LicenseNumber, hoursAddition);

                printEnergyLoadingMessage(Engine.eEngineType.Electric, currentLeftEnergyInPercentage, i_ElectricEngine.CurrentEnergy);
            }
            else
            {
                Console.WriteLine("The number of minutes you entered is out of range.");
            }
        }

        private void showVehicleData(string i_LicenseNumber)
        {
            string chosenVehicleInformation = Garage.AllVehicles[i_LicenseNumber].ToString();
            Console.WriteLine(@"
+-+-+-+-+ +-+-+-+-+-+-+-+
|Y|o|u|r| |V|e|h|i|c|l|e|
+-+-+-+-+ +-+-+-+-+-+-+-+
");
            Console.WriteLine(chosenVehicleInformation);
        }
    }
}