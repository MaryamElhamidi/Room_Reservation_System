using RoomReservation.BusinessLogic;
using System;

namespace RoomReservation.Pages;

public partial class PickRoomPage : ContentPage
{
    private MeetingRoom MeetingRoom;

    public PickRoomPage()
    {
        InitializeComponent();
        LoadRooms();
    }

    private void LoadRooms()
    {
        RoomsListView.ItemsSource = App.ReservationManager.GetAllMeetingRooms();
    }

    private void RoomsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        MeetingRoom = e.SelectedItem as MeetingRoom;
        if (MeetingRoom != null)
        {
            RoomImage.Source = MeetingRoom.RoomImageFileName;
        }
    }

    private async void AddRequestButton_Clicked(object sender, EventArgs e)
    {
        if (MeetingRoom == null)
        {
            await DisplayAlert("Error", "Please select a room first.", "OK");
            return;
        }

        await Navigation.PushAsync(new AddRequestPage(MeetingRoom));
    }

    private async void ViewRequestsButton_Clicked(object sender, EventArgs e)
    {
        if (MeetingRoom == null)
        {
            await DisplayAlert("Error", "Please select a room first.", "OK");
            return;
        }

        await Navigation.PushAsync(new ViewRequestPage(MeetingRoom));
    }
}
