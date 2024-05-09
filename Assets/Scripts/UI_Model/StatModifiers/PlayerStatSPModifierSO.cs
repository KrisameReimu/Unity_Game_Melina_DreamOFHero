using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffectSO/PlayerStatSPModifierSO")]
public class PlayerStatSPModifierSO : PlayerStatModifierSO
{
    public override void ItemEffect(GameObject character, float val)
    {
        PlayerController player = character.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeSP((int)val);
        }
    }
}
