using System;
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
        private List<MeetingRoom> meetingRooms = new List<MeetingRoom>();
        private List<ReservationRequest> reservationRequests = new List<ReservationRequest>();
        private int nextRequestId = 1;

        // Adds a new meeting room if not already exists
        public void AddMeetingRoom(string roomNumber, int seatingCapacity, RoomLayoutType roomLayoutType, string roomImageFileName)
        {
            if (!meetingRooms.Any(r => r.RoomNumber == roomNumber))
            {
                meetingRooms.Add(new MeetingRoom(roomNumber, seatingCapacity, roomLayoutType, roomImageFileName));
            }
            else
            {
                throw new Exception("A meeting room with the same room number already exists.");
            }
        }

        public bool AddReservationRequest(int requestId, string requestedBy, string meetingPurpose, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, string roomNumber)
        {
            var meetingRoom = meetingRooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (meetingRoom == null || meetingRoom.SeatingCapacity < participantCount)
            {
                return false; // Room not found or does not have enough capacity
            }

            // Check if provided requestId is already in use
            if (reservationRequests.Any(r => r.RequestID == requestId))
            {
                requestId = nextRequestId++; // Use next available ID if provided one is in use
            }
            else
            {
                // If provided ID is unique and valid but less than nextRequestId, just use it.
                // If it's greater, adjust nextRequestId to ensure it remains sequential.
                nextRequestId = Math.Max(requestId + 1, nextRequestId);
            }

            var request = new ReservationRequest(requestId, requestedBy, meetingPurpose, description, meetingDate, startTime, endTime, participantCount, roomNumber);
            reservationRequests.Add(request);
            return true;
        }

        // Retrieves all meeting rooms
        public IEnumerable<MeetingRoom> GetAllMeetingRooms()
        {
            return meetingRooms.OrderBy(r => r.RoomNumber).ToList();
        }

        // Retrieves reservation requests by room number
        public IEnumerable<ReservationRequest> GetReservationRequestsByRoom(string roomNumber)
        {
            return reservationRequests
                .Where(request => request.RoomNumber == roomNumber)
                .OrderBy(request => request.MeetingDate)
                .ThenBy(request => request.StartTime)
                .ToList();
        }

        public IEnumerable<ReservationRequest> GetRequestsByStatus(RequestStatus status)
        {
            return reservationRequests.Where(r => r.Status == status).ToList();
        }

        public bool BulkUpdateRequestStatus(DateTime targetDate, string roomNumber, RequestStatus newStatus)
        {
            var targetedRequests = reservationRequests.Where(r => r.MeetingDate.Date == targetDate.Date && r.RoomNumber == roomNumber).ToList();
            bool conflictsExist = false;

            foreach (var request in targetedRequests)
            {
                if (newStatus == RequestStatus.Accepted)
                {
                    if (!CanAcceptRequest(request))
                    {
                        conflictsExist = true;
                        continue; // Skip conflicting requests without changing their status.
                    }
                }
                request.Status = newStatus;
            }

            return !conflictsExist; // Return true if all requests were updated without any conflict.
        }

        public bool CanAcceptRequest(ReservationRequest requestToCheck)
        {
            return !reservationRequests.Any(existingRequest =>
                existingRequest.RoomNumber == requestToCheck.RoomNumber &&
                existingRequest.MeetingDate == requestToCheck.MeetingDate &&
                existingRequest.Status == RequestStatus.Accepted &&
                !((requestToCheck.EndTime <= existingRequest.StartTime) || (requestToCheck.StartTime >= existingRequest.EndTime)));
        }

        public bool UpdateReservationRequestStatus(int requestId, RequestStatus newStatus)
        {
            var request = reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
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

        // Cancels a reservation request by request ID
        public bool CancelReservationRequest(int requestId)
        {
            var request = reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
            if (request != null)
            {
                reservationRequests.Remove(request);
                return true;
            }
            return false;
        }

        // Updates a reservation request
        public bool UpdateReservationRequest(int requestId, DateTime newMeetingDate, TimeSpan newStartTime, TimeSpan newEndTime, int newParticipantCount)
        {
            var request = reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
            if (request != null && newEndTime > newStartTime)
            {
                var meetingRoom = meetingRooms.FirstOrDefault(r => r.RoomNumber == request.RoomNumber);
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
            return reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
        }
    }
}
