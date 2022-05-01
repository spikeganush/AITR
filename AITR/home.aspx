<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="AITR.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="survey__card">
        <div class="header">
            <h1 class="title">AITR Survey</h1>
            <a href="./staff/Login.aspx">
                <input id="staff" type="button" value="Staff Login" /></a>
        </div>

        <div class="introduction">AIT Research (AITR) is a market research company that allows people from the general public to register their details, buying habits etc., with AITR and then sends these respondents to market research jobs based on the needs of AITR’s clients. </div>

        <div class="start_button_area">
            <a href="./respondent/StartSurvey.aspx">
                <input id="start_survey" type="button" value="Start the survey" /></a>
        </div>
    </div>
</asp:Content>
