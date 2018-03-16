using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour 
{
    public static SceneManager instance;

	private void Awake()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
	}

    /// <summary>
    /// Goes to level.
    /// </summary>
    public void GoToLevel(int levelIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }

    /// <summary>
    /// Goes to main menu.
    /// </summary>
    public void GoToMainMenu(int menuIndex = 0)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuIndex);
    }

    public void ReloadSameScene()
    {
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
