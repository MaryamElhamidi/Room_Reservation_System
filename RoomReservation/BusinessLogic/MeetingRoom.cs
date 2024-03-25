using System;
namespace RoomReservation.BusinessLogic
{


    public class MeetingRoom
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int SeatingCapacity { get; set; }
        public RoomLayoutType RoomLayoutType { get; set; }
        public string RoomImageFileName { get; set; }

        public string RoomTypeIcon
        {
            get
            {
                string iconName = "";
                switch (RoomLayoutType)
                {
                    case RoomLayoutType.HollowSquare:
                        iconName = "hollowsquare_icon.png";
                        break;
                    case RoomLayoutType.UShape:
                        iconName = "ushape_icon.png";
                        break;
                    case RoomLayoutType.Classroom:
                        iconName = "classroom_icon.png";
                        break;
                    case RoomLayoutType.Auditorium:
                        iconName = "auditorium_icon.png";
                        break;
                    default:
                        iconName = "default_icon.png";
                        break;
                }
                return iconName;
            }
        }

        public MeetingRoom(int id, string roomNumber, int capacity, RoomLayoutType layout, string imageFileName)
        {
            Id = id;
            RoomNumber = roomNumber;
            SeatingCapacity = capacity;
            RoomLayoutType = layout;
            RoomImageFileName = imageFileName;
        }
    }
}

