using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wings : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectClip;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ActiveWings()
    {
        anim.SetTrigger("Active");
        audioSource.PlayOneShot(effectClip);
    }
}
