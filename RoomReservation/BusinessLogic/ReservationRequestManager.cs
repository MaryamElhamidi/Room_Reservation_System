using System;
using System.Collections.Generic;
using System.Linq;
using RoomReservation.BusinessLogic;


namespace RoomReservation.BusinessLogic
{
    public enum ReservationStatus
    {
        Pending,
        Accepted,
        Rejected
    }


    public class ReservationRequestManager
    {
        private List<MeetingRoom> _meetingRooms = new List<MeetingRoom>();
        private List<ReservationRequest> _reservationRequests = new List<ReservationRequest>();
        private int _nextRequestId = 1;

        // Adds a new meeting room if not already exists
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

        // Adds a new reservation request
        public bool AddReservationRequest(int requestId, string requestedBy, string meetingPurpose, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, string roomNumber)
        {
            var meetingRoom = _meetingRooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (meetingRoom == null || meetingRoom.SeatingCapacity < participantCount)
            {
                return false; // Room not found or does not have enough capacity
            }

            var request = new ReservationRequest(_nextRequestId++, requestedBy, meetingPurpose, description, meetingDate, startTime, endTime, participantCount, roomNumber);
            _reservationRequests.Add(request);
            return true;
        }

        // Retrieves all meeting rooms
        public IEnumerable<MeetingRoom> GetAllMeetingRooms()
        {
            return _meetingRooms.OrderBy(r => r.RoomNumber).ToList();
        }

        // Retrieves all reservation requests
        public IEnumerable<ReservationRequest> GetAllReservationRequests()
        {
            return _reservationRequests.OrderBy(r => r.RequestID).ToList();
        }

        // Retrieves reservation requests by room number
        public IEnumerable<ReservationRequest> GetReservationRequestsByRoom(string roomNumber)
        {
            return _reservationRequests.Where(request => request.RoomNumber == roomNumber).OrderBy(request => request.MeetingDate).ThenBy(request => request.StartTime).ToList();
        }

        // Retrieves reservation requests by status
        public IEnumerable<ReservationRequest> GetRequestsByStatus(RequestStatus status)
        {
            return _reservationRequests.Where(r => r.Status == status).ToList();
        }

        // Updates the status of a reservation request
        public bool UpdateReservationRequestStatus(int requestId, RequestStatus newStatus)
        {
            var request = _reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
            if (request == null)
            {
                return false; // Request not found
            }

            // If the request is to be accepted, check for conflicts
            if (newStatus == RequestStatus.Accepted && !CanAcceptRequest(request))
            {
                return false; // Conflict found, cannot accept the request
            }

            // Update the status as no conflicts were found or the request is being rejected
            request.Status = newStatus;
            return true;
        }

        // Checks if a reservation request can be accepted without conflicts
        public bool CanAcceptRequest(ReservationRequest requestToCheck)
        {
            return !_reservationRequests.Any(existingRequest =>
                existingRequest.RoomNumber == requestToCheck.RoomNumber &&
                existingRequest.MeetingDate == requestToCheck.MeetingDate &&
                existingRequest.Status == RequestStatus.Accepted &&
                !((requestToCheck.EndTime <= existingRequest.StartTime) || (requestToCheck.StartTime >= existingRequest.EndTime)));
        }

        // Cancels a reservation request by request ID
        public bool CancelReservationRequest(int requestId)
        {
            var request = _reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
            if (request != null)
            {
                _reservationRequests.Remove(request);
                return true;
            }
            return false;
        }

        // Updates a reservation request
        public bool UpdateReservationRequest(int requestId, DateTime newMeetingDate, TimeSpan newStartTime, TimeSpan newEndTime, int newParticipantCount)
        {
            var request = _reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
            if (request != null && newEndTime > newStartTime)
            {
                var meetingRoom = _meetingRooms.FirstOrDefault(r => r.RoomNumber == request.RoomNumber);
                if (meetingRoom != null && meetingRoom.SeatingCapacity >= newParticipantCount)
                {
                    request.MeetingDate = newMeetingDate;
                    request.StartTime = newStartTime;
                    request.EndTime = newEndTime;
                    request.ParticipantCount = newParticipantCount;
                    return true;
                }
            }
            return false;
        }

        // Retrieves a specific reservation request by request ID
        public ReservationRequest GetReservationRequestById(int requestId)
        {
            return _reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
        }
    }
}
