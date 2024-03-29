using RoomReservation.BusinessLogic;
using RoomReservation.Pages;


namespace RoomReservation;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();

    }
}


