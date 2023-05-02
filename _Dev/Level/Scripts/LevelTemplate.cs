using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelObjectType
{
    RingSilver,
    RingDiamond,
    RingRuby,
    BraceletSilver,
    ChainSilver,
    BraceletGold,
    ChainGoldBig,
    PipeLeft,
    PipeRight,
    Saw,
    PaintFall,
    PaintStack,
    ChainGold,
    ChainSilverBig,
    HammerRight,
    HammerLeft
}

public enum ChunkType
{
    Simple,
    HoleLeft,
    HoleRight,
    RampLeft,
    RampRight,
    RampStraightLeft,
    RampStraightRight,
    RampEndLeft,
    RampEndRight
}
[Serializable]
public class LevelObject
{
    public LevelObjectType type;
    public Vector2 position;
}

[Serializable]
public class ChunkTemplate
{
    public LevelObject[] objects;
    public ChunkType chunkType;
}
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
public class LevelTemplate : ScriptableObject
{
    public ChunkTemplate[] chunks;
}
