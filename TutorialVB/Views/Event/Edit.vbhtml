﻿@Code
    Layout = Nothing
End Code
<!DOCTYPE>
<html>
<head runat="server">
   	<script type="text/javascript" src="@Url.Content("~/Scripts/jquery-1.11.2.min.js")"></script>
    
    <link href="@Url.Content("~/Media/layout.css")" rel="stylesheet" type="text/css" />
   	<style>
   	p, body, td { font-family: Tahoma, Arial, Sans-Serif; font-size: 10pt; }
   	</style>
    <title></title>
</head>
<body style="padding:10px">
    <form id="f" method="post" action="@Url.Action("Edit")">
    <h2>Edit Event</h2>

    <div style="margin-top:20px">
        <div>Event text</div>
        @Html.TextBox("Text") @Html.Hidden("Id")
    </div>

    <div>Start</div>
    <div>@Html.TextBox("Start")</div>

    <div>End</div>
    <div>@Html.TextBox("End")</div>
    
    <div style="margin-top:20px">
        <input type="submit" id="ButtonSave" value="Save" />
        <a href="javascript:close()">Cancel</a>
    </div>
    
    </form>
    
    <script type="text/javascript">
        function close(result) {
            if (parent && parent.DayPilot && parent.DayPilot.ModalStatic) {
                parent.DayPilot.ModalStatic.close(result);
            }
        }

        $("#f").submit(function () {
            var f = $("#f");
            $.post(f.action, f.serialize(), function (result) {
                close(eval(result));
            });
            return false;
        });

        $(document).ready(function () {
            $("#Text").focus();
        });
    
    </script>
    
</body>
</html>
