using UnityEngine;
using UnityEngine.UI;

public class ActivateText : MonoBehaviour
{
    public Texture blankCamTexture; // Assign "BlankCamTexture" in the Inspector
    private RawImage rawImage;
    private GameObject textChild;

    private void Awake()
    {
        // Get the RawImage component attached to this GameObject
        rawImage = GetComponent<RawImage>();

        if (rawImage == null)
        {
            Debug.LogError("RawImage component missing on this GameObject.");
            return;
        }

        // Assuming the child text object is the only child or the first child
        textChild = transform.GetChild(0).gameObject;

        if (textChild == null)
        {
            Debug.LogError("No child object found to activate.");
            return;
        }

        // Check if the texture is equal to BlankCamTexture
        if (rawImage.texture == blankCamTexture)
        {
            textChild.SetActive(true); // Activate the text child object
        }
        else
        {
            textChild.SetActive(false); // Ensure it's inactive if the condition isn't met
        }
    }
}
