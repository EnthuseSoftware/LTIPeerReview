<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewUpload.aspx.cs" Inherits="LTIPeerReview.ReviewUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div>
        Developer: <asp:Label runat="server" id="SubmitterNameLabel" text="N/A" /><br />
        Assignment: <asp:Label runat="server" id="SubmissionNameLabel" text="N/A" /><br />
        Group: <asp:Label runat="server" id="GroupLabel" text="N/A" />
        
    </div>
    <form id="form1" runat="server">
        Download submission files:<br />
        <asp:Button runat="server" id="DownloadButton" Text ="Download" OnClick="DownloadButton_Click" /><br />
        Upload completed peer review form<br />
        <asp:FileUpload id="FileUploadControl" runat="server" />
        <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
        <br /><br />
        <asp:Label runat="server" id="StatusLabel" text="" />
    </form>
</body>
</html>
