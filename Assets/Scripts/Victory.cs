
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victoy : MonoBehaviour
{
    public bool hasCollided = false;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("EndScreen");
            hasCollided = true;
        }
    }
      
}
