using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject finisherScreen;
    private int _level;
    private void Awake()
    {
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerInPosition);
        _level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerInPosition);

    }

    private void OnPlayerInPosition(FinisherPlayerInPosition obj)
    {
        finisherScreen.SetActive(true);
    }

    private void OnFinisherEnd(FinisherEndEvent obj)
    {
        overlay.SetActive(false);
        finisherScreen.SetActive(false);
        winScreen.SetActive(true);
    }

    private void OnGameOver(GameOverEvent obj)
    {
        if (!obj.IsWin)
        {
            overlay.SetActive(false);
            loseScreen.SetActive(true);
        }
    }

    private void OnGameStart(GameStartEvent obj)
    {
        startScreen.SetActive(false);
        overlay.SetActive(true);
        if (_level == 1)
        {
            tutorial.SetActive(true);
        }
    }

    private void Start()
    {
        startScreen.SetActive(true);
    }
}