using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackImgMove : MonoBehaviour
{

    public Transform player;
    Vector3 backStartPoint;     // Initial position of the background
    Vector3 playeStartPoint;    // Initial position of the player
    void Start()
    {

        backStartPoint = transform.position;
        playeStartPoint = player.position;

    }

    // Update is called once per frame
    void Update()
    {
        BackMoveFunc();

    }

    public void BackMoveFunc()
    {
        float tempX = player.position.x - playeStartPoint.x;
        float tempY = player.position.y - playeStartPoint.y;    // Calculate the historical displacement on the X and Y axes
        float xValue = tempX * 0.08f;
        float yValue = tempY * 0.06f; // Calculate the X and Y axis displacement of the background
        transform.position = new Vector3(backStartPoint.x + xValue, backStartPoint.y + yValue, 0);
    }

}
