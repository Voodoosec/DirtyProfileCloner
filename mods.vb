Imports System.Text

Module mods
    Public Function SetWorkingPath(WorkingPath As String) As String
        Try
            Dim folderDlg As New System.Windows.Forms.FolderBrowserDialog

            With folderDlg
                .ShowNewFolderButton = True
                .Description = "Selected your working folder. This is where your PDF files will be saved."
                .RootFolder = Environment.SpecialFolder.MyComputer
                .SelectedPath = IIf(Len(Trim(WorkingPath)) = 0, Environment.SpecialFolder.MyComputer, WorkingPath)
                If (.ShowDialog() = DialogResult.OK) Then
                    SetWorkingPath = .SelectedPath
                Else
                    SetWorkingPath = ""
                End If
            End With
        Catch e As Exception
            MsgBox(e.Message + " (" + e.ToString() + ")", MsgBoxStyle.Critical, "SetWorkingPath Error")
            SetWorkingPath = ""
        End Try

        WorkingPath = SetWorkingPath
    End Function

    Function HexToString(ByVal hex As String) As String
        Dim text As New System.Text.StringBuilder(hex.Length \ 2)
        For i As Integer = 0 To hex.Length - 2 Step 2
            text.Append(Chr(Convert.ToByte(hex.Substring(i, 2), 16)))
        Next
        Return text.ToString
    End Function

    Public Function StringToHex(ByRef s As String) As String
        Dim Data = s
        Dim sVal As String
        Dim sHex As String = ""
        While Data.Length > 0
            sVal = Microsoft.VisualBasic.Conversion.Hex(Microsoft.VisualBasic.Strings.Asc(Data.Substring(0, 1).ToString()))
            sHex = sHex & sVal.PadLeft(2, "0"c)
            Data = Data.Substring(1, Data.Length - 1)
        End While
        Return sHex
    End Function

    Function StringToHex2(ByVal text As String) As String
        Dim hex As String
        For i As Integer = 0 To text.Length - 1
            hex &= Asc(text.Substring(i, 1)).ToString("x").ToUpper
        Next
        Return hex
    End Function

    Function replaceProfileName(file As String, match As String, replace As String)
        Dim outputLines As New List(Of String)()
        Dim stringToMatch As String = match
        Dim replacementString As String = replace

        For Each line As String In System.IO.File.ReadAllLines(file)
            Dim matchFound As Boolean
            matchFound = line.Contains(stringToMatch)

            If matchFound Then
                ' Replace line with string
                outputLines.Add(replacementString)
            Else
                outputLines.Add(line)
            End If
        Next
        System.IO.File.WriteAllLines(file, outputLines.ToArray(), Encoding.UTF8)
    End Function
End Module
