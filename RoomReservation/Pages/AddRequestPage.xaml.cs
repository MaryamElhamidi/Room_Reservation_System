using RoomReservation.BusinessLogic;
using System;
namespace RoomReservation.Pages;

public partial class AddRequestPage : ContentPage
{
    private MeetingRoom _selectedRoom;
    private static int _IDassigned = 0;
    private ReservationRequestManager _reservationRequestManager = ReservationRequestManager.Instance;

    public AddRequestPage(MeetingRoom selectedRoom)
    {
        InitializeComponent();
        _selectedRoom = selectedRoom;
        DisplaySelectedRoomDetails();
        BindingContext = selectedRoom;

    }

    private void DisplaySelectedRoomDetails()
    {
        SelectedRoomLabel.Text = $"Room {_selectedRoom.RoomNumber}";
        SelectedRoomImage.Source = ImageSource.FromFile(_selectedRoom.RoomImageFileName);
    }

    private async void OnAddRequestClicked(object sender, EventArgs e)
    {
       
            if (string.IsNullOrWhiteSpace(UserNameEntry.Text) ||
                string.IsNullOrWhiteSpace(MeetingPurposeEntry.Text) ||
                !int.TryParse(ParticipantCountEntry.Text, out int participantCount) ||
                participantCount <= 0)
            {
                await DisplayAlert("Error", "Please complete all fields with valid information.", "OK");
                return;
            }

            if (participantCount > _selectedRoom.SeatingCapacity)
            {
                await DisplayAlert("Error", "The number of participants exceeds the room's capacity.", "OK");
                return;
            }

            int requestId = ++_IDassigned;

            DateTime startTime = DateTime.Today.Add(StartTimePicker.Time);
            DateTime endTime = DateTime.Today.Add(EndTimePicker.Time);

            string formattedStartTime = startTime.ToString("hh:mm tt");
            string formattedEndTime = endTime.ToString("hh:mm tt");

            var success = _reservationRequestManager.AddReservationRequest(
                UserNameEntry.Text,
                MeetingPurposeEntry.Text, 
                MeetingDatePicker.Date, 
                StartTimePicker.Time, 
                EndTimePicker.Time, 
                participantCount, 
                _selectedRoom 
            );

            if (success)
            {
                await DisplayAlert("Success",
                $"Your reservation request has been successfully added.\n\n" +
                $"Start Time: {formattedStartTime}\n" +
                $"End Time: {formattedEndTime}\n" +
                $"Request ID: {requestId}", "OK"); 
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "There was a problem adding your reservation request. Please try again.", "OK");
            }
        

    }

    private async void OnBackToRoomsClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
