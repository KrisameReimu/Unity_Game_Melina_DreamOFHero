using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;
    // Start is called before the first frame update
    void Awake()
    {
        UI = GameObject.Find("UI");
        UI.SetActive(true);
    }
}
