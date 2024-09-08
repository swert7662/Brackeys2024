using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask; // Ensure this is set for ground layers.
    private bool isGrounded;

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private bool CheckGrounded()
    {
        Vector3[] origins = new Vector3[]
        {
            transform.position,
            transform.position + new Vector3(0.2f, 0, 0),
            transform.position + new Vector3(-0.2f, 0, 0),
            transform.position + new Vector3(0, 0, 0.2f),
            transform.position + new Vector3(0, 0, -0.2f)
        };

        isGrounded = false; // Reset isGrounded

        foreach (Vector3 origin in origins)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, Vector3.down, out hit, 0.2f, groundMask))
            {
                // Later on we can add more checks here, such as checking the angle of the surface we hit, or the type of surface we hit.
                isGrounded = true;
                break; // If grounded, no need to check further
            }
        }

        return isGrounded;
    }

    private void Update()
    {
        CheckGrounded();
    }
}
