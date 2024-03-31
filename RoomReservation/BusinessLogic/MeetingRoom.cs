using System;
using System.Globalization;

namespace RoomReservation.BusinessLogic
{
    public class MeetingRoom
    {
        private string _roomNumber;
        private int _seatingCapacity;
        public RoomLayoutType LayoutType { get; private set; }
        public string RoomImageFileName { get; private set; }

        public string RoomNumber
        {
            get { return _roomNumber; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value), "You have to provide a room number");
                }
                _roomNumber = value;
            }
        }
        public int SeatingCapacity
        {
            get { return _seatingCapacity; }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The seating capacity must be greater than 0", nameof(value));
                }
                _seatingCapacity = value;
            }
        }

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

