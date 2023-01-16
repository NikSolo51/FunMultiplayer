using System;
using CodeBase.Weapons.Reload;
using Photon.Pun;
using UnityEngine;

namespace CodeBase.PlayerScripts
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private UIIndicator _healthBar;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private float MaxHp = 100;
        private float CurrentHP;

        public void ResetHP() => CurrentHP = MaxHp;

        private void Start()
        {
            CurrentHP = MaxHp;
        }

        public float Current
        {
            get => CurrentHP;
            set
            {
                if (CurrentHP != value)
                {
                    CurrentHP = value;
                }
            }
        }

        [PunRPC]
        public void TakeDamage(float damage)
        {
            if (!_photonView.IsMine)
                return;

            if (Current <= 0)
                return;

            Current -= damage;
            
            _healthBar.AnimateIndicator(Current/MaxHp);
        }
    }
}