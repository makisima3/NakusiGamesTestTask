using Code.MapGeneration;

namespace Code.Interfaces
{
    public interface IRoomSplitter
    {
        (Room, Room) SplitUpRoom(Room room);
    }
}