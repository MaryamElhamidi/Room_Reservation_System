# 📅 Room Reservation System (.NET MAUI)

A cross-platform .NET MAUI application designed to allow users to view, add, and manage meeting room reservations. This project simulates a simple, interactive scheduling system with user input validation and basic conflict management.

## 🚀 Features

* 📋 View all available meeting rooms
* 📝 Add a reservation request for a selected room
* 📄 View reservation requests per room
* ✅ Accept or reject reservation requests
* 🔄 Update or cancel existing requests
* ⚠️ Handles date/time conflicts and validates participant limits

## 🧱 Tech Stack

* **.NET MAUI** (Multi-platform App UI)
* **C#**
* **XAML** for UI
* **MVU-style Business Logic**, no external backend

## 🗂️ Project Structure

```
RoomReservation/
├── BusinessLogic/
│   ├── MeetingRoom.cs
│   ├── ReservationRequest.cs
│   ├── ReservationRequestManager.cs
│   ├── RoomLayoutType.cs
│   └── RequestStatus.cs
├── Pages/
│   ├── PickRoomPage.xaml.cs
│   ├── AddRequestPage.xaml.cs
│   └── ViewRequestPage.xaml.cs
└── App.xaml / MainPage.xaml
```

## 🧠 Core Classes

* **MeetingRoom**: Represents a reservable room, including number, layout, and capacity.
* **ReservationRequest**: Contains user-submitted request info (e.g., description, time, participants).
* **ReservationRequestManager**: Manages the logic around adding, validating, updating, and canceling reservations.
* **Pages**:

  * `PickRoomPage`: Start screen where users select a room.
  * `AddRequestPage`: Form to submit a reservation.
  * `ViewRequestPage`: Shows existing reservations for a selected room.

## 🛠️ How to Run

1. Open the solution in **Visual Studio 2022+** with **.NET MAUI workload installed**
2. Build and run the solution on:

   * Windows (WinUI)
   * Android Emulator (like Google Pixel 5)

> Note: No external database or cloud connection is used — all data is in-memory only during runtime.

## 📌 To-Do / Future Improvements

* Persist reservations using a local SQLite database
* Add date/time pickers with better UX
* Add authentication and user roles (e.g., Admin vs. Requester)
* Unit tests for the `BusinessLogic` layer

---
