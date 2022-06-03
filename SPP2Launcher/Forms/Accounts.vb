
Imports System.Security.Cryptography
Imports SecureRemotePassword

Public Class Accounts

    ''' <summary>
    ''' ПРИ СОЗДАНИИ ОБЪЕКТА
    ''' </summary>
    Sub New()
        InitializeComponent()

        ' Расположение окна при открытии
        StartPosition = FormStartPosition.Manual
        Location = New Point(My.Settings.AppLocation.X + 40, My.Settings.AppLocation.Y + 40)

        RadioButton_Create.Checked = True
        Button_Back_Click(Me, Nothing)

    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ ДАЛЕЕ НА ПЕРВОМ ШАГЕ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Next_Click(sender As Object, e As EventArgs) Handles Button_Next1.Click
        If RadioButton_Create.Checked Then
            ' Это создание нового аккаунта
            Button_Next2_Click(sender, e)
        Else
            ' Это изменение существующего аккаунта
            SplitContainer1.Panel1Collapsed = True
            SplitContainer1.Panel2Collapsed = False
            SplitContainer2.Panel1Collapsed = False
            SplitContainer2.Panel2Collapsed = True
            ' Настройка контролов
            Me.Height = 194
            Me.Width = 214
            GroupBox2.Location = New Point(12, 8)
            GroupBox2.Height = 100
            GroupBox2.Width = 175
            ComboBox_AccountSearch.Width = 144
            Button_Back.Location = New Point(12, 119)
            Button_Next2.Location = New Point(112, 119)

            ' Получаем префикс бота
            Dim botPrefix = GV.SPP2Launcher.IniPlayerBots.ReadString("AiPlayerbotConf", "AiPlayerbot.RandomBotAccountPrefix")
            Dim accounts() = MySqlDataBases.REALMD.ACCOUNT.SELECT_REAL_ACCOUNT(botPrefix.ToUpper)
            If Not IsNothing(accounts) Then
                ComboBox_AccountSearch.Items.AddRange(accounts)
            End If
        End If
    End Sub

    ''' <summary>
    '''  НАЖАТИЕ КНОПКИ НАЗАД НА ВТОРОМ ШАГЕ / СТАРТОВАЯ СТРАНИЦА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Back_Click(sender As Object, e As EventArgs) Handles Button_Back.Click

        ' Настройка панелей
        SplitContainer1.Panel1Collapsed = False
        SplitContainer1.Panel2Collapsed = True
        SplitContainer2.Panel1Collapsed = False
        SplitContainer2.Panel2Collapsed = True
        ' Настройка контролов
        Me.Height = 194
        Me.Width = 214
        GroupBox1.Location = New Point(12, 8)
        GroupBox1.Height = 100
        GroupBox1.Width = 175
        Button_Next1.Location = New Point(112, 119)

    End Sub

    ''' <summary>
    ''' НАЖАТИЕ КНОПКИ ДАЛЕЕ НА ВТОРОМ ШАГЕ / ВЫПОЛНЕНИЕ ИЗМЕНЕНИЙ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Next2_Click(sender As Object, e As EventArgs) Handles Button_Next2.Click

        ' Это создание нового аккаунта
        SplitContainer1.Panel1Collapsed = True
        SplitContainer1.Panel2Collapsed = True
        SplitContainer2.Panel1Collapsed = True
        SplitContainer2.Panel2Collapsed = False
        ' Настройка контролов
        Me.Height = 228
        Me.Width = 278
        ' Блокируем доступ до смены расширения типа игры
        ComboBox_Expansion.Enabled = False

        If RadioButton_Create.Checked Then

            ' Изменяем текст на кнопке
            Button_Create.Text = My.Resources.P017_Create
            ' Устанавливаем GM = Player
            ComboBox_AccountType.SelectedIndex = 0

            ' И устанавливаем тип текущей игры
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    ComboBox_Expansion.SelectedIndex = 2
                Case GV.EModule.Tbc.ToString
                    ComboBox_Expansion.SelectedIndex = 1
                Case GV.EModule.Wotlk.ToString
                    ComboBox_Expansion.SelectedIndex = 0
            End Select

        Else

            ' Изменяем текст на кнопке
            Button_Create.Text = My.Resources.P018_Change
            ' Блокируем доступ до имени пользователя
            TextBox_UserName.Enabled = False
            TextBox_UserName.Text = ComboBox_AccountSearch.SelectedItem.ToString

            If CheckBox_ChangePassword.Checked = True Then
                ' Разрешаем доступ до пароля пользователя
                TextBox_Password.Enabled = True
            Else
                ' Блокируем доступ до пароля пользователя
                TextBox_Password.Enabled = False
            End If

            Dim dr = GetAccountInfo(TextBox_UserName.Text)
            If Not IsNothing(dr) Then
                ComboBox_Expansion.SelectedIndex = CInt(dr("expansion"))
                ComboBox_AccountType.SelectedIndex = CInt(dr("gmlevel"))
            End If

        End If

    End Sub

    ''' <summary>
    ''' НАЖАТИЕ НА КНОПКУ СОЗДАТЬ/ИЗМЕНИТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Create_Click(sender As Object, e As EventArgs) Handles Button_Create.Click

        If RadioButton_Create.Checked Then

            ' Это создание нового аккаунта
            If TextBox_UserName.Text.Trim.Length < 3 Then

                ' Количество символов в имени пользователя должно быть не менее 3
                MessageBox.Show(My.Resources.P034_SmallLengthUserName,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)

            Else

                If TextBox_Password.TextLength < 6 Then

                    ' Количество символов в пароле должно быть не менее 6
                    MessageBox.Show(My.Resources.P036_SmallLengthUserPass,
                                    My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)

                    TextBox_Password.Text = ""

                Else

                    ' Проверяем наличие пользователей с таким именем
                    Dim _err = New Tuple(Of Boolean, String)(False, "OK")
                    Dim dr = MySqlDataBases.REALMD.ACCOUNT.SELECT_ACCOUNT(TextBox_UserName.Text.Trim.ToUpper, _err)

                    If _err.Item1 = False And Not IsNothing(dr) Then

                        ' Пользователь с таким именем уже существует
                        MessageBox.Show(String.Format(My.Resources.P056_UserFound, TextBox_UserName.Text.Trim),
                                        My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)

                    Else

                        If GV.SPP2Launcher.WorldON Then

                            Dim sa As New SPR6(TextBox_UserName.Text, TextBox_Password.Text)
                            Dim verifier = sa.Verifier
                            Dim salt = sa.Salt

                            ' Создаём новый аккаунт
                            Dim res = MySqlDataBases.REALMD.ACCOUNT.INSERT_ACCOUNT(TextBox_UserName.Text.Trim.ToUpper,
                                                                                   ComboBox_AccountType.SelectedIndex,
                                                                                   verifier,
                                                                                   salt)
                            If res.Item1 = False Then
                                MessageBox.Show("Успешно создан!",
                                                My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show(res.Item2,
                                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If

                            ' Создаём новый аккаунт
                            'GV.SPP2Launcher.SendCommandToWorld(String.Format(".account create {0} {1}", TextBox_UserName.Text.Trim, TextBox_Password.Text.Trim))

                            ' Ожидаем выполнения команды
                            'Threading.Thread.Sleep(1000)

                            ' Меняем уровень GM
                            'GV.SPP2Launcher.SendCommandToWorld(String.Format(".account set gmlevel {0} {1}", TextBox_UserName.Text.Trim, ComboBox_AccountType.SelectedIndex))
                        Else
                            MessageBox.Show(String.Format(My.Resources.P037_WorldNotStarted),
                                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If

                    End If
                End If
            End If

        Else

            ' Если включено изменение пароля
            If CheckBox_ChangePassword.Checked Then

                If TextBox_Password.TextLength < 6 Then

                    ' Количество символов в пароле должно быть не менее 6
                    MessageBox.Show(My.Resources.P036_SmallLengthUserPass,
                                    My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)

                    TextBox_Password.Text = ""
                    Exit Sub

                End If

                ' Получаем параметры аккаунта
                Dim dr = GetAccountInfo(TextBox_UserName.Text.Trim.ToUpper)
                If Not IsNothing(dr) Then

                    If DeleteAccount(TextBox_UserName.Text.Trim.ToUpper) Then

                        ' Создаём новый аккаунт
                        GV.SPP2Launcher.SendCommandToWorld(String.Format(".account create {0} {1}", TextBox_UserName.Text.Trim, TextBox_Password.Text.Trim))

                        ' Ожидаем выполнения команды
                        CommandSuccessfull = False
                        WaitWorldMessage = "Account created: " & TextBox_UserName.Text.Trim

                        ' Обновляем параметры аккаунта
                        If UpdateAccount(dr) Then Close()
                        'MessageBox.Show(My.Resources.P058_AccountChanged,
                        'My.Resources.P023_InfoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'End If

                    End If
                End If

            Else

                If GV.SPP2Launcher.WorldON Then

                    ' Изменяем уровень GM доступа.
                    GV.SPP2Launcher.SendCommandToWorld(String.Format(".account set gmlevel {0} {1}", TextBox_UserName.Text.Trim, TextBox_Password.Text.Trim))

                End If

            End If

        End If

    End Sub

    ''' <summary>
    ''' Возвращает строку DataRow с параметрами аккаунта.
    ''' </summary>
    ''' <param name="accountName">Имя пользователя.</param>
    ''' <returns></returns>
    Private Function GetAccountInfo(accountName As String) As DataRow
        Dim dr As DataRow = Nothing
        Do
            Dim _err = New Tuple(Of Boolean, String)(False, "OK")
            dr = MySqlDataBases.REALMD.ACCOUNT.SELECT_ACCOUNT(TextBox_UserName.Text.Trim.ToUpper, _err)
            If _err.Item1 = True Then
                ' Ошибка получения параметров аккаунта
                Dim res = MessageBox.Show(_err.Item2 & vbCrLf & My.Resources.P057_Repeat,
                                          My.Resources.E003_ErrorCaption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                If res = DialogResult.Cancel Then
                    dr = Nothing
                    Exit Do
                End If
            Else
                Exit Do
            End If
        Loop
        Return dr
    End Function

    ''' <summary>
    ''' Удалаяет указанный аккаунт. При успешном удалении возвращает True. 
    ''' </summary>
    ''' <param name="accountName">Имя пользователя.</param>
    ''' <returns></returns>
    Private Function DeleteAccount(accountName As String) As Boolean
        Dim ok As Boolean
        Do
            Dim _err = New Tuple(Of Boolean, String)(False, "OK")
            If MySqlDataBases.REALMD.ACCOUNT.DELETE_ACCOUNT(accountName).Item1 = False Then
                ok = True
                Exit Do
            Else
                ' Ошибка удаления аккаунта
                Dim res = MessageBox.Show(_err.Item2 & vbCrLf & My.Resources.P057_Repeat,
                                          My.Resources.E003_ErrorCaption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                If res = DialogResult.Cancel Then
                    ok = False
                    Exit Do
                End If
            End If
        Loop
        Return ok
    End Function

    ''' <summary>
    ''' Обновляет параметры указанного аккаунта из DataRow. При успешном обновлении возвращает True.
    ''' </summary>
    ''' <param name="dr"></param>
    ''' <returns></returns>
    Private Function UpdateAccount(dr As DataRow) As Boolean
        Dim ok As Boolean
        Do
            Dim _err = New Tuple(Of Boolean, String)(False, "OK")
            If MySqlDataBases.REALMD.ACCOUNT.UPDATE_ACCOUNT(dr, ComboBox_AccountType.SelectedIndex).Item1 = False Then
                ok = True
                Exit Do
            Else
                ' Ошибка удаления аккаунта
                Dim res = MessageBox.Show(_err.Item2 & vbCrLf & My.Resources.P057_Repeat,
                                          My.Resources.E003_ErrorCaption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                If res = DialogResult.Cancel Then
                    ok = False
                    Exit Do
                End If
            End If
        Loop
        Return ok
    End Function

End Class