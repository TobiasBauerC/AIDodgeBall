using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour 
{
    [SerializeField] private GameObject _panel;

    public void OnAIButton(int aiLevelIndex)
    {
        SceneManager.instance.GoToLevel(aiLevelIndex);
    }

    public void OnPhysicsButton(int physicsLevelIndex)
    {
        SceneManager.instance.GoToLevel(physicsLevelIndex);
    }

    public void OnControllsButton()
    {
        _panel.SetActive(!_panel.activeInHierarchy);
    }
}
