using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffectSO/PlayerStatSpeedModifierSO")]
public class PlayerStatSpeedModifierSO : PlayerStatModifierSO
{
    public override void ItemEffect(GameObject character, float val)
    {
        PlayerController player = character.GetComponent<PlayerController>();
        if (player != null)
        {
            player.SpeedUp(val);
        }
    }
}
