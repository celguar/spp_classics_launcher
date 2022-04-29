<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServerSelector
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ServerSelector))
        Me.PictureBox_Classic = New System.Windows.Forms.PictureBox()
        Me.PictureBox_TBC = New System.Windows.Forms.PictureBox()
        Me.PictureBox_WotLK = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Timer_FormClosing = New System.Windows.Forms.Timer(Me.components)
        Me.Label_ClassicStatus = New System.Windows.Forms.Label()
        Me.Label_TbcStatus = New System.Windows.Forms.Label()
        Me.Label_WotlkStatus = New System.Windows.Forms.Label()
        CType(Me.PictureBox_Classic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_TBC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_WotLK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox_Classic
        '
        resources.ApplyResources(Me.PictureBox_Classic, "PictureBox_Classic")
        Me.PictureBox_Classic.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.vanilla
        Me.PictureBox_Classic.Name = "PictureBox_Classic"
        Me.PictureBox_Classic.TabStop = False
        '
        'PictureBox_TBC
        '
        resources.ApplyResources(Me.PictureBox_TBC, "PictureBox_TBC")
        Me.PictureBox_TBC.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.tbc
        Me.PictureBox_TBC.Name = "PictureBox_TBC"
        Me.PictureBox_TBC.TabStop = False
        '
        'PictureBox_WotLK
        '
        resources.ApplyResources(Me.PictureBox_WotLK, "PictureBox_WotLK")
        Me.PictureBox_WotLK.Image = Global.DevCake.WoW.SPP2Launcher.My.Resources.Resources.wotlk
        Me.PictureBox_WotLK.Name = "PictureBox_WotLK"
        Me.PictureBox_WotLK.TabStop = False
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Timer_FormClosing
        '
        Me.Timer_FormClosing.Interval = 500
        '
        'Label_ClassicStatus
        '
        resources.ApplyResources(Me.Label_ClassicStatus, "Label_ClassicStatus")
        Me.Label_ClassicStatus.Name = "Label_ClassicStatus"
        '
        'Label_TbcStatus
        '
        resources.ApplyResources(Me.Label_TbcStatus, "Label_TbcStatus")
        Me.Label_TbcStatus.Name = "Label_TbcStatus"
        '
        'Label_WotlkStatus
        '
        resources.ApplyResources(Me.Label_WotlkStatus, "Label_WotlkStatus")
        Me.Label_WotlkStatus.Name = "Label_WotlkStatus"
        '
        'ServerSelector
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label_WotlkStatus)
        Me.Controls.Add(Me.Label_TbcStatus)
        Me.Controls.Add(Me.Label_ClassicStatus)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox_WotLK)
        Me.Controls.Add(Me.PictureBox_TBC)
        Me.Controls.Add(Me.PictureBox_Classic)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ServerSelector"
        CType(Me.PictureBox_Classic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_TBC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_WotLK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox_Classic As PictureBox
    Friend WithEvents PictureBox_TBC As PictureBox
    Friend WithEvents PictureBox_WotLK As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Timer_FormClosing As Timer
    Friend WithEvents Label_ClassicStatus As Label
    Friend WithEvents Label_TbcStatus As Label
    Friend WithEvents Label_WotlkStatus As Label
End Class
