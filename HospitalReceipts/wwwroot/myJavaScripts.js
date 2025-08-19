function printDiv(divId) {
    var content = document.getElementById(divId).innerHTML;
    var myWindow = window.open('', '', 'width=800,height=600');

    myWindow.document.write('<html><head><title>Print</title>');

    // bring in your app’s CSS so print looks same as screen
    document.querySelectorAll('link[rel="stylesheet"]').forEach(function (css) {
        myWindow.document.write(css.outerHTML);
    });

    myWindow.document.write('</head><body>');
    myWindow.document.write(content);
    myWindow.document.write('</body></html>');

    myWindow.document.close();
    myWindow.focus();
    myWindow.print();
    myWindow.close();
}
