using System.Collections.Generic;
using CodeBase.Services.Update;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class Rotator : MonoBehaviour,IUpdatable
    {
        [SerializeField] private List<GameObject> _objects;
        [SerializeField,Range(-1,1)] private float _speed;
        private IUpdateService _updateService;
        
        [Inject]
        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
        }

        public void UpdateTick()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].transform.RotateAround(Vector3.forward,1 * _speed);
            }
        }
    }
}