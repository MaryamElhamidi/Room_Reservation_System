using System;
using System.Globalization;

namespace RoomReservation.BusinessLogic
{
    public class MeetingRoom
    {
        #region Fields
        private string _roomNumber;
        private int _seatingCapacity;
        #endregion

        #region Properties
        // Gets the layout type of the meeting room.
        public RoomLayoutType LayoutType { get; private set; }
        // Gets the file name of the room image.
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

        public string RoomTypeIcon
        {
            get { return $"{LayoutType.ToString().ToLower()}1.png"; }
        }


        public string RoomImageFile => $"{LayoutType}".ToLower() + ".png";

        public string LayoutTypeDisplay => LayoutType.ToString();
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingRoom"/> class.
        /// </summary>
        /// <param name="roomNumber">The room number.</param>
        /// <param name="seatingCapacity">The seating capacity of the room.</param>
        /// <param name="layoutType">The layout type of the room.</param>
        /// <param name="roomImageFileName">The file name of the room image.</param>
        public MeetingRoom(string roomNumber, int seatingCapacity, RoomLayoutType layoutType, string roomImageFileName)
        {
            RoomNumber = roomNumber;
            SeatingCapacity = seatingCapacity;
            LayoutType = layoutType;
            RoomImageFileName = roomImageFileName;
        }

        #endregion
    }
}
