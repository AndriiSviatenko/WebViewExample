using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Unit
{
    public class ResourceSlider: MonoBehaviour
    {
        [SerializeField] private Image slider;

        public void SetValue(float current, float max)
        {
            if (slider == null) return;
            var fillAmount = Mathf.Clamp(current / max, 0, 1);
            slider.fillAmount = fillAmount;
        }
    }
}