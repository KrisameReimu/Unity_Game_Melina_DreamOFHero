using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectPlayer : MonoBehaviour
{
    
    [HideInInspector]
    public bool inRange = false;

    public GameObject alertObject;
    public float alertSpeed = 0.025f;
    public float alertHeight = 0.25f;
    
    private Vector3 velocity = Vector3.zero;
    private Vector3 startingPos;

    //Detects if Player is in range to the NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }


    //EVERYTHING UNDER HERE IS OPTIONAL AND CAN BE COMMENTED OUT IF NOT WANTED
    private void Start()
    {
        startingPos = new Vector3(alertObject.transform.position.x, alertObject.transform.position.y, 0);
    }

    private void Update()
    {
        CheckRange();
    }

    void CheckRange()
    {
        if (inRange)
        {
            if (alertObject.transform.localPosition.y <= alertHeight)
            {
                Display(alertObject);
            }
        }
        else
        {
            if (alertObject.transform.localPosition.y != 0)
            {
                Hide(alertObject);
            }
        }
    }

    void Display(GameObject displayObject)
    {
        Vector3 targetPos = new Vector3(displayObject.transform.position.x, displayObject.transform.position.y + alertHeight, 0);
        displayObject.transform.position = Vector3.SmoothDamp(displayObject.transform.position, targetPos, ref velocity, alertSpeed);
    }

    void Hide(GameObject displayObject)
    {
        Vector3 targetPos = startingPos;
        displayObject.transform.position = Vector3.SmoothDamp(displayObject.transform.position, targetPos, ref velocity, alertSpeed);
    }
}
