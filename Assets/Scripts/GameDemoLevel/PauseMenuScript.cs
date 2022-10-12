using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;

    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && slider.value > 0) {
            if (gamePaused)
                ResumeGame();
            else
                PauseGame();
        }
        if (slider.value == 0) {
            GameOver();
        }
    }

    public void GameOver() {
        gameOverMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gamePaused = true;
        Time.timeScale = 0.0f;
    }

    public void RetryGame() {
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        gamePaused = false;
    }

    void PauseGame() {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
        gamePaused = true;
    }

    public void ReturnToTitle() {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }
}
