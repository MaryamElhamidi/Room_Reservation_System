using System;
using RoomReservation.BusinessLogic;


namespace RoomReservation.BusinessLogic
{
    public class ReservationRequestManager
    {
        #region Fields
        private List<MeetingRoom> _meetingRooms = new List<MeetingRoom>();
        private List<ReservationRequest> _reservationRequests = new List<ReservationRequest>();
        #endregion

        #region Read Only Properties
        public List<MeetingRoom> MeetingRooms
        {
            get { return _meetingRooms; }
        }

        public List<ReservationRequest> ReservationRequests
        {
            get { return _reservationRequests; }
        }
        #endregion

        #region Methods 
        public void PopulateMeetingRooms()
        {
            // Add predefined meeting rooms
            AddMeetingRoom("A101", 18, RoomLayoutType.Classroom, "classroom.webp");
            AddMeetingRoom("B102", 200, RoomLayoutType.Auditorium, "auditorium.jpeg");
            AddMeetingRoom("C103", 10, RoomLayoutType.UShape, "ushape.webp");
            AddMeetingRoom("D104", 20, RoomLayoutType.HollowSquare, "hollowsquare.jpeg");
            AddMeetingRoom("E105", 18, RoomLayoutType.Boardroom, "boardroom.jpeg");
        }

        public void AddMeetingRoom(string roomNumber, int seatingCapacity, RoomLayoutType roomLayoutType, string roomImageFileName)
        {
            //Explained the Any method in 3.A
            if (!_meetingRooms.Any(r => r.RoomNumber == roomNumber)) // Check if a meeting room with the same number already exists
            {
                _meetingRooms.Add(new MeetingRoom(roomNumber, seatingCapacity, roomLayoutType, roomImageFileName));
            }
            else
            {
                throw new Exception("A meeting room with the same room number already exists.");
            }
        }

        public bool AddReservationRequest(string requestedBy, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, MeetingRoom meetingRoom)
        {
            if (meetingRoom == null || meetingRoom.SeatingCapacity < participantCount)
            {
                return false; // Return false if the meeting room is invalid or does not have enough capacity
            }

            // Create a new reservation request and add it to the list
            var request = new ReservationRequest(requestedBy, description, meetingDate, startTime, endTime, participantCount, meetingRoom);
            _reservationRequests.Add(request);
            return true;
        }
        #endregion

        #region Bonus

        // Method to update the status of a reservation request
        public bool UpdateReservationRequestStatus(int requestId, RequestStatus newStatus)
        {
            ReservationRequest request = null;
            foreach (var requests in _reservationRequests)
            {
                if (requests.RequestID == requestId)
                {
                    request = requests;
                    break; // Exit the loop once we find the matching request
                }
            }

            if (request == null)
            {
                return false; // Return false if the request is not found
            }

            // Check if the request can be accepted without conflicts
            if (newStatus == RequestStatus.Accepted && !CanAcceptRequest(request))
            {
                return false; // Return false if there's a conflict and the request cannot be accepted
            }

            // Update the status of the request
            request.Status = newStatus;
            return true;
        }

        // Method to check if a reservation request can be accepted without conflicts
        public bool CanAcceptRequest(ReservationRequest requestToCheck)
        {
            //Explained Any method on 3.A
            return !_reservationRequests.Any(existingRequest =>
                existingRequest.MeetingRoom.RoomNumber == requestToCheck.MeetingRoom.RoomNumber &&
                existingRequest.MeetingDate == requestToCheck.MeetingDate &&
                existingRequest.Status == RequestStatus.Accepted &&
                !((requestToCheck.EndDateTime <= existingRequest.StartDateTime) || (requestToCheck.StartDateTime >= existingRequest.EndDateTime)));
        }

        // Method to cancel a reservation request
        public bool CancelReservationRequest(int requestId)
        {
            ReservationRequest requestToDelete = null;
            foreach (var request in _reservationRequests)
            {
                if (request.RequestID == requestId)
                {
                    requestToDelete = request;
                    break;
                }
            }

            if (requestToDelete != null)
            {
                _reservationRequests.Remove(requestToDelete);
                return true; // Return true if the request is successfully removed
            }
            return false; // Return false if the request is not found
        }

        // Method to update a reservation request
        public bool UpdateReservationRequest(int requestId, DateTime newMeetingDate, TimeSpan newStartTime, TimeSpan newEndTime, int newParticipantCount)
        {
            ReservationRequest requestToUpdate = null;
            foreach (var request in _reservationRequests)
            {
                if (request.RequestID == requestId)
                {
                    requestToUpdate = request;
                    break;
                }
            }

            if (requestToUpdate != null)
            {
                var meetingRoom = requestToUpdate.MeetingRoom;
                if (meetingRoom != null && meetingRoom.SeatingCapacity >= newParticipantCount)
                {
                    // Update the meeting date, start time, end time, and participant count of the request
                    requestToUpdate.MeetingDate = newMeetingDate;
                    requestToUpdate.StartTime = newStartTime;
                    requestToUpdate.EndTime = newEndTime;
                    requestToUpdate.ParticipantCount = newParticipantCount;
                    return true; // Return true if the request is successfully updated
                }
            }
            return false; // Return false if the request is not found or cannot be updated
        }

        // Method to get a reservation request by its ID
        public ReservationRequest GetReservationRequestById(int requestId)
        {
            foreach (var request in _reservationRequests)
            {
                if (request.RequestID == requestId)
                {
                    return request; // Return the request if found
                }
            }
            return null; // Return null if the request is not found
        }

        #endregion
    }
}