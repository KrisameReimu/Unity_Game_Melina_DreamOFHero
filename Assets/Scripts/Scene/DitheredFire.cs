using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DitheredFire;

public class DitheredFire : InteractableTrap, IFlame
{
    [SerializeField]
    private int damage = 15;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip endEffectSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamagePlayer(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DamagePlayer(collision);
    }

    private void DamagePlayer(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        int playerDirection = transform.position.x > player.transform.position.x ? 1 : -1;
        player.ChangeHP(-damage, playerDirection);
    }

    public void Extinguish()
    {
        audioSource.PlayOneShot(endEffectSound);
        Destroy(gameObject);
    }

    public interface IFlame
    {
        public void Extinguish();
    }

}
