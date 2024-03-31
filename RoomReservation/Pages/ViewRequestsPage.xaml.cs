using RoomReservation.BusinessLogic;
using RoomReservation.Pages;
using System;
using System.Collections.ObjectModel;

namespace RoomReservation.Pages
{
    public partial class ViewRequestPage : ContentPage
    {
        private MeetingRoom _selectedRoom;
        private ReservationRequestManager _reservationRequestManager = ReservationRequestManager.Instance;
        private ObservableCollection<ReservationRequest> _requestsForDisplay = new ObservableCollection<ReservationRequest>();

        public ViewRequestPage(MeetingRoom selectedRoom)
        {
            InitializeComponent();
            _selectedRoom = selectedRoom;
            PopulateRequestsForDisplay();
            BindingContext = this;
            SelectedRoomLabel.Text = $"Showing Reservation for Room {_selectedRoom.RoomNumber}";
            RequestsListView.ItemsSource = _requestsForDisplay;
        }

        private void PopulateRequestsForDisplay()
        {
            _requestsForDisplay.Clear(); // Clear existing items to refresh the list

            // Loop through all reservation requests in the manager
            foreach (var res in _reservationRequestManager.ReservationRequests)
            {
                // Compare using a unique identifier (e.g., RoomNumber) instead of direct object reference
                if (res.MeetingRoom.RoomNumber == _selectedRoom.RoomNumber)
                {
                    _requestsForDisplay.Add(res);
                }
            }
        }

        #region Bonus
        private async void OnRequestSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ReservationRequest request)
            {
                var startDateTime = request.MeetingDate.Add(request.StartTime);
                var endDateTime = request.MeetingDate.Add(request.EndTime);

                var formattedStartTime = startDateTime.ToString("hh:mm tt");
                var formattedEndTime = endDateTime.ToString("hh:mm tt");

                var message = $"Room Number: {_selectedRoom.RoomNumber}\n" +
                            $"Room Layout: {_selectedRoom.LayoutType}\n" +
                            $"Requested By: {request.RequestedBy}\n" +
                            $"Description: {request.Description}\n" +
                            $"Date: {request.MeetingDate:dd/MM/yyyy}\n" +
                            $"Time: {formattedStartTime} - {formattedEndTime}\n" +
                            $"Status: {request.Status}";

                bool accept = await DisplayAlert("Reservation Details", message, "Accept", "Reject");

                if (accept)
                {
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

                // Refresh the list view
                PopulateRequestsForDisplay();
            }
        }
        #endregion
        private void OnBackToRoomsClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
