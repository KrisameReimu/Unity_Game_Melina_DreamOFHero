using UnityEngine;
using System.Linq;

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
        GetPlayerAndBackgrounds();
    }

    void Update()
    {
        if (player == null || backgrounds == null || backgrounds.Length == 0)
        {
            GetPlayerAndBackgrounds();
        }

        if (player != null && backgrounds.Length > 0)
        {
            targetPosition = new Vector3(player.position.x, player.position.y + 1.5f, transform.position.z);
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

    void GetPlayerAndBackgrounds()
    {
        player = GameObject.FindWithTag("Player").transform;
        backgrounds = GameObject.FindGameObjectsWithTag("Background").Select(go => go.GetComponent<Renderer>()).ToArray();

        if (player == null)
        {
            Debug.LogError("Player object not found. Make sure the player object is tagged with 'Player'.");
        }

        if (backgrounds == null || backgrounds.Length == 0)
        {
            Debug.LogError("Backgrounds array is empty. Please assign GameObjects with Renderer components and tag them with 'Background'.");
        }

        if (backgrounds.Length > 0)
        {
            backgroundWidth = backgrounds[0].bounds.size.x;
        }
    }
}
