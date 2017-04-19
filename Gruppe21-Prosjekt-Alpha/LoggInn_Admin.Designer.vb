<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoggInn_Admin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                NotifManager.Dispose()
                LayoutTool.Dispose()
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPassord = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.PicLoadingSurface, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupLoggInn.SuspendLayout()
        Me.SuspendLayout()
        '
        'PicLoadingSurface
        '
        Me.PicLoadingSurface.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PicLoadingSurface.Location = New System.Drawing.Point(200, 328)
        Me.PicLoadingSurface.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PicLoadingSurface.Name = "PicLoadingSurface"
        Me.PicLoadingSurface.Size = New System.Drawing.Size(48, 49)
        Me.PicLoadingSurface.TabIndex = 9
        Me.PicLoadingSurface.TabStop = False
        '
        'GroupLoggInn
        '
        Me.GroupLoggInn.Controls.Add(Me.txtBrukernavn)
        Me.GroupLoggInn.Controls.Add(Me.Label1)
        Me.GroupLoggInn.Controls.Add(Me.txtPassord)
        Me.GroupLoggInn.Controls.Add(Me.Label2)
        Me.GroupLoggInn.Location = New System.Drawing.Point(20, 109)
        Me.GroupLoggInn.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupLoggInn.Name = "GroupLoggInn"
        Me.GroupLoggInn.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupLoggInn.Size = New System.Drawing.Size(345, 232)
        Me.GroupLoggInn.TabIndex = 10
        Me.GroupLoggInn.TabStop = False
        Me.GroupLoggInn.Text = "Logg inn"
        '
        'txtBrukernavn
        '
        Me.txtBrukernavn.Location = New System.Drawing.Point(111, 51)
        Me.txtBrukernavn.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBrukernavn.Name = "txtBrukernavn"
        Me.txtBrukernavn.Size = New System.Drawing.Size(216, 26)
        Me.txtBrukernavn.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 55)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 20)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Brukernavn"
        '
        'txtPassord
        '
        Me.txtPassord.Location = New System.Drawing.Point(111, 91)
        Me.txtPassord.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtPassord.Name = "txtPassord"
        Me.txtPassord.Size = New System.Drawing.Size(216, 26)
        Me.txtPassord.TabIndex = 2
        Me.txtPassord.UseSystemPasswordChar = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(34, 95)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Passord"
        '
        'LoggInn_Admin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1176, 709)
        Me.Controls.Add(Me.GroupLoggInn)
        Me.Controls.Add(Me.PicLoadingSurface)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "LoggInn_Admin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Logg inn som blodgiver"
        CType(Me.PicLoadingSurface, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupLoggInn.ResumeLayout(False)
        Me.GroupLoggInn.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PicLoadingSurface As PictureBox
    Friend WithEvents GroupLoggInn As GroupBox
    Friend WithEvents txtPassord As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtBrukernavn As TextBox
    Friend WithEvents Label1 As Label
End Class
