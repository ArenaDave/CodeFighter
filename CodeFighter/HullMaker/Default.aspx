<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HullMaker._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-xs-12"><asp:Label runat="server" AssociatedControlID="ddlKeel" Text="Filter by Keel"></asp:Label><asp:DropDownList runat="server" ID="ddlKeel" AutoPostBack="true" OnSelectedIndexChanged="ddlKeel_SelectedIndexChanged" /></div>

    <asp:GridView runat="server" ID="gvHulls" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnRowDataBound="gvHulls_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Edit" >
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lbnEditHull" Text="Edit" OnCommand="lbnEditHull_Command" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ClassName" HeaderText="Name" />
            <asp:TemplateField HeaderText="Keel">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litKeelName" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Hitpoints">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litHitpoints" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MaxParts" HeaderText="Max Parts" />
            <asp:TemplateField HeaderText="Weapon Mounts">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litWeapons" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Defense Points">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litDefenses" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action Parts">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litActions" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Engines">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litEngines" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br /><br />
    <asp:Button runat="server" ID="btnAddNew" OnClick="btnAddNew_Click" Text="Add New Hull" CssClass="btn btn-primary" />


</asp:Content>
