<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="EmployeeNew.aspx.cs" Inherits="EmployeeNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">


                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="widget-box">
                                            <div class="widget-header widget-header-blue widget-header-flat">
                                                <h4 class="lighter"><%=this.Dictionary["Asistente de creación de empleado"] %></h4>
                                            </div>
                                            <div class="widget-body">
                                                <div class="widget-main">
                                                    <div id="fuelux-wizard" class="row-fluid" data-target="#step-container">
                                                        <ul class="wizard-steps">
                                                            <li data-target="#step1" class="active">
                                                                <span class="step">1</span>
                                                                <span class="title"><%=this.Dictionary["Datos del empleado"] %></span>
                                                            </li>
                                                            <li data-target="#step2">
                                                                <span class="step">2</span>
                                                                <span class="title"><%=this.Dictionary["Departamentos"] %></span>
                                                            </li>
                                                            <li data-target="#step3">
                                                                <span class="step">3</span>
                                                                <span class="title"><%=this.Dictionary["Asignación de usuario"] %></span>
                                                            </li>
                                                            <li data-target="#step4">
                                                                <span class="step">4</span>
                                                                <span class="title"><%=this.Dictionary["Resumen"] %></span>
                                                            </li>
                                                        </ul>
                                                    </div>

                                                    <hr />
                                                    <div class="step-content row-fluid position-relative" id="step-container">
                                                        <div class="step-pane active" id="step1">
                                                            <form class="form-horizontal" id="validation-form" method="get">
                                                                <div class="form-group">
                                                                    <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="email"><%=this.Dictionary["Nombre"] %></label>

                                                                    <div class="col-xs-12 col-sm-9">
                                                                        <div class="clearfix">
                                                                            <input type="text" name="Name" id="Name" class="col-xs-12 col-sm-6" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="space-2"></div>
                                                                <div class="form-group">
                                                                    <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="email"><%=this.Dictionary["Primer apellido"] %>:</label>

                                                                    <div class="col-xs-12 col-sm-9">
                                                                        <div class="clearfix">
                                                                            <input type="text" name="LastName" id="LastName" class="col-xs-12 col-sm-6" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="space-2"></div>
                                                                <div class="form-group">
                                                                    <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="email"><%=this.Dictionary["Email"] %>:</label>

                                                                    <div class="col-xs-12 col-sm-9">
                                                                        <div class="clearfix">
                                                                            <input type="email" name="email" id="email2" class="col-xs-12 col-sm-6" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="space-2"></div>

                                                                <div class="form-group">
                                                                    <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="phone"><%=this.Dictionary["Teléfono"] %>:</label>

                                                                    <div class="col-xs-12 col-sm-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">
                                                                                <i class="icon-phone"></i>
                                                                            </span>
                                                                            <input type="tel" id="phone" name="phone" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </form>
                                                        </div>

                                                        <div class="step-pane" id="step2">
                                                            <div class="row-fluid">
                                                                <div class="alert alert-success">
                                                                    <button type="button" class="close" data-dismiss="alert">
                                                                        <i class="icon-remove"></i>
                                                                    </button>

                                                                    <strong>
                                                                        <i class="icon-ok"></i>
                                                                        Well done!
                                                                    </strong>

                                                                    You successfully read this important alert message.
                                                                    <br />
                                                                </div>

                                                                
                                                                <div class="form-group has-info">
                                                                    <label for="inputInfo" class="col-xs-12 col-sm-3 control-label no-padding-right">Input with info</label>

                                                                    <div class="col-xs-12 col-sm-5">
                                                                        <span class="block input-icon input-icon-right">
                                                                            <input type="text" id="Text1" class="width-100" />
                                                                            <i class="icon-info-sign"></i>
                                                                        </span>
                                                                    </div>
                                                                    <div class="help-block col-xs-12 col-sm-reset inline"> Info tip help! </div>
                                                                </div>

                                                                <div class="alert alert-danger">
                                                                    <button type="button" class="close" data-dismiss="alert">
                                                                        <i class="icon-remove"></i>
                                                                    </button>

                                                                    <strong>
                                                                        <i class="icon-remove"></i>
                                                                        Oh snap!
                                                                    </strong>

                                                                    Change a few things up and try submitting again.
                                                                    <br />
                                                                </div>

                                                                <div class="alert alert-warning">
                                                                    <button type="button" class="close" data-dismiss="alert">
                                                                        <i class="icon-remove"></i>
                                                                    </button>
                                                                    <strong>Warning!</strong>

                                                                    Best check yo self, you're not looking too good.
                                                                    <br />
                                                                </div>

                                                                <div class="alert alert-info">
                                                                    <button type="button" class="close" data-dismiss="alert">
                                                                        <i class="icon-remove"></i>
                                                                    </button>
                                                                    <strong>Heads up!</strong>

                                                                    This alert needs your attention, but it's not super important.
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="step-pane" id="step3">
                                                            <div class="center">
                                                                <h3 class="blue lighter">This is step 3</h3>
                                                            </div>
                                                        </div>

                                                        <div class="step-pane" id="step4">
                                                            <div class="center">
                                                                <h3 class="green">Congrats!</h3>
                                                                Your product is ready to ship! Click finish to continue!
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <hr />
                                                    <div class="row-fluid wizard-actions">
                                                        <button class="btn btn-prev">
                                                            <i class="icon-arrow-left"></i>
                                                            Prev
                                                        </button>

                                                        <button class="btn btn-success btn-next" data-last="Finish ">
                                                            Next
                                                            <i class="icon-arrow-right icon-on-right"></i>
                                                        </button>
                                                    </div>
                                                </div><!-- /widget-main -->
                                            </div><!-- /widget-body -->
                                        </div>
                                    </div>
                                </div>

                                <div id="modal-wizard" class="modal">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header" data-target="#modal-step-contents">
                                                <ul class="wizard-steps">
                                                    <li data-target="#modal-step1" class="active">
                                                        <span class="step">1</span>
                                                        <span class="title">Validation states</span>
                                                    </li>

                                                    <li data-target="#modal-step2">
                                                        <span class="step">2</span>
                                                        <span class="title">Alerts</span>
                                                    </li>

                                                    <li data-target="#modal-step3">
                                                        <span class="step">3</span>
                                                        <span class="title">Payment Info</span>
                                                    </li>

                                                    <li data-target="#modal-step4">
                                                        <span class="step">4</span>
                                                        <span class="title">Other Info</span>
                                                    </li>
                                                </ul>
                                            </div>

                                            <div class="modal-body step-content" id="modal-step-contents">
                                                <div class="step-pane active" id="modal-step1">
                                                    <div class="center">
                                                        <h4 class="blue">Step 1</h4>
                                                    </div>
                                                </div>

                                                <div class="step-pane" id="modal-step2">
                                                    <div class="center">
                                                        <h4 class="blue">Step 2</h4>
                                                    </div>
                                                </div>

                                                <div class="step-pane" id="modal-step3">
                                                    <div class="center">
                                                        <h4 class="blue">Step 3</h4>
                                                    </div>
                                                </div>

                                                <div class="step-pane" id="modal-step4">
                                                    <div class="center">
                                                        <h4 class="blue">Step 4</h4>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="modal-footer wizard-actions">
                                                <button class="btn btn-sm btn-prev">
                                                    <i class="icon-arrow-left"></i><%=this.Dictionary["Anterior"] %>
                                                </button>

                                                <button class="btn btn-success btn-sm btn-next" data-last="Finish ">
                                                    <%=this.Dictionary["Siguiente"] %>
                                                    <i class="icon-arrow-right icon-on-right"></i>
                                                </button>

                                                <button class="btn btn-danger btn-sm pull-left" data-dismiss="modal">
                                                    <i class="icon-remove"></i>
                                                    Cancel
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                            
                            </div><!-- /.col -->
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
</asp:Content>

