<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoggInn_Admin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PicLoadingSurface = New System.Windows.Forms.PictureBox()
        Me.GroupLoggInn = New System.Windows.Forms.GroupBox()
        Me.txtBrukernavn = New System.Windows.Forms.TextBox()
        Me.txtPassord = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.PicLoadingSurface, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupLoggInn.SuspendLayout()
        Me.SuspendLayout()
        '
        'PicLoadingSurface
        '
        Me.PicLoadingSurface.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PicLoadingSurface.Location = New System.Drawing.Point(133, 207)
        Me.PicLoadingSurface.Name = "PicLoadingSurface"
        Me.PicLoadingSurface.Size = New System.Drawing.Size(32, 32)
        Me.PicLoadingSurface.TabIndex = 9
        Me.PicLoadingSurface.TabStop = False
        '
        'GroupLoggInn
        '
        Me.GroupLoggInn.Controls.Add(Me.txtBrukernavn)
        Me.GroupLoggInn.Controls.Add(Me.txtPassord)
        Me.GroupLoggInn.Controls.Add(Me.Label1)
        Me.GroupLoggInn.Controls.Add(Me.Label2)
        Me.GroupLoggInn.Location = New System.Drawing.Point(13, 71)
        Me.GroupLoggInn.Name = "GroupLoggInn"
        Me.GroupLoggInn.Size = New System.Drawing.Size(258, 119)
        Me.GroupLoggInn.TabIndex = 10
        Me.GroupLoggInn.TabStop = False
        Me.GroupLoggInn.Text = "Logg inn"
        '
        'txtBrukernavn
        '
        Me.txtBrukernavn.Location = New System.Drawing.Point(73, 32)
        Me.txtBrukernavn.Name = "txtBrukernavn"
        Me.txtBrukernavn.Size = New System.Drawing.Size(173, 20)
        Me.txtBrukernavn.TabIndex = 0
        '
        'txtPassord
        '
        Me.txtPassord.Location = New System.Drawing.Point(73, 58)
        Me.txtPassord.Name = "txtPassord"
        Me.txtPassord.Size = New System.Drawing.Size(173, 20)
        Me.txtPassord.TabIndex = 1
        Me.txtPassord.UseSystemPasswordChar = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Brukernavn"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Passord"
        '
        'LoggInn_Admin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.GroupLoggInn)
        Me.Controls.Add(Me.PicLoadingSurface)
        Me.Name = "LoggInn_Admin"
        Me.Text = "Form1"
        CType(Me.PicLoadingSurface, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupLoggInn.ResumeLayout(False)
        Me.GroupLoggInn.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PicLoadingSurface As PictureBox
    Friend WithEvents GroupLoggInn As GroupBox
    Friend WithEvents txtBrukernavn As TextBox
    Friend WithEvents txtPassord As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class
