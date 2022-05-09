
Public Class ChangePathRepack

    Sub New()
        InitializeComponent()
        Me.WindowState = FormWindowState.Normal
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ChangePathRepack_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.DirSPP2
    End Sub

    ''' <summary>
    ''' ВЫБОР ПУТИ К РЕПАКУ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_ChangePathRepack_Click(sender As Object, e As EventArgs) Handles Button_ChangePathRepack.Click
        Using fbd As New FolderBrowserDialog
            fbd.RootFolder = Environment.SpecialFolder.MyComputer
            If fbd.ShowDialog = DialogResult.OK Then
                TextBox1.Text = fbd.SelectedPath
                My.Settings.DirSPP2 = fbd.SelectedPath
                My.Settings.Save()
                Application.Restart()
            End If
        End Using
        Me.Close()
    End Sub

End Class