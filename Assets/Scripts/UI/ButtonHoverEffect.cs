using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image buttonImage;
    public Color hoverColor;
    public Color selectColor; // Color to transition to on click
    public float lerpSpeed = 5f; // Speed of the color and alpha transition

    private Color normalColor;
    private bool isHovering = false;
    private bool isClicked = false;

    void Start()
    {
        // Get the button's image component
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogWarning("ButtonHoverEffect script requires an Image component.");
            return;
        }

        // Store the initial color (normal color and alpha)
        normalColor = buttonImage.color;
    }

    void Update()
    {
        if (buttonImage != null)
        {
            // Determine target color based on hover and click state
            Color targetColor;
            if (isClicked)
            {
                targetColor = selectColor;
            }
            else if (isHovering)
            {
                targetColor = hoverColor;
            }
            else
            {
                targetColor = normalColor;
            }

            // Lerp towards the target color
            LerpColor(targetColor);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Start hover effect
        if (!isClicked) // Only hover if not clicked
        {
            isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // End hover effect
        if (!isClicked) // Only stop hover if not clicked
        {
            isHovering = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start click effect
        isClicked = true;
        SetColor(selectColor); // Instantly change to selectColor
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // End click effect
        isClicked = false;
        if (!isHovering) // Revert to normal if not hovering
        {
            SetColor(normalColor);
        }
    }

    private void LerpColor(Color targetColor)
    {
        // Lerp the color and alpha
        Color currentColor = buttonImage.color;
        Color lerpedColor = Color.Lerp(currentColor, targetColor, Time.unscaledDeltaTime * lerpSpeed);
        buttonImage.color = lerpedColor;
    }

    private void SetColor(Color color)
    {
        buttonImage.color = color;
    }
}
