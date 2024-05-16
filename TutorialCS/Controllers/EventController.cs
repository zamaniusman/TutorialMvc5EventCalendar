using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using DayPilot.Web.Mvc.Json;
using TutorialCS.Models;

namespace TutorialCS.Controllers
{
    public class EventController : Controller
    {
        CalendarDataContext dc = new CalendarDataContext();

        public ActionResult Edit(string id)
        {
            var ids = Convert.ToInt32(id);
            var t = (from tr in dc.Events where tr.Id == ids select tr).First();
            var ev = new EventData
            {
                Id = t.Id,
                Start = t.Start,
                End = t.End,
                Text = t.Text
            };
            return View(ev);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(FormCollection form)
        {
            int id = Convert.ToInt32(form["Id"]);
            DateTime start = Convert.ToDateTime(form["Start"]);
            DateTime end = Convert.ToDateTime(form["End"]);
            string text = form["Text"];

            var record = (from e in dc.Events where e.Id == id select e).First();
            record.Start = start;
            record.End = end;
            record.Text = text;
            dc.SubmitChanges();

            return JavaScript(SimpleJsonSerializer.Serialize("OK"));
        }


        public ActionResult Create()
        {
            return View(new EventData
            {
                Start = Convert.ToDateTime(Request.QueryString["start"]),
                End = Convert.ToDateTime(Request.QueryString["end"])
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection form)
        {
            DateTime start = Convert.ToDateTime(form["Start"]);
            DateTime end = Convert.ToDateTime(form["End"]);
            string text = form["Text"];
            int resource = Convert.ToInt32(form["Resource"]);
            //string recurrence = form["Recurrence"];

            var toBeCreated = new Event() { Start = start, End = end, Text = text};
            dc.Events.InsertOnSubmit(toBeCreated);
            dc.SubmitChanges();

            return JavaScript(SimpleJsonSerializer.Serialize("OK"));
        }

        public class EventData
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public SelectList Resource { get; set; }
            public string Text { get; set; }
        }

    }
}
