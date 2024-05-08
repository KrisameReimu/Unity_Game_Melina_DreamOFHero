using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedObjList : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Show", false);
        //Debug.Log(transform.childCount);

    }
    public void InformChange()
    {
        StartCoroutine(StartChecking());
    }

    IEnumerator StartChecking()
    {
        yield return null;
        //Debug.Log(transform.childCount);
        if (transform.childCount == 1)
        {
            anim.SetBool("Show", false);
        }
        else if (transform.childCount > 1)
        {
            anim.SetBool("Show", true);
        }
        else //transform.childCount == 0
        {
            Debug.Log("The obtained title is missing");
        }
    }
}
