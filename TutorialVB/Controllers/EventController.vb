Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Web.Mvc
Imports DayPilot.Web.Mvc.Json

Namespace TutorialVB.Controllers
    Public Class EventController
        Inherits Controller

        Private dc As New CalendarDataContext()

        Public Function Edit(ByVal id As String) As ActionResult
            Dim ids = Convert.ToInt32(id)
            Dim t = ( _
                From tr In dc.Events _
                Where tr.Id = ids _
                Select tr).First()
            Dim ev = New EventData With {.Id = t.Id, .Start = t.Start, .End = t.End, .Text = t.Text}
            Return View(ev)
        End Function

        <AcceptVerbs(HttpVerbs.Post)> _
        Public Function Edit(ByVal form As FormCollection) As ActionResult
            Dim id As Integer = Convert.ToInt32(form("Id"))
            Dim start As Date = Convert.ToDateTime(form("Start"))
            Dim [end] As Date = Convert.ToDateTime(form("End"))
            Dim text As String = form("Text")

            Dim record = ( _
                From e In dc.Events _
                Where e.Id = id _
                Select e).First()
            record.Start = start
            record.End = [end]
            record.Text = text
            dc.SubmitChanges()

            Return JavaScript(SimpleJsonSerializer.Serialize("OK"))
        End Function


        Public Function Create() As ActionResult
            Return View(New EventData With {.Start = Convert.ToDateTime(Request.QueryString("start")), .End = Convert.ToDateTime(Request.QueryString("end"))})
        End Function

        <AcceptVerbs(HttpVerbs.Post)> _
        Public Function Create(ByVal form As FormCollection) As ActionResult
            Dim start As Date = Convert.ToDateTime(form("Start"))
            Dim [end] As Date = Convert.ToDateTime(form("End"))
            Dim text As String = form("Text")
            Dim resource As Integer = Convert.ToInt32(form("Resource"))
            'string recurrence = form["Recurrence"];

            Dim toBeCreated = New [Event]() With {.Start = start, .End = [end], .Text = text}
            dc.Events.InsertOnSubmit(toBeCreated)
            dc.SubmitChanges()

            Return JavaScript(SimpleJsonSerializer.Serialize("OK"))
        End Function

        Public Class EventData
            Public Property Id() As Integer
            Public Property Start() As Date
            Public Property [End]() As Date
            Public Property Resource() As SelectList
            Public Property Text() As String
        End Class

    End Class
End Namespace
