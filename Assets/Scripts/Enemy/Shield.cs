using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Enemy, INonDamagableObject
{
    [SerializeField]
    private ShieldKnight knight;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip guardClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }
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
        audioSource.PlayOneShot(guardClip);
    }
}

public interface INonDamagableObject
{
}