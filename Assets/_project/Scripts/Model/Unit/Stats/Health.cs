using System;
using Model.Unit.Stats.Interfaces;
using UnityEngine;

namespace Model.Unit.Stats
{
    public class Health : MonoBehaviour, IHeal
    {
        public event Action<float, float> OnHealthChangeEvent;
        public event Action OnDeathEvent;
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public void SetMaxHealth(float value)
        {
            MaxHealth = value;
            CurrentHealth = MaxHealth;
            OnHealthChangeEvent?.Invoke(CurrentHealth, MaxHealth);
        }

        public void SpendHealth(float value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, MaxHealth);
            OnHealthChangeEvent?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0) Death();
        }

        public void ReplenishHealth(float value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
            OnHealthChangeEvent?.Invoke(CurrentHealth, MaxHealth);
        }

        private void Death()
        {
            OnDeathEvent?.Invoke();
        }
    }
}