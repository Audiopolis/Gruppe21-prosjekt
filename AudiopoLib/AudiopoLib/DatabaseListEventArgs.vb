Public Class DatabaseListEventArgs
    Inherits EventArgs
    Public Data As DataTable
    Public ErrorOccurred As Boolean
    Public Sub New()
    End Sub
    Public Sub New(D As DataTable, ByVal Err As Boolean)
        Data = D
        ErrorOccurred = Err
    End Sub
End Class