// --------------------------------
// <copyright file="BarPopups.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;
    using System.IO;

    /// <summary>Implements BarPopup control</summary>
    public class BarPopup
    {
        public string Id { get; set; }

        public string FieldName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public string RequiredMessage { get; set; }

        public bool Duplicated { get; set; }

        public string DuplicatedMessage { get; set; }

        public int BarWidth { get; set; }

        public int UpdateWidth { get; set; }

        public int DeleteWidth { get; set; }

        public string DeleteMessage { get; set; }

        public string BarTitle { get; set; }

        public string Render
        {
            get
            {
                string pattern = @"
                            <!--- BAR POPUPS FOR {0} ITEM -->
                            <div id=""dialog{0}"" class=""hide"" style=""width:{1}px;"">
                                <div class=""table-responsive"">
                                    <table class=""table table-bordered table-striped"">
                                        <thead class=""thin-border-bottom"">
                                            <tr>
                                                <th>{4}</th>
                                                <th style=""width:150px;"">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id=""Selectable{0}"">
                                        </tbody>
                                    </table>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id=""{0}InsertDialog"" class=""hide"" style=""width:{2}px;"">
                                <p>{6}&nbsp;&nbsp;<input type=""text"" id=""Txt{0}NewName"" size=""50"" placeholder=""{6}"" maxlength=""50"" onblur=""this.value=$.trim(this.value);"" /></p>
                                {7}
                                {8}
                            </div>

                            <div id=""{0}UpdateDialog"" class=""hide"" style=""width:{2}px;"">
                                <p>{6}&nbsp;&nbsp;<input type=""text"" id=""Txt{0}Name"" size=""50"" placeholder=""{6}"" maxlength=""50"" onblur=""this.value=$.trim(this.value);"" /></p>
                                {9}
                                {10}
                            </div>

                            <div id=""{0}DeleteDialog"" class=""hide"" style=""width:{3}px;"">
                                <p>{5}&nbsp;<strong><span id=""{0}Name""></span></strong>?</p>
                            </div>";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.BarWidth,
                    this.UpdateWidth,
                    this.DeleteWidth,
                    this.Description,
                    this.DeleteMessage,
                    this.FieldName,
                    this.Required ? string.Format(@"<span class=""ErrorMessage"" id=""Txt{0}NewNameErrorRequired"" style=""display:none;"">{1}</span>", this.Id, this.RequiredMessage) : string.Empty,
                    this.Duplicated ? string.Format(@"<span class=""ErrorMessage"" id=""Txt{0}NewNameErrorDuplicated"" style=""display:none;"">{1}</span>", this.Id, this.DuplicatedMessage) : string.Empty,
                    this.Required ? string.Format(@"<span class=""ErrorMessage"" id=""Txt{0}NameErrorRequired"" style=""display:none;"">{1}</span>", this.Id, this.RequiredMessage) : string.Empty,
                    this.Duplicated ? string.Format(@"<span class=""ErrorMessage"" id=""Txt{0}NameErrorDuplicated"" style=""display:none;"">{1}</span>", this.Id, this.DuplicatedMessage) : string.Empty);
            }
        }

        public string RenderScriptsDelete
        {
            get
            {
                string pattern = string.Empty;
                using (var input = new StreamReader(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "pattern\\BarScripts.txt"))
                {
                    pattern = input.ReadToEnd();
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.BarTitle,
                    this.BarWidth > 0 ? this.BarWidth : 600);
            }
        }

        public string RenderScripts
        {
            get
            {
                string pattern = @"function {0}Changed(sender)
                    {{
                        var id= sender.parentNode.parentNode.parentNode.id * 1;
                        $('#dialogProcessType').dialog('close');
                        for(var x=0;x<{0}.length;x++)
                        {{
                            if({0}[x].Id === id)
                            {{
                                {0}Selected = id;
                                document.getElementById('{0}Type').value = {0}[x].Description;
                                break;
                            }}
                        }}

                        FillCmbTipo();
                    }                    

                    function {0}}}Update(sender) {{
                        document.getElementById('Txt{0}}}NameErrorRequired').style.display='none';
                        document.getElementById('Txt{0}}}NameErrorDuplicated').style.display='none';
                        $('#TxtProcessTypeName').val(sender.parentNode.parentNode.parentNode.childNodes[0].innerHTML);
                        Selected = sender.parentNode.parentNode.parentNode.id * 1;
                        var dialog = $('#{0}}}UpdateDialog').removeClass('hide').dialog({{
                            resizable: false,
                            width: 600,
                            modal: true,
                            title: Dictionary.Common_Edit,
                            title_html: true,
                            buttons: [
                                    {{
                                        ""html"": ""<i class=\\""icon-ok bigger-110\\""></i>&nbsp;"" + Dictionary.Common_Accept,
                                        ""class"": ""btn btn-success btn-xs"",
                                        ""click"": function () {{
                                            var ok = true;
                                            if(document.getElementById('Txt{0}}}Name').value == '')
                                            {{
                                                document.getElementById('Txt{0}}}NameErrorRequired').style.display='block';
                                                ok = false;
                                            }}
                                            else
                                            {{
                                                document.getElementById('Txt{0}}}NameErrorRequired').style.display='none';
                                            }}

                                            var duplicated = false;
                                            for(var x=0;x<processTypeCompany.length;x++)
                                            {{
                                                if(document.getElementById('Txt{0}}}Name').value.toLowerCase() == processTypeCompany[x].Description.toLowerCase() && Selected != processTypeCompany[x].Id && processTypeCompany[x].Active === true)
                                                {{
                                                    duplicated = true;
                                                    break;
                                                }}
                                            }}

                                            if(duplicated === true)
                                            {{
                                                $(""#Txt{0}}}NameErrorDuplicated"").hide();
                                                ok = false;
                                            }}
                                            else
                                            {{
                                                $(""#Txt{0}}}NameErrorDuplicated"").hide();
                                            }}


                                            if(ok === false) {{ window.scrollTo(0, 0); return false; }}

                                            $(""#Txt{0}}}NameErrorRequired"").hide();
                                            $(""#Txt{0}}}NameErrorDuplicated"").hide();
                                            $(this).dialog('close');
                                            ProcessTypeUpdateConfirmed(Selected, $(""#Txt{0}}}Name"").val());
                                        }}
                                    }},
                                    {{
                                        ""html"": ""<i class=\\""icon-remove bigger-110\\""></i>&nbsp;"" + Dictionary.Common_Cancel,
                                        ""class"": ""btn btn-xs"",
                                        ""click"": function () {{
                                            $(this).dialog(""close"");
                                        }}
                                    }}
                                ]

                        }});
                    }}";

                return string.Format(CultureInfo.InvariantCulture, pattern, this.Id);
            }
        }

        public string HttpContext { get; set; }
    }
}