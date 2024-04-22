using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardSlotPopUp : MonoBehaviour
{
    public event Action<int> OnSlotChosen;
    public void CardSlotBtnPressed(GameObject btn)
    {
        //Debug.Log("Sibling Index : " + btn.transform.GetSiblingIndex());
        int index = btn.transform.GetSiblingIndex();//get index of pressed button
        OnSlotChosen?.Invoke(index);
        Toggle(false);
    }
    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
