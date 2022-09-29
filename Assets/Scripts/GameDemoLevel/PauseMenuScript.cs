using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gamePaused)
                ResumeGame();
            else
                PauseGame();

        }
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gamePaused = false;
    }

    void PauseGame() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        gamePaused = true;
    }

    public void ReturnToTitle() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
