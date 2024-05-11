using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStarter : MonoBehaviour
{
    private IBoss boss;
    private void Awake()
    {
        boss = transform.root.GetComponent<IBoss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateBoss(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        ActivateBoss(collision);
    }

    private void ActivateBoss(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                boss.ActivateBoss();
                Destroy(this.gameObject);
            }
        }
    }
}
