using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UI.Game;
using UI.Game.Bottom;
using UnityEngine;
using Controller = UI.Game.Controller;
using Random = UnityEngine.Random;

namespace Model
{
    public class UIHandler : MonoBehaviour
    {
        public event Action<float> SetHeadProtectionEvent;
        public event Action<float> SetBodyProtectionEvent;
        public event Action<float> TakePlayerReplishHealEvent;
        public event Action<int> TakeEnemyDamageEvent;
        public event Action TakePlayerDamageEvent;

        [Space(5)]
        [Header("Guns")]
        [SerializeField] private List<GunCase> guns;
        private Dictionary<Guns, GunCase> _gun;
        private List<Case> _cases;
        private UI.Game.Controller _ui;
        private RelocateItemCase _relocateItemCase;
        private Guns _currentGun;

        private void Awake()
        {
            _gun = new Dictionary<Guns, GunCase>();
            _relocateItemCase = new RelocateItemCase();

            foreach (var gun in guns)
            {
                _gun.Add(gun.Type, gun);
            }
        }

        public void Initialize(UI.Game.Controller ui, Inventory inventory)
        {
            _ui = ui;
            _cases = inventory.Cases;

            _ui.Bottom.EventActions += SelectGun;
            _ui.Bottom.EventShoot += Shoot;
            _ui.Popup.ApplyEvent += OnClickApplyButton;
            _ui.Popup.DeleteEvent += OnClickDeleteButton;
            _ui.Inventory.ClickItemEvent += Show;

            _relocateItemCase.RewriteInfoCompeteEvent += ui.Inventory.RewriteInfoItem;

            SetActualHeadAndBody();

            _ui.Inventory.UpdateInventory(_cases);
            _ui.Bottom.SetGun1(guns[0].Sprite,guns[0].Damage);
            _ui.Bottom.SetGun2(guns[1].Sprite,guns[1].Damage);
        }

        #region Game

        #region Inventory

        private void SetActualHeadAndBody()
        {
            foreach (var @case in _cases.Where(@case => @case.Type is Types.Body or Types.Head)
                         .Where(@case => @case.IsUsed))
            {
                if (@case.Type == Types.Body)
                {
                    SetItemInfo(@case, _ui.Player.Body);
                    SetBodyProtectionEvent?.Invoke(@case.Apply);
                }
                else
                {
                    SetItemInfo(@case, _ui.Player.Head);
                    SetHeadProtectionEvent?.Invoke(@case.Apply);
                }
            }
        }

        public void StartGenerateItem()
        {
            var result = Random.Range(0, _cases.Count);
            var countResult = Random.Range(0, 15);
            var count = _cases[result].Count + countResult;
            _cases[result].UpdateCount(count);
        }

        #endregion

        #region Bottom

        private void SelectGun(Guns value)
        {
            _currentGun = value;
        }

        private void Shoot()
        {
            if (_gun.TryGetValue(_currentGun, out var @case))
            {
                if (TryFindCase(@case.ProjectileID, Types.Projectiles, out var findingCase))
                {
                    if (findingCase.Count >= @case.MinProjectileCount)
                    {
                        findingCase.UpdateCount(Math.Clamp(findingCase.Count - @case.MinProjectileCount, 0,
                            int.MaxValue));
                        TakeEnemyDamageEvent?.Invoke(@case.Damage);
                        TakePlayerDamageEvent?.Invoke();
                        _ui.Inventory.UpdateInventory(_cases);
                    }
                }
            }
        }

        #endregion

        #endregion


        #region popup

        private void Show(int id, Types value)
        {
            if (TryFindCase(id, value, out var @case) == false) return;

            _ui.Popup.Initialize(@case.Name, @case.Sprite, @case.Weigth.ToString(CultureInfo.CurrentCulture),
                @case.Apply.ToString(), @case.Type,
                @case.ID);
            _ui.PopupPanel();
        }

        private void OnClickDeleteButton(Types value, int id)
        {
            if (TryFindCase(id, value, out var @case) == false) return;
            @case.UpdateCount(0);
            _ui.Inventory.UpdateInventory(_cases);
            _ui.ClosePopupPanel();
        }

        private void OnClickApplyButton(Types type, int id)
        {
            Case findingCase;
            switch (type)
            {
                case Types.Body:
                    if (TryFindCase(id, Types.Body, out findingCase))
                    {
                        _relocateItemCase.SetCurrentItemCase(findingCase,
                            _ui.Inventory.GetItemFromIDAndTypes(id, type));
                        TryFindCase(_ui.Player.Body.CaseID, Types.Body, out var newCase);
                        _relocateItemCase.SetNewItemCase(newCase, _ui.Player.Body);
                        SetBodyProtectionEvent?.Invoke(findingCase.Apply);
                    }

                    break;
                case Types.Head:
                    if (TryFindCase(id, Types.Head, out findingCase))
                    {
                        _relocateItemCase.SetCurrentItemCase(findingCase,
                            _ui.Inventory.GetItemFromIDAndTypes(id, type));
                        TryFindCase(_ui.Player.Head.CaseID, Types.Head, out var newCase);
                        _relocateItemCase.SetNewItemCase(newCase, _ui.Player.Head);
                        SetHeadProtectionEvent?.Invoke(findingCase.Apply);
                    }

                    break;
                case Types.Medicine:
                    if (TryFindCase(id, Types.Medicine, out findingCase))
                    {
                        TakePlayerReplishHealEvent?.Invoke(findingCase.Apply);
                        findingCase.UpdateCount(Math.Clamp(findingCase.Count - 1, 0, int.MaxValue));
                    }

                    break;
                case Types.Projectiles:
                    if (TryFindCase(id, Types.Projectiles, out findingCase))
                        findingCase.UpdateCount(Math.Clamp(findingCase.Count + 1, 0, int.MaxValue));
                    break;
                case Types.Null:
                    break;
            }

            _ui.Inventory.UpdateInventory(_cases);
            _ui.ClosePopupPanel();
        }

        #endregion

        private bool TryFindCase(int id, Types value, out Case outCase)
        {
            foreach (var @case in _cases)
            {
                if (@case.ID == id && @case.Type == value)
                {
                    outCase = @case;
                    return true;
                }
            }

            outCase = null;
            return false;
        }

        private void SetItemInfo(Case value, Item item)
        {
            if (value == null || value.Count == 0)
            {
                item.RemoveID();
                return;
            }

            item.SetID(value.ID, value.Type);
            item.SetImage(value.Sprite);
            item.SetText(value.Apply.ToString());
        }
    }
}