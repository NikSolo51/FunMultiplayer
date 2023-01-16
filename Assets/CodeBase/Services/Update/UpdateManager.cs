using System;
using System.Collections.Generic;

namespace CodeBase.Services.Update
{
    public class UpdateManager : IUpdateService
    {
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;

        public readonly List<IUpdatable> UpateSystems = new List<IUpdatable>(16);
        public readonly List<IFixedUpdatable> FixedUpdateSystems = new List<IFixedUpdatable>(8);
        public readonly List<ILateUpdatable> LateUpdateSystems = new List<ILateUpdatable>(4);

        public void Register(ICacheUpdate obj)
        {
            if (obj == null) throw new ArgumentNullException();

            if (obj is IUpdatable)
            {
                UpateSystems.Add(obj as IUpdatable);
            }
            else if (obj is IFixedUpdatable)
            {
                FixedUpdateSystems.Add(obj as IFixedUpdatable);
            }
            else if (obj is ILateUpdatable)
            {
                LateUpdateSystems.Add(obj as ILateUpdatable);
            }
        }

        public void Unregister(ICacheUpdate obj)
        {
            if (obj == null) throw new ArgumentNullException();

            if (obj is IUpdatable)
            {
                UpateSystems.Remove(obj as IUpdatable);
            }
            else if (obj is IFixedUpdatable)
            {
                FixedUpdateSystems.Remove(obj as IFixedUpdatable);
            }
            else if (obj is ILateUpdatable)
            {
                LateUpdateSystems.Remove(obj as ILateUpdatable);
            }
        }

        public void Update()
        {
            for (int i = 0; i < UpateSystems.Count; i++)
            {
                UpateSystems[i].UpdateTick();
            }

            OnUpdate?.Invoke();
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < FixedUpdateSystems.Count; i++)
            {
                FixedUpdateSystems[i].FixedUpdateTick();
            }

            OnFixedUpdate?.Invoke();
        }

        public void LateUpdate()
        {
            for (int i = 0; i < LateUpdateSystems.Count; i++)
            {
                LateUpdateSystems[i].LateUpdateTick();
            }

            OnLateUpdate?.Invoke();
        }
    }
}