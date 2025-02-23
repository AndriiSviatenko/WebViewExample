using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class Case
    {
        [field: Header("General properties")]
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public Types Type { get; private set; }
        [field: SerializeField] public bool IsUsed { get; private set; }

        [field: Space(5)]
        [field: Header("Icons")]
        [field: SerializeField] public Sprite Sprite { get; private set; }

        [field: Space(5)]
        [field: Header("Specific information")]
        [field: SerializeField, Range(0, 100)] public int Count { get; private set; }
        [field: SerializeField, Range(0, 100f)] public float Weigth { get; private set; }
        [field: SerializeField, Range(0, 100)] public int Apply { get; private set; }

        public void UpdateCount(int value) => Count = value;
        public void SetUse(bool value) => IsUsed = value;
    }
}