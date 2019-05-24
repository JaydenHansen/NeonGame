using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public static bool isGamePaused;
    public GameObject resumeCanvas;
    public GameObject deathCanvas;
    public GameObject mainCanvas;
    public PlayerController player;

    private void Awake()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isGamePaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }


        if (player.currentHealth <= 0)
        {
            mainCanvas.SetActive(false);
            resumeCanvas.SetActive(false);
            deathCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Resume() {
        resumeCanvas.SetActive(false);
        deathCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause() {
        resumeCanvas.SetActive(true);
        deathCanvas.SetActive(false);
        mainCanvas.SetActive(false);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Quit() {
        SceneManager.LoadScene(0);
    }

    public void Restart() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

}
