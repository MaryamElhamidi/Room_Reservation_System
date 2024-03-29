using RoomReservation.BusinessLogic;
using System;
namespace RoomReservation.Pages;

public partial class AddRequestPage : ContentPage
{
    private MeetingRoom _selectedRoom;
    private static int _IDassigned = 0;


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
        try
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

            var success = App.ReservationManager.AddReservationRequest(
                UserNameEntry.Text, // RequestedBy
                MeetingPurposeEntry.Text, // Description
                MeetingDatePicker.Date, // Meeting Date
                StartTimePicker.Time, // Start Time
                EndTimePicker.Time, // End Time
                participantCount, // Participant Count
                _selectedRoom // MeetingRoom object
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
        catch (Exception ex)
        {
            // Log the exception details, inform the user something went wrong
            // For example, using DisplayAlert to show a generic error message
            await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            // Consider logging the exception somewhere more substantial (e.g., log file, analytics)
            Console.WriteLine(ex.ToString()); // Or use your preferred logging mechanism
        }
    }

    private async void OnBackToRoomsClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
