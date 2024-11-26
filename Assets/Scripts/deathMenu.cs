using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathMenu : MonoBehaviour
{
     public GameObject DeadMenu; 
     private PlayerController player;
    public static bool isPaused;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        DeadMenu.SetActive(false);
    }

    void Update()
    {
        if(player.Dead)
        {
            if(isPaused)
            {
                ResumeGame();
            }else
            {
                PauseGame();
            }
        }
    }
   
    public void PauseGame()
    {
        DeadMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        DeadMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("Level");
        Debug.Log("Loading level");
    }
   
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("loading menu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quitting game");
    }
}
