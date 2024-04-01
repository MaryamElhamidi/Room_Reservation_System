using RoomReservation.BusinessLogic;
using System;

namespace RoomReservation.Pages
{
    public partial class PickRoomPage : ContentPage
    {
        #region Fields
        // Field to store the selected meeting room
        private MeetingRoom _selectedRoom;

        // Manager for handling reservation requests
        private ReservationRequestManager _reservationRequestManager = new ReservationRequestManager();

        #endregion

        #region Constructor
        public PickRoomPage()
        {
            InitializeComponent();

            // Populate meeting rooms list
            _reservationRequestManager.PopulateMeetingRooms();

            // Set the items source of the ListView to the meeting rooms list
            RoomsListView.ItemsSource = _reservationRequestManager.MeetingRooms;
        }
        #endregion

        #region Methods
        // Event handler for when an item in the room list is selected
        private void RoomsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Retrieve the selected meeting room
            _selectedRoom = e.SelectedItem as MeetingRoom; //Explained e.SelectedItem in 4.A in my report

            // If a meeting room is selected, display its image
            if (_selectedRoom != null)
            {
                RoomImage.Source = _selectedRoom.RoomImageFileName;
            }
        }

        // Event handler for when the "Add Request" button is clicked
        private void AddRequestButton_Clicked(object sender, EventArgs e)
        {
            // Check if a room is selected
            if (_selectedRoom == null)
            {
                // Display an error message if no room is selected
                DisplayAlert("Error", "Please select a room first.", "OK");
                return;
            }

            // Navigate to the AddRequestPage and pass the selected room and reservation manager
            Navigation.PushAsync(new AddRequestPage(_selectedRoom, _reservationRequestManager));
        }

        // Event handler for when the "View Requests" button is clicked
        private void ViewRequestsButton_Clicked(object sender, EventArgs e)
        {
            // Check if a room is selected
            if (_selectedRoom == null)
            {
                // Display an error message if no room is selected
                DisplayAlert("Error", "Please select a room first.", "OK");
                return;
            }

            // Navigate to the ViewRequestPage and pass the selected room and reservation manager
            Navigation.PushAsync(new ViewRequestPage(_selectedRoom, _reservationRequestManager));
        }
        #endregion
    }
}
