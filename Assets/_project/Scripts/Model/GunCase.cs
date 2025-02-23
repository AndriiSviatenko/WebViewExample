using System;
using UI.Game.Bottom;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class GunCase
    {
        [field: SerializeField] public Guns Type { get; private set; }
        [field: SerializeField] public Sprite Sprite{ get; private set; }
        [field: SerializeField] public int ProjectileID { get; private set; }
        [field: SerializeField] public int MinProjectileCount { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }

}