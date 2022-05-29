using Code.MapGeneration;

namespace Code.Interfaces
{
    public interface IRoomSplitter
    {
        (RoomHolder, RoomHolder) SplitUpRoom(RoomHolder room);
    }
}