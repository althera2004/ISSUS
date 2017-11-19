<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MultipleSession.aspx.cs" Inherits="MultipleSession" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <title>ISSUS</title>
        <link rel="icon" href="/favicon.ico?v=2" type="image/x-icon" />
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
        <style type="text/css">            
            @media only screen and (min-width:481px){.only-480{display:none!important;}}

            .widget-main{-webkit-box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
-moz-box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);}
        </style>

        <!--[if lte IE 8]>
          <link rel="stylesheet" href="assets/css/ace-ie.min.css" />
        <![endif]-->

        <!-- inline styles related to this page -->

        <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

        <!--[if lt IE 9]>
        <script src="assets/js/html5shiv.js"></script>
        <script src="assets/js/respond.min.js"></script>
        <![endif]-->
    </head>

    <body class="login-layout" style="background-color:#fff;background-image:url(/WelcomeBackgrounds/<%=this.BK%>);background-size:cover;">
        <div class="main-container">
            <div class="main-content">
                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <br />
                            <div class="center">
                                <img src="issus.png" alt="Issus" title="Issus" />
                            </div>

                            <div class="space-6"></div>

                            <div class="position-relative">
                                <div id="login-box" class="login-box visible widget-box no-border" style="background-color:transparent;">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header black lighter bigger">Detectada nueva sesión</h4>

                                            <div class="space-6"></div>
                                            <p>Se ha detectado que el usuario actual ha iniciado una sesión más recientes en otro equipo o navegador.</p>
                                        </div>
                                        <!-- /widget-main -->
                                    </div>
                            
                                    <!-- /widget-body -->

                                    <div class="only-480" style="width:100%;top:75%;background-color:transparent;text-align:center;">
                                        <br /><br />
                            <img src="betaversion.png" title="Beta version" style="max-width:100%;" />
                            <br /><br />
                            <a href="http://www.scrambotika.com" style="color:#000;font-size:18px;">www.scrambotika.com</a>

                        </div>
                                </div>

                                
                                <!-- /login-box -->
                            </div>
                            
                            <!-- /position-relative -->
                        </div>
                    </div><!-- /.col -->
                </div><!-- /.row -->
            </div>
        </div><!-- /.main-container -->

        

                        <div id="BetaVersionPC" class="hidden-480" style="width:100%;top:75%;position:absolute;text-align:center;">
                            <img src="betaversion.png" title="Beta version" style="max-width:100%;" />
                            <br /><br />
                            <a href="http://www.scrambotika.com" style="color:#000;font-size:22px;">www.scrambotika.com</a>

                        </div>

        <!-- basic scripts -->		

        <script type="text/javascript">
            if ("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
        </script>
    </body>
</html>
