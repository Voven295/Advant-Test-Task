using System.Globalization;
using Game.Data;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Logic
{
    public class BusinessSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ConfigSO _configSo;
        private readonly Business[] _businesses;
        private readonly BusinessUI[] _businessUI;
        private readonly BusinessTitle[] _businessTitles;
        private readonly MoneyController _moneyController;
    
        private int _businessCount;

        public void Init()
        {
            _businessCount = _businesses.Length;

            for (int i = 0; i < _businessCount; i++)
            {
                InitBusinessView(i);

                if (!_businesses[i].IsPurchased)
                {
                    break;
                }
            }
        }

        private void LevelUp(int businessIndex)
        {
            var businessCount = _businesses.Length;
            var currentBusiness = _businesses[businessIndex];
            var levelUpPrice = currentBusiness.IsPurchased
                ? _configSo.GetLevelUpPrice(currentBusiness)
                : currentBusiness.StartPrice;
        
            if (_moneyController.Money < levelUpPrice)
            {
                return;
            }
        
            if (businessIndex >= businessCount)
            {
                return;
            }

            _moneyController.Money -= levelUpPrice;
        
            var businessUI = _businessUI[businessIndex];

            var currentLevel = currentBusiness.Level;

            if (currentLevel == 0 && businessIndex + 1 < _businessCount)
            {
                InitBusinessView(businessIndex + 1);
            }
        
            _businesses[businessIndex].Level = ++currentLevel;

            var currentIncome = _configSo.GetCurrentIncome(_businesses[businessIndex]);
            levelUpPrice = _configSo.GetLevelUpPrice(_businesses[businessIndex]);

            businessUI.Income = currentIncome.ToString(CultureInfo.InvariantCulture);
            businessUI.LevelUpPrice = levelUpPrice.ToString();
            businessUI.Level = currentLevel.ToString();
            businessUI.SetInteractableUpgradeButtons(true);
        }

        private void InitBusinessView(int index)
        {
            var currentBusinessUI = _businessUI[index];
            var currentBusiness = _businesses[index];

            var currentIncome = currentBusiness.IsPurchased ? _configSo.GetCurrentIncome(currentBusiness) 
                : currentBusiness.StartIncome;
        
            var levelUpPrice = currentBusiness.IsPurchased ? _configSo.GetLevelUpPrice(currentBusiness) 
                : currentBusiness.StartPrice;
        
            currentBusinessUI.Init(_businesses[index], _businessTitles[index], currentIncome, levelUpPrice);
            currentBusinessUI.LevelUpButtonClickedEvent.AddListener(() => LevelUp(index));

            currentBusinessUI.FirstUpgradeButtonClickedEvent.AddListener(() => 
                BuyUpgrade(index, 0));
            currentBusinessUI.SecondUpgradeButtonClickedEvent.AddListener(() => 
                BuyUpgrade(index, 1));
        }

        private void BuyUpgrade(int businessIndex, int businessUpgradeIndex)
        {
            var businessUI = _businessUI[businessIndex];
            var business = _businesses[businessIndex];
        
            switch (businessUpgradeIndex)
            {
                case 0:
                    var firstUpgradePrice = business.FirstUpgrade.Price;
                    if (_moneyController.Money >= firstUpgradePrice)
                    {
                        _moneyController.Money -= firstUpgradePrice;

                        _businesses[businessIndex].FirstUpgrade.IsPurchased = true;
                        businessUI.UpdateFirstUpgradePrice(0);
                        businessUI.Income = _configSo.GetCurrentIncome(_businesses[businessIndex])
                            .ToString(CultureInfo.InvariantCulture);
                    }
                    break;
                case 1:
                    var secondUpgradePrice = business.SecondUpgrade.Price;
                    if (_moneyController.Money >= secondUpgradePrice)
                    {
                        _moneyController.Money -= secondUpgradePrice;
                    
                        _businesses[businessIndex].SecondUpgrade.IsPurchased = true;
                        businessUI.UpdateSecondUpgradePrice(0);
                        businessUI.Income = _configSo.GetCurrentIncome(_businesses[businessIndex])
                            .ToString(CultureInfo.InvariantCulture);
                    }
                    break;
            }

        }

        public void Run()
        {
            for (int i = 0; i < _businessCount; i++)
            {
                var business = _businesses[i];

                if (!business.IsPurchased)
                {
                    continue;
                }

                float deltaTime = Time.deltaTime;
            
                if (business.PassedTime + deltaTime >= business.IncomeDelay)
                {
                    _businesses[i].PassedTime = 0;
                    _businesses[i].IncomeProgress = 1;
                    _moneyController.Money += _configSo.GetCurrentIncome(_businesses[i]);
                }
                else
                {
                    _businesses[i].PassedTime += deltaTime;
                    var incomeDelay = business.IncomeDelay;
                    _businesses[i].IncomeProgress = 
                        1 - (incomeDelay - _businesses[i].PassedTime % incomeDelay) / incomeDelay;
                }
            
                _businessUI[i].FillingProgress = _businesses[i].IncomeProgress;
            }
        }
    }
}