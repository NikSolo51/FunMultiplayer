using UnityEngine;

namespace CodeBase.PlayerScripts
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private HeroMove _heroMove;
        public void Die()
        {
            _heroMove.ResetPosition();
        }
    }
}