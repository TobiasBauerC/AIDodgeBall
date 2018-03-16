using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance = null;

    [SerializeField] private List<AIAgent> _blueAgents = new List<AIAgent>();
    [SerializeField] private List<AIAgent> _redAgents = new List<AIAgent>();
    [SerializeField] private Button _startButton;
    [SerializeField] private Text _winText;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _redScoreText;
    [SerializeField] private Text _blueScoreText;

    private static string BLUE_WIN_MESSAGE = "BLUE WINS!";
    private static string RED_WIN_MESSAGE = "RED WINS!";

    private bool _gameOver = false;

    private int _blueScore = 0;
    private int _redScore = 0;

    void Start () 
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Physics.IgnoreLayerCollision(8, 9);
        _blueScore = 0;
        _redScore = 0;
        UpdateScoreText();
	}

	private void Update()
	{
        if (_gameOver)
            return;
        UpdateAgentLists();
        CheckGameOver();
	}

    private void UpdateScoreText()
    {
        _redScoreText.text = string.Format("Score: {0}", _redScore);
        _blueScoreText.text = string.Format("Score: {0}", _blueScore);
    }

	private void UpdateAgentLists()
	{
        if (_blueAgents.Contains(null) || _redAgents.Contains(null))
        {
            int nullIndex = -1;

            for (int i = 0; i < _blueAgents.Count; i++)
            {
                if (_blueAgents[i] == null)
                    nullIndex = i;
            }

            if (nullIndex > -1)
            {
                _blueAgents.Remove(_blueAgents[nullIndex]);
                AddScore(AIAgent.Team.red, 1);
            }

            nullIndex = -1;

            for (int i = 0; i < _redAgents.Count; i++)
            {
                if (_redAgents[i] == null)
                    nullIndex = i;
            }

            if (nullIndex > -1)
            {
                _redAgents.Remove(_redAgents[nullIndex]);
                AddScore(AIAgent.Team.blue, 1);
            }
        }
	}

    private void CheckGameOver()
    {
        if(_blueAgents.Count <= 0)
        {
            _winText.text = RED_WIN_MESSAGE;
            _winText.color = Color.red;

            foreach (AIAgent agent in _redAgents)
            {
                agent.ActivateAgent();
            }

            _panel.SetActive(true);
            _gameOver = true;
        }
        else if(_redAgents.Count <= 0)
        {
            _winText.text = BLUE_WIN_MESSAGE;
            _winText.color = Color.blue;

            foreach (AIAgent agent in _blueAgents)
            {
                agent.ActivateAgent();
            }

            _panel.SetActive(true);
            _gameOver = true;
        }
    }

	public void StartAgents()
    {
        foreach (AIAgent agent in _blueAgents)
        {
            agent.ActivateAgent();
        }

        foreach (AIAgent agent in _redAgents)
        {
            agent.ActivateAgent();
        }

        _startButton.GetComponent<Image>().enabled = false;
        _startButton.GetComponent<Button>().enabled = false;
        _startButton.GetComponentInChildren<Text>().enabled = false;
    }

    public void AddScore(AIAgent.Team team, int value)
    {
        if (team == AIAgent.Team.blue)
            _blueScore += value;
        else
            _redScore += value;

        UpdateScoreText();
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
