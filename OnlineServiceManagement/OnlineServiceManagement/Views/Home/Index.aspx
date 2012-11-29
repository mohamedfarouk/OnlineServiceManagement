<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OnlineServiceManagement.Models.WindowsServicesModel>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Managing windows services online : </h1>
                <h2>listing <%: Model.Services.Count %> services</h2>
            </hgroup>
            <p>
                Provides facility to manage windows services online using ASP.NET MVC, Signal-R, KnockoutJS, WCF with Dual HTTP binding. 
                Listed windows services can be started/stopped/restarted.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">    
    <div id="message"></div>
    <div data-bind="template: {name:'Services-Template', foreach: Services}"></div>
    <script type="text/html" id="Services-Template">
        <% Html.BeginForm("Index", "Home", FormMethod.Post); %>           
             <ol class="round">
                    <li data-bind="attr:{class:CSSClass}">  
                    <h5 data-bind="text:Name"></h5>
                    <span data-bind="text:Description"></span>
                    <p>
                        <span data-bind="text:Status"></span>
                    </p>
                    <p>
                        <span data-bind="attr: {id: ServiceName}"></span>
                    </p>
                    <p>
                        <button data-bind="click: koHelper.StartService">Start</button>
                        <button data-bind="click: koHelper.StopService">Stop</button>
                        <button data-bind="click: koHelper.RestartService">Restart</button>                       
                    </p>
                    </li>
            </ol>
        <% Html.EndForm();%>
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            var jsonModel = '<%=Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model))%>';
            var clientViewModel = ko.utils.parseJson(jsonModel);
            koHelper.LoadViewModel(clientViewModel);
            koHelper.UpdateStatus("Loaded view model");
        });

     </script>
    <div style=" position:absolute; top: 300px; left: 1300px; height: 400px; width:250px; border:2px dotted Red">
        <div><b>Status updates</b></div>
        <div id="statusInfo" style="height:95%; overflow-y:scroll;"></div>
    </div>
         
</asp:Content>
