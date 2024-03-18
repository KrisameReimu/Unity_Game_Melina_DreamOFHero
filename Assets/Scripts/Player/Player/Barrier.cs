using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Barrier : MonoBehaviour
{
    GameObject player;
    PlayerController pc;
    private void Awake()
    {
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)player.transform.position + new Vector2(0, 0.8f);
        if(pc.EX<=0)
            Destroy(gameObject);
    }


}
