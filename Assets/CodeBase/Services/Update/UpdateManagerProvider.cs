using Zenject;

namespace CodeBase.Services.Update
{
    public class UpdateManagerProvider : MonoInstaller
    {
        private IUpdateService _updateService;
        
        [Inject]
        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        public override void InstallBindings()
        {
        }

        private void Update()
        {
            if (_updateService != null)
                _updateService.Update();
        }

        private void FixedUpdate()
        {
            if (_updateService != null)
                _updateService.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (_updateService != null)
                _updateService.LateUpdate();
        }
    }
}