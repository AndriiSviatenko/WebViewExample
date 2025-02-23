using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Types = Model.Types;

namespace UI.Game.Inventory
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Item prefab;
        [SerializeField] private int count;
        private List<Item> _freeItems;
        private Dictionary<(int, Types), Item> _oldBusyItems;
        private Dictionary<(int, Types), Item> _actualBusyItems;
        public event Action<int, Types> ClickItemEvent;

        private void Awake()
        {
            _freeItems = new List<Item>();
            _oldBusyItems = new Dictionary<(int, Types), Item>();
            _actualBusyItems = new Dictionary<(int, Types), Item>();

            for (int i = 0; i < count; i++)
            {
                var instance = Instantiate(prefab, transform);
                instance.SetActiveText(false);
                instance.ClickEvent += OnClick;
                _freeItems.Add(instance);
            }
        }
        public void UpdateInventory(List<Case> values)
        {
            _oldBusyItems = new (_actualBusyItems);
            _actualBusyItems.Clear();

            var notFoundCaseInItems = new List<Case>();

            if (_oldBusyItems.Count > 0)
            {
                foreach (var @case in values)
                {
                    if (TryUpdateBusyItemInfo(@case) == false)
                    {
                        notFoundCaseInItems.Add(@case);
                    }
                }
            }
            else
            {
                notFoundCaseInItems = values;
            }

            foreach (var @case in notFoundCaseInItems)
            {
                TrySetFreeItemInfo(@case);
            }

            foreach (var item in _oldBusyItems.Values)
            {
                item.RemoveID();
            }

            _oldBusyItems.Clear();
        }

        public Item GetItemFromIDAndTypes(int id, Types types) => 
            _actualBusyItems[(id, types)];

        public void RewriteInfoItem(int id, Types types, int newId, Types newTypes)
        {
            _actualBusyItems.Remove((id, types), out var item);
            _actualBusyItems.Add((newId, newTypes), item);
        }

        private void OnClick(int id, Types value) => 
            ClickItemEvent?.Invoke(id, value);

        private bool TrySetFreeItemInfo(Case value)
        {
            if (_freeItems.Count <= 0) return false;
            var item = _freeItems[0];

            if (TrySetItemInfo(value, item))
            {
                _freeItems.Remove(item);
                _actualBusyItems.Add((value.ID, value.Type), item);
                return true;
            }

            return false;
        }

        private bool TryUpdateBusyItemInfo(Case value)
        {
            if (!_oldBusyItems.TryGetValue((value.ID, value.Type), out Item item)) return false;
            if (TrySetItemInfo(value, item))
            {
                _oldBusyItems.Remove((value.ID, value.Type));
                _actualBusyItems.Add((value.ID, value.Type), item);
                return true;
            }

            return false;
        }

        private bool TrySetItemInfo(Case value, Item item)
        {
            if (value == null || value.Count == 0 || value.IsUsed)
            {
                item.RemoveID();
                return false;
            }

            item.SetID(value.ID, value.Type);
            item.SetImage(value.Sprite);
            item.SetActiveText(value.Count > 1);
            item.SetText(value.Count.ToString());
            return true;
        }
    }
}