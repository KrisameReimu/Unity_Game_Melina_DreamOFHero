using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIStatusBar : MonoBehaviour
{
    public static UIStatusBar instance { get; private set; }

    public Image HPMask;
    public Image SPMask;
    public Image BurstMask;
    public Image burstBar;
    public Sprite BurstNotReady;
    public Sprite BurstReady;
    float HPOriginalSize;
    float SPOriginalSize;
    float BurstOriginalSize;

    void Awake()
    {
        instance = this;
        //burstBar = BurstMask.transform.Find("Burst bar").gameObject.GetComponent<Image>();
    }

    void Start()
    {
        HPOriginalSize = HPMask.rectTransform.rect.width;
        SPOriginalSize = SPMask.rectTransform.rect.width;
        BurstOriginalSize = BurstMask.rectTransform.rect.width;
    }

    public void changeGaugeColor()
    {
        if (Mathf.Approximately(BurstMask.rectTransform.rect.width, BurstOriginalSize))
        {
            burstBar.sprite = BurstReady;
        }
        else
        {
            burstBar.sprite = BurstNotReady;
        }
    }


    public void SetHPValue(float value)
    {
        HPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, HPOriginalSize * value);
    }
    public void SetSPValue(float value)
    {
        SPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SPOriginalSize * value);
    }
    public void SetBurstValue(float value)
    {
        BurstMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BurstOriginalSize * value);
    }
}
