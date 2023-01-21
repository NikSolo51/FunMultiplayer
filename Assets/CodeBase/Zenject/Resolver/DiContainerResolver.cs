using Zenject;

namespace CodeBase.Zenject.Resolver
{
    public class DiContainerResolver : MonoInstaller
    {
        public DiContainer _container;
        public override void InstallBindings()
        {
            _container = Container.Resolve<DiContainer>();
        }
    }
}
