﻿using System;
using RoomReservation.BusinessLogic;

namespace RoomReservation.BusinessLogic
{
    public class ReservationRequest
    {
        #region Fields
        private static int _lastAssignedId = 0; //Explained static int on 1.A
        private string _requestedBy;
        private string _description;
        private DateTime _meetingDate;
        private TimeSpan _startTime; //Explained TimeSpan on 2.A
        private TimeSpan _endTime;
        private int _participantCount;
        private MeetingRoom _meetingRoom;
        #endregion

        #region Properties
        public int RequestID { get => _lastAssignedId; init { } }

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

        // Properties to combine date and time for full DateTime
        public DateTime StartDateTime => MeetingDate.Add(StartTime);
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

        // Property for request status
        // Initialized to the value "RequestStatus.Pending", so that by default, the status of an instance of the class will be set to "Pending" unless explicitly changed.
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public MeetingRoom MeetingRoom
        {
            get { return _meetingRoom; }
            set
            {
                //Checks if it is nullable.
                _meetingRoom = value ?? throw new ArgumentException("You must provide a valid meeting room.");
            }
        }
        #endregion

        #region Constructor
        public ReservationRequest(string requestedBy, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, MeetingRoom meetingRoom)
        {
            RequestID = ++_lastAssignedId;
            RequestedBy = requestedBy;
            Description = description;
            MeetingDate = meetingDate;
            StartTime = startTime;
            EndTime = endTime;
            ParticipantCount = participantCount;
            MeetingRoom = meetingRoom;
        }
        #endregion

        #region Methods
        // Method to update request status
        public void UpdateRequestStatus(RequestStatus newStatus)
        {
            Status = newStatus;
        }

        // Method to provide string representation of the reservation request
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
        #endregion
    }
}
