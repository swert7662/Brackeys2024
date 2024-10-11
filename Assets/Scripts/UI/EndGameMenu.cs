using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    public GameObject spacerPanel;
    public GameObject endGamePanel;

    public RawImage[] endGameScreens;

    private int index = 0;

    public void ShowEndGamePanel()
    {
        spacerPanel.SetActive(false);
        endGamePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetEndGameScreen(RenderTexture renderTexture)
    {
        endGameScreens[index].texture = renderTexture;
        index++;
    }
}
