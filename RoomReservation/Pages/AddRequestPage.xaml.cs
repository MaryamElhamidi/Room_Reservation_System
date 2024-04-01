using RoomReservation.BusinessLogic;
using System;
namespace RoomReservation.Pages;

public partial class AddRequestPage : ContentPage
{
    #region Fields
    // Fields to store the selected room, assigned ID, and reservation request manager
    private MeetingRoom _selectedRoom;
    private static int _IDassigned = 0;
    private ReservationRequestManager _reservationRequestManager;
    #endregion

    #region Constructor
    public AddRequestPage(MeetingRoom selectedRoom, ReservationRequestManager reservationRequestManager)
    {
        InitializeComponent();
        _selectedRoom = selectedRoom;
        _reservationRequestManager = reservationRequestManager;

        // Display details of the selected room
        DisplaySelectedRoomDetails();

        // Set the binding context to the selected room
        BindingContext = selectedRoom;
    }

    #endregion

    #region Methods

    // Method to display details of the selected room
    private void DisplaySelectedRoomDetails()
    {
        SelectedRoomLabel.Text = $"Room {_selectedRoom.RoomNumber}";
        SelectedRoomImage.Source = ImageSource.FromFile(_selectedRoom.RoomImageFileName);
    }

    // Method to handle the "Add Request" button click event
    private void OnAddRequestClicked(object sender, EventArgs e)
    {
        try
        {
            // Extract start and end times
            DateTime startTime = DateTime.Today.Add(StartTimePicker.Time);
            DateTime endTime = DateTime.Today.Add(EndTimePicker.Time);

            // Add the reservation request to the manager
            var success = _reservationRequestManager.AddReservationRequest(
                UserNameEntry.Text,
                MeetingPurposeEntry.Text,
                MeetingDatePicker.Date,
                StartTimePicker.Time,
                EndTimePicker.Time,
                int.Parse(ParticipantCountEntry.Text),
                _selectedRoom
            );

            // Display success or error message based on request addition success
            if (success)
            {
                // Increment ID only on successful addition
                int requestId = ++_IDassigned;
                DisplaySuccessAlert(requestId);
            }
            else
            {
                DisplayAlert("Error", "There was a problem adding your reservation request. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Display any validation errors from the ReservationRequest constructor
            DisplayAlert("Error", ex.Message, "Ok.");
        }
    }

    // Method to display a success alert with reservation details
    private void DisplaySuccessAlert(int requestId)
    {
        string formattedStartTime = DateTime.Today.Add(StartTimePicker.Time).ToString("hh:mm tt");
        string formattedEndTime = DateTime.Today.Add(EndTimePicker.Time).ToString("hh:mm tt");

        DisplayAlert("Success",
            $"Your reservation request has been successfully added.\n\n" +
            $"Start Time: {formattedStartTime}\n" +
            $"End Time: {formattedEndTime}\n" +
            $"Request ID: {requestId}", "OK");
    }

    #endregion

    #region Event Handlers

    // Event handler for when the "Back to Rooms" button is clicked
    private void OnBackToRoomsClicked(object sender, EventArgs e)
    {
        // Navigate back to the previous page
        Navigation.PopAsync();
    }

    #endregion
}
