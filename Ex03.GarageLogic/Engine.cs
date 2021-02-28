namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        protected readonly float r_MaxEnergyCapacity;
        protected float m_CurrentEnergyInPercentage;

        public enum eEngineType
        {
            Fuel = 1,
            Electric
        }

        // C'tor
        protected Engine(float i_MaxEnergyCapacity, float i_CurrentEnergy)
        {
            this.r_MaxEnergyCapacity = i_MaxEnergyCapacity;
            CurrentEnergy = i_CurrentEnergy;
        }

        // Properties
        public float MaxEnergyCapacity
        {
            get
            {
                return this.r_MaxEnergyCapacity;
            }
        }

        public float CurrentEnergy
        {
            get
            {
                return this.m_CurrentEnergyInPercentage;
            }

            set
            {
                if(value < 0 || value > 100)
                {
                    throw new ValueOutOfRangeException(0, 100);
                }

                this.m_CurrentEnergyInPercentage = value;
            }
        }

        public abstract override string ToString();

        public void FillEnergy(float i_AmountOfEnergyAddition)
        {
            float currentEnergyValue = CurrentEnergy * MaxEnergyCapacity / 100;

            if (currentEnergyValue + i_AmountOfEnergyAddition > MaxEnergyCapacity)
            {
                throw new ValueOutOfRangeException(0, MaxEnergyCapacity);
            }

            CurrentEnergy = (currentEnergyValue + i_AmountOfEnergyAddition) / MaxEnergyCapacity * 100;
        }
    }
}