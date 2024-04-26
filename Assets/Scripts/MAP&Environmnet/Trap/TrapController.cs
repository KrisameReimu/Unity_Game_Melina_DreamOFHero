using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum to define the direction of trap movement
public enum TrapMoveDir
{
    None,
    Down,
    Up,
}

// Class responsible for controlling the trap movement
public class TrapController : MonoBehaviour
{
    [Header("Interpolation Value Range")]
    public float moveSpeed; // Speed of trap movement
    [Header("Control if the trap moves")]
    public bool isMove = true; // Flag to control trap movement
    public Transform startTrans; // Starting position of the trap
    public Transform endTrans; // Ending position of the trap
    [Header("Trap Object")]
    public Transform trapObj; // The trap object to be moved

    TrapMoveDir moveDir; // Current direction of trap movement

    // Start is called before the first frame update
    void Start()
    {
        if (isMove)
        {
            MovePointInit();
            moveDir = TrapMoveDir.Down; // Set initial movement direction to Down
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            TrapMove(); // Move the trap if it is set to move
        }
    }

    // Initialize the start, end positions, and trap object
    public void MovePointInit()
    {
        startTrans = transform.Find("TrapEndDown").transform;
        endTrans = transform.Find("TrapEndUp").transform;
        trapObj = transform.Find("TrapObj").transform;
    }

   

    // Move the trap based on its current direction
    public void TrapMove()
    {
        if (moveDir == TrapMoveDir.Down)
        {
            trapObj.transform.position += transform.up * moveSpeed * Time.deltaTime * -1; // Move the trap downwards
            float distance = Vector3.Distance(trapObj.transform.position, startTrans.position);
            if (distance < 0.5f) // Check if the trap has reached the starting position
            {
                moveDir = TrapMoveDir.Up; // Change the movement direction to Up
            }
        }
        else if (moveDir == TrapMoveDir.Up)
        {
            trapObj.transform.position += transform.up * moveSpeed * Time.deltaTime; // Move the trap upwards
            float distance = Vector3.Distance(trapObj.transform.position, endTrans.position);
            if (distance < 0.5f) // Check if the trap has reached the ending position
            {
                moveDir = TrapMoveDir.Down; // Change the movement direction to Down
            }
        }
        else
        {
            Debug.Log("Trap control has encountered a parameter error"); // Log an error message for unexpected parameter values
        }
    }
}
