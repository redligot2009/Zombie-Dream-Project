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
            if (music.clip != deathTheme)
            {
                music.clip = deathTheme;
                music.loop = false;
                music.Play();
            }
            ShowDeadMenu();
        }
        else
        {
            if(music.clip != bgTheme)
            {
                music.clip = bgTheme;
                music.loop = true;
                music.Play();
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
            if (Input.GetKey(KeyCode.M))
            {
                LoadMenu();
            }
        }

        if (GamePaused) music.Pause();
        else
        {
            if(!music.isPlaying)
                music.Play();
        }
    }
}
