using RoomReservation.BusinessLogic;
using RoomReservation.Pages;
using System;
using System.Collections.ObjectModel;

namespace RoomReservation.Pages
{
    public partial class ViewRequestPage : ContentPage
    {
        private MeetingRoom _selectedRoom;
        private ReservationRequestManager _reservationRequestManager;
        private ObservableCollection<ReservationRequest> _requestsListViewDisplay = new ObservableCollection<ReservationRequest>();

        public ViewRequestPage(MeetingRoom selectedRoom, ReservationRequestManager reservationRequestManager)
        {
            InitializeComponent();
            _selectedRoom = selectedRoom;
            _reservationRequestManager = reservationRequestManager;
            LoadRequestsForDisplay();
            BindingContext = this;
            SelectedRoomLabel.Text = $"Showing Reservation for Room {_selectedRoom.RoomNumber}";
            RequestsListView.ItemsSource = _requestsListViewDisplay;
        }

        private void LoadRequestsForDisplay()
        {
            _requestsListViewDisplay.Clear();

            foreach (var reservation in _reservationRequestManager.ReservationRequests)
            {
                if (reservation.MeetingRoom.RoomNumber == _selectedRoom.RoomNumber)
                {
                    _requestsListViewDisplay.Add(reservation);
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
                LoadRequestsForDisplay();
            }
        }
        #endregion
        private void OnBackToRoomsClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
