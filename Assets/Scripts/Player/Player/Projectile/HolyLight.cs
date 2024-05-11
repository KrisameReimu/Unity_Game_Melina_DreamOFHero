using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyLight : MonoBehaviour
{
    [SerializeField]
    private int damage = 20;
    private int direction;
    private int remainingRound = 4;
    private List<GameObject> hittedObjects;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectSoundClip;


    private void Awake()
    {
        PlayerController player = PlayerController.GetPlayerInstance();
        direction = player.direction;
        transform.position = (Vector2)player.transform.position + new Vector2(direction, 1.5f);
        hittedObjects = new List<GameObject>();
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayClip()
    {
        audioSource.PlayOneShot(effectSoundClip);
    }
    private void MoveForward()
    {
        transform.position += Vector3.right * direction*2;
    }

    private void EndOfRound()
    {
        remainingRound -= 1;
        //Debug.Log(remainingRound);
        hittedObjects.Clear();

        if (remainingRound <= 0) 
        { 
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy")
            return;
        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
        {
            if (hittedObjects.Contains(e.gameObject))//hitted in this round
                return;

            IUndead undead = e as IUndead;
            if (undead != null)
            {
                undead.Purify(-damage);
                return;
            }
            e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
            e.Knockback(25f, -direction);
            hittedObjects.Add(e.gameObject);
        }
    }
}
