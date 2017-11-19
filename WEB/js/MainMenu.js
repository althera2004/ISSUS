$(document).ready(function () {
    FillEmpresaForm();
    RenderMainPanel();

    $('.MainPanelButton').on('click', Redirect);
    $('.MainPanelButton').on('mouseover', MouseOver);
    $('.MainPanelButton').on('mouseout', MouseOut);
});

function MouseOver(e) {
    document.getElementById(e.currentTarget.id).className = 'MainPanelButtonOver';
}

function MouseOut(e) {
    document.getElementById(e.currentTarget.id).className = 'MainPanelButton';
}

function Redirect(e) {
    switch (e.currentTarget.id) {
        case 'CompanyData': document.location = 'Company.aspx'; break;
        case 'Departments': document.location = 'Departments.aspx'; break;
        case 'Employees': document.location = 'Employees.aspx'; break;
        case 'Documents': document.location = 'Documents.aspx'; break;
        case 'Providers': document.location = 'Providers.aspx'; break;
        default: break;
    }

    return false;
}

function RenderMainPanel() {
    var target = document.getElementById('MainPanel');

    for (var x = 0; x < User.Grants.length; x++) {
        var grant = User.Grants[x];
        var div = document.createElement('div');
        div.id = grant.Id;
        div.className = 'MainPanelButton';
        var label = document.createElement('div');
        label.className = 'MainPanelButtonLabel';
        label.appendChild(document.createTextNode(grant.Description));
        div.appendChild(label);
        target.appendChild(div);
    }
}