using RoomReservation.BusinessLogic;
using RoomReservation.Pages;


namespace RoomReservation;

public partial class App : Application
{
    public static ReservationRequestManager ReservationManager { get; private set; }

    public App()
    {
        InitializeComponent();
        ReservationManager = new ReservationRequestManager();
        PopulateMeetingRooms();
        MainPage = new NavigationPage(new PickRoomPage());
    }

    private void PopulateMeetingRooms()
    {
        ReservationManager.AddMeetingRoom("A101", 18, RoomLayoutType.Classroom, "classroom.webp");
        ReservationManager.AddMeetingRoom("B102", 200, RoomLayoutType.Auditorium, "auditorium.jpeg");
        ReservationManager.AddMeetingRoom("C103", 10, RoomLayoutType.UShape, "ushape.webp");
        ReservationManager.AddMeetingRoom("D104", 20, RoomLayoutType.HollowSquare, "hollowsquare.jpeg");
        ReservationManager.AddMeetingRoom("E105", 18, RoomLayoutType.Boardroom, "boardroom.jpeg");
    }
}


