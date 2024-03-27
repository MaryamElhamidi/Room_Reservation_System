using System;
namespace RoomReservation.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages collections of MeetingRoom and ReservationRequest instances.
    /// </summary>
    public class ReservationRequestManager
    {
        private List<MeetingRoom> _meetingRooms = new List<MeetingRoom>();
        private List<ReservationRequest> _reservationRequests = new List<ReservationRequest>();
        private int _nextRequestId = 1;

        /// <summary>
        /// Adds a new MeetingRoom if one with the same room number does not already exist.
        /// </summary>
        public void AddMeetingRoom(string roomNumber, int seatingCapacity, RoomLayoutType roomLayoutType, string roomImageFileName)
        {
            if (!_meetingRooms.Any(r => r.RoomNumber == roomNumber))
            {
                _meetingRooms.Add(new MeetingRoom(roomNumber, seatingCapacity, roomLayoutType, roomImageFileName));
            }
            else
            {
                throw new Exception("A meeting room with the same room number already exists.");
            }
        }

        /// <summary>
        /// Attempts to add a new ReservationRequest. Automatically assigns the next request ID.
        /// </summary>
        public bool AddReservationRequest(string requestedBy, string meetingPurpose, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, string roomNumber)
        {
            var meetingRoom = _meetingRooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (meetingRoom == null || meetingRoom.SeatingCapacity < participantCount)
            {
                return false; // Room not found or insufficient capacity.
            }

            var requestId = _nextRequestId++;
            var request = new ReservationRequest(requestId, requestedBy, meetingPurpose, description, meetingDate, startTime, endTime, participantCount, roomNumber);
            _reservationRequests.Add(request);
            return true;
        }

        /// <summary>
        /// Retrieves all meeting rooms sorted by room number.
        /// </summary>
        public IEnumerable<MeetingRoom> GetAllMeetingRooms() => _meetingRooms.OrderBy(r => r.RoomNumber).ToList();

        /// <summary>
        /// Retrieves all reservation requests for a specific room, ordered by meeting date and start time.
        /// </summary>
        public IEnumerable<ReservationRequest> GetReservationRequestsByRoom(string roomNumber) => _reservationRequests.Where(request => request._roomNumber == roomNumber).OrderBy(request => request._meetingDate).ThenBy(request => request._startTime).ToList();

        /// <summary>
        /// Retrieves all reservation requests with a specific status.
        /// </summary>
        public IEnumerable<ReservationRequest> GetRequestsByStatus(RequestStatus status) => _reservationRequests.Where(r => r._status == status).ToList();

        /// <summary>
        /// Attempts to update the status of all requests for a given date and room to a new status, considering potential conflicts for accepted requests.
        /// </summary>
        public bool BulkUpdateRequestStatus(DateTime targetDate, string roomNumber, RequestStatus newStatus)
        {
            var targetedRequests = _reservationRequests.Where(r => r._meetingDate.Date == targetDate.Date && r._roomNumber == roomNumber).ToList();
            bool conflictsExist = false;

            foreach (var request in targetedRequests)
            {
                if (newStatus == RequestStatus.Accepted && !CanAcceptRequest(request))
                {
                    conflictsExist = true;
                    continue; // Skip this request to avoid conflicts.
                }
                request.UpdateRequestStatus(newStatus);
            }

            return !conflictsExist;
        }

        /// <summary>
        /// Checks if a request can be accepted without causing scheduling conflicts.
        /// </summary>
        private bool CanAcceptRequest(ReservationRequest requestToCheck) => !_reservationRequests.Any(existingRequest => existingRequest._roomNumber == requestToCheck._roomNumber && existingRequest._meetingDate == requestToCheck._meetingDate && existingRequest._status == RequestStatus.Accepted && !((requestToCheck._endTime <= existingRequest._startTime) || (requestToCheck._startTime >= existingRequest._endTime)));

        /// <summary>
        /// Updates the status of a specific reservation request.
        /// </summary>
        public bool UpdateReservationRequestStatus(int requestId, RequestStatus newStatus)
        {
            var request = _reservationRequests.FirstOrDefault(r => r._requestID == requestId);
            if (request == null || (newStatus == RequestStatus.Accepted && !CanAcceptRequest(request)))
            {
                return false; // Request not found or conflict detected.
            }

            request.UpdateRequestStatus(newStatus);
            return true;
        }

        /// <summary>
        /// Cancels a specific reservation request by its ID.
        /// </summary>
        public bool CancelReservationRequest(int requestId)
        {
            var request = _reservationRequests.FirstOrDefault(r => r._requestID == requestId);
            if (request != null)
            {
                _reservationRequests.Remove(request);
                return true; // Successfully canceled the reservation request.
            }
            return false; // No request found with the specified ID.
        }

        /// <summary>
        /// Updates the details of an existing reservation request.
        /// </summary>
        public bool UpdateReservationRequest(int requestId, DateTime newMeetingDate, TimeSpan newStartTime, TimeSpan newEndTime, int newParticipantCount)
        {
            var request = _reservationRequests.FirstOrDefault(r => r._requestID == requestId);
            if (request == null)
            {
                return false; // Request not found.
            }

            var meetingRoom = _meetingRooms.FirstOrDefault(r => r.RoomNumber == request._roomNumber);
            if (meetingRoom == null || meetingRoom.SeatingCapacity < newParticipantCount)
            {
                return false; // Room not found or insufficient capacity.
            }

            if (newEndTime <= newStartTime)
            {
                return false; // Invalid time range.
            }

            // Update the request details.
            request._meetingDate = newMeetingDate;
            request._startTime = newStartTime;
            request._endTime = newEndTime;
            request._participantCount = newParticipantCount;

            return true;
        }

        /// <summary>
        /// Retrieves a specific reservation request by its ID.
        /// </summary>
        public ReservationRequest GetReservationRequestById(int requestId) => _reservationRequests.FirstOrDefault(r => r._requestID == requestId);
    }


}