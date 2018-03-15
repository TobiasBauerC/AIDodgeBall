using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour 
{
    public void OnAIButton(int aiLevelIndex)
    {
        SceneManager.instance.GoToLevel(aiLevelIndex);
    }

    public void OnPhysicsButton(int physicsLevelIndex)
    {
        SceneManager.instance.GoToLevel(physicsLevelIndex);
    }
}
