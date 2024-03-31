using System;
using RoomReservation.BusinessLogic;

namespace RoomReservation.BusinessLogic
{


    public class ReservationRequest
    {
        private static int _lastAssignedId = 0;
        private string _requestedBy;
        private string _description;
        private DateTime _meetingDate;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private int _participantCount;
        private MeetingRoom _meetingRoom;

        public int RequestID { get; private set; }
        public string RequestedBy
        {
            get { return _requestedBy; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("You must provide a request name.");
                }
                _requestedBy = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("You must provide a description.");
                }
                _description = value;
            }
        }

        public DateTime MeetingDate
        {
            get { return _meetingDate; }
            set
            {
                if (value < DateTime.Today)
                {
                    throw new ArgumentException("The meeting date must be in the future.");
                }
                _meetingDate = value;
            }
        }

        // Combining StartDate and StartTime for full DateTime
        public DateTime StartDateTime => MeetingDate.Add(StartTime);

        // Combining MeetingDate and EndTime for full DateTime
        public DateTime EndDateTime => MeetingDate.Add(EndTime);

        public TimeSpan StartTime
        {
            get { return _startTime; }
            set { _startTime = value; } 
        }

        public TimeSpan EndTime
        {
            get { return _endTime; }
            set { _endTime = value; } 
        }

        public int ParticipantCount
        {
            get { return _participantCount; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The participant count must be greater than 0");
                }
                _participantCount = value;
            }
        }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public MeetingRoom MeetingRoom
        {
            get { return _meetingRoom; }
            set
            {
                _meetingRoom = value ?? throw new ArgumentException("You must provide a valid meeting room.");
            }
        }

        public ReservationRequest(string requestedBy, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, MeetingRoom meetingRoom)
        {
            RequestID = ++_lastAssignedId;
            RequestedBy = requestedBy;
            Description = description;
            MeetingDate = meetingDate;

            // Validate combined DateTime for start and end
            var startDateTime = meetingDate.Add(startTime);
            var endDateTime = meetingDate.Add(endTime);
            if (startDateTime >= endDateTime)
            {
                throw new ArgumentException("End time must be after start time.");
            }

            StartTime = startTime;
            EndTime = endTime;
            ParticipantCount = participantCount;
            MeetingRoom = meetingRoom;
        }
    


        public void UpdateRequestStatus(RequestStatus newStatus)
        {
            Status = newStatus;
        }

        public override string ToString()
        {
            return $"RequestID: {RequestID}, " +
                   $"RequestedBy: {RequestedBy}, " +
                   $"Description: {Description}, " +
                   $"MeetingDate: {MeetingDate.ToString("yyyy-MM-dd")}, " +
                   $"StartTime: {StartTime.ToString(@"hh\:mm")}, " +
                   $"EndTime: {EndTime.ToString(@"hh\:mm")}, " +
                   $"ParticipantCount: {ParticipantCount}, " +
                   $"Status: {Status}";
        }
    }
}