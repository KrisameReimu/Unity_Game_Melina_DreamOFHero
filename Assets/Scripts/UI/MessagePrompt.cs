using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePrompt : MonoBehaviour
{
    [SerializeField]
    private GameObject messagePrefab;
    public void PromptMessage(string message)
    {
        GameObject msg = Instantiate(messagePrefab, transform);
        msg.GetComponent<TMPro.TextMeshProUGUI>().text = message;
        StartCoroutine(ClearMessage(msg));
    }
    IEnumerator ClearMessage(GameObject message)
    {
        yield return new WaitForSeconds(2);
        Destroy(message);
    }
}
