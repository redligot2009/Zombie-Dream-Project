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

    public AudioSource music;
    public AudioClip deathTheme;
    public AudioClip bgTheme;
    public bool playDead = true;

    void Start ()
    {
        music = transform.Find("musicSource").GetComponent<AudioSource>();
        GamePaused = false;
        isDead = false;
        Time.timeScale = 1f;
        player = FindObjectOfType<PlayerControls>();
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

    public static void HardReset()
    {
        isDead = false;
        GamePaused = false;
        currentWeapon = null;
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
            if (!playDead)
            {
                music.clip = deathTheme;
                music.loop = false;
                music.Play();
                playDead = true;
            }
            ShowDeadMenu();
        }
        else
        {
            if(playDead)
            {
                Debug.Log("YO");
                music.clip = bgTheme;
                music.loop = true;
                music.Play();
                playDead = false;
            }
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
