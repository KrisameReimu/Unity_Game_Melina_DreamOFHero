using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapNameDisplay : MonoBehaviour
{
    public Text mapNameText; // Text element to display the map name
    public string mapName; // The name of the map to display
    public float displayDuration = 3f; // Duration to display the map name
    public float fadeDuration = 1f; // Duration for fade out effect

    public AudioClip soundEffect; // Sound effect clip to play when displaying the map name

    private AudioSource audioSource; // Audio source to play the sound effect

    void Start()
    {
        mapNameText.text = mapName; // Set the text to display the map name

        GameObject audioObject = new GameObject("AudioObject"); // Create a new empty GameObject for audio
        audioSource = audioObject.AddComponent<AudioSource>(); // Add AudioSource component to the new GameObject
        audioSource.clip = soundEffect; // Set the sound effect clip
        audioSource.Play(); // Play the sound effect

        StartCoroutine(FadeOutAfterDelay(displayDuration)); // Start the fade out coroutine
    }

    IEnumerator FadeOutAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay before starting the fade out

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            mapNameText.color = new Color(mapNameText.color.r, mapNameText.color.g, mapNameText.color.b, alpha);
            yield return null;
        }
    }
}
