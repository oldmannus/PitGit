using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit
{
    public class BS_InventoryActor : BS_InventoryContainer
    {
        public enum InventorySlot
        {
            LeftHand,
            RightHand,
            Chest,
            Head,
            Legs,
            Feet,
            Count
        }

        [SerializeField]
        BS_InventoryItem[] _equippedItems = new BS_InventoryItem[(int)InventorySlot.Count];

        public BS_InventoryItem GetSlot(InventorySlot inventorySlot)
        {
            return _equippedItems[(int)inventorySlot];
        }
    }
}