<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="StartSurvey.aspx.cs" Inherits="AITR.survey" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="title">Welcome to the AITR survey!</h1>
    <p>
        You can participate in this survey anonymously or register your personal information. (your email is mandatory)</p>
    <div class="email_area">
        Email:
        <input id="email" type="text" /></div>
</asp:Content>
