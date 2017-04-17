<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HullDetail.aspx.cs" Inherits="HullMaker.HullDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-xs-12"><asp:Label runat="server" AssociatedControlID="ddlKeel" Text="Keel"></asp:Label><asp:DropDownList runat="server" id="ddlKeel" CssClass="form-control" /></div>
    <div class="col-xs-12">
        <asp:Label runat="server" AssociatedControlID="tbxHitpoints" Text="Hitpoints"></asp:Label><asp:TextBox runat="server" ID="tbxHitpoints" CssClass="form-control" /><br />
        <asp:Label runat="server" AssociatedControlID="tbxMaxParts" Text="Max Parts"></asp:Label><asp:TextBox runat="server" ID="tbxMaxParts" CssClass="form-control" /><br />
        
    </div>
</asp:Content>
