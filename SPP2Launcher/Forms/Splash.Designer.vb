<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Splash
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Splash))
        Me.FadeIn = New System.Windows.Forms.Timer(Me.components)
        Me.FadeOut = New System.Windows.Forms.Timer(Me.components)
        Me.Label_Error = New System.Windows.Forms.Label()
        Me.PictureBox_M1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox_Error = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox_M2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox_M3 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox_M1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_Error, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_M2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_M3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FadeIn
        '
        Me.FadeIn.Interval = 10
        '
        'FadeOut
        '
        Me.FadeOut.Interval = 10
        '
        'Label_Error
        '
        Me.Label_Error.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_Error.ForeColor = System.Drawing.Color.Black
        Me.Label_Error.Location = New System.Drawing.Point(98, 349)
        Me.Label_Error.Name = "Label_Error"
        Me.Label_Error.Size = New System.Drawing.Size(740, 34)
        Me.Label_Error.TabIndex = 5
        Me.Label_Error.Text = "Label1"
        '
        'PictureBox_M1
        '
        Me.PictureBox_M1.BackColor = System.Drawing.Color.Silver
        Me.PictureBox_M1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBox_M1.Location = New System.Drawing.Point(83, 19)
        Me.PictureBox_M1.Name = "PictureBox_M1"
        Me.PictureBox_M1.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox_M1.TabIndex = 6
        Me.PictureBox_M1.TabStop = False
        Me.PictureBox_M1.WaitOnLoad = True
        '
        'PictureBox_Error
        '
        Me.PictureBox_Error.Location = New System.Drawing.Point(60, 339)
        Me.PictureBox_Error.Name = "PictureBox_Error"
        Me.PictureBox_Error.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox_Error.TabIndex = 4
        Me.PictureBox_Error.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Location = New System.Drawing.Point(8, 9)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(850, 386)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'PictureBox_M2
        '
        Me.PictureBox_M2.BackColor = System.Drawing.Color.Silver
        Me.PictureBox_M2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBox_M2.Location = New System.Drawing.Point(153, 19)
        Me.PictureBox_M2.Name = "PictureBox_M2"
        Me.PictureBox_M2.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox_M2.TabIndex = 7
        Me.PictureBox_M2.TabStop = False
        Me.PictureBox_M2.WaitOnLoad = True
        '
        'PictureBox_M3
        '
        Me.PictureBox_M3.BackColor = System.Drawing.Color.Silver
        Me.PictureBox_M3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBox_M3.Location = New System.Drawing.Point(223, 19)
        Me.PictureBox_M3.Name = "PictureBox_M3"
        Me.PictureBox_M3.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox_M3.TabIndex = 8
        Me.PictureBox_M3.TabStop = False
        Me.PictureBox_M3.WaitOnLoad = True
        '
        'Splash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(867, 405)
        Me.Controls.Add(Me.PictureBox_M3)
        Me.Controls.Add(Me.PictureBox_M2)
        Me.Controls.Add(Me.PictureBox_M1)
        Me.Controls.Add(Me.Label_Error)
        Me.Controls.Add(Me.PictureBox_Error)
        Me.Controls.Add(Me.PictureBox1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Splash"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SplashForm"
        Me.TransparencyKey = System.Drawing.Color.Silver
        CType(Me.PictureBox_M1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_Error, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_M2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_M3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents PictureBox1 As PictureBox
    Friend WithEvents FadeIn As Timer
    Friend WithEvents FadeOut As Timer
    Friend WithEvents PictureBox_Error As PictureBox
    Friend WithEvents Label_Error As Label
    Friend WithEvents PictureBox_M1 As PictureBox
    Friend WithEvents PictureBox_M2 As PictureBox
    Friend WithEvents PictureBox_M3 As PictureBox
End Class
