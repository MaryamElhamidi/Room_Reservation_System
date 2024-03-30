using RoomReservation.BusinessLogic;
using RoomReservation.Pages;
using System;

namespace RoomReservation.Pages
{
    public partial class ViewRequestPage : ContentPage
    {
        private MeetingRoom _selectedRoom;
        private ReservationRequestManager _reservationRequestManager = ReservationRequestManager.Instance;

        public ViewRequestPage(MeetingRoom selectedRoom)
        {
            InitializeComponent();
            LoadAllReservationRequests();
            _selectedRoom = selectedRoom;
            BindingContext = this;
            SelectedRoomLabel.Text = $"Showing Reservation for Room {_selectedRoom.RoomNumber}";

        }

        private void LoadAllReservationRequests()
        {
            var allRequests = _reservationRequestManager.GetAllReservationRequests();

            if (!allRequests.Any())
            {
                DisplayAlert("Info", "No reservation requests found.", "OK");
                return;
            }

            RequestsListView.ItemsSource = allRequests;
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
                LoadAllReservationRequests();
            }
        }
        #endregion
        private void OnBackToRoomsClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
