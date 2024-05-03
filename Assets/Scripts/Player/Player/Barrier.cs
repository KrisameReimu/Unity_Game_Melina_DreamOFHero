using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Barrier : MonoBehaviour
{
    GameObject playerObject;
    PlayerController player;
    private void Awake()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<PlayerController>();
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)playerObject.transform.position + new Vector2(0, 0.8f);
        if(player.EX<=0)
            Destroy(gameObject);
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
            e.Knockback(10f, knockbackDirection);
        }
    }


}
