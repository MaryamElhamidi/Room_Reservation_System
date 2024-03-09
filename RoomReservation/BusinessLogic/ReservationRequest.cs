using System;
using RoomReservation.BusinessLogic;

public class ReservationRequest
{
    public int RequestID { get; private set; }

    private static int requestCounter = 0; // Static counter for generating request IDs.

    public string RequestedBy
    {
        get => _requestedBy;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("RequestedBy cannot be blank.");
            }
            _requestedBy = value;
        }
    }
    private string _requestedBy;

    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Description cannot be blank.");
            }
            _description = value;
        }
    }
    private string _description;

    public DateTime StartDateTime
    {
        get => _startDateTime;
        set
        {
            if (value <= DateTime.Now)
            {
                throw new ArgumentException("StartDateTime must be a future date.");
            }
            _startDateTime = value;
        }
    }
    private DateTime _startDateTime;

    public DateTime EndDateTime
    {
        get => _endDateTime;
        set
        {
            if (value <= StartDateTime)
            {
                throw new ArgumentException("EndDateTime must be greater than StartDateTime.");
            }
            _endDateTime = value;
        }
    }
    private DateTime _endDateTime;

    public int ParticipantCount
    {
        get => _participantCount;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("ParticipantCount must be greater than 0.");
            }
            _participantCount = value;
        }
    }
    private int _participantCount;

    public RequestStatus Status { get; set; }

    public MeetingRoom AssociatedRoom { get; set; }


 
    public ReservationRequest(string requestedBy, string description, DateTime startDateTime, DateTime endDateTime, int participantCount, MeetingRoom associatedRoom)
    {
        RequestedBy = requestedBy;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        ParticipantCount = participantCount;
        Status = RequestStatus.Pending;
        AssociatedRoom = associatedRoom;

        RequestID = GenerateNextRequestID();
    }

   

    private int GenerateNextRequestID()
    {
        return ++requestCounter;

    }

}

