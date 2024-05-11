using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class Barrier : MonoBehaviour
{
    PlayerController player;
    
    

    private void Awake()
    {
        player = PlayerController.GetPlayerInstance();
        DontDestroyOnLoad(gameObject);
        player.OnGettingHitInvincile += BarrierVFX;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)player.transform.position + new Vector2(0, 0.8f);
        if (player.EX <= 0)
        {
            Destroy(gameObject);
            player.OnGettingHitInvincile -= BarrierVFX;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag != "Enemy")
            return;
        Enemy e = other.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.ChangeHP(0); //call the function to decrease enemies' HP
            int knockbackDirection = transform.position.x > e.transform.position.x ? 1 : -1;
            e.Knockback(5f, knockbackDirection);

            BarrierVFX();
        }
    }

    private void BarrierVFX()
    {
        if(gameObject==null) 
            return;

        Renderer r = gameObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", Color.yellow);
        r.material.DOColor(Color.white, 0.3f);
    }

}
