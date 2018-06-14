<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgreementNotice.aspx.cs" Inherits="AgreementNotice" %>

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
    <script type="text/javascript">
        var language = "<%=this.LanguageBrowser %>";
        var ip = "<%=this.IP %>";
        var Dictionary =
            {
                "es": {
                    "Title": "Acuerdo de prestación de servicios",
                    "Message": "Para hacer uso de la aplicación la persona de contacto debe aceptar los términos y condiciones."
                },
                "ca": {
                    "Title": "Acord de prestació de serveis",
                    "Message": "Per fer ús de l'aplicació la persona de contacte han d'acceptar els termes i condicions."
                }
            };

        window.onload = function () {
            $("#PageTitle").html(Dictionary[language].Title);
            $("#Message").html(Dictionary[language].Message);
        }
    </script>
</head>

<body class="login-layout" style="background-color: #fff; background-image: url(/WelcomeBackgrounds/<%=this.BK%>); background-size: cover;">
    <div class="main-container">
        <div class="main-content">
            <div class="row">
                <div class="col-sm-10 col-sm-offset-1">
                    <div class="login-container" style="width: 750px!important;">
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
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>