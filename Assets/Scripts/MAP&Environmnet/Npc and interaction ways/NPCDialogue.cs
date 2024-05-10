using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public Text dialogueText;
    public RawImage speakerImage;
    public GameObject dialoguePanel;
    public Sprite speakerSprite; // 说话者的头像
    public string[] dialogues;
    private int currentDialogueIndex = 0;
    private bool isTalking = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextDialogue();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is near NPC");
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialoguePanel.SetActive(true);
        speakerImage.texture = speakerSprite.texture;
        ShowNextDialogue();
    }

    void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialoguePanel.SetActive(false);
    }
}
