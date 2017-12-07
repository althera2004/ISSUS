<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Select.aspx.cs" Inherits="Select" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>ISSUS</title>
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="pragma" content="no-cache" />
    <link rel="icon" href="/favicon.ico?v=2" type="image/x-icon" />
    <!--[if !IE]> -->
    <script type="text/javascript" src="assets/js/jquery-2.0.3.min.js"></script>
    <!-- <![endif]-->

    <!--[if IE]>
        <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <![endif]-->

    <meta name="description" content="User login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />

    <!--[if IE 7]>
          <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
    <![endif]-->

    <link rel="stylesheet" href="assets/css/ace-fonts.css" />
    <link rel="stylesheet" href="assets/css/ace.min.css" />
    <link rel="stylesheet" href="assets/css/ace-rtl.min.css" />
    <style type="text/css">
        .logo {
            display: block;
            max-width: 150px;
            max-height: 95px;
            width: auto;
            height: auto;
        }

        .odd {
            background-color: #eee;
            cursor: pointer;
        }

        .pair {
            background-color: #fff;
            cursor: pointer;
        }
    </style>

    <!--[if lte IE 8]>
          <link rel="stylesheet" href="assets/css/ace-ie.min.css" />
    <![endif]-->
    <!--[if lt IE 9]>
        <script src="assets/js/html5shiv.js"></script>
        <script src="assets/js/respond.min.js"></script>
        <![endif]-->

    <script type="text/javascript" src="js/Login.js"></script>
    <script type="text/javascript">
        var language = navigator.language || navigator.userLanguage;
        var ip = "<%=this.IP %>";
        function Go(companyId, userId) {
            document.getElementById("CompanyId").value = companyId;
            document.getElementById("UserId").value = userId;
            document.getElementById("LoginForm").submit();
        }

        if ("ontouchend" in document) {
            document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
        }

        function GoFromCombo() {
            var value = document.getElementById("CmbCompany").value;
            if (value === "0") {
                return false;
            }

            var cId = value.split('|')[0];
            var uId = value.split('|')[1];
            Go(cId, uId);
        }

        var Dictionary =
        {
            "es": {
                "Title": "Acceso a diferentes empresas",
                "Message": "Su usuario tiene acceso a poder gestionar distintas empresas.<br />Seleccionar con cual quiere trabajar.<br />Para trabajar con otra empresa, deberá salir de la aplicación y volver a hacer login.",
                "Btn": "Acceder"
            },
            "ca": {
                "Title": "Accès a diferents empreses",
                "Message": "El seu usuari te accès a poder gestionar distintes empreses.<br />Sel·leccioni amb quina vol treballar.<br />Per a traballar amb una altra empresa, haurà de sortir de l'aplicació i tonar a validar-se.",
                "Btn": "Accedir"
            }
        };

        window.onload = function () {
            document.getElementById("PageTitle").innerHTML = Dictionary[language].Title;
            document.getElementById("Message").innerHTML = Dictionary[language].Message;
            if (document.getElementById("BtnLoginCmb") !== null) {
                document.getElementById("BtnLogin").innerHTML = Dictionary[language].Btn;
            }
        }
    </script>
</head>

<body class="login-layout" style="background-color: #fff; background-image: url(/WelcomeBackgrounds/<%=this.BK%>); background-size: cover;">
    <div class="main-container">
        <div class="main-content">
            <div class="row">
                <div class="col-sm-10 col-sm-offset-1">
                    <div class="login-container" style="width: 500px!important;">
                        <br />
                        <div class="center">
                            <img src="issus.png" alt="Issus" title="Issus" />
                        </div>
                        <div class="space-6"></div>
                        <div class="position-relative">
                            <div id="login-box" class="login-box visible widget-box no-border" style="border: 1px solid #ccc; z-index: 99999; background-color: transparent; box-shadow: 4px 4px 4px rgba(0, 0, 0, 0.25)">
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <h4 class="header black lighter bigger" id="PageTitle"></h4>
                                        <div class="alert alert-info">
                                            <p id="Message"></p>
                                        </div>
                                        <table style="width: 100%;" cellspacing="4">
                                            <tbody>
                                                <asp:Literal runat="server" ID="LtCompanies"></asp:Literal>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%; top: 75%; position: absolute; text-align: center;">
        <img src="betaversion.png" title="Beta version" />
        <br />
        <br />
        <a href="http://www.scrambotika.com" style="color: #000; font-size: 22px;">www.scrambotika.com</a>
    </div>
    <form id="LoginForm" action="InitSession.aspx" method="post">
        <div style="display: none;">
            <input type="text" name="LoginId" id="LoginId" value="<%=this.userId %>" />
            <input type="text" name="UserId" id="UserId" value="" />
            <input type="text" name="CompanyId" id="CompanyId" />
        </div>
    </form>
</body>
</html>