namespace Appointment.Business.Core.Results
{
    public sealed record Error(string Code, string Description)
    {
        public static readonly Error UserNotExist = new("UserNotExist", "The specified user does not exist.");
        public static readonly Error UserAlreadyExisting = new("UserAlreadyExisting", "A user with this email or phone number already exists.");
        public static readonly Error appointmentalreadyavailable = new("appointmentalreadyavailable", "You can make up to 2 appointments today.");
        public static readonly Error InvalidStartTime = new("InvalidStartTime", "StartTime must be between 09:00 and 18:00.");
        public static readonly Error InvalidEndTime = new("InvalidEndTime", "EndTime must be between 09:00 and 18:00..");
        public static readonly Error InvalidDuration = new("InvalidDuration", "Appointment duration must be between 30 minutes and 2 hours.");
        public static readonly Error AppointmentTimeConflict = new("AppointmentTimeConflict", "Appointment time conflicts with another appointment.");
        public static readonly Error MaxDailyAppointmentsReached = new("MaxDailyAppointmentsReached", "You have reached the maximum number of daily appointments.");
        public static readonly Error Appointmentnotfound = new("Appointmentnotfound", "Appointment not found.");
        public static readonly Error UserHasActiveAppoinment = new("UserHasActiveAppoinment", "User cannot be deleted because they have active appoinment.");

    }
}
