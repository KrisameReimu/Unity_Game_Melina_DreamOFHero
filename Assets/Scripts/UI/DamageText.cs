using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro m_Text;
    
    

    private void FixedUpdate()
    {
        transform.position += Vector3.up * 0.02f;
    }

    public void SetText(string text)
    {
        m_Text.text = text;
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
