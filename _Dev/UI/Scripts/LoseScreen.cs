using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen: MonoBehaviour
{
    
    [SerializeField] private Button nextButton;
    [SerializeField] private Text moneyCollectedText;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
    }

    private void Start()
    {
        moneyCollectedText.text = VarSaver.MoneyCollected.ToString();
    }

    private void OnNextButtonClick()
    { 
        int startMoney = VarSaver.MoneyCollected;
        int endMoney = (int) (startMoney * VarSaver.Multiplier);
        PlayerPrefs.SetInt(PlayerPrefsStrings.MoneyTotal,
            PlayerPrefs.GetInt(PlayerPrefsStrings.MoneyTotal, 0) + endMoney);
        PlayerPrefs.Save();
        SceneLoader.ReloadLevel();
    }
}
