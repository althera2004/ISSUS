﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="ResetPassword" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <title>ISUS</title>
        
        <!--[if !IE]> -->
        <script type="text/javascript" src="assets/js/jquery-2.0.3.min.js"></script>
        <!-- <![endif]-->

        <!--[if IE]>
        <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
        <![endif]-->

        <meta name="description" content="User login page" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />

        <!-- basic styles -->

        <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
        <link rel="stylesheet" href="assets/css/font-awesome.min.css" />

        <!--[if IE 7]>
          <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
        <![endif]-->

        <!-- page specific plugin styles -->

        <!-- fonts -->

        <link rel="stylesheet" href="assets/css/ace-fonts.css" />

        <!-- ace styles -->

        <link rel="stylesheet" href="assets/css/ace.min.css" />
        <link rel="stylesheet" href="assets/css/ace-rtl.min.css" />

        <!--[if lte IE 8]>
          <link rel="stylesheet" href="assets/css/ace-ie.min.css" />
        <![endif]-->

        <!-- inline styles related to this page -->

        <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

        <!--[if lt IE 9]>
        <script src="assets/js/html5shiv.js"></script>
        <script src="assets/js/respond.min.js"></script>
        <![endif]-->

        <script type="text/javascript" src="js/ResetPassword.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript">
            var user = <%=this.User%>;
            var companyId = <%=this.CompanyId%>;
            var op = '<%=this.op%>';
            window.onload = function()
            {
                document.getElementById('UserName').innerHTML = user.Login;
                //document.getElementById('EmployeeName').innerHTML = user.Employee.Name + ' ' + user.Employee.LastName;
                document.getElementById('UserId').value = user.Id;
                document.getElementById('CompanyId').value = companyId;
            }
        </script>
    </head>

    <body class="login-layout" style="background-color:#fff;">
        <div class="main-container">
            <div class="main-content">
                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="center">
                                <h1>
                                    <span class="blue">ISSUS 1.2</span>
                                </h1>
                            </div>

                            <div class="space-6"></div>

                            <div class="position-relative">
                                <div id="login-box" class="login-box visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <table runat="server" id="TableCompany" style="width:100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Image runat="server" ID="ImgCompnayLogo" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <h4 class="header black lighter bigger">Cambiar contraseña</h4>
                                            <p>Por motivos de seguridad es necesario cambiar la contraseña</p>
                                            <div class="space-6"></div>
                                            <label class="col-sm-12 control-label no-padding-right">User:&nbsp;<strong><span id="UserName"></span></strong></label>
                                            <!--<label class="col-sm-12 control-label no-padding-right"><%=this.Dictionary["Item_Employee"] %>:&nbsp;<strong><span id="EmployeeName"></span></strong></label>-->
                                            <form id="LoginForm" action="PreInitSession.aspx" method="post">                                                
                                                <div style="display:none;">
                                                    <input type="text" name="UserId" id="UserId" />
                                                    <input type="text" name="CompanyId" id="CompanyId" />
                                                    <input type="text" name="Password" id="Password" />
                                                </div>
                                                <fieldset>
                                                    <label class="block clearfix">
                                                        <span class="block input-icon input-icon-right">
                                                            <input type="password" class="form-control" placeholder="Password" id="TxtPassword1" />
                                                            <i class="icon-user"></i>
                                                        </span>
                                                    </label>
                                                    <label class="block clearfix">
                                                        <span class="block input-icon input-icon-right">
                                                            <input type="password" class="form-control" placeholder="Confirm password" id="TxtPassword2" />
                                                            <i class="icon-lock"></i>
                                                        </span>
                                                    </label>
                                                    <div class="space"></div>
                                                    <div class="clearfix">
                                                        <button type="button" class="width-35 pull-right btn btn-sm btn-primary" id="BtnLogin">Cambiar</button>
                                                    </div>
                                                    <div class="space-4"></div>
                                                    <h4>
                                                        <span id="ErrorMessage"></span>
                                                    </h4>
                                                </fieldset>
                                            </form>
                                            <asp:Literal runat="server" ID="LtCompnayName"></asp:Literal>
                                        </div><!-- /widget-main -->
                                    </div><!-- /widget-body -->
                                </div><!-- /login-box -->
                            </div><!-- /position-relative -->
                        </div>
                    </div><!-- /.col -->
                </div><!-- /.row -->
            </div>
        </div><!-- /.main-container -->

        <!-- basic scripts -->		

        <script type="text/javascript">
            if ("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
        </script>
    </body>
</html>
