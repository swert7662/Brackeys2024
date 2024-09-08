using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    [SerializeField] private float followLerpSpeed = 10f;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            // Lerp the position of the object to the target position
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, followLerpSpeed * Time.fixedDeltaTime);
            objectRigidbody.MovePosition(newPosition);
        }
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
    }
}
