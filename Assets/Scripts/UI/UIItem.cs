using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMP_Text quantityText;
    private Image background;


    [SerializeField]
    public event Action<UIItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

    private bool empty = true;

    // Start is called before the first frame update
    void Awake()
    {
        ResetData();
        background = GetComponent<Image>();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Select()
    {
        //borderImage.enabled = true;
        background.color = new Color32(255, 105, 28, 255);
    }
    public void Deselect()
    {
        //borderImage.enabled = false;
        background.color = new Color32(106, 106, 106, 255);
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive (true);
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity.ToString();
        empty = false;
    }

    public void OnBeginDrag()
    {
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        if (empty)
            return;
        PointerEventData pointerData = (PointerEventData)data;
        if(pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else 
        {
            Debug.Log("Clicked");
            OnItemClicked?.Invoke(this);
        }
    }

}
