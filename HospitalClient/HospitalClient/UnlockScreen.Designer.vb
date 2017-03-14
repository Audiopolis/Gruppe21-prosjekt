<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnlockScreen
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
        Me.GroupLoggInn = New System.Windows.Forms.GroupBox()
        Me.txtNøkkel = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PicLoadingSurface = New System.Windows.Forms.PictureBox()
        Me.GroupLoggInn.SuspendLayout()
        CType(Me.PicLoadingSurface, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupLoggInn
        '
        Me.GroupLoggInn.Controls.Add(Me.txtNøkkel)
        Me.GroupLoggInn.Controls.Add(Me.Label1)
        Me.GroupLoggInn.Location = New System.Drawing.Point(274, 71)
        Me.GroupLoggInn.Name = "GroupLoggInn"
        Me.GroupLoggInn.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupLoggInn.Size = New System.Drawing.Size(241, 96)
        Me.GroupLoggInn.TabIndex = 8
        Me.GroupLoggInn.TabStop = False
        Me.GroupLoggInn.Text = "Logg inn"
        '
        'txtNøkkel
        '
        Me.txtNøkkel.Location = New System.Drawing.Point(56, 31)
        Me.txtNøkkel.Name = "txtNøkkel"
        Me.txtNøkkel.Size = New System.Drawing.Size(176, 20)
        Me.txtNøkkel.TabIndex = 0
        Me.txtNøkkel.UseSystemPasswordChar = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Nøkkel"
        '
        'PicLoadingSurface
        '
        Me.PicLoadingSurface.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PicLoadingSurface.Location = New System.Drawing.Point(387, 114)
        Me.PicLoadingSurface.Name = "PicLoadingSurface"
        Me.PicLoadingSurface.Size = New System.Drawing.Size(32, 32)
        Me.PicLoadingSurface.TabIndex = 9
        Me.PicLoadingSurface.TabStop = False
        '
        'UnlockScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(807, 261)
        Me.Controls.Add(Me.PicLoadingSurface)
        Me.Controls.Add(Me.GroupLoggInn)
        Me.Name = "UnlockScreen"
        Me.Text = "Lås opp"
        Me.GroupLoggInn.ResumeLayout(False)
        Me.GroupLoggInn.PerformLayout()
        CType(Me.PicLoadingSurface, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupLoggInn As GroupBox
    Friend WithEvents txtNøkkel As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PicLoadingSurface As PictureBox
End Class
