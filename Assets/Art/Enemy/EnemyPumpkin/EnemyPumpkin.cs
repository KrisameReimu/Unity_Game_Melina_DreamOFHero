using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPumpkin : Enemy
{
    public bool isAlive, isIdle, jumpAttact, isJumpUp , slideAttact, isHurt;
    GameObject player;
    Animator myAnim;
    Vector3 slideTargetPosition;
    SpriteRenderer mySr;


    //new added
    public int Life;
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
        slideAttact = false;
        isHurt = false;
        //new added
        Life = 15;
        damage = 50;



        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            if (isIdle)
            {
                if(Vector3.Distance(player.transform.position,transform.position) <= 2.0f)
                {
                    //slideattack
                    isIdle = false;
                    StartCoroutine("IdleToSlideAttact");
                    

                }
                else
                {
                    isIdle = false;
                    StartCoroutine("IdleToJumpAttack");
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
                    StartCoroutine("JumpDownToIdle");

                }

            }
            else if (slideAttact)
            {
                myAnim.SetBool("slide", true);
                transform.position = Vector3.MoveTowards(transform.position, slideTargetPosition, 4.0f * Time.deltaTime);

                if(transform.position == slideTargetPosition)
                {
                    myAnim.SetBool("slide", false);
                    slideAttact = false;
                    isIdle = true;
                }
            }
        }
        
    }
    IEnumerator JumpDownToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true;
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
    }

    IEnumerator IdleToJumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        jumpAttact = true;
    }
    public override void ChangeHP(float amount)
    {
        //Debug.Log("HP: " + enemyLife);
        Life += (int)amount;
        
        {
            Life--;
            if (Life >= 1)
            {
                isIdle = false;
                jumpAttact = false;
                slideAttact = false;
                
                isHurt = true;
                StopCoroutine("JumpDownToIdle");
                StopCoroutine("IdleToJumpAttack");
                StopCoroutine("IdleToSlideAttact");

                myAnim.SetBool("Hurt", true);
                StartCoroutine("SetAnimHurtToFalse");

            }
            else
            {
                isAlive = false;
                
                StopAllCoroutines();
                myAnim.SetBool("Die",true);
                StartCoroutine("AfterDie");
            }
            
        }
    }
    IEnumerator IdleToSlideAttact()
    {
        yield return new WaitForSeconds(1.0f);

        slideTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        slideAttact = true;
    }
    IEnumerator SetAnimHurtToFalse()
    {
        yield return new WaitForSeconds(1.0f);
        myAnim.SetBool("Hurt", false);
        isHurt = false;
        isIdle = true;
    }
    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}

