using RoomReservation.BusinessLogic;

namespace RoomReservation.Pages;

public partial class AddRequestPage : ContentPage
{
    private MeetingRoom _selectedRoom;

    public AddRequestPage(MeetingRoom selectedRoom) // Fix parameter name to avoid confusion
    {
        InitializeComponent();
        _selectedRoom = selectedRoom; // Correct assignment
        DisplaySelectedRoomDetails();
    }

    private void DisplaySelectedRoomDetails()
    {
        // Update your UI to display selected room details
        SelectedRoomLabel.Text = $"Room {_selectedRoom.RoomNumber}: {_selectedRoom.SeatingCapacity} Seats";
        SelectedRoomType.Source = ImageSource.FromFile(_selectedRoom.RoomImageFileName);
    }

    private async void OnAddRequestClicked(object sender, EventArgs e)
    {
        // Input validation checks
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

        int request = GenerateUniqueRequestId();

        // Convert TimePicker TimeSpans to DateTime to use for formatting to string with AM/PM
        DateTime startTime = DateTime.Today.Add(StartTimePicker.Time);
        DateTime endTime = DateTime.Today.Add(EndTimePicker.Time);

        // Now format it with AM/PM
        string formattedStartTime = startTime.ToString("hh:mm tt");
        string formattedEndTime = endTime.ToString("hh:mm tt");

        // Create a new reservation request
        var success = App.ReservationManager.AddReservationRequest(
            request,
            UserNameEntry.Text, // requestedBy
            MeetingPurposeEntry.Text, // meetingPurpose
            MeetingPurposeEntry.Text, // Assuming this is the new 'description' parameter
            MeetingDatePicker.Date, // meetingDate
            StartTimePicker.Time, // startTime as TimeSpan
            EndTimePicker.Time, // endTime as TimeSpan
            participantCount, // participantCount
            _selectedRoom.RoomNumber // roomNumber
        );

        if (success)
        {
            await DisplayAlert("Success",
            $"Your reservation request has been successfully added.\n\n" +
            $"Start Time: {formattedStartTime}\n" +
            $"End Time: {formattedEndTime}\n" +
            $"Request ID: {request}", "OK");
            await Navigation.PopAsync(); // Navigate back
        }
        else
        {
            await DisplayAlert("Error", "There was a problem adding your reservation request. Please try again.", "OK");
        }
    }

    private int GenerateUniqueRequestId()
    {
        return new Random().Next(1000, 9999);
    }

    private async void OnBackToRoomsClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Navigate back
    }
}
