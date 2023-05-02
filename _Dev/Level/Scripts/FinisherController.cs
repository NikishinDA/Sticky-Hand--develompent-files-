using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinisherController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] multipliers;
    private int _objectNum;
    private int _score;
    private float[] _multiplier = {2f, 3f,  4f, 5f};
    private Animator _animator;
    [SerializeField] private ParticleSystem appearEffect;
    [SerializeField]
    private float vibrationDelay = 0.5f;

    [SerializeField] private Slider multiplierProgressBar;
    private float _progress;
    private int _upgradeLevel;
    [SerializeField] private ParticleSystem coinRain;
    [SerializeField] private TMP_Text moneyText;
    private int _money;
    private bool _count = false;
    private void Awake()
    {
        EventManager.AddListener<FinisherJewelryDroppedEvent>(OnJewelryDropped);
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerInPosition);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.AddListener<PlayerFinishCrossedEvent>(OnFinishCrossed);
        _upgradeLevel = PlayerPrefs.GetInt(PlayerPrefsStrings.UpgradeLevel);
        _animator = GetComponent<Animator>();
        VarSaver.Multiplier = 1;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<FinisherJewelryDroppedEvent>(OnJewelryDropped);
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerInPosition);
        EventManager.RemoveListener<PlayerFinishCrossedEvent>(OnFinishCrossed);

        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);

    }

    private void OnFinishCrossed(PlayerFinishCrossedEvent obj)
    {
        
        coinRain.Play();
    }

    private void OnFinisherEnd(FinisherEndEvent obj)
    {
        _animator.SetTrigger("End");
        
        if (_objectNum == 0) return;
        multipliers[_objectNum-1].SetActive(true);
        appearEffect.Play();
        _money = VarSaver.MoneyCollected;
        moneyText.text = _money + " x " + (VarSaver.Multiplier);
        StartCoroutine(MoneyDisplayCor(VarSaver.MoneyCollected, (int) (VarSaver.MoneyCollected * VarSaver.Multiplier), 1f));
        StartCoroutine(EffectDelay(vibrationDelay));
    }

    private void OnPlayerInPosition(FinisherPlayerInPosition obj)
    {
        //_animator.SetTrigger("Start");
        _count = true;
        //StartCoroutine(MoneyDisplayCor(0, VarSaver.MoneyCollected, 0.5f));
        //StartCoroutine(MultiDisplayCor(1f));
    }

    private void OnJewelryDropped(FinisherJewelryDroppedEvent obj)
    {
        _animator.SetTrigger("Start");
        switch (obj.Type)
        {
            case JewelryType.RingSilver:
            case JewelryType.RingDiamond:
            case JewelryType.RingRuby:
            {
                _score += 1;
            }
                break;
            case JewelryType.BraceletSilver:
            case JewelryType.BraceletGold:
            case JewelryType.ChainGold:
            case JewelryType.ChainSilver:
            {
                _score += 8;
            }
                break;
            case JewelryType.ChainGoldBig:
            case JewelryType.ChainSilverBig:
            {
                _score += 20;
            }
                break;
        }

        if (_score / 20 > _objectNum && _objectNum < multipliers.Length && _objectNum < _upgradeLevel)
        {
           /* multipliers[_objectNum].SetActive(true);
           if (_objectNum - 1 >= 0)
            {
                //multipliers[_objectNum - 1].SetActive(false);
            }*/

            VarSaver.Multiplier = _multiplier[_objectNum];
            _objectNum++;
        }
    }

    private void Update()
    {
        if (_progress < ( VarSaver.Multiplier -1) / 4)
        {
            _progress += 0.7f * Time.deltaTime;
            multiplierProgressBar.value = _progress;
        }

        if (_count)
        {
            _money = (int) Mathf.Clamp(_money + 0.01f * VarSaver.MoneyCollected, 0, VarSaver.MoneyCollected);
            moneyText.text = _money + " x " + (VarSaver.Multiplier);
        }

    }

private IEnumerator MoneyDisplayCor(int start, int end, float time)
    {
        yield return new WaitForSeconds(1f);
        _count = false;
        int money = start;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            money = (int) Mathf.Lerp(0, end, t / time);
            moneyText.text = money.ToString();
            yield return null;
        }

        money = end;
        moneyText.text = money.ToString();
    }

    private IEnumerator EffectDelay(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        #if UNITY_EDITOR
        Debug.Log("VIBR");
        #endif
        Taptic.Heavy();
    }
}
