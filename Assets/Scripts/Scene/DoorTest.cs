using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    public int sceneIndex;
    public Vector2 position;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("Enter");
                SceneController.Instance.LoadNextScene(sceneIndex, position);
            }
        }
    }
}
