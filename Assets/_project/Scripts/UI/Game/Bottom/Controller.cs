using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Bottom
{
    public class Controller : MonoBehaviour
    {
        [Header("Guns")] 
        [SerializeField] private Gun gun1;
        [SerializeField] private Gun gun2;

        [Header("Buttons")] 
        [SerializeField] private Button shoot;
        private Guns _currentGun;
        public event Action<Guns> EventActions;
        public event Action EventShoot;

        private void Awake()
        {
            OnСlick(Guns.Gun1);
            gun1.Button.onClick.AddListener(() => OnСlick(Guns.Gun1));
            gun2.Button.onClick.AddListener(() => OnСlick(Guns.Gun2));

            shoot.onClick.AddListener(OnShoot);
        }

        private void OnShoot() => 
            EventShoot?.Invoke();

        private void OnСlick(Guns value)
        {
            _currentGun = value;

            if (_currentGun == Guns.Gun1)
            {
                gun1.Image.color = Color.green;
                gun2.Image.color = Color.white;
            }
            else
            {
                gun2.Image.color = Color.green;
                gun1.Image.color = Color.white;
            }

            EventActions?.Invoke(_currentGun);
        }

        public void SetGun1(Sprite sprite, int damage) => 
            gun1.SetInfo(sprite, damage);

        public void SetGun2(Sprite sprite, int damage) => 
            gun2.SetInfo(sprite, damage);
    }
}