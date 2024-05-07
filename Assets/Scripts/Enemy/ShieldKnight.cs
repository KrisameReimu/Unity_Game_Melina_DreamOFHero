using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnight : Enemy
{
    private Animator anim;
    [SerializeField]
    private float maxHP = 50f;
    [field: SerializeField]
    public float HP { get; private set; }
    [SerializeField]
    private Vector2 targetPosition;
   

    private void Awake()
    {
        HP = maxHP;
        damage = 1;
        isAttacking = false;
        anim = GetComponent<Animator>();
        targetPosition = PlayerController.GetPlayerInstance().transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag != "Player")
            return;
        PlayerController player = c.gameObject.GetComponent<PlayerController>();
        targetPosition = player.transform.position;
    }

    private void ChangeDirection()
    {
        if (transform.position.x <= targetPosition.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public override void ChangeHP(float amount)
    {
        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        Renderer r = gameObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", Color.red);
        r.material.DOColor(Color.white, 0.5f);
        ShowDamageText(amount);
        
        PlayerController player = PlayerController.GetPlayerInstance();
        targetPosition = player.transform.position;
        

        if (HP > 0)
            anim.SetTrigger("Hurt");
        else
        {
            /*
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.simulated = false;
            */
            isAttacking = false;
            anim.SetTrigger("Die");
            DropCard();
        }
    }


    private void Vanish()
    {
        Destroy(gameObject);
    }

}
