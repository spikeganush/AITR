<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="EndSurvey.aspx.cs" Inherits="AITR.respondent.EndSurvey" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="survey__card">
        <div class="header">
            <h1 class="title">AITR Survey</h1>
         </div>

        <div class="introduction">
            <p>Thanks for your time and your answers.</p>
            <p>Have a good one!</p>
            <asp:Label ID="LabelSuccess" runat="server" />
        </div>

        <div class="start_button_area"><a href="../home.aspx">
            <input id="start_survey" type="button" value="Back home page" /></a></div>
    </div>
</asp:Content>
