using System;
namespace RoomReservation.BusinessLogic
{
    /// <summary>
    /// Represents the layout type of a meeting room.
    /// </summary>
    public enum RoomLayoutType
    {
        HollowSquare,
        UShape,
        Classroom,
        Auditorium
    }

    /// <summary>
    /// Represents the status of a reservation request.
    /// </summary>
    public enum RequestStatus
    {
        Accepted,
        Rejected,
        Pending
    }
}