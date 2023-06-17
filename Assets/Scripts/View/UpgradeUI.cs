using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _incomeMultiplierText;
        [SerializeField] private TMP_Text _priceText;

        public string IncomeMultiplier
        {
            set => _incomeMultiplierText.text = "Income: " + '+' + value + '%';
        }

        public string PriceText
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _priceText.text = "Purchased";
                }
                else
                {
                    _priceText.text = "Price: " + value;
                }
            }
        }

        public string Title
        {
            set => _titleText.text = value;
        }

        public Button UpgradeButton => _upgradeButton;
    }
}