using System;
using RoomReservation.BusinessLogic;


namespace RoomReservation.BusinessLogic
{
    public class ReservationRequestManager
    {
        private List<MeetingRoom> _meetingRooms = new List<MeetingRoom>();
        private List<ReservationRequest> _reservationRequests = new List<ReservationRequest>();

        public void PopulateMeetingRooms()
        {
            AddMeetingRoom("A101", 18, RoomLayoutType.Classroom, "classroom.webp");
            AddMeetingRoom("B102", 200, RoomLayoutType.Auditorium, "auditorium.jpeg");
            AddMeetingRoom("C103", 10, RoomLayoutType.UShape, "ushape.webp");
            AddMeetingRoom("D104", 20, RoomLayoutType.HollowSquare, "hollowsquare.jpeg");
            AddMeetingRoom("E105", 18, RoomLayoutType.Boardroom, "boardroom.jpeg");
        }

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

        // Adjusted to match the updated ReservationRequest constructor
        public bool AddReservationRequest(string requestedBy, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, MeetingRoom meetingRoom)
        {
            if (meetingRoom == null || meetingRoom.SeatingCapacity < participantCount)
            {
                return false; // Room not found or does not have enough capacity
            }

            var request = new ReservationRequest(requestedBy, description, meetingDate, startTime, endTime, participantCount, meetingRoom);
            _reservationRequests.Add(request);
            return true;
        }
        public IEnumerable<MeetingRoom> GetAllMeetingRooms()
        {
            return _meetingRooms.OrderBy(r => r.RoomNumber).ToList();
        }

        public IEnumerable<ReservationRequest> GetAllReservationRequests()
        {
            return _reservationRequests.OrderBy(r => r.RequestID).ToList();
        }

        // Adjusted methods that relied on 'RoomNumber' to use 'MeetingRoom.RoomNumber'
        public IEnumerable<ReservationRequest> GetReservationRequestsByRoom(string roomNumber)
        {
            return _reservationRequests.Where(request => request.MeetingRoom.RoomNumber == roomNumber).OrderBy(request => request.MeetingDate).ThenBy(request => request.StartDateTime).ToList();
        }

        public bool UpdateReservationRequestStatus(int requestId, RequestStatus newStatus)
        {
            var request = _reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
            if (request == null)
            {
                return false; // Request not found
            }

            if (newStatus == RequestStatus.Accepted && !CanAcceptRequest(request))
            {
                return false; // Conflict found, cannot accept the request
            }

            request.Status = newStatus;
            return true;
        }

        // Checks if a reservation request can be accepted without conflicts, now checks against MeetingRoom
        public bool CanAcceptRequest(ReservationRequest requestToCheck)
        {
            return !_reservationRequests.Any(existingRequest =>
                existingRequest.MeetingRoom.RoomNumber == requestToCheck.MeetingRoom.RoomNumber &&
                existingRequest.MeetingDate == requestToCheck.MeetingDate &&
                existingRequest.Status == RequestStatus.Accepted &&
                !((requestToCheck.EndDateTime <= existingRequest.StartDateTime) || (requestToCheck.StartDateTime >= existingRequest.EndDateTime)));
        }

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

        // Updated to reflect DateTime changes
        public bool UpdateReservationRequest(int requestId, DateTime newMeetingDate, TimeSpan newStartTime, TimeSpan newEndTime, int newParticipantCount)
        {
            var request = _reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
            if (request != null)
            {
                var meetingRoom = request.MeetingRoom; // Direct reference to MeetingRoom
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

        public ReservationRequest GetReservationRequestById(int requestId)
        {
            return _reservationRequests.FirstOrDefault(r => r.RequestID == requestId);
        }
    }
}
