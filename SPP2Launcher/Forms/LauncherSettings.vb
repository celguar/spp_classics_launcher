
Public Class LauncherSettings

    Private _isLoading As Boolean = True

    Sub New()
        InitializeComponent()
        ComboBox_Theme.Items.Clear()
        ComboBox_FontSize.Items.Clear()
        ComboBox_FontStyle.Items.Clear()
        ComboBox_Theme.Items.AddRange({"Black Theme", "Light Theme"})
        ComboBox_Theme.SelectedItem = My.Settings.ConsoleTheme
        For Each fnt In F.Families
            ComboBox_FontSize.Items.Add(fnt.Name & " 8pt")
            ComboBox_FontSize.Items.Add(fnt.Name & " 9pt")
            ComboBox_FontSize.Items.Add(fnt.Name & " 10pt")
            ComboBox_FontSize.Items.Add(fnt.Name & " 11pt")
        Next
        Try
            ComboBox_FontSize.SelectedIndex = CInt(My.Settings.ConsoleFontSize) - 8
        Catch
        End Try
        ComboBox_FontStyle.Items.AddRange({"Regular", "Bold", "Italic"})
        ComboBox_FontStyle.SelectedIndex = CInt(My.Settings.ConsoleFontStyle)
        StartPosition = FormStartPosition.Manual
        Location = New Point(My.Settings.AppLocation.X + 40, My.Settings.AppLocation.Y + 20)
    End Sub

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LauncherSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox_DirSPP2.Text = My.Settings.DirSPP2
        _isLoading = False
    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ ВЫБРАТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_DirSPP2_Click(sender As Object, e As EventArgs) Handles Button_DirSPP2.Click
        Using fbd As New FolderBrowserDialog
            fbd.RootFolder = Environment.SpecialFolder.MyComputer
            If fbd.ShowDialog = DialogResult.OK Then
                TextBox_DirSPP2.Text = fbd.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ ОК
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_OK_Click(sender As Object, e As EventArgs) Handles Button_OK.Click
        My.Settings.DirSPP2 = TextBox_DirSPP2.Text
        My.Settings.ConsoleFontStyle = CType(ComboBox_FontStyle.SelectedIndex, FontStyle)
        My.Settings.ConsoleFontSize = CSng(ComboBox_FontSize.SelectedIndex + 8)
        My.Settings.ConsoleTheme = If(IsNothing(ComboBox_Theme.SelectedItem.ToString), "Black Theme", ComboBox_Theme.SelectedItem.ToString)
        My.Settings.Save()
        If Application.OpenForms("Launcher") Is Nothing Then
            Application.Restart()
        Else
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ СТИЛЯ ШРИФТА КОНСОЛИ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBox_FontStyle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_FontStyle.SelectedIndexChanged
        If Not _isLoading Then
            My.Settings.ConsoleFontStyle = CType(ComboBox_FontStyle.SelectedIndex, FontStyle)
            My.Settings.Save()
            GV.SPP2Launcher.ChangeFont()
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ ШРИФТА КОНСОЛИ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBox_Font_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_FontSize.SelectedIndexChanged
        If Not _isLoading Then
            My.Settings.ConsoleFontSize = CSng(ComboBox_FontSize.SelectedIndex + 8)
            My.Settings.Save()
            GV.SPP2Launcher.ChangeFont()
        End If
    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ ТЕМЫ КОНСОЛИ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBox_Theme_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_Theme.SelectedIndexChanged
        If Not _isLoading Then
            My.Settings.ConsoleTheme = ComboBox_Theme.SelectedItem.ToString
            My.Settings.Save()
            GV.SPP2Launcher.SetConsoleTheme()
        End If
    End Sub

End Class