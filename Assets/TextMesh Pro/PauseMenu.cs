using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{


    public static bool PausedNaAngLaro = false;

    public GameObject pauseMenuUI;
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausedNaAngLaro)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
		
	}
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        PausedNaAngLaro = false;
    }
     void Paused()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        PausedNaAngLaro = true;
    }
    public void Load()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    
}
