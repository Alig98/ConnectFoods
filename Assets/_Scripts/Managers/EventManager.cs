using System.Collections.Generic;
using Events;
using UnityEngine.Events;

//static class for handle all events.
public static class EventManager
{
    public static readonly GameEvent StatisticsChanged = new GameEvent();
    public static readonly MoveHasBeenMadeEvent MoveHasBeenMade = new MoveHasBeenMadeEvent();
}

namespace Events
{
    public class GameEvent : UnityEvent {}
    public class MoveHasBeenMadeEvent : UnityEvent<List<Tile>> {}
}