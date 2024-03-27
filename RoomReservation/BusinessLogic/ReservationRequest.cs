using System;
using RoomReservation.BusinessLogic;

/// <summary>
/// Represents a reservation request for a meeting room.
/// </summary>
public class ReservationRequest
{
    // Public properties with unconventional naming.
    public int _requestID { get; set; }
    public string _requestedBy { get; set; }
    public string _description { get; set; }
    public DateTime _meetingDate { get; set; }
    public string _meetingPurpose { get; set; }
    public TimeSpan _startTime { get; set; }
    public TimeSpan _endTime { get; set; }
    public int _participantCount { get; set; }
    public string _roomNumber { get; set; }
    public RequestStatus _status { get; set; } = RequestStatus.Pending; // Default value

    /// <summary>
    /// Constructor to initialize a new instance of the ReservationRequest class.
    /// </summary>
    public ReservationRequest(int requestID, string requestedBy, string meetingPurpose, string description,
        DateTime meetingDate, TimeSpan startTime, TimeSpan endTime, int participantCount, string roomNumber)
    {
        _requestID = requestID;
        _requestedBy = requestedBy;
        _meetingPurpose = meetingPurpose;
        _description = description;
        _meetingDate = meetingDate;
        _startTime = startTime;
        _endTime = endTime;
        _participantCount = participantCount;
        _roomNumber = roomNumber;
    }

    /// <summary>
    /// Updates the status of the reservation request.
    /// </summary>
    /// <param name="newStatus">The new status to be set.</param>
    public void UpdateRequestStatus(RequestStatus newStatus)
    {
        _status = newStatus;
    }

    /// <summary>
    /// Provides a string representation of the reservation request, suitable for logging or debugging.
    /// </summary>
    /// <returns>A string detailing the reservation request's properties.</returns>
    public override string ToString()
    {
        return $"RequestID: {_requestID}, " +
               $"Room: {_roomNumber}, " +
               $"RequestedBy: {_requestedBy}, " +
               $"Description: {_description}, " +
               $"MeetingDate: {_meetingDate.ToString("yyyy-MM-dd")}, " +
               $"StartTime: {_startTime.ToString(@"hh\:mm")}, " +
               $"EndTime: {_endTime.ToString(@"hh\:mm")}, " +
               $"ParticipantCount: {_participantCount}, " +
               $"Status: {_status}";
    }
}