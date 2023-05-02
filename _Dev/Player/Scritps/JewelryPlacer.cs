using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

public enum JewelryType
{
    RingSilver,
    RingDiamond,
    RingRuby,
    BraceletSilver,
    ChainSilver,
    BraceletGold,
    ChainGold,
    ChainSilverBig,
    ChainGoldBig
}

public class JewelryPlacer : MonoBehaviour
{
    [Header("Rings discard")] [SerializeField]
    private int ringNumberToDiscard;

    [SerializeField] private float silverChance;
    [SerializeField] private float diamondChance;
    [SerializeField] private float rubyChance;

    [Header("Bracelets discard")] [SerializeField]
    private int braceletNumberToDiscard;

    [SerializeField] private float silverBraceletChance;
    [SerializeField] private float silverChainChance;
    [SerializeField] private float goldBraceletChance;
    [SerializeField] private float goldChainChance;
    
    private int _ringCount;
    private int _braceletCount;
    private int _bigChainCount;
    private Dictionary<JewelryType, int> _rings;
    private Dictionary<JewelryType, int> _bracelets;
    private JewelryType _bigChainType;
    [SerializeField] private JewelryView jewelryView;

    [SerializeField]
    private float handWidth;

    private float fingerWidth;

    private int _ringCap;
    [SerializeField] private ParticleSystem[] _silverRingEffects;
    [SerializeField] private ParticleSystem[] _goldRingEffects;
    [SerializeField] private ParticleSystem _goldWristEffect;
    [SerializeField] private ParticleSystem _silverWristEffect;
    [SerializeField] private ParticleSystem _goldBigChainEffect;
    [SerializeField] private ParticleSystem _silverBigChainEffect;
    private void Awake()
    {
        _rings = new Dictionary<JewelryType, int>();
        for (JewelryType type = JewelryType.RingSilver; type <= JewelryType.RingRuby; type++)
        {
            _rings.Add(type, 0);
        }
        _bracelets = new Dictionary<JewelryType, int>();
        for (JewelryType type = JewelryType.BraceletSilver; type <= JewelryType.ChainGold; type++)
        {
            _bracelets.Add(type, 0);
        }
        float chanceSum = silverChance + diamondChance + rubyChance;
        silverChance /= chanceSum;
        diamondChance /= chanceSum;
        rubyChance /= chanceSum;
        chanceSum = silverBraceletChance + silverChainChance + goldBraceletChance + goldChainChance;
        silverBraceletChance /= chanceSum;
        silverChainChance /= chanceSum;
        goldBraceletChance /= chanceSum;
        goldChainChance /= chanceSum;
        fingerWidth = handWidth / 5;
        EventManager.AddListener<ItemCollectEvent>(OnItemCollect);
        EventManager.AddListener<JewelryThrowEvent>(OnJewelryThrow);
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
        EventManager.AddListener<PlayerFinishCrossedEvent>(OnFinishCrossed);
        
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<PlayerHandShakeTick>(OnShakeTick);

    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<ItemCollectEvent>(OnItemCollect);
        EventManager.RemoveListener<JewelryThrowEvent>(OnJewelryThrow);
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
        EventManager.RemoveListener<PlayerFinishCrossedEvent>(OnFinishCrossed);
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<PlayerHandShakeTick>(OnShakeTick);

    }

    private void OnShakeTick(PlayerHandShakeTick obj)
    {
        jewelryView.ShakeJewelry();
    }

    private void OnGameStart(GameStartEvent obj)
    {
        
        int nailUpgrade = PlayerPrefs.GetInt(PlayerPrefsStrings.NailUpgradeLevel, 0);
        if (nailUpgrade < 5)
        {
            _ringCap = 5 * (5 - nailUpgrade);
        }
        else
        {
            _ringCap = 0;
        }
    }

    private void OnFinishCrossed(PlayerFinishCrossedEvent obj)
    {
        var evt = GameEventsHandler.GameOverEvent;
        evt.PlayerTransform = transform.root;
        evt.IsWin = !jewelryView.CheckEmpty();
        EventManager.Broadcast(evt);
    }

    private void OnJewelryThrow(JewelryThrowEvent obj)
    {
        if (_bigChainCount > 0)
        {
            DiscardBigChain();
        }
        else if (_braceletCount > 0)
        {
            DiscardBracelets();
        }
        else if (_ringCount > 0)
        {
            DiscardRings();
        }
    }

    private void OnPlayerOnPosition(FinisherPlayerInPosition obj)
    {
        jewelryView.StartDropping();
    }

    private void OnItemCollect(ItemCollectEvent obj)
    {
        if (obj.Type <= JewelryType.RingRuby)
        {
            EquipRing(obj.Type, obj.IsSilver, obj.RingOffset);
        }
        else if (obj.Type <= JewelryType.ChainGold)
        {
            EquipBracelet(obj.Type);
        }
        else if (obj.Type <= JewelryType.ChainGoldBig)
        {
            EquipBigBracelet(obj.Type);
        }
        else
        {
            throw new Exception("Can't equip. Wrong type.");
        }
        Taptic.Medium();
    }

