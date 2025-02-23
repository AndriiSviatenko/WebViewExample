using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Bottom
{
    [Serializable]
    public class Gun
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private Image gun;

        public void SetInfo(Sprite sprite, int damage)
        {
            gun.sprite = sprite;
            damageText.text = damage.ToString();
        }
    }
}