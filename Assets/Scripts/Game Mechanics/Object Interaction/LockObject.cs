using UnityEngine;

public class LockObject : MonoBehaviour
{
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Unlock(string keyName)
    {
        // Print the debug message indicating the lock was unlocked
        Debug.Log($"(Target collided with {keyName}) unlocked.");
        rb.useGravity = true;
        // You can add additional logic here for what happens when the lock is unlocked
    }
}
