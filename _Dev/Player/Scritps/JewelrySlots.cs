using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class JewelrySlots : MonoBehaviour
{
    [SerializeField] private JewelryHolder[] holders;

    public float Fullness
    {
        get
        {
            int c = 0;
            foreach (var holder in holders)
            {
                if (holder.Jewelry)
                {
                    c++;
                }
            }

            return (float) c / holders.Length;
        }
    }

    public int Capacity => holders.Length;


    public void AddJewelry(HandJewelryController ringGO, JewelryType type)
    {
        JewelryHolder emptyHolder = GetEmptySlot();
        if (emptyHolder)
        {
            emptyHolder.Jewelry = Instantiate(ringGO, emptyHolder.transform);
            switch (type)
            {
                case JewelryType.BraceletSilver:
                case JewelryType.ChainSilver:
                case JewelryType.ChainSilverBig:
                    emptyHolder.PlayEffect(true);
                    break;
                case JewelryType.BraceletGold:
                case JewelryType.ChainGold:
                case JewelryType.ChainGoldBig:
                    emptyHolder.PlayEffect(false);
                    break;
            }
        }
        else
            throw new Exception("Finger is full");
    }

    public void AddJewelry(HandRingController ringGO, bool isSilver = true)
    {
        JewelryHolder emptyHolder = GetEmptySlot();
        if (emptyHolder)
        {
            var jewelry = Instantiate(ringGO, emptyHolder.transform);
            jewelry.Initialize(isSilver);
            emptyHolder.Jewelry = jewelry;
            emptyHolder.PlayEffect(isSilver);
        }
        //else
         //   throw new Exception("Finger is full");
    }

    private JewelryHolder GetEmptySlot()
    {
        foreach (var holder in holders)
        {
            if (!holder.Jewelry)
            {
                return holder;
            }
        }

        return null;
    }

    public bool ContainsJewelry(JewelryType type)
    {
        foreach (var holder in holders)
        {
            if (holder.Jewelry?.type == type)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveRandomJewelry(JewelryType type)
    {
        List<JewelryHolder> finger = ShuffleCollection(holders);
        foreach (var holder in finger)
        {
            if (holder.Jewelry?.type == type)
            {
                holder.Jewelry.JewelryPop();
                holder.Jewelry = null;
                CompactJewelry();
                break;
            }
        }
    }

    private void CompactJewelry()
    {
        for (int i = 0; i < holders.Length - 1; i++)
        {
            if (!holders[i].Jewelry)
            {
                for (int j = i + 1; j < holders.Length; j++)
                {
                    if (holders[j].Jewelry)
                    {
                        holders[i].Jewelry = holders[j].Jewelry;
                        holders[j].Jewelry = null;
                        holders[i].Jewelry.transform.SetParent(holders[i].transform);
                        holders[i].Jewelry.transform.localPosition = Vector3.zero;
                        break;
                    }
                }
            }
        }
    }

    private List<T> ShuffleCollection<T>(IEnumerable<T> original)
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

    private void JewelryPop(GameObject go)
    {
        go.transform.SetParent(null);
        Rigidbody gorb = go.GetComponent<Rigidbody>();
        gorb.isKinematic = false;
        gorb.useGravity = true;
        gorb.AddExplosionForce(
            10f,
            transform.position + Vector3.down,
            10f,
            0,
            ForceMode.Impulse);
        gorb.AddTorque(Random.insideUnitSphere * 5, ForceMode.Impulse);
    }

    public bool DropJewelry()
    {
        for (int i = holders.Length - 1; i >= 0; i--)
        {
            if (holders[i].Jewelry)
            {
                holders[i].Jewelry.JewelryDrop();
                holders[i].Jewelry = null;
                return true;
            }
        }

        return false;
    }
}