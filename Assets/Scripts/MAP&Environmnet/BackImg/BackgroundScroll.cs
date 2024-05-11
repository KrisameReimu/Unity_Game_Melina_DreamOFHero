using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public Transform player;
    public Renderer[] backgrounds;
    private Vector3 targetPosition;

    private int currentBackgroundIndex = 0;
    private float backgroundWidth;

    void Start()
    {
        backgroundWidth = backgrounds[0].bounds.size.x;
    }

    void Update()
    {
        targetPosition = new Vector3(player.position.x, player.position.y+1.5f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, scrollSpeed * Time.deltaTime);

        // Check if player is close to the edge of the current background
        if (player.position.x > backgrounds[currentBackgroundIndex].transform.position.x + backgroundWidth / 2)
        {
            // Move the next background to the right side of the current background
            int nextBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Length;
            backgrounds[nextBackgroundIndex].transform.position = new Vector3(backgrounds[currentBackgroundIndex].transform.position.x + backgroundWidth, backgrounds[nextBackgroundIndex].transform.position.y, backgrounds[nextBackgroundIndex].transform.position.z);

            currentBackgroundIndex = nextBackgroundIndex;
        }
        else if (player.position.x < backgrounds[currentBackgroundIndex].transform.position.x - backgroundWidth / 2)
        {
            // Move the previous background to the left side of the current background
            int prevBackgroundIndex = (currentBackgroundIndex - 1 + backgrounds.Length) % backgrounds.Length;
            backgrounds[prevBackgroundIndex].transform.position = new Vector3(backgrounds[currentBackgroundIndex].transform.position.x - backgroundWidth, backgrounds[prevBackgroundIndex].transform.position.y, backgrounds[prevBackgroundIndex].transform.position.z);

            currentBackgroundIndex = prevBackgroundIndex;
        }
    }
}
