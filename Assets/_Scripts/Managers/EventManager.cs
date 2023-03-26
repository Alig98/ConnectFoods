using Events;
using UnityEngine.Events;

//static class for handle all events.
public static class EventManager
{
    public static readonly GameEvent MoveHasBeenMade = new GameEvent();
    public static readonly TilePopEvent TilePopEvent = new TilePopEvent();
}

namespace Events
{
    public class GameEvent : UnityEvent {}
    public class TilePopEvent : UnityEvent<Tile> {}
}