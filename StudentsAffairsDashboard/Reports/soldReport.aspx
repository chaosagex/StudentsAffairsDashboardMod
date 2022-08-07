<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="soldReport.aspx.cs" Inherits="StudentsAffairsDashboard.Reports.soldReport" %>

 <%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
 
 
<!DOCTYPE html>
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form3" runat="server">
        <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>
        From<asp:Calendar ID="fromDate" runat="server" SelectedDate="08/07/2022 15:15:50"></asp:Calendar>
        TO<asp:Calendar ID="toDate" runat="server" SelectedDate="08/07/2022 15:15:58"></asp:Calendar>
        <asp:Button ID="Button1" runat="server" style="width: 56px; height: 26px;" Text="GET" OnClick="btnGet_Click" />
        <rsweb:ReportViewer ID="ReportViewer3" runat="server" AsyncRendering="False" ShowBackButton="False" ShowExportControls="False" ShowFindControls="False" ShowPageNavigationControls="False" Width="100%" DocumentMapWidth="100%"  BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226">
  <ServerReport ReportServerUrl="https://localhost" ReportPath="/Reports" />
            <LocalReport ReportPath="Reports\endOfDay.rdlc">
            </LocalReport>
</rsweb:ReportViewer>
    </form>
</body>
</html>
