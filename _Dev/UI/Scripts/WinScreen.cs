using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
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
        StartCoroutine(MoneyCounter(1f));
    }

    private void OnNextButtonClick()
    {
        SceneLoader.LoadNextLevel();
    }

    private IEnumerator MoneyCounter(float time)
    {
        int startMoney = VarSaver.MoneyCollected;
        int endMoney = (int) (startMoney * VarSaver.Multiplier);
        PlayerPrefs.SetInt(PlayerPrefsStrings.MoneyTotal,
            PlayerPrefs.GetInt(PlayerPrefsStrings.MoneyTotal, 0) + endMoney);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(1f);
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            moneyCollectedText.text = ((int) Mathf.Lerp(startMoney, endMoney, t / time)).ToString();
            yield return null;
        }

        moneyCollectedText.text = endMoney.ToString();
    }
}