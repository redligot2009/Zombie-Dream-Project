using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static bool GamePaused = false, isDead = false;

    public GameObject pauseMenuUI, deadMenuUI;

    void Start ()
    {
        Resume();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void ShowDeadMenu()
    {
        deadMenuUI.SetActive(true);
        Time.timeScale = 0.5f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if(isDead)
        {
            ShowDeadMenu();
        }
        if(GamePaused || isDead)
        {
            if (Input.GetKey(KeyCode.R))
            {
                Restart();
            }
        }
    }
}
