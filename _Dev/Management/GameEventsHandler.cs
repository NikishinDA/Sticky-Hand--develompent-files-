// The Game Events used across the Game.
// Anytime there is a need for a new event, it should be added here.

using System;
using UnityEngine;

public static class GameEventsHandler
{
    public static readonly LevelInitializeEvent LevelInitializeEvent = new LevelInitializeEvent();
    public static readonly GameStartEvent GameStartEvent = new GameStartEvent();
    public static readonly GameOverEvent GameOverEvent = new GameOverEvent();
    public static readonly PlayerProgressEvent PlayerProgressEvent = new PlayerProgressEvent();
    public static readonly PlayerFinishCrossedEvent PlayerFinishCrossedEvent = new PlayerFinishCrossedEvent();
    public static readonly ItemCollectEvent ItemCollectEvent = new ItemCollectEvent();
    public static readonly PlayerTakeDamageEvent PlayerTakeDamageEvent = new PlayerTakeDamageEvent();
    public static readonly PlayerHandShakeTick PlayerHandShakeTick = new PlayerHandShakeTick();
    public static readonly ItemDiscardEvent ItemDiscardEvent = new ItemDiscardEvent();
    public static readonly FinisherJewelryDroppedEvent FinisherJewelryDroppedEvent = new FinisherJewelryDroppedEvent();
    public static readonly JewelryThrowEvent JewelryThrowEvent = new JewelryThrowEvent();
    public static readonly FinisherPlayerInPosition FinisherPlayerInPosition = new FinisherPlayerInPosition();
    public static readonly PlayerHandUpgradeEvent PlayerHandUpgradeEvent = new PlayerHandUpgradeEvent();
    public static readonly FinisherEndEvent FinisherEndEvent = new FinisherEndEvent();
    public static readonly DebugCallEvent DebugCallEvent = new DebugCallEvent();
    public static readonly PlayerNailUpgradeEvent PlayerNailUpgradeEvent = new PlayerNailUpgradeEvent();
}

public class GameEvent {}

public class GameStartEvent : GameEvent
{
}

public class GameOverEvent : GameEvent
{
    public Transform PlayerTransform;
    public bool IsWin;
}

public class PlayerProgressEvent : GameEvent
{
    
}

public class PlayerFinishCrossedEvent : GameEvent
{
    
}

public class ItemCollectEvent : GameEvent
{
    public JewelryType Type;
    public bool IsSilver;
    public float RingOffset;
}

public class PlayerTakeDamageEvent : GameEvent
{
    public ObstacleType ObstacleType;
}

public class JewelryThrowEvent : GameEvent
{
    
}

public class ItemDiscardEvent : GameEvent
{
    public JewelryType Type;
}

public class LevelInitializeEvent : GameEvent
{
    public int LevelLength;
}

public class FinisherJewelryDroppedEvent : GameEvent
{
    public JewelryType Type;
}

public class PlayerHandShakeTick : GameEvent
{
}

public class FinisherPlayerInPosition : GameEvent
{
}

public class PlayerHandUpgradeEvent : GameEvent
{
    public int UpgradeLevel;
}

public class FinisherEndEvent : GameEvent
{
    
}

public class DebugCallEvent : GameEvent
{
    public float SpeedX;
    public float SpeedZ;
    public float BoundX;
}

public class PlayerNailUpgradeEvent : GameEvent
{
    public int UpgradeLevel;
}



