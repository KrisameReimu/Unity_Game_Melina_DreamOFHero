using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wings : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectClip;
    [SerializeField]
    private GameObject featherPrefab;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ActiveWings()
    {
        anim.SetTrigger("Active");
        audioSource.PlayOneShot(effectClip);
        StartCoroutine(CreateFeather());
    }
    IEnumerator CreateFeather()
    {
        int direction = transform.parent.localScale.x > 0 ? 1 : -1;
        GameObject feather = Instantiate(featherPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 90 + 90 * direction)));
        float animTime = 0;
        Vector2 originalScale = feather.transform.localScale;
        Vector2 finalScale = new Vector2(originalScale.x * 1.5f, originalScale.y * 1.5f);

        Quaternion originalRotation = feather.transform.localRotation;
        Quaternion finalRotation = Quaternion.Euler(new Vector3(0, 0, 90 + 50 * direction));
        while (animTime <= 0.5f)
        {
            animTime += Time.deltaTime;
            feather.transform.localScale = Vector2.Lerp(originalScale, finalScale, animTime / 0.5f);
            feather.transform.localRotation = Quaternion.Lerp(originalRotation, finalRotation, animTime / 0.5f);

            yield return null;
        }
        

        yield return new WaitForSeconds(0.2f);
        Destroy(feather);
    }
}
