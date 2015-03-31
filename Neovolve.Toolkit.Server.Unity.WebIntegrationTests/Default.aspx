<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Neovolve.Toolkit.Server.Unity.WebIntegrationTests._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Unity testing
    </h2>
    <p>
        The hash of <asp:Label runat=server ID="Original"/> is <asp:Label runat=server ID="HashValue" />.
    </p>
</asp:Content>
