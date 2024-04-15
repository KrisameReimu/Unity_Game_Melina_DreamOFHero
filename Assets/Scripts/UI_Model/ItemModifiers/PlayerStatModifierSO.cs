using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStatModifierSO : ScriptableObject
{
    public abstract void ItemEffect(GameObject character, float val);
}
