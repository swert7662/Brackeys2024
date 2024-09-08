using UnityEngine;

public class KeyObject : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object the key collided with has the LockObject component
        LockObject lockObject = collision.gameObject.GetComponent<LockObject>();
        if (lockObject != null)
        {
            // Call the Unlock method on the lock object
            lockObject.Unlock(gameObject.name);
            Destroy(gameObject);            
        }
    }
}
