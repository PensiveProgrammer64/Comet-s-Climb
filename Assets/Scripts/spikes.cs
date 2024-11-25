using UnityEngine;

public class Spikes : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.collider.GetComponent<PlayerController>()) 
        {

            collision.collider.GetComponent<PlayerController>().Die();
            Debug.Log("Dead");
        }

    }

}
