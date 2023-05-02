using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Text moneyText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Text upgradeCost;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private Sprite[] upgradeSprites;
    [SerializeField] private Button nailUpgradeButton;
    [SerializeField] private Text nailUpgradeCost;
    [SerializeField] private Image nailUpgradeImage;
    [SerializeField] private Sprite[] nailUpgradeSprites;
    private int _upgrade;
    private int _nailUpgrade;
    private int _money;
    private int _upgradeLevel;
    private int _nailUpgradeLevel;
    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
        nailUpgradeButton.onClick.AddListener(OnNailUpgradeButtonClick);
        _upgrade = PlayerPrefs.GetInt(PlayerPrefsStrings.UpgradeCost, 1000);
        _nailUpgrade = PlayerPrefs.GetInt(PlayerPrefsStrings.NailUpgradeCost, 1000);
        upgradeCost.text = _upgrade.ToString();
        nailUpgradeCost.text = _nailUpgrade.ToString();
        _money = PlayerPrefs.GetInt(PlayerPrefsStrings.MoneyTotal, 0);
        _upgradeLevel = PlayerPrefs.GetInt(PlayerPrefsStrings.UpgradeLevel, 0);
        _nailUpgradeLevel = PlayerPrefs.GetInt(PlayerPrefsStrings.NailUpgradeLevel, 0);
        SetUpgradeImage(_upgradeLevel);
        SetNailUpgradeImage(_nailUpgradeLevel);
        moneyText.text = _money.ToString();
    }

    private void SetNailUpgradeImage(int nailUpgradeLevel)
    {
        if (nailUpgradeLevel < nailUpgradeSprites.Length)
        {
            nailUpgradeImage.sprite = nailUpgradeSprites[nailUpgradeLevel];
        }
        else
        {
            nailUpgradeImage.sprite = nailUpgradeSprites[nailUpgradeSprites.Length-1];
        }
    }

    private void OnUpgradeButtonClick()
    {
        if (_money > _upgrade)
        {
            _upgradeLevel++;
            PlayerPrefs.SetInt(PlayerPrefsStrings.UpgradeLevel, _upgradeLevel);
            SetUpgradeImage(_upgradeLevel);
            _money -= _upgrade;
            moneyText.text = _money.ToString();
            PlayerPrefs.SetInt(PlayerPrefsStrings.MoneyTotal, _money);
            _upgrade = 1000 + 2000 * _upgradeLevel;
            upgradeCost.text = _upgrade.ToString();
            PlayerPrefs.SetInt(PlayerPrefsStrings.UpgradeCost, _upgrade);
            var evt = GameEventsHandler.PlayerHandUpgradeEvent;
            evt.UpgradeLevel = _upgradeLevel;
            EventManager.Broadcast(evt);
            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(VibrationCor(0.15f));
        }
    }

    private void OnNailUpgradeButtonClick()
    {
        if (_money > _nailUpgrade)
        {
            _nailUpgradeLevel++;
            PlayerPrefs.SetInt(PlayerPrefsStrings.NailUpgradeLevel, _nailUpgradeLevel);
            SetNailUpgradeImage(_nailUpgradeLevel);
            _money -= _nailUpgrade;
            moneyText.text = _money.ToString();
            PlayerPrefs.SetInt(PlayerPrefsStrings.MoneyTotal, _money);
            _nailUpgrade = 1000 + 2000 * _nailUpgradeLevel;
            nailUpgradeCost.text = _nailUpgrade.ToString();
            PlayerPrefs.SetInt(PlayerPrefsStrings.NailUpgradeCost, _nailUpgrade);
            var evt = GameEventsHandler.PlayerNailUpgradeEvent;
            evt.UpgradeLevel = _nailUpgradeLevel;
            EventManager.Broadcast(evt);
            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(VibrationCor(0.15f));
        }
    }

    private void SetUpgradeImage(int level)
    {
        if (level < upgradeSprites.Length)
        {
            upgradeImage.sprite = upgradeSprites[level];
        }
        else
        {
            upgradeImage.sprite = upgradeSprites[upgradeSprites.Length - 1];
        }
    }
    private void OnStartButtonClick()
    {
        EventManager.Broadcast(GameEventsHandler.GameStartEvent);
    }

    private IEnumerator VibrationCor(float delay)
    {
        Taptic.Heavy();
        yield return new WaitForSeconds(delay);
        Taptic.Heavy();
    }
}