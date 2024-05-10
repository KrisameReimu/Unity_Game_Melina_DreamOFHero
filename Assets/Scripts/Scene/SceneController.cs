using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField] 
    Animator transitionAnim;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private bool isSceneChanging = false;
    
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

        player = PlayerController.GetPlayerInstance();

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

    public void SavePlayer() 
    {
        SaveSystem.SavePlayer(player);
    }

    public void LoadPlayer()
    {
        player = PlayerController.GetPlayerInstance();

        //load data and set
        PlayerData data = SaveSystem.LoadPlayer();

        player.SetHP(data.HP);
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


        Debug.Log("Data Loaded");
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
