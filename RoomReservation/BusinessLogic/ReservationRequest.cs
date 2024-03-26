using System;
using RoomReservation.BusinessLogic;

/// <summary>
/// Represents a reservation request for a meeting room.
/// </summary>
public class ReservationRequest
{
    private static int _nextId = 1;
    private string _requestedBy;
    private string _description;
    private DateTime _startDateTime;
    private DateTime _endDateTime;
    private int _participantCount;
    public int RequestID { get; private set; }

    /// <summary>
    /// The name of the person who posted the request. Cannot be blank.
    /// </summary>
    public string RequestedBy
    {
        get => _requestedBy;
        set => _requestedBy = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Requested by name cannot be blank.");
    }

    /// <summary>
    /// A description of the purpose of the meeting. Cannot be blank.
    /// </summary>
    public string Description
    {
        get => _description;
        set => _description = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Description cannot be blank.");
    }

    /// <summary>
    /// The start date and time of the meeting. Must be in the future.
    /// </summary>
    public DateTime StartDateTime
    {
        get => _startDateTime;
        set => _startDateTime = value > DateTime.Now ? value : throw new ArgumentException("Start date time must be in the future.");
    }

    /// <summary>
    /// The date and time when the meeting is scheduled to end. Must be greater than the start date time.
    /// </summary>
    public DateTime EndDateTime
    {
        get => _endDateTime;
        set => _endDateTime = value > _startDateTime ?

