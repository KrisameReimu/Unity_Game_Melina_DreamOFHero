using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += CheckScene;
    }

    private void CheckScene(Scene s1, Scene s2)
    {
        if (s1.buildIndex != s2.buildIndex)  //scene changed
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= CheckScene;
    }
}
