using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpArea : MonoBehaviour
{
    public static BossHpArea instance;
    [SerializeField]
    private GameObject hpBarPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public BossHpBar InitHpBar()
    {
        return Instantiate(hpBarPrefab, transform).GetComponent<BossHpBar>();
    }
}
