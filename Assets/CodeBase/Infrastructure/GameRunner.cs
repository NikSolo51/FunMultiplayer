using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;
     
        [Inject] 
        private DiContainer _diContainer;
        private void Awake()
        {
            GameBootstrapper boostrapper = FindObjectOfType<GameBootstrapper>();
            
            if (boostrapper == null)
            {
                GameBootstrapper _bootstrapper = Instantiate(BootstrapperPrefab);
                _diContainer.Inject(_bootstrapper);   
            }
        }
    }
}