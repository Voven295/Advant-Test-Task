using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "ConfigSO", menuName = "ScriptableObjects/ConfigSO", order = 0)]
    public class ConfigSO : ScriptableObject
    {
        public Business[] Businesses;

        public float GetCurrentIncome(Business business)
        {
            var firstUpgrade = business.FirstUpgrade;
            var secondUpgrade = business.SecondUpgrade;
        
            var firstUpgradeIncomeMultiplier = firstUpgrade.IsPurchased ? 
                firstUpgrade.IncomeMultiplierInPercentage : 0;
            var secondUpgradeIncomeMultiplier = secondUpgrade.IsPurchased ? 
                secondUpgrade.IncomeMultiplierInPercentage : 0;

            return business.Level * business.StartIncome * (1 + firstUpgradeIncomeMultiplier 
                                                              + secondUpgradeIncomeMultiplier);
        }

        public int GetLevelUpPrice(Business business)
        {
            return (business.Level + 1) * business.StartPrice;
        } 
    }
}