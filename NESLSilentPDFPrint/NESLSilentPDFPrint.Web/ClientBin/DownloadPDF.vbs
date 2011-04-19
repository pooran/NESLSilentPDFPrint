SaveWebBinary WScript.Arguments(0) ,WScript.Arguments(1), WScript.Arguments(2)
Function SaveWebBinary(strUrl, strFile, strPrinter)
    strUrl =Mid (strUrl, 1, Len(strUrl)-1) 
    strFile =Mid (strFile, 1, Len(strFile)-1) 
    Dim EXEToRun
    EXEToRun=  """C:\\Program Files (x86)\\Total PDF Printerx\\PDFPrinterx.exe"""

    Const adTypeBinary = 1
    Const adSaveCreateOverWrite = 2
    Const ForWriting = 2
    Dim web, varByteArray, strData, strBuffer, lngCounter, ado
    'On Error Resume Next
    'Download the file with any available object
    Err.Clear
    Set web = Nothing
    Set web = CreateObject("WinHttp.WinHttpRequest.5.1")
    If web Is Nothing Then Set web = CreateObject("WinHttp.WinHttpRequest")
    If web Is Nothing Then Set web = CreateObject("MSXML2.ServerXMLHTTP")
    If web Is Nothing Then Set web = CreateObject("Microsoft.XMLHTTP")
    If web Is Nothing Then MsgBox "PDF download Failed"
    web.Open "GET", strURL, False
    web.Send
    If Err.Number <> 0 Then
        SaveWebBinary = False
        Set web = Nothing
        Exit Function
    End If
    If web.Status <> "200" Then
        SaveWebBinary = False
        Set web = Nothing
        Exit Function
    End If
    varByteArray = web.ResponseBody
    Set web = Nothing
    'Now save the file with any available method
    On Error Resume Next
    Set ado = Nothing
    Set ado = CreateObject("ADODB.Stream")
    If ado Is Nothing Then
        Set fs = CreateObject("Scripting.FileSystemObject")
        Set ts = fs.OpenTextFile(strFile, ForWriting, True)
        strData = ""
        strBuffer = ""
        For lngCounter = 0 to UBound(varByteArray)
            ts.Write Chr(255 And Ascb(Midb(varByteArray,lngCounter + 1, 1)))
        Next
        ts.Close
    Else
        ado.Type = adTypeBinary
        ado.Open
        ado.Write varByteArray
        ado.SaveToFile strFile, adSaveCreateOverWrite
        ado.Close
        
        EXEToRun = EXEToRun +" " +""""+strFile+""""
        EXEToRun = EXEToRun +" -p" +"""" +strPrinter +"""" 
        Dim oShell
        Set oShell = WScript.CreateObject ("WScript.Shell")
        oShell.run EXEToRun
        Set oShell = Nothing
    End If
    SaveWebBinary = True
    
End Function