using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenObstacle : InteractableTrap, IBurnable
{
    [SerializeField]
    private int durability = 4;
    public void Burn()
    {
        durability -= 1;
        if(durability <= 0 )
            Destroy(gameObject);
    }
}

public interface IBurnable
{
    public void Burn();
}
