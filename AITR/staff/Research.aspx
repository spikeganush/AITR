<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Research.aspx.cs" Inherits="AITR.staff.Research" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="survey__card">
        <div class="header">
            <h1 class="title_login">Staff research page</h1>
        </div>
        <div class="input__area">
            <div class="input__row">
                <div class="input__input">
                    <asp:TextBox class="input__input__input" ID="TextBoxLoginEmail" runat="server"></asp:TextBox>
                     </div>
                <div class="input__img">
                    <img alt="user" src="../asset/user.png" />
                </div>
            </div>
            <div class="input__row">
                <div class="input__input">
                    <asp:TextBox class="input__input__input" ID="TextBoxLoginPassword" runat="server"></asp:TextBox>
                    </div>
                <div class="input__img">
                    <img alt="user" src="../asset/lock.png" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
