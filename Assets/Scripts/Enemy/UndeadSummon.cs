using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSummon : Enemy, IEnemyProjectile, IUndead
{
    private bool isActivated = false;
    [SerializeField]
    private GameObject targetObj;
    private Vector2 targetPosition;
    private int speed = 2;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectClip;

    int IEnemyProjectile.damage => this.damage;

    [SerializeField]
    private float lifetime = 7;

    private void Awake()
    {
        targetObj = PlayerController.GetPlayerInstance().gameObject;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(effectClip);
        ChangeDirection();
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
            return;

        targetPosition = targetObj.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        ChangeDirection();

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            GetComponent<Animator>().SetTrigger("Die");
    }

    private void StartAction()
    {
        isActivated = true;
    }

    public override void ChangeHP(float amount) //die if is damaged
    {
        lifetime += amount;
        ShowDamageText(amount);

        if (lifetime <= 0)
            GetComponent<Animator>().SetTrigger("Die");
    }

    private void Vanish()
    {
        audioSource.PlayOneShot(effectClip);
        Destroy(gameObject);
    }

    public void IsGuarded()
    {
        ChangeHP(-10);
    }

    public void Purify(float value)
    {
        ChangeHP(value);
    }


    private void ChangeDirection()
    {
        if (transform.position.x <= targetPosition.x)
            transform.localScale = new Vector3(1 * System.Math.Abs(transform.localScale.x), transform.localScale.y, 1f);
        else
            transform.localScale = new Vector3(-1 * System.Math.Abs(transform.localScale.x), transform.localScale.y, 1f);
    }
}
