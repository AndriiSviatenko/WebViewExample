using UnityEngine;

namespace UI.Game
{
    public class Controller : MonoBehaviour
    {
        [field: Header("Units")]
        [field: SerializeField] public Unit.Controller Player { get; private set; }
        [field: SerializeField] public Unit.Controller Enemy { get; private set; }

        [field: Space(5)]
        [field: Header("Inventory")]
        [field: SerializeField] public Inventory.Controller Inventory { get; private set; }

        [field: Space(5)]
        [field: Header("Screens")]
        [field: SerializeField] public Bottom.Controller Bottom { get; private set; }
        [field: SerializeField] public Popup.Controller Popup { get; private set; }
        [field: SerializeField] public GameOver.Controller GameOver { get; private set; }

        private PanelRechanger _panelRechanger;

        private void Awake() => 
            _panelRechanger = new PanelRechanger();

        public void GameOverPanel() => 
            _panelRechanger.SetNewPanel(GameOver);

        public void PopupPanel() => 
            _panelRechanger.SetNewPanel(Popup);

        public void ClosePopupPanel() => 
            _panelRechanger.SetNewPanel(null);
    }
}