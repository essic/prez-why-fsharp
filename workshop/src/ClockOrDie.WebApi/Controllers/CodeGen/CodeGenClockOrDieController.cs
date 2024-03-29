//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v11.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"

namespace ClockOrDie.WebApi.Controllers.CodeGen
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v11.0.0.0))")]
    public interface ICodeGenClockOrDieController
    {
        /// <summary>Save activities tracking, for later validation.</summary>
        /// <returns>Saved</returns>
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveActivitiesTrackingAsync(System.Collections.Generic.IEnumerable<TimeSheetCell> content);
    
        /// <summary>Ask for the timesheet for the current week</summary>
        /// <returns>Weekly time sheets with activities status</returns>
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<TimeSheet>> GetTrackingForCurrentWeekAsync();
    
        /// <summary>Ask for the weekly timesheet for the given date.</summary>
        /// <returns>Weekly time sheets with activities status</returns>
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<TimeSheet>> GetTrackingForWeekAsync(int weekNumber, int weekYear);
    
        /// <summary>Get activities for connected used.</summary>
        /// <returns>List of activities for the user</returns>
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.ICollection<Activity>>> GetActivitiesForUserAsync();
    
        /// <summary>Add or update activities.</summary>
        /// <returns>Created</returns>
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<Activity>> CreateActivityAsync(string activityName, ActivityDescription body);
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class CodeGenClockOrDieController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private ICodeGenClockOrDieController _implementation;
    
        public CodeGenClockOrDieController(ICodeGenClockOrDieController implementation)
        {
            _implementation = implementation;
        }
    
        /// <summary>Save activities tracking, for later validation.</summary>
        /// <returns>Saved</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("activity-tracking/save")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveActivitiesTracking([Microsoft.AspNetCore.Mvc.FromBody] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] System.Collections.Generic.IEnumerable<TimeSheetCell> body)
        {
            return _implementation.SaveActivitiesTrackingAsync(body);
        }
    
        /// <summary>Ask for the timesheet for the current week</summary>
        /// <returns>Weekly time sheets with activities status</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("timessheets/week/current")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<TimeSheet>> GetTrackingForCurrentWeek()
        {
            return _implementation.GetTrackingForCurrentWeekAsync();
        }
    
        /// <summary>Ask for the weekly timesheet for the given date.</summary>
        /// <returns>Weekly time sheets with activities status</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("timessheets/week/{week_number}/{week_year}")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<TimeSheet>> GetTrackingForWeek([Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] int week_number, [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] int week_year)
        {
            return _implementation.GetTrackingForWeekAsync(week_number, week_year);
        }
    
        /// <summary>Get activities for connected used.</summary>
        /// <returns>List of activities for the user</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("timesheets/activities")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.ICollection<Activity>>> GetActivitiesForUser()
        {
            return _implementation.GetActivitiesForUserAsync();
        }
    
        /// <summary>Add or update activities.</summary>
        /// <returns>Created</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("admin/activities/{activity_name}")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<Activity>> CreateActivity([Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] string activity_name, [Microsoft.AspNetCore.Mvc.FromBody] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] ActivityDescription body)
        {
            return _implementation.CreateActivityAsync(activity_name, body);
        }
    
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ActivityDescription 
    {
        [Newtonsoft.Json.JsonProperty("description", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
    
        [Newtonsoft.Json.JsonProperty("tags", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<string> Tags { get; set; }
    
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
        public static ActivityDescription FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityDescription>(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Activity 
    {
        [Newtonsoft.Json.JsonProperty("activityLabel", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string ActivityLabel { get; set; }
    
        [Newtonsoft.Json.JsonProperty("activityId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1D, double.MaxValue)]
        public long ActivityId { get; set; }
    
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
        public static Activity FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Activity>(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ActivityTracking 
    {
        [Newtonsoft.Json.JsonProperty("activityLabel", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string ActivityLabel { get; set; }
    
        [Newtonsoft.Json.JsonProperty("activityId", Required = Newtonsoft.Json.Required.Always)]
        public int ActivityId { get; set; }
    
        [Newtonsoft.Json.JsonProperty("timeTracked", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(0.25D, 1D)]
        public double TimeTracked { get; set; }
    
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
        public static ActivityTracking FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityTracking>(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TimeSheetCell 
    {
        [Newtonsoft.Json.JsonProperty("date", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Date { get; set; }
    
        [Newtonsoft.Json.JsonProperty("validated", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Validated { get; set; }
    
        [Newtonsoft.Json.JsonProperty("activities", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.List<ActivityTracking> Activities { get; set; } = new System.Collections.Generic.List<ActivityTracking>();
    
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
        public static TimeSheetCell FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TimeSheetCell>(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TimeSheet 
    {
        [Newtonsoft.Json.JsonProperty("weekNumber", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(1, 52)]
        public int WeekNumber { get; set; }
    
        [Newtonsoft.Json.JsonProperty("weekYear", Required = Newtonsoft.Json.Required.Always)]
        public int WeekYear { get; set; }
    
        [Newtonsoft.Json.JsonProperty("timeCells", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.List<TimeSheetCell> TimeCells { get; set; } = new System.Collections.Generic.List<TimeSheetCell>();
    
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
        public static TimeSheet FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TimeSheet>(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Error 
    {
        [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Code { get; set; }
    
        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Message { get; set; }
    
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
        public static Error FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Error>(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Errors : System.Collections.ObjectModel.Collection<Error>
    {
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
        public static Errors FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Errors>(data, new Newtonsoft.Json.JsonSerializerSettings());
        }
    
    }

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108
#pragma warning restore 3016