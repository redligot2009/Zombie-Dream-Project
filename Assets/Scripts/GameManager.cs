using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static bool GamePaused = false;
    public static bool isDead = false;
    public static WeaponObject currentWeapon;

    public GameObject pauseMenuUI, deadMenuUI;

    PlayerControls player;

    void Start ()
    {
        GamePaused = false;
        isDead = false;
        Time.timeScale = 1f;
        player = GameObject.FindObjectOfType<PlayerControls>();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GamePaused = true;
        Time.timeScale = 0f;
    }

    public void ShowDeadMenu()
    {
        deadMenuUI.SetActive(true);
        Time.timeScale = 0.5f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        GamePaused = false;
        isDead = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isDead = false;
        GamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
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
        else
        {
            if(player.currentWeapon != currentWeapon)
                player.currentWeapon = currentWeapon;
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
