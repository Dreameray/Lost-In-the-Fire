using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    [Tooltip("How many items have we picked up so far?")]
    public int itemsCollected = 0;

    [Tooltip("How many items the player needs to collect for the quest.")]
    public int requiredItems = 3;

    // Make sure your Player GameObject has:
    //  • a Rigidbody2D (Dynamic or Kinematic)
    //  • a Collider2D (non‑trigger)
    //  • this script attached

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // “Item” must match the Tag you gave your prefab
        if (collision.CompareTag("Item"))
        {
            itemsCollected++;
            Debug.Log($"Picked up item {itemsCollected}/{requiredItems}");

            // remove the item from the world
            Destroy(collision.gameObject);

            // (Optional) check for quest completion
            if (itemsCollected >= requiredItems)
            {
                Debug.Log("🎉 Quest complete!");
                // TODO: fire your quest‑complete logic here
            }
        }
    }
}
