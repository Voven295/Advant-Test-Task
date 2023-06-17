namespace Game.Data
{
    [System.Serializable]
    public struct Business
    {
        public int Level;
        public bool IsPurchased => Level > 0;
        public float IncomeDelay;
        public int StartPrice;
        public int StartIncome;
        public float IncomeProgress;
        public BusinessUpgrade FirstUpgrade;
        public BusinessUpgrade SecondUpgrade;
        public float PassedTime;
    }
}