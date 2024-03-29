using RoomReservation.BusinessLogic;
using System;

namespace RoomReservation.Pages;

public partial class PickRoomPage : ContentPage
{
    private MeetingRoom _selectedRoom;

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
        _selectedRoom = e.SelectedItem as MeetingRoom;
        if (_selectedRoom != null)
        {
            RoomImage.Source = _selectedRoom.RoomImageFileName;
        }
    }

    private async void AddRequestButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedRoom == null)
        {
            await DisplayAlert("Error", "Please select a room first.", "OK");
            return;
        }

        await Navigation.PushAsync(new AddRequestPage(_selectedRoom));
    }

    private async void ViewRequestsButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedRoom == null)
        {
            await DisplayAlert("Error", "Please select a room first.", "OK");
            return;
        }

        await Navigation.PushAsync(new ViewRequestPage(_selectedRoom));
    }
}