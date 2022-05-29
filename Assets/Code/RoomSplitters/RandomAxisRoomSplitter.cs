using Code.InitDatas;
using Code.Interfaces;
using Code.MapGeneration;
using UnityEngine;

namespace Code.RoomSplitters
{
    public class RandomAxisRoomSplitter : IRoomSplitter
    {
        public (RoomHolder, RoomHolder) SplitUpRoom(RoomHolder room)
        {
            var isVertical = !room.IsVertical;

            var axis = isVertical ? room.Width : room.Height;
            var splitPoint = Random.Range(axis / 2 - axis / room.MINBorder, axis / 2 + axis / room.MINBorder);

            var xA = room.X;
            var yA = room.Y;
            var widthA = isVertical ? splitPoint : room.Width;
            var heightA = isVertical ? room.Height : splitPoint;

            var xB = isVertical ? room.X + splitPoint : room.X;
            var yB = isVertical ? room.Y : room.Y + splitPoint;
            var widthB = isVertical ? room.Width - splitPoint : room.Width;
            var heightB = isVertical ? room.Height : room.Height - splitPoint;

            var roomA = new RoomHolder(new RoomInitData()
            {
                X = xA,
                Y = yA,
                Width = widthA,
                Height = heightA,
                IsVertical = isVertical,
                MinBorder = room.MINBorder,
            });

            var roomB = new RoomHolder(new RoomInitData()
            {
                X = xB,
                Y = yB,
                Width = widthB,
                Height = heightB,
                IsVertical = isVertical,
                MinBorder = room.MINBorder,
            });

            return (roomA, roomB);
        }
    }
}