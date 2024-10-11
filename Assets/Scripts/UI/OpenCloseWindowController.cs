using System.Collections;
using UnityEngine;

public class OpenCloseWindowController : MonoBehaviour
{
    [SerializeField] private float delayBetweenOpens = 0.5f;

    private void Start()
    {
        StartCoroutine(CycleThroughWindows());
    }

    private IEnumerator CycleThroughWindows()
    {
        // Get all child objects with the OpenCloseWindow component
        OpenCloseWindow[] windows = GetComponentsInChildren<OpenCloseWindow>();

        foreach (OpenCloseWindow window in windows)
        {
            // Open each window with the ToggleOpenClose method
            Debug.Log($"Opening {window.name}");
            window.OpenWindow();
            yield return new WaitForSeconds(delayBetweenOpens);
        }
    }
}
