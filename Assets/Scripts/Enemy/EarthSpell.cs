using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpell : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundClip;
    private AudioSource audioSource;
    [SerializeField]
    private int damage;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(soundClip);
    }
    public void SetDamage(int value)
    {
        damage = value;
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Attack(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Attack(collision);
    }

    private void Attack(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            int direction = transform.localRotation.z == 0f ? -1 : 1;
            player.ChangeHP(-damage, direction);
        }
    }
}
