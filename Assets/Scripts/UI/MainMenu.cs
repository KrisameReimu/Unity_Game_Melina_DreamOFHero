using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string newgameSceneName = "TestScene1";
    [SerializeField]
    private GameObject continueBtn;
    [SerializeField]
    private SceneController sceneController;
    [SerializeField]
    private GameObject confirmWindow;

    private void Awake()
    {
        
        if(PlayerController.playerInstance)//player exist
            Destroy(PlayerController.playerInstance.gameObject);
        if(UIStatusBar.instance)
            Destroy(UIStatusBar.instance.transform.root.gameObject);
        



        if (!File.Exists(GetFilePath()))
        {
            continueBtn.SetActive(false);
        }
        
    }
    private string GetFilePath()
    {
        return Application.persistentDataPath + "/group4.sdgame";

    }
    public void NewGame()
    {
        if (!File.Exists(GetFilePath()))
        {
            ConfirmStartNewGame();
        }
        else
            ShowConfirmWindow();
    }

    public void ConfirmStartNewGame()
    {
        //Debug.Log("new game");
        File.Delete(GetFilePath());
        //"Tutorial - The Memory-laden Enchanted Grove"
        //SceneManager.LoadScene(newgameSceneName);
        sceneController = SceneController.Instance;
        sceneController.LoadNextScene(newgameSceneName, Vector2.zero);
    }

    public void ContinueGame()
    {
        if (File.Exists(GetFilePath()))
        {
            sceneController = SceneController.Instance;
            sceneController.LoadGame();
        }
        else
        {
            Debug.Log("Unexpected error");
        }        
    }

    public void Arena() //Boss Run
    {
        sceneController = SceneController.Instance;
        sceneController.LoadNextScene("BossRoom", Vector2.zero);
    }

    public void CloseWindow()
    {
        confirmWindow.SetActive(false);
    }

    private void ShowConfirmWindow()//if have existing record
    {
        confirmWindow.SetActive(true);
    }
}
