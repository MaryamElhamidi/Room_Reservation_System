using System;
using System.Globalization;

namespace RoomReservation.BusinessLogic
{
    public class MeetingRoom
    {
        // Properties of the MeetingRoom class
        public string RoomNumber { get; private set; }
        public int SeatingCapacity { get; private set; }
        public RoomLayoutType LayoutType { get; private set; }
        public string RoomImageFileName { get; private set; }
        public string Name { get; set; } // Ensure this property exists
        public string Layout { get; set; }
        public string ImageUri { get; set; } // URL to the room's image

        // Constructor for the MeetingRoom class
        public MeetingRoom(string roomNumber, int seatingCapacity, RoomLayoutType layoutType, string roomImageFileName)
        {
            RoomNumber = roomNumber;
            SeatingCapacity = seatingCapacity;
            LayoutType = layoutType;
            RoomImageFileName = roomImageFileName;
        }

        public string RoomTypeIcon
        {
            get { return $"{LayoutType.ToString().ToLower()}1.png"; }
        }

        public string RoomImageFile => $"{LayoutType}".ToLower() + ".png";
        public string LayoutTypeDisplay => LayoutType.ToString();

    }
}

