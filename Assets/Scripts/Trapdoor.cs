using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapdoorTilemap : MonoBehaviour
{
    [SerializeField] private float activationTime = 2f; // Time before the trapdoor drops
    [SerializeField] private float reactivationTime = 3f; // Time before trapdoor resets
    private float timer = 0f;
    private bool isPlayerOnTrapdoor = false;

    private CompositeCollider2D compositeCollider; // Composite Collider for the trapdoor tilemap
    private Tilemap tilemap; // Reference to the tilemap for potential visual feedback

    private void Start()
    {
        // Get references to necessary components
        compositeCollider = GetComponent<CompositeCollider2D>();
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        // Start timing if the player is on the trapdoor
        if (isPlayerOnTrapdoor)
        {
            timer += Time.deltaTime;
            if (timer >= activationTime)
            {
                DropTrapdoor();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect if the player is in contact with the trapdoor
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOnTrapdoor = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset the timer when the player leaves the trapdoor
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOnTrapdoor = false;
            timer = 0f;
        }
    }

    private void DropTrapdoor()
    {
        // Disable the composite collider to make the trapdoor "drop"
        compositeCollider.isTrigger = true;
        isPlayerOnTrapdoor = false;
        timer = 0f;

        // Optional: Change the tilemap's appearance for feedback
        if (tilemap != null)
        {
            tilemap.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent
        }

        // Re-enable the trapdoor after a delay
        Invoke(nameof(ResetTrapdoor), reactivationTime);
    }

    private void ResetTrapdoor()
    {
        // Re-enable the composite collider
        compositeCollider.isTrigger = false;

        // Restore tilemap's appearance
        if (tilemap != null)
        {
            tilemap.color = new Color(1f, 1f, 1f, 1f); // Fully opaque
        }
    }
}