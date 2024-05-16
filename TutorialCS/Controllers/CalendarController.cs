﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
using TutorialCS.Models;

namespace TutorialCS.Controllers
{
    
    public class CalendarController : Controller
    {
        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }

        class Dpc : DayPilotCalendar
        {
            CalendarDataContext dc = new CalendarDataContext();

            protected override void OnInit(InitArgs e)
            {
                UpdateWithMessage("Welcome!", CallBackUpdateType.Full);
            }

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "refresh":
                        Update();
                        break;
                }
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                var item = (from ev in dc.Events where ev.Id == Convert.ToInt32(e.Id) select ev).First();
                if (item != null)
                {
                    item.Start = e.NewStart;
                    item.End = e.NewEnd;
                    dc.SubmitChanges();                    
                }
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                var item = (from ev in dc.Events where ev.Id == Convert.ToInt32(e.Id) select ev).First();
                if (item != null)
                {
                    item.Start = e.NewStart;
                    item.End = e.NewEnd;
                    dc.SubmitChanges();
                }
            }

            protected override void OnEventDelete(EventDeleteArgs e)
            {
                var item = (from ev in dc.Events where ev.Id == Convert.ToInt32(e.Id) select ev).First();
                dc.Events.DeleteOnSubmit(item);
                dc.SubmitChanges();
                Update();
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                DataIdField = "Id";
                DataStartField = "Start";
                DataEndField = "End";
                DataTextField = "Text";

                Events = from e in dc.Events where !((e.End <= VisibleStart) || (e.Start >= VisibleEnd)) select e;
            }
        }
    }

}
