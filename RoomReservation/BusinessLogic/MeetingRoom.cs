using System;
using System.Globalization;

namespace RoomReservation.BusinessLogic
{
    public class LayoutToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string layoutType = value.ToString();
            switch (layoutType)
            {
                case "HollowSquare":
                    return "hsquare1.png";
                case "UShape":
                    return "utable1.png";
                case "Classroom":
                    return "classroom1.png";
                case "Auditorium":
                    return "auditorium1.png";
                case "Boardroom":
                    return "broom1.png";
                default:
                    return "question.webp";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

        public string RoomImageFile => $"{LayoutType}".ToLower() + "_img.png";

    }
}

