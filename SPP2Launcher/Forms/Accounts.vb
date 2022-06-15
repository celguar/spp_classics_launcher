
Imports System.Security.Cryptography

Public Class Accounts

    ''' <summary>
    ''' Параметры текущего редактируемого аккаунта
    ''' </summary>
    Private DR As DataRow = Nothing

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
            ChangeAccount()
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

            ComboBox_AccountSearch.Items.Clear()

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
    ''' НАЖАТИЕ КНОПКИ ДАЛЕЕ ПРИ ИЗМЕНЕНИИ АККАУНТА
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Next2_Click(sender As Object, e As EventArgs) Handles Button_Next2.Click
        If Not IsNothing(ComboBox_AccountSearch.SelectedItem) AndAlso ComboBox_AccountSearch.SelectedItem.ToString.Length > 0 Then
            ChangeAccount()
        Else
            ' Введите имя пользователя!
            MessageBox.Show(My.Resources.P064_NeedUserName,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' ИЗМЕНЕНИЕ ПАРАМЕТРОВ АККАУНТА
    ''' </summary>
    Private Sub ChangeAccount()

        If GV.SPP2Launcher.MysqlON Then

            ' Настраиваем форму
            SplitContainer1.Panel1Collapsed = True
            SplitContainer1.Panel2Collapsed = True
            SplitContainer2.Panel1Collapsed = True
            SplitContainer2.Panel2Collapsed = False
            ' Настройка контролов
            Me.Height = 228
            Me.Width = 278

            If RadioButton_Create.Checked Then

                ' Блокируем доступ до смены расширения типа игры
                ComboBox_Expansion.Enabled = False

                ' Изменяем текст на кнопке
                Button_Create.Text = My.Resources.P017_Create

                ' Устанавливаем GM = Player
                ComboBox_AccountType.SelectedIndex = 0

                ' И устанавливаем тип текущей игры
                Select Case My.Settings.LastLoadedServerType
                    Case GV.EModule.Classic.ToString
                        ComboBox_Expansion.SelectedIndex = 0
                    Case GV.EModule.Tbc.ToString
                        ComboBox_Expansion.SelectedIndex = 1
                    Case GV.EModule.Wotlk.ToString
                        ComboBox_Expansion.SelectedIndex = 2
                End Select

            Else

                ' Снимаем блок до смены расширения типа игры
                ComboBox_Expansion.Enabled = True

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

                DR = GetAccountInfo(TextBox_UserName.Text)
                If Not IsNothing(dr) Then
                    ComboBox_Expansion.SelectedIndex = CInt(DR("expansion"))
                    ComboBox_AccountType.SelectedIndex = CInt(DR("gmlevel"))
                End If

            End If

        Else
            ' Необходим запущенный MySQL
            MessageBox.Show(My.Resources.P054_NeedMySQL,
                            My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

    ''' <summary>
    ''' НАЖАТИЕ НА КНОПКУ СОЗДАТЬ/ИЗМЕНИТЬ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Create_Click(sender As Object, e As EventArgs) Handles Button_Create.Click

        If RadioButton_Create.Checked Then

            ' Это создание аккаунта
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

                    ' Генерим соль и верификатор
                    Dim srp = New SRP6(TextBox_UserName.Text.Trim.ToUpper, TextBox_Password.Text.Trim.ToUpper)

                    ' Создаём аккаунт
                    Dim _err = MySqlDataBases.REALMD.ACCOUNT.INSERT_ACCOUNT(TextBox_UserName.Text.Trim.ToUpper,
                                                                            srp.Verifier,
                                                                            srp.Salt,
                                                                            ComboBox_AccountType.SelectedIndex,
                                                                            ComboBox_Expansion.SelectedIndex)
                    If _err.Item1 Then
                        ' Ошибка
                        MessageBox.Show(_err.Item2,
                                        My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Else
                        ' Новый пользователь успешно создан
                        MessageBox.Show(My.Resources.P065_AccountCreated,
                                        My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Close()
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

                ' Генерим верификатор на основании соли
                Dim srp = New SRP6(TextBox_UserName.Text.Trim.ToUpper, TextBox_Password.Text.Trim.ToUpper, dr("s").ToString)
                DR("v") = srp.Verifier

            End If

            'Обновляем параметры пользователя
            DR("gmlevel") = ComboBox_AccountType.SelectedIndex
            DR("expansion") = ComboBox_Expansion.SelectedIndex
            Dim _err = MySqlDataBases.REALMD.ACCOUNT.UPDATE_ACCOUNT(DR)
            If _err.Item1 Then
                ' Ошибка
                MessageBox.Show(_err.Item2,
                                My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                ' Данные пользователя успешно изменены
                MessageBox.Show(My.Resources.P066_AccountChanged,
                                My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Close()
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

End Class