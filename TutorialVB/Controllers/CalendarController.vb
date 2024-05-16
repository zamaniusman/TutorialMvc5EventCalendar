Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Runtime.ExceptionServices
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.Mvc
Imports DayPilot.Web.Mvc
Imports DayPilot.Web.Mvc.Enums
Imports DayPilot.Web.Mvc.Events.Calendar

Namespace TutorialVB.Controllers

    Public Class CalendarController
        Inherits Controller

        Public Function Backend() As ActionResult
            Return (New Dpc()).CallBack(Me)
        End Function

        Private Class Dpc
            Inherits DayPilotCalendar

            Private dc As New CalendarDataContext()

            Protected Overrides Sub OnInit(ByVal e As InitArgs)
                UpdateWithMessage("Welcome!", CallBackUpdateType.Full)
            End Sub

            Protected Overrides Sub OnCommand(ByVal e As CommandArgs)
                Select Case e.Command
                    Case "refresh"
                        Update()
                End Select
            End Sub

            Protected Overrides Sub OnEventMove(ByVal e As EventMoveArgs)
                Dim item = ( _
                    From ev In dc.Events _
                    Where ev.Id = Convert.ToInt32(e.Id) _
                    Select ev).First()
                If item IsNot Nothing Then
                    item.Start = e.NewStart
                    item.End = e.NewEnd
                    dc.SubmitChanges()
                End If
            End Sub

            Protected Overrides Sub OnEventResize(ByVal e As EventResizeArgs)
                Dim item = ( _
                    From ev In dc.Events _
                    Where ev.Id = Convert.ToInt32(e.Id) _
                    Select ev).First()
                If item IsNot Nothing Then
                    item.Start = e.NewStart
                    item.End = e.NewEnd
                    dc.SubmitChanges()
                End If
            End Sub

            Protected Overrides Sub OnEventDelete(ByVal e As EventDeleteArgs)
                Dim item = ( _
                    From ev In dc.Events _
                    Where ev.Id = Convert.ToInt32(e.Id) _
                    Select ev).First()
                dc.Events.DeleteOnSubmit(item)
                dc.SubmitChanges()
                Update()
            End Sub

            Protected Overrides Sub OnFinish()
                If UpdateType = CallBackUpdateType.None Then
                    Return
                End If

                DataIdField = "Id"
                DataStartField = "Start"
                DataEndField = "End"
                DataTextField = "Text"

                Events = From e In dc.Events Where Not ((e.End <= VisibleStart) Or (e.Start >= VisibleEnd)) _
                         Select e
            End Sub
        End Class
    End Class

End Namespace
