
Imports System.IO
Imports System.Reflection.Metadata
Imports System.Runtime
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Permissions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar
Imports System.Diagnostics.Process
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net.Security
Imports System.DirectoryServices.ActiveDirectory

'Save profile to store class - its in store.lb
'Need to edit  profile file with profile name
'Need to create a folder, rename it with hex used in profile name
'Create new folder, name dirty,copy contents of profile to dirty folder
'Copy files from another profile to new profile
'SII_Decrypt needs to be in the same directory to decrypt for whatever reason

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("ETS2")
        ComboBox1.Items.Add("ATS")
    End Sub

    Private Shared processOutput As StringBuilder = Nothing

    Private Shared Sub OutputHandler(sendingProcess As Object, outLine As DataReceivedEventArgs)
        ' Collect the sort command output.
        If Not String.IsNullOrEmpty(outLine.Data) Then
            ' Add the text to the collected output.
            processOutput.AppendLine(outLine.Data)
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            Store.GameType = 0
            ListBox1.Items.Clear()
            For Each dr As String In Directory.GetDirectories(Store.ETS)
                Dim dirName As New DirectoryInfo(dr)
                Dim a = HexToString(dirName.Name)
                ListBox1.Items.Add(dirName.Name + "-" + a)
            Next
        Else
            Store.GameType = 1
            ListBox1.Items.Clear()
            For Each dr As String In Directory.GetDirectories(Store.ATS)
                Dim dirName As New DirectoryInfo(dr)
                Dim a = HexToString(dirName.Name)
                ListBox1.Items.Add(dirName.Name + "-" + a)
            Next
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Store.lb = ListBox1.SelectedItem

    End Sub

    Private Function MakeFolder(path As String)
        If Not System.IO.Directory.Exists(path) Then
            System.IO.Directory.CreateDirectory(path)
        End If
        Return 0
    End Function

    Private Function copyFiles(src As String, dst As String)
        Dim SourcePath As String = src
        Dim DestinationPath As String = dst
        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(SourcePath, DestinationPath)
    End Function

    Private Sub EditProfile()
        Dim json As JObject = JObject.Parse(Me.TextBox1.Text)
        MsgBox(json.SelectToken("SiiNunit.profile_name"))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim newProfilePath As String
        Dim copyFolderHex() As String = Split(Store.lb, "-")
        If ComboBox1.SelectedIndex = 0 Then
            MakeFolder(Store.ETS & mods.StringToHex2(Store.profileNewName))
            copyFiles(Store.ETS & copyFolderHex(0), Store.ETS & Store.profileNewName)
            newProfilePath = Store.ETS & Store.profileNewName
        Else
            MakeFolder(Store.ATS & mods.StringToHex2(Store.profileNewName))
            copyFiles(Store.ATS & copyFolderHex(0), Store.ATS & mods.StringToHex2(Store.profileNewName))
            newProfilePath = Store.ATS & mods.StringToHex2(Store.profileNewName)
        End If

        Dim fileToDecrypt As String = newProfilePath

        processOutput = New System.Text.StringBuilder()
        Dim NewProcess As New System.Diagnostics.Process()
        Dim arg1 As String = "-i "
        Dim arg2 = Chr(34) & fileToDecrypt & "\profile.sii"
        Dim Arguments = arg1 & arg2
        With NewProcess.StartInfo
            .FileName = "SII_Decrypt.exe"
            .RedirectStandardOutput = True
            .RedirectStandardError = True
            .RedirectStandardInput = True
            .UseShellExecute = False
            .WindowStyle = ProcessWindowStyle.Normal
            .CreateNoWindow = False
            .Arguments = arg1 & arg2
        End With

        ' Set our event handler to asynchronously read the sort output.
        AddHandler NewProcess.OutputDataReceived, AddressOf OutputHandler
        NewProcess.Start()
        NewProcess.BeginOutputReadLine()
        NewProcess.WaitForExit()
        MsgBox(processOutput.ToString())

        mods.replaceProfileName("" & fileToDecrypt & "\profile.sii", "profile_name:", "profile_name: " & Store.profileNewName)

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Store.profileNewName = TextBox1.Text
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) 
        Dim json As JObject = JObject.Parse(Store.ATS & mods.StringToHex2(Store.profileNewName) & "\profile.sii")
        MsgBox(json.SelectToken("SiiNunit.profile_name"))
    End Sub
End Class
