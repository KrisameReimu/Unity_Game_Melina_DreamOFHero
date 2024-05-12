using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausingMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField] 
    private Image pauseBtnImage;
    [SerializeField]
    private Sprite pauseImage;
    [SerializeField]
    private Sprite resumeImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI menuText;
  
    
    private bool isPausing = false;

    public void OnPauseButtonClicked()
    {
        if (isPausing)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        menuText.text = "Pausing";

        Time.timeScale = 0f;
        PlayerController.SetIsGamePause(true);
        pauseMenu.SetActive(true);
        pauseBtnImage.sprite = resumeImage;
        isPausing = true;
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        PlayerController.SetIsGamePause(false);
        pauseMenu.SetActive(false);
        pauseBtnImage.sprite = pauseImage;
        isPausing = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseButtonClicked();
        }
    }

    public void SavePlayer()
    {
        PlayerController player = PlayerController.GetPlayerInstance();
        SaveSystem.SavePlayer(player);

        menuText.text = "Saved";
    }

    public void BackToMenu()//Exit Btn
    {
        SceneManager.LoadScene("MainMenu");
    }
}
