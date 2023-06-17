using System.Globalization;
using Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class BusinessUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Image _progressBarFillingImage;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private TMP_Text _levelUpPriceText;
        [SerializeField] private UpgradeUI _firstUpgradeUI;
        [SerializeField] private UpgradeUI _secondUpgradeUI;

        public float FillingProgress
        {
            set => _progressBarFillingImage.fillAmount = value;
        }

        public string Level
        {
            set => _levelText.text = "LVL: " + value;
        }

        public string LevelUpPrice
        {
            set => _levelUpPriceText.text = "Price: " + value + "$";
        }
    
        public string Income
        {
            set => _incomeText.text = "Income: " + value + "$";
        }

        public Button.ButtonClickedEvent LevelUpButtonClickedEvent => _levelUpButton.onClick;
        public Button.ButtonClickedEvent FirstUpgradeButtonClickedEvent => _firstUpgradeUI.UpgradeButton.onClick;
        public Button.ButtonClickedEvent SecondUpgradeButtonClickedEvent => _secondUpgradeUI.UpgradeButton.onClick;

        public void UpdateFirstUpgradePrice(float price)
        {
            _firstUpgradeUI.PriceText = price == 0 ? string.Empty : price.ToString(CultureInfo.InvariantCulture);
        }

        public void UpdateSecondUpgradePrice(float price)
        {
            _secondUpgradeUI.PriceText = price == 0 ? string.Empty : price.ToString(CultureInfo.InvariantCulture);
        }

        public void SetInteractableUpgradeButtons(bool interactable)
        {
            _firstUpgradeUI.UpgradeButton.interactable = interactable;
            _secondUpgradeUI.UpgradeButton.interactable = interactable;
        }
    
        public void Init(Business business, BusinessTitle businessTitle, float currentIncome, int levelUpPrice)
        {
            gameObject.SetActive(true);

            _titleText.text = businessTitle.BusinessName;
            _firstUpgradeUI.Title = businessTitle.FirstUpgradeTitle;
            _secondUpgradeUI.Title = businessTitle.SecondUpgradeTitle;
        
            Level = business.Level.ToString();
            LevelUpPrice = levelUpPrice.ToString();
            Income = currentIncome.ToString(CultureInfo.InvariantCulture);

            var firstUpgrade = business.FirstUpgrade;
            var secondUpgrade = business.SecondUpgrade;
        
            _firstUpgradeUI.IncomeMultiplier = (firstUpgrade.IncomeMultiplierInPercentage * 100).
                ToString(CultureInfo.InvariantCulture);
            _secondUpgradeUI.IncomeMultiplier = (secondUpgrade.IncomeMultiplierInPercentage * 100).
                ToString(CultureInfo.InvariantCulture);

            SetInteractableUpgradeButtons(business.IsPurchased);
            UpdateFirstUpgradePrice(firstUpgrade.IsPurchased ? 0 : firstUpgrade.Price);
            UpdateSecondUpgradePrice(secondUpgrade.IsPurchased ? 0 : secondUpgrade.Price);
        }
    }
}