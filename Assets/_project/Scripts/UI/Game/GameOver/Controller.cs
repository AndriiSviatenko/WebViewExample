using UnityEngine;

namespace UI.Game.GameOver
{
    public class Controller : MonoBehaviour, IHidenable
    {
        [SerializeField] private CanvasGroup canvasGroup;

        void IHidenable.Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.ignoreParentGroups = true;
        }

        void IHidenable.Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.ignoreParentGroups = false;
        }
    }
}