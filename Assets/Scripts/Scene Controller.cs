using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    // Start is called before the first frame update
   public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quitting game");
    }
}
