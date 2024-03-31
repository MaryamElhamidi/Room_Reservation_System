using System;
using RoomReservation.BusinessLogic;


namespace RoomReservation.BusinessLogic
{
    public class ReservationRequestManager
    {
        private List<MeetingRoom> _meetingRooms = new List<MeetingRoom>();
        private List<ReservationRequest> _reservationRequests = new List<ReservationRequest>();

        public List<MeetingRoom> MeetingRooms
        {
            get { return _meetingRooms; }
        }
        public List<ReservationRequest> ReservationRequests
        { 
            get { return _reservationRequests; } 
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
            if (!_meetingRooms.Any(r => r.RoomNumber == roomNumber)) //Explained on 1.A in Report
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


        #region Bonus
        public bool UpdateReservationRequestStatus(int requestId, RequestStatus newStatus)
        {
            ReservationRequest request = null;
            foreach (var r in _reservationRequests)
            {
                if (r.RequestID == requestId)
                {
                    request = r;
                    break; // Exit the loop once we find the matching request
                }
            }

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
                return true;
            }
            return false;
        }

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
                    requestToUpdate.MeetingDate = newMeetingDate;
                    requestToUpdate.StartTime = newStartTime;
                    requestToUpdate.EndTime = newEndTime;
                    requestToUpdate.ParticipantCount = newParticipantCount;
                    return true;
                }
            }
            return false;
        }

        public ReservationRequest GetReservationRequestById(int requestId)
        {
            foreach (var request in _reservationRequests)
            {
                if (request.RequestID == requestId)
                {
                    return request;
                }
            }
            return null;
        }
        #endregion

    }

}