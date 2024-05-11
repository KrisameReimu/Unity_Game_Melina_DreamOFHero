using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Parser))]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager manager { get; set; }

    [Header("NPC UI")]
    public TextMeshProUGUI dialogueText;
    public Image sprite;
    public TextMeshProUGUI characterName;
    [Space(10)]
    [Header("Type Writer"), Space(5)]
    public float typeWriterSpeed = 0.025f;
    [Space(10)]
    public bool autoNextLine = false;
    public float waitForNextLine;
    [Space(10)]
    public bool pauseOnPunctuation = false;
    public float pauseLength = 0.6f;
    [Space(10)]
    public bool playTextAudio = false;
    public AudioClip audioClip;
    public AudioSource audioSource;
    [Space(10)]
    [Header("Player Controls")]
    //this can be whatever player controller you are using
    //simply just change the reference
    public PlayerController playerController;
    public KeyCode interactKey;

    private void Awake()
    {
        if (manager == null) 
        { 
            manager = this; 
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void OnCharReveal(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        audioSource.pitch = Random.Range(1f, 1.2f);
    }
}