    private void EquipRing(JewelryType type, bool isSilver, float offset)
    {
        int fingerNum;
        if (offset > 1.5f * fingerWidth)
        {
            fingerNum = 3;
        }
        else if (offset > 0.5f * fingerWidth)
        {
            fingerNum = 2;
        }
        else if (offset < -1.5f * fingerWidth)
        {
            fingerNum = 4;
        }
        else if (offset < -0.5f * fingerWidth)
        {
            fingerNum = 0;
        }
        else
        {
            fingerNum = 1;
        }
        if (_ringCount < 40 - _ringCap)
        {
            
            jewelryView.AddRing(type, isSilver, fingerNum);
            _rings[type]++;
            _ringCount++;
        }
        else
        {
            if (isSilver)
            {
                _silverRingEffects[fingerNum].Play();
            }
            else
            {
                _goldRingEffects[fingerNum].Play();
            }
        }
    }

    

    private void EquipBracelet(JewelryType type)
    {
        if (_braceletCount < 5)
        {
            jewelryView.AddBracelet(type);
            _bracelets[type]++;
            _braceletCount++;
        }
        else
        {
            switch (type)
            {
                case JewelryType.BraceletGold:
                    case JewelryType.ChainGold:
                    _goldWristEffect.Play();
                    break;
                case JewelryType.BraceletSilver:
                case JewelryType.ChainSilver:
                    _silverWristEffect.Play();
                    break;
            }
        }
    }

    private void EquipBigBracelet(JewelryType type)
    {
        if (_bigChainCount < 1)
        {
            jewelryView.AddBigBracelet(type);
            _bigChainType = type;
            _bigChainCount++;
        }
        else
        {
            switch (type)
            {
                case JewelryType.ChainGoldBig:
                    _goldBigChainEffect.Play();
                    break;
                case JewelryType.ChainSilverBig:
                    _silverBigChainEffect.Play();
                    break;
            }
        }
    }
    
    private void DiscardRings()
    {
        for (int j = 0; j < ringNumberToDiscard; j++)
        {
            List<float> collectiveProbability = new List<float>();
            float accumulatedProbability = 0;
            if (_rings[JewelryType.RingSilver] > 0)
            {
                accumulatedProbability += silverChance;
            }

            collectiveProbability.Add(accumulatedProbability);
            if (_rings[JewelryType.RingDiamond] > 0)
            {
                accumulatedProbability += diamondChance;
            }

            collectiveProbability.Add(accumulatedProbability);
            if (_rings[JewelryType.RingRuby] > 0)
            {
                accumulatedProbability += rubyChance;
            }

            collectiveProbability.Add(accumulatedProbability);

            float probability = Random.value * accumulatedProbability;
            for (int i = 0; i < collectiveProbability.Count; i++)
            {
                if (collectiveProbability[i] > probability)
                {
                    jewelryView.RemoveRing((JewelryType) (i));
                    _rings[(JewelryType) (i)]--;
                    _ringCount--;
                    BroadcastDiscardEvent((JewelryType) (i));
                    break;
                }
            }
        }
    }

    private void DiscardBracelets()
    {
        for (int j = 0; j < braceletNumberToDiscard; j++)
        {
            List<float> collectiveProbability = new List<float>();
            float accumulatedProbability = 0;
            if (_bracelets[JewelryType.BraceletSilver] > 0)
            {
                accumulatedProbability += silverBraceletChance;
            }

            collectiveProbability.Add(accumulatedProbability);
            if (_bracelets[JewelryType.ChainSilver] > 0)
            {
                accumulatedProbability += silverChainChance;
            }

            collectiveProbability.Add(accumulatedProbability);
            if (_bracelets[JewelryType.BraceletGold] > 0)
            {
                accumulatedProbability += goldBraceletChance;
            }

            collectiveProbability.Add(accumulatedProbability);
            if (_bracelets[JewelryType.ChainGold] > 0)
            {
                accumulatedProbability += goldChainChance;
            }

            collectiveProbability.Add(accumulatedProbability);

            float probability = Random.value * accumulatedProbability;
            for (int i = 0; i < collectiveProbability.Count; i++)
            {
                if (collectiveProbability[i] > probability)
                {
                    jewelryView.RemoveBracelet(JewelryType.BraceletSilver + (i));
                    _bracelets[JewelryType.BraceletSilver + (i)]--;
                    _braceletCount--;
                    BroadcastDiscardEvent(JewelryType.BraceletSilver + (i));
                    break;
                }
            }
        }
    }
    private void DiscardBigChain()
    {
        if (_bigChainCount > 0)
        {
            jewelryView.RemoveBigBracelet(_bigChainType);
            _bigChainCount--;
            BroadcastDiscardEvent(_bigChainType);
        }
    }
    private void BroadcastDiscardEvent(JewelryType type)
    {
        var evt = GameEventsHandler.ItemDiscardEvent;
        evt.Type = type;
        EventManager.Broadcast(evt);
    }
   


}