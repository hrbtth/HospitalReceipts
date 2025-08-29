function createPages(numPages) {
    let container = document.getElementById("pagesContainer");
    container.innerHTML = ""; // Clear previous pages
    let fragment = document.createDocumentFragment();//temp object to hold elements of base page to be created.
    for (let i = 0; i < numPages; i++) {
        let newPage = createBasePage(i + 1,"This is Header Text","This is Footer Text");
        let content = document.createElement("div");
        content.className = "pageContent";
        content.textContent = `This is the content of page ${i + 1}.`;
        newPage.appendChild(content);
        fragment.appendChild(newPage);
    }
    container.appendChild(fragment);// add the temp object to DIv "pagesContainer"
}
function createPageAndAddToContainer(pageNo,pageHeaderText,ContentHtmlText,pageFooterText) {
    let container = document.getElementById("pagesContainer");
    // do not clear previous pages /html inner content.
    
    let newPage = createBasePage(pageNo, pageHeaderText, pageFooterText);
    // Page content
    let content = document.createElement("div");
    content.className = "pageContent";
    content.innerHTML = ContentHtmlText; // use innerHTML if you want HTML text, not just plain text

    // Insert content above footer
    newPage.insertBefore(content, newPage.querySelector(".pageFooter"));  
    //add new page to container
    container.appendChild(newPage);// add the temp object to DIv "pagesContainer"
}
function createBasePage(pageNo,headerTxt,footerTxt) {
    // Create page div
    let page = document.createElement("div");
    page.className = "a4page"; 

    ////twocolumn container in side page
    //let twoColumnContainer = document.createElement("div");
    //twoColumnContainer.className = "two-column-container";
    //page.appendChild(twoColumnContainer);

    ////page content container inside two column container
    //let pageContent = document.createElement("div");
    //pageContent.className = "pageContent";
    //twoColumnContainer.appendChild(pageContent);

    // Page number 
    let pageNum = document.createElement("div");
    pageNum.className = "pageNumber";
    pageNum.textContent = `Page ${pageNo}`;
    page.appendChild(pageNum);

    // page header
    let pageHeader = document.createElement("div");
    pageHeader.textContent = headerTxt;
    pageHeader.className = "pageHeader";
    page.appendChild(pageHeader);


    // page footer
    let pageFooter = document.createElement("div");
    pageFooter.textContent = footerTxt;
    pageFooter.className = "pageFooter";
    page.appendChild(pageFooter);    

    return  page;
}
function paginateContent(reportHeaderTxt,pageFooterTxt) {
    //source division
    let source = document.getElementById("sourceContent");
    let contentElements = Array.from(source.children);

    //Actual division where html elements are rendered with pagination
    let container = document.getElementById("pagesContainer");
    container.innerHTML = ""; // Clear previous pages

    //First create a New page(just a division with styles of page)

    //-------------------------new page Creation Starts -------------------------//
    let pageNo = 1;
    let  currentPage = createBasePage(pageNo, reportHeaderTxt, pageFooterTxt);
    //immidiately append this page division to parent containor.Otherwise we can't get measurements of its chaild elements.
    container.appendChild(currentPage);

    //create a two column container and add it to page.
    let twoColumnContainer = document.createElement("div");
    twoColumnContainer.className = "two-column-container"; //this style is inside Razor page.
    currentPage.appendChild(twoColumnContainer);

    //create a "pagecontent" container and add it to two column container.
    pageContent = document.createElement("div");
    pageContent.className = "pageContent"; //this style is inside Razor page.    
    twoColumnContainer.appendChild(pageContent);

    //column header:
    //do not add column header to first page.it is there in source content.

    //later html elements should be added to this pageContent container /division.
    
    
    //pageUsableHeight and pageusable width 
    let pageBoxHeight = currentPage.clientHeight;
    let pageBoxWidth = currentPage.clientWidth;
    //TO get padding, get the style of page container
    let style = window.getComputedStyle(currentPage);
    //    //// Extract padding values
    let paddingTop = parseFloat(style.paddingTop);     // in pixels
    let paddingBottom = parseFloat(style.paddingBottom); // in pixels
    let paddingLeft = parseFloat(style.paddingLeft);
    let paddingRight = parseFloat(style.paddingRight);
    //Total available height inside padding
    let pageUsableHeight = pageBoxHeight - paddingTop - paddingBottom;
    pageUsableHeight += 2; // add 2px to account for down shift of inner containers.
    //Total available width inside padding
    let pageUsableWidth = pageBoxWidth - paddingLeft - paddingRight;
    pageUsableWidth += 2; // add 2px to account for right shift of inner containers.
    let columnHeaderFlag = true;
    //----------------------------------new page Creation Ends -------------------------//

    //Now in a loop add html elements from source division to the page content division.    
    for (let el of contentElements) {        
        let clone = el.cloneNode(true);  //get one html element from source
        pageContent.appendChild(clone); // add this element to page
        //now measur the distance of last element in page from page top
        let contentHeight = pageContent.offsetHeight;
        console.log("content height:", contentHeight);        
        //also measure the distance of last element in page from page left
        let contentWidth = pageContent.offsetWidth;        

        //check to add new columnHeader
        if ((contentWidth > (pageUsableWidth / 2)) && columnHeaderFlag) {
            pageContent.removeChild(clone);
            let columnHeader = document.createElement("div");
            columnHeader.className = "columnHeader";
            pageContent.appendChild(columnHeader);
            columnHeader.innerHTML = "<span>R No</span><span>Name</span><span>Amount</span>";
            pageContent.appendChild(clone);
            columnHeaderFlag = false;
           
        }
       

        // check to add new page
        if ((contentHeight > pageUsableHeight) || (contentWidth > pageUsableWidth)) {
            // if content (height or width) exceeds page, remove last added element from page
            pageContent.removeChild(clone);
            //-------------------------new page Creation Starts -------------------------//
            pageNo++;
            let currentPage = createBasePage(pageNo, "", pageFooterTxt);//leave header blank for next page.
            //immidiately append this page division to parent containor.Otherwise we can't get measurements of its chaild elements.
            container.appendChild(currentPage);
            //create a two column container and add to page.
            let twoColumnContainer = document.createElement("div");
            twoColumnContainer.className = "two-column-container"; //this style is inside Razor page.
            currentPage.appendChild(twoColumnContainer);
            //create a "pagecontent" container and add to two column container.
            pageContent = document.createElement("div");
            pageContent.className = "pageContent"; //this style is inside Razor page.
            //pageContent.className = "two-column-container"; //this style is inside Razor page.
            //immidiately add it to page division. otherwise we cant do measurement inside it.
            twoColumnContainer.appendChild(pageContent);
            //add column header to new page
            let columnHeader = document.createElement("div");
            columnHeader.className = "columnHeader";
            pageContent.appendChild(columnHeader);
            columnHeader.innerHTML = "<span>R No</span><span>Name</span><span>Amount</span>";
            pageContent.appendChild(columnHeader);

            //pageUsableHeight and pageusable width 
            let pageBoxHeight = currentPage.clientHeight;
            let pageBoxWidth = currentPage.clientWidth;
            //TO get padding, get the style of page container
            let style = window.getComputedStyle(currentPage);
            //Extract padding values
            let paddingTop = parseFloat(style.paddingTop);     // in pixels
            let paddingBottom = parseFloat(style.paddingBottom); // in pixels
            let paddingLeft = parseFloat(style.paddingLeft);
            let paddingRight = parseFloat(style.paddingRight);
            //Total available height inside padding
            let pageUsableHeight = pageBoxHeight - paddingTop - paddingBottom;
            pageUsableHeight += 2; //add 2px to account for down shift of inner containers.
            //Total available width inside padding
            let pageUsableWidth = pageBoxWidth - paddingLeft - paddingRight;
            pageUsableWidth += 2; // add 2px to account for right shift of inner containers.
            columnHeaderFlag = true; // reset column header flag for new page.
            //----------------------------------new page Creation Ends -------------------------//

            //add the element that was removed earlier from previous page.
            pageContent.appendChild(clone); 
        }
    }   
}
      
    





