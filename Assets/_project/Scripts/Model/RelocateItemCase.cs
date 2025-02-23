using System;
using UI.Game;

namespace Model
{
    public class RelocateItemCase
    {
        public event Action<int, Types, int, Types> RewriteInfoCompeteEvent;
        private Case _currentCase;
        private Item _currentItem;

        public void SetCurrentItemCase(Case newCase, Item newItem)
        {
            _currentCase = newCase;
            _currentItem = newItem;
        }

        public void SetNewItemCase(Case newCase, Item newItem)
        {
            if (_currentItem == newItem || _currentCase == newCase) 
                return;
            _currentItem.RemoveID();

            SetItemInfo(newCase, _currentItem);

            if (newCase != null)
                newCase.SetUse(false);

            SetItemInfo(_currentCase, newItem);
            _currentCase.SetUse(true);

            if (newCase != null)
                RewriteInfoCompeteEvent?.Invoke(_currentCase.ID, _currentCase.Type, newCase.ID, newCase.Type);
        }

        private void SetItemInfo(Case caseValue, Item item)
        {
            if (caseValue == null || caseValue.Count == 0)
            {
                item.RemoveID();
                return;
            }

            item.SetID(caseValue.ID, caseValue.Type);
            item.SetImage(caseValue.Sprite);

            if (caseValue.Type == Types.Body || caseValue.Type == Types.Head)
            {
                item.SetText(caseValue.Apply.ToString());
            }

            else
            {
                item.SetActiveText(caseValue.Count > 1);
                item.SetText(caseValue.Count.ToString());
            }
        }
    }
}