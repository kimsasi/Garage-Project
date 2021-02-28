using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private readonly float r_MaxValue;
        private readonly float r_MinValue;

        // C'tor
        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue)
            : base(string.Format("Entered value out of given range. Min:{0} Max:{1}\n", i_MinValue, i_MaxValue))
        {
            this.r_MaxValue = i_MaxValue;
            this.r_MinValue = i_MinValue;
        }

        // Properties
        public float MaxValue
        {
            get
            {
                return r_MaxValue;
            }
        }

        public float MinValue
        {
            get
            {
                return r_MinValue;
            }
        }
    }
}