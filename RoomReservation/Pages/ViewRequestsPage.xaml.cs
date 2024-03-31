using RoomReservation.BusinessLogic;
using RoomReservation.Pages;
using System;
using System.Collections.ObjectModel;

namespace RoomReservation.Pages
{
    public partial class ViewRequestPage : ContentPage
    {
        #region Fields
        // Selected room for which reservation requests are being viewed
        private MeetingRoom _selectedRoom;

        // Manager for handling reservation requests
        private ReservationRequestManager _reservationRequestManager;

        // Collection to display reservation requests in the UI
        private ObservableCollection<ReservationRequest> _requestsListViewDisplay = new ObservableCollection<ReservationRequest>();

        #endregion

        #region Constructor    
        public ViewRequestPage(MeetingRoom selectedRoom, ReservationRequestManager reservationRequestManager)
        {
            InitializeComponent();
            _selectedRoom = selectedRoom;
            _reservationRequestManager = reservationRequestManager;

            // Load reservation requests for the selected room
            LoadRequestsForDisplay();

            // Set the binding context and UI elements
            BindingContext = this;
            SelectedRoomLabel.Text = $"Showing Reservation for Room {_selectedRoom.RoomNumber}";
            RequestsListView.ItemsSource = _requestsListViewDisplay;
        }
        #endregion

        #region Methods

        // Method to load and display reservation requests for the selected room
        private void LoadRequestsForDisplay()
        {
            // Clear the current list of reservation requests
            _requestsListViewDisplay.Clear();

            // Iterate through all reservation requests
            foreach (var reservation in _reservationRequestManager.ReservationRequests)
            {
                // Check if the reservation request is for the selected room
                if (reservation.MeetingRoom.RoomNumber == _selectedRoom.RoomNumber)
                {
                    // Add the reservation request to the display list
                    _requestsListViewDisplay.Add(reservation);
                }
            }
        }

        #endregion

        #region Event Handlers

        // Event handler for when a reservation request is selected
        private async void OnRequestSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Check if a reservation request item is selected
            if (e.SelectedItem is ReservationRequest request)
            {
                // Calculate start and end times for the meeting
                var startDateTime = request.MeetingDate.Add(request.StartTime);
                var endDateTime = request.MeetingDate.Add(request.EndTime);

                // Format start and end times for display
                var formattedStartTime = startDateTime.ToString("hh:mm tt");
                var formattedEndTime = endDateTime.ToString("hh:mm tt");

                // Construct message with reservation details
                var message = $"Room Number: {_selectedRoom.RoomNumber}\n" +
                            $"Room Layout: {_selectedRoom.LayoutType}\n" +
                            $"Requested By: {request.RequestedBy}\n" +
                            $"Description: {request.Description}\n" +
                            $"Date: {request.MeetingDate:dd/MM/yyyy}\n" +
                            $"Time: {formattedStartTime} - {formattedEndTime}\n" +
                            $"Status: {request.Status}";

                // Display alert dialog with reservation details and options to accept or reject
                bool accept = await DisplayAlert("Reservation Details", message, "Accept", "Reject");

                // Handle user response to accept or reject the reservation request
                if (accept)
                {
                    // Update reservation request status to accepted
                    bool updated = _reservationRequestManager.UpdateReservationRequestStatus(request.RequestID, BusinessLogic.RequestStatus.Accepted);
                    if (updated)
                    {
                        await DisplayAlert("Success", "Reservation accepted.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to update the reservation request.", "OK");
                    }
                }
                else
                {
                    // Update reservation request status to rejected
                    bool updated = _reservationRequestManager.UpdateReservationRequestStatus(request.RequestID, BusinessLogic.RequestStatus.Rejected);
                    if (updated)
                    {
                        await DisplayAlert("Rejected", "Reservation rejected.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to update the reservation request.", "OK");
                    }
                }

                // Refresh the list view to reflect the updated reservation status
                LoadRequestsForDisplay();
            }
        }

        // Event handler for when back button is clicked
        private void OnBackToRoomsClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page
            Navigation.PopAsync();
        }

        #endregion
    }
}
