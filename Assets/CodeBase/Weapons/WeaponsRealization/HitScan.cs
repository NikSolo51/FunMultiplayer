using UnityEngine;

namespace CodeBase.Weapons.WeaponsRealization
{
    public class HitScan : MonoBehaviour
    {
        [SerializeField]private Transform _shootOrigin;
        [SerializeField]private LayerMask _layermask;

        public RaycastHit GetHit()
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(_shootOrigin.position, _shootOrigin.forward, out hitinfo, Mathf.Infinity, _layermask))
            {
                return hitinfo;
            }

            return hitinfo;
        }
        
        
        public RaycastHit GetHit(Vector3 direction)
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(_shootOrigin.position, direction, out hitinfo, Mathf.Infinity, _layermask))
            {
                return hitinfo;
            }

            return hitinfo;
        }
    }
}