using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] 
    private Image HPMask;
    private float HPOriginalSize;

    private void Awake()
    {
        HPOriginalSize = HPMask.rectTransform.rect.width;
    }

    public void SetHPValue(float value)
    {
        HPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, HPOriginalSize * value);
    }

    public void SetBossName(string name)
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;
    }
}
