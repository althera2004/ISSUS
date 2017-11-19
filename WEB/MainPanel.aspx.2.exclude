<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPanel.aspx.cs" Inherits="MainPanel" %>

<!doctype html>
<!--[if lt IE 7]> <html class="no-js lt-ie9 lt-ie8 lt-ie7" lang="en"> <![endif]-->
<!--[if IE 7]>    <html class="no-js lt-ie9 lt-ie8" lang="en"> <![endif]-->
<!--[if IE 8]>    <html class="no-js lt-ie9" lang="en"> <![endif]-->
<!--[if gt IE 8]><!-->
<html class="no-js" lang="en"> <!--<![endif]-->
<head runat="server">
    <meta charset="utf-8" />
    <title>Esthetics Admin - Clean &amp; Responsive Admin Template</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="scripts/script.js"></script>
    <link href='http://fonts.googleapis.com/css?family=Droid+Sans' rel='stylesheet' type='text/css' />
    <script type="text/javascript">
        var CompanyId = <%=this.CompanyId %>;
        var CompanyData = <%=this.CompanyData %>;
        var User = <%=this.User %>;
    </script>
</head>

<body>
    <asp:Literal runat="server" ID="LeftMenu"></asp:Literal>
  
  
  
  
  
    <div class="main" style="margin-left:100px;">
  
        <div class="non-shortable-content" style="width:50%;float:left;">
            <h1><span id="CompanyName"></span></h1>
            <h6><%=this.Dictionary["Panel principal"]  %></h6> 
        </div>
        <div class="user" style="width:30%;float:right;border:none;">
            <img src="images/user.jpg" alt="Esthetics Admin"/>
            <span id="UserName"></span>
            <p>Administrador</p>
        </div> 
    
  <div class="shortable-content">
  
   <div class="box _50">
    <div class="box-header">
      Full Calendar
      </div>
    <div class="box-content">
      <div id="calendar"></div>
    </div>
  </div> <!--CALENDAR ENDS HERE-->
  
  
  <div class="box _50">
    <div class="box-header">
      Chatting Layout
      <i class="icon-remove-sign close" title="Close"></i>
      <i class="icon-minus-sign minimize" title="Minimize/Maximize"></i>
      </div>
    <div class="box-content  padd-10">
      <ul class="messages">
          <li class="incoming">
            <a href="#">
              <img src="images/user.jpg" />
            </a>
            <div class="message_area">
            <span class="arow"></span>
            <div class="message_info">
              <span class="sender">Akshay</span><span class="says">says :</span><span class="time">3 hours ago</span>
            </div>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam vel est enim, vel eleifend felis. Ut volutpat, leo eget euismod scelerisque, eros purus lacinia velit, nec rhoncus mi dui eleifend orci.
            </div>
          </li>
          
          <li class="outgoing">
            <a href="#">
              <img src="images/user.jpg" />
            </a>
            <div class="message_area">
            <span class="arow"></span>
            <div class="message_info">
              <span class="sender">Akshay</span><span class="says">says :</span><span class="time">3 hours ago</span>
            </div>
              Ipsum dolor sit amet, consectetur adipiscing elit. Nam vel est enim, vel eleifend felis. Ut volutpat, leo eget euismod scelerisque, eros purus lacinia velit, nec rhoncus mi dui eleifend orci.<br/> <br/> Nam vel est enim, vel eleifend felis. Ut volutpat, leo eget euismod scelerisque, eros purus lacinia velit, nec rhoncus
            </div>
          </li>
          
          <li class="incoming">
            <a href="#">
              <img src="images/user.jpg" />
            </a>
            <div class="message_area">
            <span class="arow"></span>
            <div class="message_info">
              <span class="sender">Akshay</span><span class="says">says :</span><span class="time">3 hours ago</span>
            </div>
              leo eget euismod scelerisque, eros purus lacinia velit, nec rhoncus mi dui eleifend orci.
            </div>
          </li>
          
          <li>
            <input type="text" class="_75" placeholder="Message to Akshay" />
            <div class="float_r">
            <input type="reset" class="grey" />
            <input type="submit" value="Send" />
            </div>
          </li>
         
      </ul>
    </div>
  </div>  <!--MESSAGES ENDS HERE-->
  

  </div><!--SHORTABLECONTENT-ENDS-->

  </div><!--MAIN ENDS-->
</body>
</html>
