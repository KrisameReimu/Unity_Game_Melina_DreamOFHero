using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Enemy
{
    [SerializeField]
    private ShieldKnight knight;
    
    private void Update()
    {
        damage = knight.damage;
    }
    public override void ChangeHP(float amount)
    {
        ShowDamageText("Guard", Color.gray);
        Renderer r = knight.GetComponent<Renderer>();
        r.material.SetColor("_Color", Color.yellow);
        r.material.DOColor(Color.white, 0.5f);
    }
}
