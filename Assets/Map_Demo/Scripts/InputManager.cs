using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    [Header("控制是否使用自定义按键")]
    public bool keyIsSet;
    [Header("移动按键,暂时无法使用")]
    public KeyCode moveKey;
    [Header("攻击按键")]
    public KeyCode attackKey;
    [Header("跳跃按键")]
    public KeyCode jumpKey;
    [Header("冲刺按键")]
    public KeyCode sprintKey;
    [Header("超级冲刺按键")]
    public KeyCode superKey;

    private void Awake()
    {
        if (Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        KeyInit();

    }


    public void KeyInit()
    {
        if (!keyIsSet)
        {
            attackKey = KeyCode.K;
            jumpKey = KeyCode.Space;
            sprintKey = KeyCode.J;
            superKey = KeyCode.L;
        }
    }
}
