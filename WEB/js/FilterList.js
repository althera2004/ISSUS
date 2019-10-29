function FilterList(e, listId, listDataHeader, totalDOMElement) {
    if (typeof listId === "undefined" || listId === null) {
        listId = "ListDataTable";
    }

    if (typeof listDataHeader === "undefined" || listDataHeader === null) {
        listDataHeader = "ListDataHeader";
    }


    if (document.getElementById(listId) !== null) {
        var searcheablesIndex = new Array();
        var headerList = document.getElementById(listDataHeader).childNodes;
        var cont = 0;
        var totalizableIndex = -1;
        for (var h = 0; h < headerList.length; h++) {
            if (headerList[h].tagName !== "TH") { continue; }

            if (headerList[h].className !== null && headerList[h].className.indexOf("search") !== -1) {
                searcheablesIndex.push(cont);
            }

            if (headerList[h].className.indexOf("totalizable") !== -1) {
                totalizableIndex = cont;
            }

            cont++;
        }


        var pattern = document.getElementById("nav-search-input").value.toUpperCase();
        var list = document.getElementById(listId);
        var cont = 0;
        var total = 0;
        for (var x = 0; x < list.childNodes.length; x++) {
            var row = list.childNodes[x];
            if (row.tagName === "TR") {
                var match = false;

                for (var y = 0; y < searcheablesIndex.length; y++) {
                    if (row.childNodes[searcheablesIndex[y]].childNodes.length > 0) {
                        var item = row.childNodes[searcheablesIndex[y]].innerText.toUpperCase();
                        if (item.indexOf(pattern) !== -1) {
                            match = true;
                            break;
                        }
                    }
                }

                if (match === true) {
                    cont++;
                    if (totalizableIndex > 0) {
                        var totalText = row.childNodes[totalizableIndex].innerText;
                        total += StringToNumber(totalText, ".", ",");
                    }
                }

                row.style.display = match ? "" : "none";
            }
        }

        if (typeof totalDOMElement !== "undefined" && totalDOMElement !== null && totalDOMElement !== $("#" + totalDOMElement).length > 0) {
            $("#" + totalDOMElement).html(cont);
        }
        else {
            $("#TotalList").html(cont);
        }
        $("#TotalAmount").html(ToMoneyFormat(total, 2));
    }
}