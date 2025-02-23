using System;
using Model.Unit.Stats;
using Model.Unit.Stats.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model.Unit
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Health health;
        private float _head = 0;
        private float _body = 0;
        public IHeal Heal => health;
        public event Action DeathEvent;
        public void Initialize(float hp)
        {
            health.SetMaxHealth(hp);
        }
        public void SetHead(float value) => _head = value;
        public void SetBody(float value) => _body = value;
        public void Damage(float value)
        {
            var result = Random.Range(0, 1);
            health.SpendHealth(result == 0 ? value - _head * 0.1f : value - _body * 0.1f);
        }

        private void Awake()
        {
            health.OnDeathEvent += Death;
        }

        private void Death()
        {
            DeathEvent?.Invoke();
        }
    }
}