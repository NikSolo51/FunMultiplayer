using System;
using CodeBase.Weapons;
using Photon.Pun;
using UnityEngine;

namespace CodeBase.PlayerScripts
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private float MaxHp = 100;
        [SerializeField] private PlayerDeath _playerDeath;
        public event Action<float> OnHpPercent;
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

            Current -= damage;
            OnHpPercent?.Invoke(Current / MaxHp);
            if (Current <= 0)
            {
                _playerDeath.Die();
                ResetHP();
                OnHpPercent?.Invoke(Current / MaxHp);
            }
        }
    }
}