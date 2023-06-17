using System.Globalization;
using Game.Data;
using TMPro;

namespace Game.Logic
{
    public class MoneyController
    {
        private readonly PlayerData _playerData;
        private readonly TMP_Text _moneyText;
        private readonly float _startMoney = 150f; 
        public float Money
        {
            get => _playerData.Money;
            set
            {
                _playerData.Money = value;
                _moneyText.text = "Balance: " + _playerData.Money.ToString(CultureInfo.InvariantCulture) + '$';
            }
        }
    
        public MoneyController(PlayerData playerData, TMP_Text moneyText)
        {
            _playerData = playerData;
            _moneyText = moneyText;

            Money = Money == 0 ? _startMoney : _playerData.Money; 
        }
    }
}