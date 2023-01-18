using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class Shop : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private IGameFactory _gameFactory;

        [Inject]
        public void Constructor(IStaticDataService staticDataService,IGameFactory gameFactory)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            Initialize();
        }

        private void Initialize()
        {
            WeaponStaticData[] allWeaponsData = _staticDataService.AllWeapons();
            for (int i = 0; i < allWeaponsData.Length; i++)
            {
                _gameFactory.CreateWeaponUI(allWeaponsData[i],transform);
            }
        }
    }
}