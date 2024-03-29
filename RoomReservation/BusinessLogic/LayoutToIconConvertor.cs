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
}

