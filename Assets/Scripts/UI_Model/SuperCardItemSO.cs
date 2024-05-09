using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "ItemSO/SuperCardItemSO")]
    public class SuperCardItemSO : CardItemSO, INonDestroyableItem
    {
        //Non-Consumable Card
    }

    public interface INonDestroyableItem { }

    
}