using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStatModifierSO/PlayerStatHealthModifierSO")]
public class PlayerStatHealthModifierSO : PlayerStatModifierSO
{
    public override void ItemEffect(GameObject character, float val)
    {
        PlayerController player = character.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHP((int)val, 0);
        }
    }
}
