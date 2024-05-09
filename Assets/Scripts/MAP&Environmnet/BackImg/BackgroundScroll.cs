using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private Renderer rend;
    public Transform player;
    private Vector3 targetPosition;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, scrollSpeed * Time.deltaTime);
    }
}
