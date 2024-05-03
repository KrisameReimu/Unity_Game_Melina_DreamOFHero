using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField] Animator transitionAnim;
    private PlayerController player;
    [SerializeField]
    private bool isSceneChanging = false;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            DontDestroyOnLoad(transform.root.gameObject);
            transform.Find("CrossFade").gameObject.SetActive(true);
        }
        else
        {
            Destroy(transform.root.gameObject);
        }


        //StartCoroutine(LoadSceneData());
    }

    public void LoadNextScene(string sceneName, Vector2 position) 
    {
        //Debug.Log("Save");
        /*
        SaveSceneData(position);
        StartCoroutine(LoadLevel(sceneIndex, position));
        */
        if(!isSceneChanging) 
        {
            isSceneChanging = true;
            StartCoroutine(ChangeSceneTask(sceneName,position));
        }
    }

    /*
    IEnumerator LoadLevel(int sceneIndex, Vector2 position)
    {
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }
    */

    IEnumerator ChangeSceneTask(string sceneName, Vector2 position)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        //start loading animation
        transitionAnim.SetTrigger("Start");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        transitionAnim.SetTrigger("End");

        player.MoveToNewPosition(position);
        isSceneChanging = false;
    }

    /*
    private void SaveSceneData(Vector2 position) 
    {
        //PlayerPrefs.SetFloat("HP", player.HP);
        //PlayerPrefs.SetFloat("SP", player.SP);
        //PlayerPrefs.SetFloat("EX", player.EX);

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
            //Debug.Log("Load");


            //float HP = PlayerPrefs.GetFloat("HP");
            //float SP = PlayerPrefs.GetFloat("SP");
            //float EX = PlayerPrefs.GetFloat("EX");

            Vector2 newPosition = new Vector2(PlayerPrefs.GetFloat("pos_x"), PlayerPrefs.GetFloat("pos_y"));
            //Debug.Log(newPosition);
            //player.InitPlayerData(HP, SP, EX, newPosition);
            player.MoveToNewPosition(newPosition);

            PlayerPrefs.DeleteAll();
        }
    }
    */
}
