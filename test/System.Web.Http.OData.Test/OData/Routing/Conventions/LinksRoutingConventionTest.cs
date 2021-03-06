﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Microsoft.TestCommon;

namespace System.Web.Http.OData.Routing.Conventions
{
    public class LinksRoutingConventionTest
    {
        [Theory]
        [InlineData("POST", "RoutingCustomers(1)/$links/Products")]
        [InlineData("PUT", "RoutingCustomers(1)/$links/Products")]
        [InlineData("DELETE", "RoutingCustomers(1)/$links/Products")]
        [InlineData("DELETE", "RoutingCustomers(1)/$links/Products(10)")]
        public void SelectAction_ReturnsNull_IfActionIsMissing(string httpMethod, string path)
        {
            ODataPath odataPath = new DefaultODataPathHandler().Parse(ODataRoutingModel.GetModel(), path);
            ILookup<string, HttpActionDescriptor> emptyActionMap = new HttpActionDescriptor[0].ToLookup(desc => (string)null);
            HttpControllerContext controllerContext = new HttpControllerContext();
            controllerContext.Request = new HttpRequestMessage(new HttpMethod(httpMethod), "http://localhost/");
            controllerContext.Request.SetRouteData(new HttpRouteData(new HttpRoute()));

            string selectedAction = new LinksRoutingConvention().SelectAction(odataPath, controllerContext, emptyActionMap);

            Assert.Null(selectedAction);
            Assert.Empty(controllerContext.Request.GetRouteData().Values);
        }
    }
}
