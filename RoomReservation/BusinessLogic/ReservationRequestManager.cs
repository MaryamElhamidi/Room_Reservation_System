using System;
using RoomReservation.BusinessLogic;


namespace RoomReservation.BusinessLogic
{
    public class ReservationRequestManager
    {
        private static ReservationRequestManager _instance;

        private List<MeetingRoom> _meetingRooms = new List<MeetingRoom>();
        private List<ReservationRequest> _reservationRequests = new List<ReservationRequest>();

        public List<MeetingRoom> MeetingRooms
        {
            get { return _meetingRooms; }
        }
        // Private constructor to prevent instantiation outside this class
        private ReservationRequestManager() { }

        // Public static method to get the instance
        public static ReservationRequestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReservationRequestManager();
                }
                return _instance;
            }
        }
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

        public bool AddReservationRequest(string requestedBy, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, MeetingRoom meetingRoom)
        {
            if (meetingRoom == null || meetingRoom.SeatingCapacity < participantCount)
            {
                return false; 
            }

            var request = new ReservationRequest(requestedBy, description, meetingDate, startTime, endTime, participantCount, meetingRoom);
            _reservationRequests.Add(request);
            return true;
        }

        public IEnumerable<ReservationRequest> GetAllReservationRequests()
        {
            return _reservationRequests.OrderBy(r => r.RequestID).ToList();
        }

        #region Bonus
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
        #endregion

    }

}