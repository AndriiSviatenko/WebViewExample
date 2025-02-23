namespace UI.Game
{
    public class PanelRechanger
    {
        private IHidenable _currentPanel;

        public void SetNewPanel(IHidenable newPanel)
        {
            _currentPanel?.Hide();
            _currentPanel = newPanel;
            _currentPanel?.Show();
        }
    }
}