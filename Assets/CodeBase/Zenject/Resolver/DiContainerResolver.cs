using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DiContainerResolver : MonoInstaller
{
    public DiContainer _container;
    public override void InstallBindings()
    {
        _container = Container.Resolve<DiContainer>();
    }
}
