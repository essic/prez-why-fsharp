namespace ClockOrDie.WebApi;

public record ActivityDescription(string Description, string[]? Tags);

public record Activity(long ActivityId, string ActivityLabel);

public record ActivityTracking(long ActivityId, string ActivityLabel, double TimeTracked);

public record TimeSheetCell(string Date, bool Validated, ActivityTracking[] Activities);

public record TimeSheet(int WeekNumber, int WeekYear, TimeSheetCell[] TimeCells);

public record ApiError(string Code, string Message);
