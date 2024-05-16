@imports DayPilot.Web.Mvc
@imports DayPilot.Web.Mvc.Events.Calendar
@imports ViewType = DayPilot.Web.Mvc.Enums.Calendar.ViewType

@Code
    ViewBag.Title = "ASP.NET MVC 5 Event Calendar"
End Code

<script src="@Url.Content("~/Scripts/DayPilot/daypilot-all.min.js")" type="text/javascript"></script>

@Html.DayPilotCalendar("dp", New DayPilotCalendarConfig With
{
    .BackendUrl = Url.Action("Backend", "Calendar"),
    .ViewType = ViewType.Week,
    .TimeRangeSelectedHandling = TimeRangeSelectedHandlingType.JavaScript,
    .TimeRangeSelectedJavaScript = "create(start, end)",
    .EventClickHandling = EventClickHandlingType.JavaScript,
    .EventClickJavaScript = "edit(e)",
    .EventMoveHandling = EventMoveHandlingType.Notify,
    .EventResizeHandling = EventResizeHandlingType.Notify,
    .EventDeleteHandling = EventDeleteHandlingType.CallBack
})

<script type="text/javascript">

    function create(start, end) {
        var m = new DayPilot.Modal();
        m.closed = function () {
            if (this.result == "OK") {
                dp.commandCallBack('refresh');
            }
            dp.clearSelection();
        };
        m.showUrl('@Url.Action("Create", "Event")?start=' + start + '&end=' + end);
    }

    function edit(e) {
        var m = new DayPilot.Modal();
        m.closed = function () {
            if (this.result == "OK") {
                dp.commandCallBack('refresh');
            }
            dp.clearSelection();
        };
        m.showUrl('@Url.Action("Edit", "Event")/' + e.id());
    }

</script>