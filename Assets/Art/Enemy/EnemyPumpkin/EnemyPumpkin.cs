using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPumpkin : MonoBehaviour
{
    public bool isAlive, isIdle, jumpAttact, isJumpUp;
    GameObject player;
    Animator myAnim;




    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        player = GameObject.Find("Player");
        isAlive = true;
        isIdle = true;
        jumpAttact = false;
        isJumpUp = true;



        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            if (isIdle)
            {
                if(Vector3.Distance(player.transform.position,transform.position) <= 2.0f)
                {

                }
                else
                {
                    isIdle = false;
                    jumpAttact = true;
                }
            }
            else if (jumpAttact)
            {
                if (isJumpUp)
                {
                    Vector3 myTarget = new Vector3(player.transform.position.x, 0.5f, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, 5.0f * Time.deltaTime);
                    myAnim.SetBool("JumpUp", true);
                }
                else
                {
                    myAnim.SetBool("JumpUp", false);
                    myAnim.SetBool("JumpDown", true);
                    Vector3 myTarget = new Vector3(transform.position.x, -2.5f, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, 5.0f * Time.deltaTime);
                }

                if (transform.position.y == 0.5f)
                {
                    isJumpUp = false;
                }
                else if (transform.position.y == -2.5f)
                {
                    jumpAttact = false;
                    isIdle = true;
                    isJumpUp = true;
                    myAnim.SetBool("JumpUp", false);
                    myAnim.SetBool("JumpDown", false);

                }

            }
        }
        
    }
}
