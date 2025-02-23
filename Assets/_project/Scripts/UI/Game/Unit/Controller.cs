using UnityEngine;

namespace UI.Game.Unit
{
    public class Controller : MonoBehaviour
    {
        [field: Header("Items")]
        [field: SerializeField] public Item Head { get; private set; }
        [field: SerializeField] public Item Body { get; private set; }

        [field: Space(5)]
        [field: Header("Sliders")]
        [field: SerializeField] public ResourceSlider HP { get; private set; }
    }
}