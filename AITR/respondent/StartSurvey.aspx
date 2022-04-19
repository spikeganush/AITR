<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="StartSurvey.aspx.cs" Inherits="AITR.Survey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="survey__card">
        <h1 class="title">Welcome to the AITR survey!</h1>
        <p>
            You can participate in this survey anonymously or register your personal information.
        </p>
        <div class="anonymous">
            <div class="anonymous__title">
                Taking the survey anonymously?
            </div>
        </div>
        <div class="anonymous__radio">
            <asp:RadioButton ID="anonymous_yes" runat="server" Text="Yes" GroupName="anonymous" Checked="true" />
            <asp:RadioButton ID="anonymous_no" runat="server" Text="No" GroupName="anonymous" />
        </div>
        <div class="anonymous__buttons">
            <a href="../home.aspx">
                <input id="anonymous__cancel" type="button" value="Cancel" /></a><asp:Button ID="ButtonNext" runat="server" Text="Next" OnClick="ButtonNext_Click" />
        </div>

    </div>
</asp:Content>
