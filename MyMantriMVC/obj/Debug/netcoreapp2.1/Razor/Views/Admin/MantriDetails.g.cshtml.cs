#pragma checksum "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4e3eb06a3c12d487134001f0cce956c02f22cc89"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_MantriDetails), @"mvc.1.0.view", @"/Views/Admin/MantriDetails.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Admin/MantriDetails.cshtml", typeof(AspNetCore.Views_Admin_MantriDetails))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\_ViewImports.cshtml"
using MyMantriMVC;

#line default
#line hidden
#line 2 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\_ViewImports.cshtml"
using MyMantriMVC.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e3eb06a3c12d487134001f0cce956c02f22cc89", @"/Views/Admin/MantriDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b1a506a7d8800df8509df3ababcde133a260efcb", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_MantriDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MyMantriMVC.Models.Mantri>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("color:#cebc81"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "GetMantri", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(34, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
  
    ViewData["Title"] = "Mantri Details";
    Layout = "~/Views/Shared/AdminLayout.cshtml";

#line default
#line hidden
            BeginContext(137, 131, true);
            WriteLiteral("\r\n<h3 style=\"color:#cebc81\">Minister Details</h3>\r\n<br />\r\n\r\n<div>\r\n    <dl>\r\n        <dt>\r\n            Minister ID :\r\n            ");
            EndContext();
            BeginContext(269, 40, false);
#line 15 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.MantriId));

#line default
#line hidden
            EndContext();
            BeginContext(309, 63, true);
            WriteLiteral("\r\n        </dt>\r\n        <dt>\r\n            Name :\r\n            ");
            EndContext();
            BeginContext(373, 36, false);
#line 19 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.Name));

#line default
#line hidden
            EndContext();
            BeginContext(409, 67, true);
            WriteLiteral("\r\n        </dt>\r\n        <dt>\r\n            Email ID :\r\n            ");
            EndContext();
            BeginContext(477, 39, false);
#line 23 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.EmailId));

#line default
#line hidden
            EndContext();
            BeginContext(516, 69, true);
            WriteLiteral("\r\n        </dt>\r\n        <dt>\r\n            Mobile No. :\r\n            ");
            EndContext();
            BeginContext(586, 38, false);
#line 27 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.Mobile));

#line default
#line hidden
            EndContext();
            BeginContext(624, 72, true);
            WriteLiteral("\r\n        </dt>\r\n        <dt>\r\n            Date of Birth :\r\n            ");
            EndContext();
            BeginContext(697, 43, false);
#line 31 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.DateOfBirth));

#line default
#line hidden
            EndContext();
            BeginContext(740, 65, true);
            WriteLiteral("\r\n        </dt>\r\n        <dt>\r\n            Gender :\r\n            ");
            EndContext();
            BeginContext(806, 38, false);
#line 35 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.Gender));

#line default
#line hidden
            EndContext();
            BeginContext(844, 66, true);
            WriteLiteral("\r\n        </dt>\r\n        <dt>\r\n            Address :\r\n            ");
            EndContext();
            BeginContext(911, 39, false);
#line 39 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.Address));

#line default
#line hidden
            EndContext();
            BeginContext(950, 71, true);
            WriteLiteral("\r\n        </dt>\r\n        <dt>\r\n            Constituency :\r\n            ");
            EndContext();
            BeginContext(1022, 44, false);
#line 43 "C:\Users\satyam06.trn\Desktop\MyMantriFinal\MyMantriMVC\Views\Admin\MantriDetails.cshtml"
       Write(Html.DisplayFor(model => model.Constituency));

#line default
#line hidden
            EndContext();
            BeginContext(1066, 55, true);
            WriteLiteral("\r\n        </dt>\r\n    </dl>\r\n</div>\r\n<br />\r\n<div>\r\n    ");
            EndContext();
            BeginContext(1121, 64, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "db12e83ff4ea413c812ec61831089aa2", async() => {
                BeginContext(1169, 12, true);
                WriteLiteral("Back to List");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1185, 10, true);
            WriteLiteral("\r\n</div>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MyMantriMVC.Models.Mantri> Html { get; private set; }
    }
}
#pragma warning restore 1591