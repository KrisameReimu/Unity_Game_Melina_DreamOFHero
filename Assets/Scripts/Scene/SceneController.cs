using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField] Animator transitionAnim;
    private PlayerController player;

    
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        Instance = this;
        transform.Find("CrossFade").gameObject.SetActive(true);

        StartCoroutine(LoadSceneData());
    }

    public void LoadNextScene(int sceneIndex, Vector2 position) 
    {
        Debug.Log("Save");
        SaveSceneData(position);
        StartCoroutine(LoadLevel(sceneIndex, position));
    }

    IEnumerator LoadLevel(int sceneIndex, Vector2 position)
    {
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }

    private void SaveSceneData(Vector2 position) 
    {
        PlayerPrefs.SetFloat("HP", player.HP);
        PlayerPrefs.SetFloat("SP", player.SP);
        PlayerPrefs.SetFloat("EX", player.EX);

        PlayerPrefs.SetFloat("pos_x", position.x);
        PlayerPrefs.SetFloat("pos_y", position.y);

        PlayerPrefs.SetInt("SaveKey", 114514);
    }

    IEnumerator LoadSceneData() 
    {

        if (PlayerPrefs.GetInt("SaveKey")!=114514)
        {
            PlayerPrefs.DeleteAll();
            //return;
        }
        else
        {
            yield return null;
            Debug.Log("Load");


            float HP = PlayerPrefs.GetFloat("HP");
            float SP = PlayerPrefs.GetFloat("SP");
            float EX = PlayerPrefs.GetFloat("EX");

            Vector2 newPosition = new Vector2(PlayerPrefs.GetFloat("pos_x"), PlayerPrefs.GetFloat("pos_y"));
            //Debug.Log(newPosition);
            player.InitPlayerData(HP, SP, EX, newPosition);

            PlayerPrefs.DeleteAll();
        }
    }
}
