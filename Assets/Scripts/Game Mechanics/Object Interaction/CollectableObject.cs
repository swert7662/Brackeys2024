using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] private string pickupSound;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Play the pickup sound at the object's position
            if (pickupSound != null)
            {
                AudioManager.Instance.PlaySFX(pickupSound);
            }

            // Log that the item was picked up
            Debug.Log("Banana Picked Up");

            // Destroy the collectable object
            Destroy(gameObject);
        }
    }
}
