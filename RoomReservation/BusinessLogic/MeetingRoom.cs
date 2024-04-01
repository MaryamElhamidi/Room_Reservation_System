using System;
using System.Globalization;

namespace RoomReservation.BusinessLogic
{
    public class MeetingRoom
    {
        #region Fields
        private string _roomNumber;
        private int _seatingCapacity;
        private RoomLayoutType _layoutType;
        private string _roomImageFileName;
        #endregion

        #region Properties
        // Gets the layout type of the meeting room.
        public RoomLayoutType LayoutType
        {
            get { return _layoutType; }
            init { } //Added this so I can use it in my constructor because otherwise it is a read only property.
            //init explained: It is a keyword instead of set, only time I want to set something only in the constuctor.
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/init 
        }

        // Gets the room image file name of the meeting room.
        public string RoomImageFileName
        {
            get { return _roomImageFileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value), "The file name of the room image cannot be null or empty.");
                }
                _roomImageFileName = value;
            }
        }

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
            _layoutType = layoutType;
            RoomImageFileName = roomImageFileName;
        }

        #endregion

        #region Methods
        public string RoomTypeIcon
        {
            get { return $"{LayoutType.ToString().ToLower()}1.png"; }
        }

        public string RoomImageFile => $"{LayoutType}".ToLower() + ".png";

        public string LayoutTypeDisplay => LayoutType.ToString();
        #endregion
    }
}