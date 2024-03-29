using System;
using RoomReservation.BusinessLogic;

namespace RoomReservation.BusinessLogic
{
    public enum RequestStatus
    {
        Pending,
        Accepted,
        Rejected,
    }

    public class ReservationRequest
    {
        public int RequestID { get; set; }
        public string RequestedBy { get; set; }
        public string Description { get; set; }
        public DateTime MeetingDate { get; set; }
        public string MeetingPurpose { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int ParticipantCount { get; set; }
        public string RoomNumber { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public ReservationRequest(int requestID, string requestedBy, string meetingPurpose, string description, DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, string roomNumber)
        {
            RequestID = requestID;
            RequestedBy = requestedBy;
            MeetingPurpose = meetingPurpose;
            Description = description;
            MeetingDate = meetingDate;
            StartTime = startTime;
            EndTime = endTime;
            ParticipantCount = participantCount;
            RoomNumber = roomNumber;
        }

        public void UpdateRequestStatus(RequestStatus newStatus)
        {
            Status = newStatus;
        }

        public override string ToString()
        {
            // Improved date and time formatting for better readability
            return $"RequestID: {RequestID}, " +
                   $"Room: {RoomNumber}, " +
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