using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGameManager : MonoBehaviour 
{
    public static PhysicsGameManager instance;

    [SerializeField] private GameObject _panel;

	private void Start()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}

    public void GameOver()
    {
        _panel.SetActive(true);
    }

	public void OnReplayButton()
    {
        SceneManager.instance.ReloadSameScene();
    }

    public void OnMenuButton(int menuIndex)
    {
        SceneManager.instance.GoToMainMenu(menuIndex);
    }
}
