
Public Class QuickServerSet

    Private _IsLoading As Boolean = True

    ''' <summary>
    ''' Список миров в БД
    ''' </summary>
    Private Realms As DataRow()

    ''' <summary>
    ''' IP адрес в БД
    ''' </summary>
    Private CurrentIP As String = ""

    Sub New()
        InitializeComponent()

        ' Расположение окна при открытии
        StartPosition = FormStartPosition.Manual
        Location = New Point(My.Settings.AppLocation.X + 40, My.Settings.AppLocation.Y + 40)

    End Sub

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub QuickServerSet_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Получаем идентификатор мира, с которым работает данный сервер
        Dim CurrentId = GV.SPP2Launcher.IniWorld.ReadInt32("MangosdConf", "RealmID")

        ' Устанавливаем тип сервера
        Dim GameType = GV.SPP2Launcher.IniWorld.ReadInt32("MangosdConf", "GameType")
        Select Case GameType
            Case 0
                ComboBox_GameType.SelectedIndex = 0
            Case 1
                ComboBox_GameType.SelectedIndex = 1
            Case 4
                ComboBox_GameType.SelectedIndex = 0
            Case 6
                ComboBox_GameType.SelectedIndex = 2
            Case 8
                ComboBox_GameType.SelectedIndex = 3
            Case 16
                ComboBox_GameType.SelectedIndex = 4
        End Select

        ' Получаем список МИРОВ
        Dim _err = New Tuple(Of Boolean, String)(False, "OK")
        Realms = MySqlDataBases.REALMD.REALMLIST.SELECT_REALMS(_err)
        If _err.Item1 Then
            MessageBox.Show(_err.Item2,
                            My.Resources.P016_WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Close()
        End If

        ' Заполняем ID и имя мира
        SetCurrentServerName(CurrentId)

        ' Заполняем список локальных IpV4 адресов и устанавливаем текущий
        ComboBox_BindIP.Items.Clear()
        ComboBox_BindIP.Items.AddRange(GetLocalIpAddresses(False).ToArray)
        ComboBox_BindIP.SelectedItem = CurrentIP

        _IsLoading = False

    End Sub

    ''' <summary>
    ''' При изменении идентификатора выбранного мира
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBox_RealmId_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_RealmId.SelectedIndexChanged
        If _IsLoading Then Exit Sub
        SetCurrentServerName(CInt(ComboBox_RealmId.SelectedItem))
    End Sub

    ''' <summary>
    ''' Заполняет список идентификаторов миров и устанавливает текущий с выводом его имени.
    ''' </summary>
    ''' <param name="currentId"></param>
    Private Sub SetCurrentServerName(currentId As Integer)
        For Each realm In Realms
            Dim realmId As Integer = CInt(realm("id"))
            ComboBox_RealmId.Items.Add(realmId)
            If realmId = currentId Then
                ' Устанавливаем имя мира
                TextBox_ServerName.Text = realm("name").ToString
                ' Сохраняем IP адрес мира
                CurrentIP = realm("address").ToString
            End If
        Next
        ComboBox_RealmId.SelectedItem = currentId
    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ - ИЗМЕНИТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Change_Click(sender As Object, e As EventArgs) Handles Button_Change.Click

        If ComboBox_BindIP.SelectedItem.ToString.Length = 0 Then
            MessageBox.Show(My.Resources.P061_NeedIpAddress,
                            My.Resources.P016_WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If TextBox_ServerName.Text.Length = 0 Then
            MessageBox.Show(My.Resources.P062_NeedServerName,
                            My.Resources.P016_WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Пытаемся изменить настройки в БД
        Dim _err = MySqlDataBases.REALMD.REALMLIST.UPDATE_REALM(id:=ComboBox_RealmId.SelectedItem.ToString,
                                                                name:=TextBox_ServerName.Text,
                                                                address:=ComboBox_BindIP.SelectedItem.ToString)
        If _err.Item1 = True Then
            MessageBox.Show(_err.Item2,
                            My.Resources.P016_WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        Else

            ' Сохраняем тип сервера
            Dim GameType As Integer
            Select Case ComboBox_GameType.SelectedIndex
                Case 0
                    GameType = 0
                Case 1
                    GameType = 1
                Case 2
                    GameType = 6
                Case 3
                    GameType = 8
                Case 4
                    GameType = 16
            End Select
            GV.SPP2Launcher.IniWorld.Write("MangosdConf", "GameType", GameType)

            ' Сохраняем идентификатор сервера
            GV.SPP2Launcher.IniWorld.Write("MangosdConf", "RealmID", ComboBox_RealmId.SelectedItem.ToString)

            MessageBox.Show(My.Resources.P063_QuickSettingsChanged,
                            My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Close()

        End If

    End Sub

End Class