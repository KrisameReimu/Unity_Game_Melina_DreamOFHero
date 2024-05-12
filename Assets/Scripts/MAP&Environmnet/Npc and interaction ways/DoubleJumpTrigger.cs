using UnityEngine;

public class DoubleJumpTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.UnlockDoubleJump(true);
                Debug.Log("DoubleJump");
            }
        }
    }
}
