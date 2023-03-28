Imports System.Runtime.InteropServices

Public Class Store
    Public Shared GameType As Integer = 0
    Public Shared lb As String
    Public Shared profileFile As String
    Public Shared profileNewName As String
    Public Shared profileNewHex As String
    Public Shared toCloneStr As String
    Public Shared toCloneHex As String
    Public Shared HasBeenDecrypted As Boolean
    Public Shared ATS As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\American Truck Simulator\profiles\"
    Public Shared ETS As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\Euro Truck Simulator 2\profiles\"
End Class
