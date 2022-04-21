<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Survey.aspx.cs" Inherits="AITR.respondent.Survey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="survey__card">
    <asp:Label class="page__title" ID="page_title" runat="server"></asp:Label>
    <asp:Label class="question__title" ID="question_title" runat="server"></asp:Label>
    <asp:Label class="question__description" ID="question_description" runat="server"></asp:Label>
    <asp:Label class="question__option" ID="question_option" runat="server"></asp:Label>
    <div class="options">
        <asp:PlaceHolder ID="Option" runat="server"></asp:PlaceHolder>
    </div>
    <div class="survey__buttons__area">        
        <asp:Button ID="Button_next" runat="server" Text="Next" OnClick="Button_next_Click" />
        <asp:Button ID="Button_finish" runat="server" Text="Finish" OnClick="Button_finish_Click" />
    </div>
    <asp:Label class="error__message" ID="error_message" runat="server"></asp:Label>
    </div>
</asp:Content>
