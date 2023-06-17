using System;
using Game.Data;
using Game.View;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace Game.Logic
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private ConfigSO _configSo;
        [SerializeField] private BusinessTitlesSO _businessTitlesSO;
        [SerializeField] private Transform _businessUiRoot;
        [SerializeField] private TMP_Text _moneyText;

        private EcsWorld _world;
        private EcsSystems _systems;

        private Business[] _businesses;
        private PlayerData _playerData;
        private MoneyController _moneyController;
        private JsonSerializer _jsonSerializer;

        private void Awake()
        {
            _jsonSerializer = new JsonSerializer();
            var saveData = _jsonSerializer.LoadDataFromJson<SaveData>();

            if (saveData == null)
            {
                _playerData = new PlayerData();
                _businesses = Array.Empty<Business>();
            }
            else
            {
                _playerData = saveData.PlayerData;
                _businesses = saveData.Businesses;
            }

            var businessCount = _configSo.Businesses.Length;
            
            if (_businesses.Length == 0)
            {
                _businesses = new Business[businessCount];
                Array.Copy(_configSo.Businesses, _businesses, businessCount);
            }

            for (int i = 0; i < businessCount; i++)
            {
                _businesses[i].IncomeDelay = _configSo.Businesses[i].IncomeDelay;
            }
            
            _moneyController = new MoneyController(_playerData, _moneyText);

            var businessUi = _businessUiRoot.GetComponentsInChildren<BusinessUI>(true);

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _systems
                .Add(new BusinessSystem())
                .Inject(_configSo)
                .Inject(_businesses)
                .Inject(businessUi)
                .Inject(_businessTitlesSO.BusinessTitles)
                .Inject(_moneyController)
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems == null)
            {
                return;
            }

            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
        }

        private void OnApplicationQuit()
        {
            _jsonSerializer.SaveDataToJson(_playerData, _businesses);
        }
    }
}