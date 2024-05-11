using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField] 
    Animator transitionAnim;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private GameObject gameoverWindow;
    public static bool isSceneChanging { get; private set; } = false;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
            transform.Find("CrossFade").gameObject.SetActive(true);
        }
        else
        {
            Destroy(transform.root.gameObject);
        }

        if (SceneManager.GetActiveScene().name!="MainMenu")
            player = PlayerController.GetPlayerInstance();

        //StartCoroutine(LoadSceneData());
    }

    public void LoadNextScene(string sceneName, Vector2 position) //pass Vector2.zero to skip changing player position
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

        if (position != Vector2.zero)
        {
            player = PlayerController.GetPlayerInstance();
            player.MoveToNewPosition(position);
        }
        
        isSceneChanging = false;
    }

    public void SavePlayer() 
    {
        SaveSystem.SavePlayer(player);
    }
    
    public void LoadGame() //Continue
    {
        if (!isSceneChanging)
        {
            isSceneChanging = true;
            StartCoroutine(PrepareLoadGame());
        }
    }

    IEnumerator PrepareLoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();


        var asyncLoad = SceneManager.LoadSceneAsync("PreparationScene");
        transitionAnim.SetTrigger("Start");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        transitionAnim.SetTrigger("End");


        asyncLoad = SceneManager.LoadSceneAsync(data.currentSceneName);
        transitionAnim.SetTrigger("Start");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        transitionAnim.SetTrigger("End");


        LoadPlayer(data);
    }

    public void LoadPlayer(PlayerData data)
    {
        player = PlayerController.GetPlayerInstance();

        //load data and set
        //PlayerData data = SaveSystem.LoadPlayer();

        player.SetHP(data.HP);
        player.SetSP(data.SP);
        player.SetEx(data.EX);
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        player.MoveToNewPosition(position);

        InventoryController inventoryController = player.GetComponent<InventoryController>();
        inventoryController.LoadInventoryData(data.inventoryData);

        AgentCard ac = player.GetComponent<AgentCard>();
        ac.LoadAndEquipAllCards(data.equipingCardsIndex);

        player.UnlockDoubleJump(data.ableToDoubleJump);

        isSceneChanging = false;


        Debug.Log("Data Loaded");
    }


    public void GameOver()
    {
        gameoverWindow.SetActive(true);
    }

    public void RetryStage()
    {
        gameoverWindow.SetActive(false);
        if (PlayerController.playerInstance)//player exist
            Destroy(PlayerController.playerInstance.gameObject);
        if (UIStatusBar.instance)
            Destroy(UIStatusBar.instance.transform.root.gameObject);

        transitionAnim.SetTrigger("Start");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        transitionAnim.SetTrigger("End");

    }

    public void BackToMainMenu()
    {
        gameoverWindow.SetActive(false);
        transitionAnim.SetTrigger("Start");
        SceneManager.LoadScene("MainMenu");
        transitionAnim.SetTrigger("End");
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
