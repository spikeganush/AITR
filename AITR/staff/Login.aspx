<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AITR.staff.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 47px;
            height: 61px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="survey__card">
        <div class="header">
            <h1 class="title_login">Staff Login</h1>
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
        <asp:Button ID="ButtonLogin" runat="server" Text="LOGIN" OnClick="ButtonLogin_Click" />
        <!--<asp:RequiredFieldValidator ID="LoginEmail_validator" runat="server" ErrorMessage="Please fill the field." ControlToValidate="TextBoxLoginEmail" />
        <asp:RequiredFieldValidator ID="LoginPassword_validator" runat="server" ErrorMessage="Please fill the field." ControlToValidate="TextBoxLoginPassword" />-->
    </div>
</asp:Content>
