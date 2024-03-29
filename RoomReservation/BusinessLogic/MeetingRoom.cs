using System;
namespace RoomReservation.BusinessLogic
{
    public class MeetingRoom
    {
        // Properties of the MeetingRoom class
        public string RoomNumber { get; private set; }
        public int SeatingCapacity { get; private set; }
        public RoomLayoutType LayoutType { get; private set; }
        public string RoomImageFileName { get; private set; }

        // Constructor for the MeetingRoom class
        public MeetingRoom(string roomNumber, int seatingCapacity, RoomLayoutType layoutType, string roomImageFileName)
        {
            RoomNumber = roomNumber;
            SeatingCapacity = seatingCapacity;
            LayoutType = layoutType;
            RoomImageFileName = roomImageFileName;
        }
    }
}

