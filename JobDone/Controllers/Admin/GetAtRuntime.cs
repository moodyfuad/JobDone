using JobDone.Models;
using JobDone.Models.Seller;
using JobDone.Models.SellerOldWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json.Linq;
using System;

namespace JobDone.Controllers.Admin
{
    public class GetAtRuntime
    {
        private readonly ISeller _sellers;

        public GetAtRuntime(ISeller sellers)
        {
            _sellers = sellers;
        }
    }
}
