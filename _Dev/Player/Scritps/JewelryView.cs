using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class JewelryView : MonoBehaviour
{
    [Header("Objects")] [SerializeField] private HandJewelryController[] jewelryPrefabs;
    [Header("Holders")] [SerializeField] private JewelrySlots indexHolders;
    [SerializeField] private JewelrySlots middleHolders;
    [SerializeField] private JewelrySlots ringHolders;
    [SerializeField] private JewelrySlots pinkyHolders;
    [SerializeField] private JewelrySlots thumbHolders;
    [SerializeField] private JewelrySlots wristHolders;
    private float[] _fingersRingsCount;
    private JewelrySlots[] _fingers;

    [Header("Big Bracelet")] [SerializeField]
    private GameObject bigBracelet;

    private JewelryType bigBraceletType;
    private Renderer _bigBraceletRenderer;
    [SerializeField] private Material goldMaterial;
    [SerializeField] private Material silverMaterial;
    [SerializeField] private Transform bigChainSpawnPoint;
    [SerializeField] private HandJewelryController bigSilverChainPrefab;
    [SerializeField] private HandJewelryController bigGoldChainPrefab;
    [SerializeField] private ParticleSystem _silverEffect;
    [SerializeField] private ParticleSystem _goldEffect;
    private List<JewelrySlots> _allSlots;
    private bool _droppable;
    private void Awake()
    {
        _fingers = new[] {indexHolders, middleHolders, ringHolders, pinkyHolders, thumbHolders};
        
        _allSlots = _fingers.Append(wristHolders).ToList();
        _bigBraceletRenderer = bigBracelet.GetComponent<Renderer>();
    }

    public void AddRing(JewelryType type, bool isSilver, int fingerNum)
    {
        //int fingerNum = GetMinFinger();
        _fingers[fingerNum].AddJewelry(jewelryPrefabs[(int) type] as HandRingController, isSilver);
    }

    public void RemoveRing(JewelryType type)
    {
        List<int> randomFingers = ShuffleList(new int[] {0, 1, 2, 3, 4});
        foreach (var finger in randomFingers)
        {
            if (_fingers[finger].ContainsJewelry(type))
            {
                _fingers[finger].RemoveRandomJewelry(type);
                break;
            }
        }
    }

    public void AddBracelet(JewelryType type)
    {
        wristHolders.AddJewelry(jewelryPrefabs[(int) type], type);
    }

    public void RemoveBracelet(JewelryType type)
    {
        if (wristHolders.ContainsJewelry(type))
        {
            wristHolders.RemoveRandomJewelry(type);
        }
    }

    public void AddBigBracelet(JewelryType type)
    {
        bigBracelet.SetActive(true);
        if (type == JewelryType.ChainSilverBig)
        {
            _bigBraceletRenderer.material = silverMaterial;
            _silverEffect.Play();
        }
        else if (type == JewelryType.ChainGoldBig)
        {
            _bigBraceletRenderer.material = goldMaterial;
            _goldEffect.Play();
        }
        else
        {
            throw new Exception("Big chain bracelet wrong type.");
        }

        bigBraceletType = type;
    }

    public void RemoveBigBracelet(JewelryType type)
    {
        if (type == JewelryType.ChainGoldBig)
            Instantiate(bigGoldChainPrefab, bigChainSpawnPoint.position, Quaternion.identity).JewelryPop();
        else if (type == JewelryType.ChainSilverBig)
            Instantiate(bigSilverChainPrefab, bigChainSpawnPoint.position, Quaternion.identity).JewelryPop();
        bigBracelet.SetActive(false);
    }

    public bool CheckEmpty()
    {
        float fullness = 0;
        List<JewelrySlots> allSlots = _fingers.Append(wristHolders).ToList();
        foreach (var slot in allSlots)
        {
            fullness += slot.Fullness;
        }

        return Math.Abs(fullness) < Mathf.Epsilon;
    }
    public void StartDropping()
    {
            //StartCoroutine(FinisherDropJewelry(0.1f));
            _droppable = true;

    }

    private IEnumerator FinisherDropJewelry(float time)
    {
        var wait = new WaitForSeconds(time);
        bool dropping = true;
        while (dropping)
        {
            dropping = false;
            foreach (var slots in _allSlots)
            {
                dropping = slots.DropJewelry() || dropping;
            }

            yield return wait;
        }
        if (bigBracelet.activeInHierarchy)
        {
            if (bigBraceletType == JewelryType.ChainGoldBig)
                Instantiate(bigGoldChainPrefab, bigChainSpawnPoint.position, Quaternion.identity).JewelryDrop();
            else if (bigBraceletType == JewelryType.ChainSilverBig)
                Instantiate(bigSilverChainPrefab, bigChainSpawnPoint.position, Quaternion.identity).JewelryDrop();
            bigBracelet.SetActive(false);
        }
        EventManager.Broadcast(GameEventsHandler.FinisherEndEvent);
    }

    public void ShakeJewelry()
    {
        if(_droppable)
        {
            _droppable = false;
            foreach (var slots in _allSlots)
            {
                _droppable = slots.DropJewelry() || _droppable;
            }

            if(_droppable) return;
        }
        if (bigBracelet.activeInHierarchy)
        {
            if (bigBraceletType == JewelryType.ChainGoldBig)
                Instantiate(bigGoldChainPrefab, bigChainSpawnPoint.position, Quaternion.identity).JewelryDrop();
            else if (bigBraceletType == JewelryType.ChainSilverBig)
                Instantiate(bigSilverChainPrefab, bigChainSpawnPoint.position, Quaternion.identity).JewelryDrop();
            bigBracelet.SetActive(false);
        }
        EventManager.Broadcast(GameEventsHandler.FinisherEndEvent);
    }
    private int GetMinFinger()
    {
        float minval = 1f;
        float val = 0f;
        float[] newMas = new float[5];
        for (int i = 0; i < _fingers.Length; i++)
        {
            val = _fingers[i].Fullness;
            if (val < minval)
                minval = val;
            newMas[i] = val;
        }

        List<int> n = new List<int>();
        for (int i = 0; i < newMas.Length; i++)
        {
            if (Mathf.Abs(newMas[i] - minval) < Mathf.Epsilon)
            {
                n.Add(i);
            }
        }

        return n[Random.Range(0, n.Count)];
    }

    private List<T> ShuffleList<T>(IEnumerable<T> original)
    {
        List<T> result = original.ToList();
        var count = result.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = result[i];
            result[i] = result[r];
            result[r] = tmp;
        }

        return result;
    }
}