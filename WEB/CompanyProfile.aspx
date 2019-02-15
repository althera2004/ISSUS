<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="CompanyProfile.aspx.cs" Inherits="CompanyProfile" %>

<asp:Content ID="PageStyle" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link href="/nv.d3/nv.d3.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        scaleImages = true;
        var addresses= <%=this.Addresses %>;
        var addressSelected = <%=this.DefaultAddressId %>;
        var ddData = [<%=this.CountryData %>];
        var action = 0;
        var actionAddress = -1;
        var countries = <%=this.Countries %>;
        var diskQuote = <%=this.DiskQuote %>;
        var CompanyName = "<%=this.Company.Name %>";
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <form class="form-horizontal" role="form">
                                <div class="tabbable">
                                    <ul class="nav nav-tabs padding-18">
                                        <li class="active" id="TabHomeSelector">
                                            <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_Company_Tab_Principal"] %></a>
                                        </li>
                                        <li class="" id="TabDisk"onclick="$('#PieWidget').show();">
                                            <a data-toggle="tab" href="#disk"><%=this.Dictionary["Item_Company_Tab_DiskQuote"] %></a>
                                        </li>
                                        <li class="" id="TabContrato"onclick="$('#PieWidget').show();">
                                            <a data-toggle="tab" href="#contrato"><%=this.Dictionary["Item_Company_Tab_Agreement"] %></a>
                                        </li>
                                    </ul>                                    
                                    <div class="tab-content no-border padding-24">
                                        <div id="home" class="tab-pane active">
                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right" id="TxtHeadquartersLabel"><%=this.Dictionary["Item_CompanyProfile_FieldLabel_Headquarters"] %><span style="color:#f00">*</span></label>
                                                <div class="col-sm-11">
                                                    <input type="text" id="TxtHeadquarters" placeholder="Item_CompanyProfile_FieldLabel_Headquarters" class="col-xs-12 col-sm-12 tooltip-info" data-rel="tooltip" value="<%=this.Company.Headquarters %>" maxlength="50" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right" id="TxtNameLabel"><%=this.Dictionary["Item_CompanyProfile_FieldLabel_Name"] %><span style="color:#f00">*</span></label>
                                                <div class="col-sm-7">
                                                    <input type="text" id="TxtName" placeholder="Item_CompanyProfile_FieldLabel_Name" class="col-xs-12 col-sm-12 tooltip-info" data-rel="tooltip" value="<%=this.Company.Name %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                                    <span class="ErrorMessage" id="TxtNameErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                                </div>
                                                <label class="col-sm-1 control-label no-padding-right" id="TxtNifLabel"><%=this.Dictionary["Item_CompanyProfile_FieldLabel_Nif"] %><span style="color:#f00">*</span></label>
                                                <div class="col-sm-3">
                                                    <input type="text" id="TxtNif" placeholder="Item_CompanyProfile_FieldLabel_Name" class="col-xs-12 col-sm-12 tooltip-info" data-rel="tooltip" value="<%=this.Company.FiscalNumber %>" maxlength="15" onblur="this.value=$.trim(this.value);this.value = this.value.toUpperCase();" />
                                                    <span class="ErrorMessage" id="TxtNifErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                                    <span class="ErrorMessage" id="TxtNifErrorMalformed"><%=this.Dictionary["Common_Error_IncorrectNIF"] %></span>
                                                </div>
                                            </div>
                                            <div class="space-4"></div>
                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_CompanyData_FieldLabel_Address"] %><span style="color:#f00">*</span></label>
                                                <div class="col-sm-10" id="DivCmbAddress" style="height: 35px !important;">
                                                    <select id="CmbAddress" class="col-xs-12 col-sm-12 tooltip-info" onchange="CmbAddressChanged();"></select>
                                                    <input style="display: none;" readonly="readonly" type="text" id="TxtDireccion" placeholder="<%=this.Dictionary["Item_CompanyData_FieldLabel_Address"] %>" class="col-xs-12 col-sm-12" value="<%=this.CompanyDefaultAddress.Address %>" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <span class="btn btn-light" style="height: 30px;" id="BtnShowAddress" title="<%=this.Dictionary["Item_Company_Button_CompanyAddressBAR"] %>">...</span>
                                                </div>
                                            </div>

                                            <div class="space-4"></div>

                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_PostalAddress"] %></label>
                                                <div class="col-sm-2">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtPostalCode" value="<%=this.CompanyDefaultAddress.PostalCode %>" />
                                                </div>
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_City"] %></label>
                                                <div class="col-sm-8">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtCity" value="<%=this.CompanyDefaultAddress.City %>" />
                                                </div>
                                            </div>

                                            <div class="space-4"></div>

                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_Province"] %></label>
                                                <div class="col-sm-4">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtProvince" value="<%=this.CompanyDefaultAddress.Province %>" />
                                                </div>
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_Country"] %></label>
                                                <div class="col-sm-6">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtCountry" value="<%=this.DefaultCountry %>" />
                                                </div>
                                            </div>

                                            <div class="space-4"></div>

                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_Phone"] %></label>
                                                <div class="col-sm-3">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtPhone" value="<%=this.CompanyDefaultAddress.Phone %>" />
                                                </div>
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_Fax"] %></label>
                                                <div class="col-sm-3">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtFax" value="<%=this.CompanyDefaultAddress.Fax %>" />
                                                </div>
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_Cellular"] %></label>
                                                <div class="col-sm-3">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtMobile" value="<%=this.CompanyDefaultAddress.Mobile %>" />
                                                </div>
                                            </div>

                                            <div class="space-4"></div>

                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly"><%=this.Dictionary["Item_CompanyData_FieldLabel_Email"] %></label>
                                                <div class="col-sm-11">
                                                    <input readonly="readonly" type="text" class="col-xs-12 col-sm-12" id="TxtEmail" value="<%=this.CompanyDefaultAddress.Email %>" />
                                                </div>
                                            </div>
                                            <div class="space-4"></div>
                                            <div class="form-group">
                                                <label class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Profile_FieldLabel_Language"] %></label>
                                                <div class="col-xs-3" id="DivCmbIdioma" style="height:35px !important;">
                                                    <select id="CmbIdioma" class="col-xs-12">
                                                        <asp:Literal runat="server" ID="LtIdiomas"></asp:Literal>
                                                    </select>
                                                </div>
                                            </div>
                                            <hr />
                                            <div class="row">
                                                <div class="col-xs-4">
                                                    <%=this.ImgLogo.Render %>
                                                </div>
                                                <div class="col-xs-2" style="display: none;">
                                                    <div class="pink" id="DivLogo1">
                                                        <button class="btn btn-info" type="button" id="BtnChangeLogo" onclick="document.getElementById('DivLogo1').style.display='none';document.getElementById('DivLogo2').style.display='';">
                                                            <%=this.Dictionary["Item_CompanyData_Button_ChangeLogo"]%>
                                                        </button>
                                                    </div>
                                                    <div class="pink" id="DivLogo2" style="display: none;">
                                                        <table>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <input type="file" id="imageInput" accept="image/x-png, image/jpeg" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <button class="col-xs-12 btn btn-success" type="button" id="BtnSaveLogo">
                                                                        <%=this.Dictionary["Common_Accept"]%>
                                                                    </button>
                                                                </td>
                                                                <td>
                                                                    <button class="col-xs-12 btn btn-danger" type="button" id="BtnCancelLogo" onclick="document.getElementById('DivLogo1').style.display='';document.getElementById('DivLogo2').style.display='none';">
                                                                        <%=this.Dictionary["Common_Cancel"]%>
                                                                    </button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="col-xs-4">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <%=this.FormFooter %>
                                        </div>
                                        <div id="disk" class="tab-pane active">
                                            <!--<h2><%=this.Dictionary["Item_Attach_DiskQuote"] %>&nbsp;<%=this.AsignedQuote %>MB</h2>
                                            <h4><%=this.Dictionary["Item_Attach_DiskFreeLabel1"] %><span id="_QuotePercentage"></span><%=this.Dictionary["Item_Attach_DiskFreeLabel2"] %></h4>
                                            <div class="widget-main">
											    <div id="piechart-placeholder" style="width: 90%; min-height: 150px; height:400px; padding: 0px; position: relative;"><canvas class="flot-base" width="358" height="150" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 358px; height: 150px;"></canvas><canvas class="flot-overlay" width="358" height="150" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 358px; height: 150px;"></canvas><div class="legend"><div style="position: absolute; width: 93px; height: 120px; top: 15px; right: -30px; background-color: rgb(255, 255, 255); opacity: 0.85;"> </div><table style="position:absolute;top:15px;right:-30px;;font-size:smaller;color:#545454"><tbody><tr><td class="legendColorBox"><div style="border:1px solid null;padding:1px"><div style="width:4px;height:0;border:5px solid #68BC31;overflow:hidden"></div></div></td><td class="legendLabel">social networks</td></tr><tr><td class="legendColorBox"><div style="border:1px solid null;padding:1px"><div style="width:4px;height:0;border:5px solid #2091CF;overflow:hidden"></div></div></td><td class="legendLabel">search engines</td></tr><tr><td class="legendColorBox"><div style="border:1px solid null;padding:1px"><div style="width:4px;height:0;border:5px solid #AF4E96;overflow:hidden"></div></div></td><td class="legendLabel">ad campaigns</td></tr><tr><td class="legendColorBox"><div style="border:1px solid null;padding:1px"><div style="width:4px;height:0;border:5px solid #DA5430;overflow:hidden"></div></div></td><td class="legendLabel">direct traffic</td></tr><tr><td class="legendColorBox"><div style="border:1px solid null;padding:1px"><div style="width:4px;height:0;border:5px solid #FEE074;overflow:hidden"></div></div></td><td class="legendLabel">other</td></tr></tbody></table></div></div>
											</div>-->
                                            <div class="row">        
                                                <div class="col-xs-12 col-sm-6 widget-container-col ui-sortable" style="min-height: 300px;">										
			                                        <div class="widget-box ui-sortable-handle" style="opacity: 1; z-index: 0;" id="PieWidget">
				                                        <div class="widget-header"><h5 class="widget-title"><%=this.Dictionary["Item_Attach_DiskQuote"] %>&nbsp;<%=this.AsignedQuote %>MB</h5></div>
				                                        <div class="widget-body">
                                                            <div class="alert alert-info">
						                                        <i class="ace-icon fa icon-info-sign fa-2x"></i>
                                                                <span id="changeMessage"><%=this.Dictionary["Item_Attach_DiskFreeLabel1"] %><span id="QuotePercentage"></span><%=this.Dictionary["Item_Attach_DiskFreeLabel2"] %></span>
					                                        </div>
					                                        <div id="Pie1" style="height:300px;"><svg class="nvd3-svg"></svg></div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 col-sm-6 widget-container-col ui-sortable" style="min-height: 300px;"></div>
                                            </div>
                                        </div>
                                        <!--
                                        <div id="countries" class="tab-pane">
                                            <h4><%=this.Dictionary["Item_CompanyCountries_Status_Selected"]%></h4>
								            <div class="row">
									            <div class="col-xs-12" id="SelectedDiv">
                                                    <asp:Literal runat="server" ID="LtSelected"></asp:Literal>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8">&nbsp;</div>
                                                <div class="col-xs-4">
                                                    <h4 class="pink">
                                                        <button class="btn btn-success" type="button" id="BtnCountryDiscard">
                                                            <i class="icon-minus-sign bigger-110"></i>
                                                            <%=this.Dictionary["Item_CompanyCountry_Button_Discard"] %>
                                                        </button>
                                                    </h4>
                                                </div> 
                                            </div>
                                            <h4><%=this.Dictionary["Item_CompanyCountries_Status_Availables"]%></h4>   
                                            <div class="form-group">
								                <label class="col-sm-1 control-label" id="TxtCountryNameLabel"><%=this.Dictionary["Common_Search"] %></label>                                        
								                <div class="col-sm-4">
                                                    <input type="text" id="TxtCountryName" placeholder="<%=this.Dictionary["Item_CompanyCountry_FieldLabel_Name"] %>" class="col-xs-12 col-sm-12" value="" maxlength="50" onblur="this.value=$.trim(this.value);" onkeyup="filter();" />                                        
                                                </div>
							                </div>                          
								            <div class="row">
									            <div class="col-xs-12" id="AvailablesDiv">
                                                    <asp:Literal runat="server" ID="LtAvailables"></asp:Literal>
                                                </div>
                                            </div>  
                                            <div class="form-group">
                                                <div class="col-xs-8">&nbsp;</div>
                                                <div class="col-xs-4">
                                                    <h4 class="pink">
                                                        <button class="btn btn-success" type="button" id="BtnCountryAdd">
                                                            <i class="icon-plus-sign bigger-110"></i>
                                                            <%=this.Dictionary["Item_CompanyCountry_Button_Select"] %>
                                                        </button>
                                                    </h4>
                                                </div> 
                                            </div>  
                                            <div class="alert alert-info">icon-ok
                                                <strong><%=this.Dictionary["Common_Warning"] %></strong>
                                                <%=this.Dictionary["Item_Country_WarningMessage"] %><br />
                                            </div>
                                            <div style="display:none;">
                                                <input type="text" id="SelectedCountriesDelete" style="display:block;" />
                                                <input type="text" id="SelectedCountries" style="display:block;" />
                                            </div>
                                        </div> -->                                        
                                        <div id="contrato" class="tab-pane active">
                                            <iframe src="/viewer/viewer.html?file=/Agreement/Agreement_<%=this.Company.Name %>.pdf" style="width:100%;" id="pdfViewer"></iframe>
                                        </div>
                                        <div id="trazas" class="tab-pane">													
                                            <table class="table table-bordered table-striped">
                                                <thead class="thin-border-bottom">
                                                    <tr>
                                                        <th style="width:150px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"]%></th>
                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"]%></th>
                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"]%></th>
                                                        <th style="width:250px;"><%= this.Dictionary["Item_Tace_ListHeader_User"]%></th>													
                                                    </tr>
                                                </thead>
                                                <tbody><asp:Literal runat="server" ID="LtTrazas"></asp:Literal></tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </form>
                            <div id="AddressDeleteDialog" class="hide">
                                <p><%=this.Dictionary["Item_CompanyAddress_PopupDelete_Message"] %>&nbsp;<strong><span id="AddressName"></span></strong>?</p>
                            </div>
                            <div id="dialogShowAddress" class="hide" style="width:800px;">
                                <table class="table table-bordered table-striped">
                                    <thead class="thin-border-bottom">
                                        <tr>
                                            <th><%=this.Dictionary["Item_CompanyAddress_Header_Address"] %></th>
                                            <th style="width:120px;">&nbsp;</th>													
                                        </tr>
                                    </thead>
                                    <tbody id="DireccionesSelectable"></tbody>
                                </table>
                            </div>
                            <div id="dialogAddAddress" class="hide" style="width:800px;">
                                <form class="form-horizontal" role="form" id="FormNewAddressDialog">                                    
                                    <div class="form-group">
                                        <label id ="TxtNewAddressLabel" class="col-sm-2 control-label no-padding-right" for="TxtNewAddress"><%=this.Dictionary["Item_CompanyAddress_FieldLabel_Address"] %><span style="color:#f00">*</span></label>
                                        <div class="col-sm-9">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtNewAddress" placeholder="<%=this.Dictionary["Item_CompanyAddress_FieldLabel_Address"] %>" value="" maxlength="100" onblur="this.value=$.trim(this.value);" />
                                            <span class="ErrorMessage" id="TxtNewAddressErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>
                                    <div class="space-4"></div>                                    
                                    <div class="form-group">
                                        <label id="TxtNewAddressPostalCodeLabel" class="col-sm-2 control-label no-padding-right" for="TxtNewAddressPostalCode"><%=this.Dictionary["Item_CompanyAddress_FieldLabel_PostalCode"] %><span style="color:#f00">*</span></label>
                                        <div class="col-sm-3">
                                            <input  type="text" class="col-xs-12 col-sm-12" id="TxtNewAddressPostalCode" placeholder="<%=this.Dictionary["Item_CompanyAddress_FieldLabel_PostalCode"] %>" value="" maxlength="10" onblur="this.value=$.trim(this.value);" />
                                            <span class="ErrorMessage" id="TxtNewAddressPostalCodeErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                        <label id="TxtNewAddressCityLabel" class="col-sm-1 control-label no-padding-right" for="TxtNewAddressCity"><%=this.Dictionary["Item_CompanyAddress_FieldLabel_City"] %><span style="color:#f00">*</span></label>
                                        <div class="col-sm-5">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtNewAddressCity" placeholder="<%=this.Dictionary["Item_CompanyAddress_FieldLabel_City"] %>" value="" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                            <span class="ErrorMessage" id="TxtNewAddressCityErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>
                                    <div class="space-4"></div>                                    
                                    <div class="form-group">
                                        <label id="TxtNewAddressProvinceLabel" class="col-sm-2 control-label no-padding-right" for="TxtNewAddressProvince"><%=this.Dictionary["Item_CompanyAddress_FieldLabel_Province"] %><span style="color:#f00">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtNewAddressProvince" placeholder="<%=this.Dictionary["Item_CompanyAddress_FieldLabel_Province"] %>" value="" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                            <span class="ErrorMessage" id="TxtNewAddressProvinceErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                        <label id="TxtNewAddressCountryLabel" class="col-sm-1 control-label no-padding-right" for="TxtNewAddressCountry"><%=this.Dictionary["Item_CompanyAddress_FieldLabel_Country"] %></label>
                                        <div class="col-sm-5" id="DivCmbPais" style="height:35px !important;">                                            
                                            <select id="CmbPais" onchange="document.getElementById('TxtNewAddressCountry').value = this.value;" class="col-xs-12 col-sm-12"></select>
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtNewAddressCountry" value="" maxlength="15" onblur="this.value=$.trim(this.value);" style="display:none;" />
                                            <span class="ErrorMessage" id="TxtNewAddressCountryErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>
                                    <div class="space-4"></div>                                    
                                    <div class="form-group">
                                        <label id="TxtNewAddressPhoneLabel" class="col-sm-2 control-label no-padding-right" for="TxtNewAddressPhone"><%=this.Dictionary["Item_CompanyData_FieldLabel_Phone"] %><span style="color:#f00">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12" placeholder="<%=this.Dictionary["Item_CompanyData_FieldLabel_Phone"] %>" id="TxtNewAddressPhone" value="" onkeypress="validate(event)" maxlength="15" onblur="this.value=$.trim(this.value);" />
                                            <span class="ErrorMessage" id="TxtNewAddressPhoneErrorRequired"><%=this.Dictionary["Item_CompanyData_Error_Message_PhoneOrCellular"]%></span>
                                        </div>
                                        <label id="TxtNewAddressFaxLabel" class="col-sm-1 control-label no-padding-right" for="TxtNewAddressFax"><%=this.Dictionary["Item_CompanyData_FieldLabel_Fax"] %></label>
                                        <div class="col-sm-5">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtNewAddressFax" placeholder="<%=this.Dictionary["Item_CompanyData_FieldLabel_Fax"] %>" value="" onkeypress="validate(event)" maxlength="15" onblur="this.value=$.trim(this.value);" />
                                        </div>
                                    </div>
                                    <div class="space-4"></div>                                    
                                    <div class="form-group">
                                        <label id="TxtNewAddressMobileLabel" class="col-sm-2 control-label no-padding-right" for="TxtNewAddressMobile"><%=this.Dictionary["Item_CompanyAddress_FieldLabel_Cellular"] %><span style="color:#f00">*</span></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtNewAddressMobile" placeholder="<%=this.Dictionary["Item_CompanyAddress_FieldLabel_Cellular"] %>" value="" onkeypress="validate(event)" maxlength="15" onblur="this.value=$.trim(this.value);" />
                                            <span class="ErrorMessage" id="TxtNewAddressMobileErrorRequired"><%=this.Dictionary["Item_CompanyData_Error_Message_PhoneOrCellular"]%></span>
                                        </div>
                                        <label id="TxtNewAddressEmailLabel" class="col-sm-1 control-label no-padding-right" for="TxtNewAddressEmail"><%=this.Dictionary["Item_CompanyData_FieldLabel_Email"] %><span style="color:#f00">*</span></label>
                                        <div class="col-sm-5">
                                            <input type="text" class="col-xs-12 col-sm-12" id="TxtNewAddressEmail" placeholder="<%=this.Dictionary["Item_CompanyData_FieldLabel_Email"] %>" value="" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                            <span class="ErrorMessage" id="TxtNewAddressEmailErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtNewAddressEmailErrorMalformed"><%=this.Dictionary["Common_Error_IncorrectMail"] %></span>
                                        </div>
                                    </div>
                                </form>
                            </div>
                            <div id="ChangeImageDialog" class="hide" style="width:850px;">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label"><%=this.Dictionary["Item_Equipment_Field_Image_Actual_Label"] %></label>
                                    <label class="col-sm-6 control-label"><%=this.Dictionary["Item_Equipment_Field_Image_New_Label"] %></label>
                                    <div class="col-sm-6" style="border:1px solid #aaa;vertical-align:middle;text-align:center;height:200px;background-color:#fefefe;padding:4px;">
                                        <img id="actual" src="/images/Logos/<%=this.Logo %>.jpg" style="max-width:192px;max-height:192px;" alt="<%=this.Dictionary["Item_Equipment_Field_Image_New_Label"] %>"/>
                                    </div>
                                    <div class="col-sm-6" style="border:1px solid #aaa;border-left:none;vertical-align:middle;text-align:center;height:200px;background-color:#fefefe;padding:4px;">
                                        <img id="blah" src="/images/noimage.jpg"alt="<%=this.Dictionary["Item_Equipment_Field_Image_Actual_Label"] %>"  style="max-width:192px;max-height:192px;" />                                                                    
                                    </div>
                                    <div class="col-sm-12" style="margin-top:12px;">
                                        <input type='file' id="imgInp" />
                                    </div>
                                </div>                                
                            </div>                           
                            <div id="DeleteImageDialog" class="hide" style="width:500px;">
                                <p>¿Seguro&nbsp;<strong><span id="Span1"></span></strong>?</p>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/assets/js/chosen.jquery.min.js"></script>
        <script type="text/javascript" src="/assets/js/fuelux/fuelux.spinner.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/moment.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/daterangepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-colorpicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.knob.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.autosize.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.inputlimiter.1.3.1.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.maskedinput.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-tag.min.js"></script>
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/Country.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/CompanyProfile.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/CompanyAddress.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/CompanyLogo.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.2/d3.js" charset="utf-8"></script>
        <script type="text/javascript" src="/nv.d3/nv.d3.js"></script>
        <script type="text/javascript">
            var chartPie1, chartPie1Data;
            var dataPie1 =
                [
                    {"label": "ALPHABET", "value": 313},
                    {"label": "ARVAL", "value": 241},
                    {"label": "LEASEPLAN", "value": 12},
                    {"label": "LEASEPLAN RUMANIA", "value": 2}
                ];
            nv.addGraph(function () {
                console.log("DiskQuote",diskQuote);
                chartPie1 = nv.models.pieChart()
                    .x(function (d) { return d.label })
                    .y(function (d) { return d.value })                    
                    
                    .height(300)
                    .showLabels(true)
                    .labelType("percent")
                    .donut(true).donutRatio(0.1);

                chartPie1Data = d3.select('#Pie1 svg').datum(diskQuote);
                chartPie1Data.transition().duration(500).call(chartPie1);
                nv.utils.windowResize(chartPie1.update);

                $("#PieWidget").hide();

                return chartPie1;
            });

            if(typeof ApplicationUser.Grants.CompanyProfile === "undefined" || ApplicationUser.Grants.CompanyProfile.Write === false){
                $(".btn-danger").hide();
                $("input").attr("disabled",true);
                $("textarea").attr("disabled",true);
                $("select").attr("disabled",true);
                $("select").css("background-color","#eee");
                $("#BtnEquipmentChangeImage").hide();                
            }

            if (user.PrimaryUser && 1 === 2) {
                var res = "<button class=\"btn btn-info\" type=\"button\" id=\"BtnAgreement\" onclick=\"DownloadAgreement();\"><i class=\"icon-file bigger-110\"></i><%=this.Dictionary["Agreement_Button_Download"] %></button>&nbsp;&nbsp;";
                $("#ItemButtons").prepend(res);
            }

            function DownloadAgreement() {
                window.open("/Agreement/Agreement_<%=this.Company.Name %>.pdf");
            }

            $("#CmbIdioma").val(Company.Language);

            function Resize() {
                var listTable = document.getElementById('pdfViewer');
                var containerHeight = $(window).height();
                listTable.style.height = (containerHeight - 310) + 'px';
            }

            Resize();
        </script>
</asp:Content>