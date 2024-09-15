using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSInteract : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private float pickUpRange = 5f;
    [SerializeField] private Image reticleImage;
    [SerializeField] private Color highlightReticleColor = Color.red;

    private GrabbableObject currentGrabbableObject;
    private Vector2 defaultReticleSize = new Vector2(5f, 5f); 
    private Vector2 highlightReticleSize = new Vector2(20f, 20f); 
    private Color defaultReticleColor;
    private float sizeLerpSpeed = 10f;

    private void Start()
    {
        defaultReticleColor = reticleImage.color; // Store the default reticle color
    }
    private void Update()
    {
        HandleRaycast();
    }

    public void OnInteractPressed()
    {
        // If we are already holding an object, drop it. Otherwise, check if we are looking at an object to pick up
        if (currentGrabbableObject == null)
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickUpRange, interactableLayers))
            {
                // Currently only Grabbable and Openable objects are supported, consider a switch statement if more types are added
                currentGrabbableObject = hit.transform.GetComponentInParent<GrabbableObject>();

                if (currentGrabbableObject != null)
                {
                    // Debug log that shows the name of the object we are holding
                    Debug.Log("Holding: " + currentGrabbableObject.name);
                    currentGrabbableObject.Grab(objectGrabPointTransform);
                }
                else if (hit.transform.GetComponentInParent<OpenableObject>() is OpenableObject openableObject)
                {
                    openableObject.Open();
                }
                else if (hit.transform.GetComponentInParent<ClickableObject>() is ClickableObject clickableObject) 
                {
                    clickableObject.OnPressed();
                }
            }
        }
        else
        {
            currentGrabbableObject.Drop();
            currentGrabbableObject = null;
        }

    }

    private void HandleRaycast()
    {
        // If we are already holding an object, we don't want to check for other objects
        if (currentGrabbableObject) { return;}

        // Check if we are looking at an interactable object and change the reticle color accordingly
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickUpRange, interactableLayers))
        {
            // Currently only Grabbable and Openable objects are supported, but if we add more types, we may want to rework how we check for them
            if (hit.transform.TryGetComponent(out GrabbableObject grabbableObject) || hit.transform.TryGetComponent(out OpenableObject openableObject) || hit.transform.TryGetComponent(out ClickableObject clickableObject) )
            {
                reticleImage.color = highlightReticleColor; // Change reticle color when hovering over a grabbable or openable object
                reticleImage.rectTransform.sizeDelta = Vector2.Lerp(reticleImage.rectTransform.sizeDelta, highlightReticleSize, Time.deltaTime * sizeLerpSpeed);
            }
            else
            {
                ResetReticle(); // Reset to default color when not hovering over a grabbable object
            }
        }
        else
        {
            ResetReticle(); // Reset to default color when not hitting anything

        }
    }

    private void ResetReticle()
    {
        reticleImage.color = defaultReticleColor; 
        reticleImage.rectTransform.sizeDelta = Vector2.Lerp(reticleImage.rectTransform.sizeDelta, defaultReticleSize, Time.deltaTime * sizeLerpSpeed);
    }
}
