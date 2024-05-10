using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;
    private int direction;
    private List<GameObject> hittedObjects;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectSoundClip;
    [SerializeField]
    private GameObject hittedEffect;


    private void Awake()
    {
        PlayerController player = PlayerController.GetPlayerInstance();
        direction = player.direction;
        transform.position = (Vector2)player.transform.position + new Vector2(direction*2, 1f);
        transform.localScale = new Vector2(transform.localScale.x * direction, transform.localScale.y);
        hittedObjects = new List<GameObject>();
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayClip()
    {
        audioSource.PlayOneShot(effectSoundClip);
        hittedObjects.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Attack(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Attack(other);
    }
    private void Vanish()
    {
        Destroy(gameObject);
    }


    private void Attack(Collider2D other)
    {
        if (!(other.tag == "Enemy" || other.tag == "Trap"))
            return;

        if (hittedObjects.Contains(other.gameObject))//hitted in this round
            return;
        if(other.tag == "Enemy")
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
                e.Knockback(25f, -direction);
            }
        }
        else if(other.tag == "Trap")
        {
            IBurnable woodenObstacle = other.GetComponent<IBurnable>();
            if(woodenObstacle != null)
            {
                woodenObstacle.Burn();
                //Debug.Log("hit");
            }
        }

        hittedObjects.Add(other.gameObject);
        Instantiate(hittedEffect, other.transform);
    }

    
}
