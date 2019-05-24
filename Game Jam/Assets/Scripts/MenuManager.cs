using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManger : MonoBehaviour
{
    public static bool isGamePaused;
    public GameObject resumeCanvas;
    public GameObject mainCanvas;

    private void Awake()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause")) {
            if (isGamePaused == true)
            {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        resumeCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause() {
        resumeCanvas.SetActive(true);
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
