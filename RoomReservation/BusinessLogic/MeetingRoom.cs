using System;
namespace RoomReservation.BusinessLogic
{
    /// <summary>
    /// Represents a meeting room.
    /// </summary>
    public class MeetingRoom
    {
        private string _roomNumber;
        private int _seatingCapacity;
        private string _roomImageFileName;

        /// <summary>
        /// The room number. Must be a non-empty string.
        /// </summary>
        public string RoomNumber
        {
            get => _roomNumber;
            set => _roomNumber = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Room number is required.");
        }

        /// <summary>
        /// The seating capacity of the room. Must be greater than 0.
        /// </summary>
        public int SeatingCapacity
        {
            get => _seatingCapacity;
            set => _seatingCapacity = value > 0 ? value : throw new ArgumentException("Seating capacity must be greater than 0.");
        }

        /// <summary>
        /// The room's layout type.
        /// </summary>
        public RoomLayoutType RoomLayoutType { get; set; }

        /// <summary>
        /// The name of the file containing the actual picture of the room. Must be a non-empty string.
        /// </summary>
        public string RoomImageFileName
        {
            get => _roomImageFileName;
            set => _roomImageFileName = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Room image file name is required.");
        }

        /// <summary>
        /// A computed property that evaluates to the name of the image icon file representing the RoomLayoutType.
        /// </summary>
        public string RoomTypeIcon => $"{RoomLayoutType.ToString().ToLower()}_icon.png";

        public MeetingRoom(string roomNumber, int seatingCapacity, RoomLayoutType roomLayoutType, string roomImageFileName)
        {
            RoomNumber = roomNumber;
            SeatingCapacity = seatingCapacity;
            RoomLayoutType = roomLayoutType;
            RoomImageFileName = roomImageFileName;
        }
    }
}

