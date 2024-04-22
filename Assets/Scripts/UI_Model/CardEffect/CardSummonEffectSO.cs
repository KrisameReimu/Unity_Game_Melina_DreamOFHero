using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffectSO/CardSummonEffectSO")]
public class CardSummonEffectSO : CardEffectSO
{
    public override void CardEffect(GameObject character, GameObject prefab)
    {
        PlayerController player = character.GetComponent<PlayerController>();
        if (player != null)
        {
            GameObject summonObject = Instantiate(prefab, (Vector2)player.transform.position + new Vector2(player.direction, 0.8f), Quaternion.identity);
        }
    }
}
