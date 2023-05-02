using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class JewelryPriceList
{
    [Serializable]
    public class Pair
    {
        public JewelryType type;
        public int price;
    }

    public Pair[] list;
}

public class Overlay : MonoBehaviour
{
    [SerializeField] private Text moneyCounter;
    private int _moneyCount;

    [SerializeField] private JewelryPriceList priceList;
    private Dictionary<JewelryType, int> _dictionaryPriceList;

    [SerializeField] private Button restartButton;

    [SerializeField] private Slider progressBarSlider;
    private float _progress;
    private int _levelLength;

    [SerializeField] private Text levelText;

    private void Awake()
    {
        _dictionaryPriceList = new Dictionary<JewelryType, int>();
        _levelLength = VarSaver.LevelLength - 1;
        foreach (var pair in priceList.list)
        {
            _dictionaryPriceList.Add(pair.type, pair.price);
        }

        levelText.text = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1) + " lvl";
        EventManager.AddListener<ItemCollectEvent>(OnItemCollect);
        EventManager.AddListener<ItemDiscardEvent>(OnItemDiscard);
        EventManager.AddListener<PlayerProgressEvent>(OnPlayerProgress);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    private void OnRestartButtonClick()
    {
        SceneLoader.ReloadLevel();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<ItemCollectEvent>(OnItemCollect);
        EventManager.RemoveListener<ItemDiscardEvent>(OnItemDiscard);
        EventManager.RemoveListener<PlayerProgressEvent>(OnPlayerProgress);

        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
    }

    private void OnGameOver(GameOverEvent obj)
    {
        VarSaver.MoneyCollected = _moneyCount;
    }

    private void OnPlayerProgress(PlayerProgressEvent obj)
    {
        _progress += 1f / _levelLength;
    }

    private void OnItemDiscard(ItemDiscardEvent obj)
    {
        _moneyCount -= _dictionaryPriceList[obj.Type];
        moneyCounter.text = _moneyCount.ToString();
    }

    private void OnItemCollect(ItemCollectEvent obj)
    {
        _moneyCount += _dictionaryPriceList[obj.Type];
        moneyCounter.text = _moneyCount.ToString();
    }

    private void Update()
    {
        progressBarSlider.value = Mathf.Lerp(progressBarSlider.value, _progress, Time.deltaTime);
    }
}