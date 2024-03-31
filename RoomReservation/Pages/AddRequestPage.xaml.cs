using RoomReservation.BusinessLogic;
using System;
namespace RoomReservation.Pages;

public partial class AddRequestPage : ContentPage
{
    private MeetingRoom _selectedRoom;
    private static int _IDassigned = 0;
    private ReservationRequestManager _reservationRequestManager;

    public AddRequestPage(MeetingRoom selectedRoom, ReservationRequestManager reservationRequestManager)
    {
        InitializeComponent();
        _selectedRoom = selectedRoom;
        _reservationRequestManager = reservationRequestManager;
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
        int requestId = ++_IDassigned;
        DateTime startTime = DateTime.Today.Add(StartTimePicker.Time);
        DateTime endTime = DateTime.Today.Add(EndTimePicker.Time);

        var success = _reservationRequestManager.AddReservationRequest(
            UserNameEntry.Text,
            MeetingPurposeEntry.Text,
            MeetingDatePicker.Date,
            StartTimePicker.Time,
            EndTimePicker.Time,
            int.Parse(ParticipantCountEntry.Text), // Consider using TryParse if not validated elsewhere.
            _selectedRoom
        );

        if (success)
        {
            await DisplaySuccessAlert(requestId);
        }
        else
        {
            await DisplayAlert("Error", "There was a problem adding your reservation request. Please try again.", "OK");
        }
    }
    catch (Exception ex)
    {
        // This catches any validation errors from the ReservationRequest constructor.
        await DisplayAlert("Error", ex.Message, "Ok.");
    }
}

    private async Task DisplaySuccessAlert(int requestId)
    {
        string formattedStartTime = DateTime.Today.Add(StartTimePicker.Time).ToString("hh:mm tt");
        string formattedEndTime = DateTime.Today.Add(EndTimePicker.Time).ToString("hh:mm tt");

        await DisplayAlert("Success",
            $"Your reservation request has been successfully added.\n\n" +
            $"Start Time: {formattedStartTime}\n" +
            $"End Time: {formattedEndTime}\n" +
            $"Request ID: {requestId}", "OK");
    }
    private async void OnBackToRoomsClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
