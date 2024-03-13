using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaleZombie : Enemy
{

    public Vector3 targetPosition;
    public float mySpeed;
    Animator myAnim;
    Vector3 originPosition, turnPoint;
    GameObject myPlayer;
    public int enemyLife;






    // Start is called before the first frame update


    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        myPlayer = GameObject.Find("Player");

        enemyLife = 3;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(myPlayer.transform.position, transform.position) < 1.5f)
        {
            if(myPlayer.transform.position.x <= transform.position.x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attact"))
            {
                return;
            }
            myAnim.SetTrigger("Attact");
            return;
        }
        else
        {
            if (turnPoint == targetPosition)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else if (turnPoint == originPosition)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        if(transform.position.x == targetPosition.x)
        {
            myAnim.SetTrigger("Idle");
            turnPoint = originPosition;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (transform.position.x == originPosition.x)
        {
            myAnim.SetTrigger("Idle");
            turnPoint = targetPosition;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        
        if(myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
        }
        
    }
    public override void ChangeHP(float amount)
    {
        {
            Debug.Log("HP: "+enemyLife);
            enemyLife += (int)amount;
            if(enemyLife >= 1)
            {
                myAnim.SetTrigger("Hurt");
            }
            else if (enemyLife < 1)
            {
                myAnim.SetTrigger("Die");
            }
        }
    }
}
