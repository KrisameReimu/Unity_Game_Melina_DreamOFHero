using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI Components")]
    public Image headImage;
    public Text textLabel;
    public Text dialogueNameText; // Text component to display dialogue name

    [Header("Text File")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

    [Header("Head Images")]
    public Sprite headPlayer;
    public Sprite headNPC;

    public string dialogueName; // Variable to store the name of the speaker
    public string dialogueNamePlayer = "Melina";
    public string dialogueNameNpc;

    bool textFinished;  // Check if the text display is finished
    bool isTyping;  // Check if typing is in progress

    List<string> textList = new List<string>();

    void Awake()
    {
        GetTextFromFile(textFile);
    }

    void OnEnable()
    {
        index = 0;  // Reset dialogue when shown
        textFinished = true;  // Set text display status to finished when shown
        StartCoroutine(setTextUI());
    }

    void Update()
    {
        // Close dialogue box if "F" key is pressed and all text is displayed
        if (Input.GetKeyDown(KeyCode.F) && index == textList.Count)
        {
            textLabel.text = "";  // Clear text content
            textFinished = true;  // Set text display status to finished
            isTyping = false;    // Stop typing effect

            // Hide only the UI elements of the dialog box, keep NPC character visible
            textLabel.gameObject.SetActive(false);
            dialogueNameText.gameObject.SetActive(false); // Hide dialogue name

            return;
        }

        // If "F" key is pressed, start coroutine if current line is finished, otherwise display current line
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (textFinished)
            {
                StartCoroutine(setTextUI());
            }
            else if (!textFinished)
            {
                isTyping = false;
            }
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        // Clear text content
        textList.Clear();

        // Split text file content line by line and add to list
        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator setTextUI()
    {
        textFinished = false;   // Enter text display state
        textLabel.text = "";    // Reset text content

        // Check the content of the text file
        switch (textList[index].Trim())
        {
            case "A":
                headImage.sprite = headPlayer;
                dialogueName = dialogueNamePlayer; // Set dialogue name for player
                index++;
                break;
            case "B":
                headImage.sprite = headNPC;
                dialogueName = dialogueNameNpc; // Set dialogue name for NPC
                index++;
                break;
        }

        dialogueNameText.text = dialogueName; // Update dialogue name display

        // Display one word at a time for each press of "F" key
        int word = 0;
        while (isTyping && word < textList[index].Length - 1)
        {
            // Typewriter effect
            textLabel.text += textList[index][word];
            word++;
            yield return new WaitForSeconds(textSpeed);
        }

        // Quickly display the entire line of text
        textLabel.text = textList[index];

        isTyping = true;
        textFinished = true;
        index++;
    }
}
