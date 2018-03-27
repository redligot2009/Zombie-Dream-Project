//Usage:
//1) Save this file to disk.
//2) Load the target *.fla file in Flash.
//3) Commands | Run Command, Choose this file.
//4) Choose a target output folder, then sit back and wait.

fl.outputPanel.clear();

var outputScaleFactor = 2;

var folderURI = fl.browseForFolderURL('Select output folder');

var doc = fl.getDocumentDOM();
var newDoc = fl.createDocument();
var newDom = fl.getDocumentDOM();

for(i in doc.library.items)
{
    var currentItem = doc.library.items[i];
    if(currentItem.itemType == "graphic" ||
        currentItem.itemType == "bitmap" ||
        currentItem.itemType == "movie clip")
    {
        exportItemAsPng(currentItem, outputScaleFactor);
    }
}

fl.closeDocument(newDoc, false);

function exportItemAsPng(item, scaleFactor)
{
    var itemName = item.name.split('.')[0];
    var exportPath = folderURI + "/" + itemName + ".png";

    newDoc.addItem({x:0.0, y:0.0}, item);
    newDom.library.selectItem(item.name, false);
    newDoc.scaleSelection(scaleFactor, scaleFactor);

    // verify that the path exists
    // this will still fail if the library item is more than one folder deep
    FLfile.createFolder(exportPath.substring(0, exportPath.lastIndexOf("/")));

    if(item.itemType == "movie clip")
        newDoc.exportInstanceToPNGSequence(exportPath);
    else
        newDoc.exportPNG(exportPath, true, false);

    newDoc.deleteSelection();
}