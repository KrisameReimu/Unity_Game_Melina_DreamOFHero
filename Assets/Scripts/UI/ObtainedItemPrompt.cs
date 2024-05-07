using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObtainedItemPrompt : MonoBehaviour
{
    [SerializeField] 
    private CollectedObjList parentList;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI message;

    private void Awake()
    {
        parentList = transform.parent.gameObject.GetComponent<CollectedObjList>();
        parentList.InformChange();
    }
    public void SetData(Sprite image, string name, int quantity)
    {
        itemImage.sprite = image;
        message.text = name+" x"+quantity;
    }
    private void Vanish()
    {
        Destroy(gameObject);
        parentList.InformChange();
    }

    
}
