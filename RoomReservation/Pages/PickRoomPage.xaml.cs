using RoomReservation.BusinessLogic;
using System;

namespace RoomReservation.Pages;

public partial class PickRoomPage : ContentPage
{
    private MeetingRoom MeetingRoom;
    private ReservationRequestManager _reservationRequestManager = new ReservationRequestManager();

    public PickRoomPage()
    {
        InitializeComponent();
        _reservationRequestManager.PopulateMeetingRooms();
        RoomsListView.ItemsSource = _reservationRequestManager.MeetingRooms;

    }

    private void RoomsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        MeetingRoom = e.SelectedItem as MeetingRoom;
        if (MeetingRoom != null)
        {
            RoomImage.Source = MeetingRoom.RoomImageFileName;
        }
    }

    private void AddRequestButton_Clicked(object sender, EventArgs e)
    {
        if (MeetingRoom == null)
        {
            DisplayAlert("Error", "Please select a room first.", "OK");
            return;
        }

        Navigation.PushAsync(new AddRequestPage(MeetingRoom, _reservationRequestManager));
    }

    private void ViewRequestsButton_Clicked(object sender, EventArgs e)
    {
        if (MeetingRoom == null)
        {
            DisplayAlert("Error", "Please select a room first.", "OK");
            return;
        }

        Navigation.PushAsync(new ViewRequestPage(MeetingRoom, _reservationRequestManager));
    }
}
