using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(menuName = "Create Inventory", fileName = "Inventory", order = 0)]
    public class Inventory : ScriptableObject
    {
        [field: SerializeField] public List<Case> Cases { get; private set; }
    }
}