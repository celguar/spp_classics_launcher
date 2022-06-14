
Imports System.Security.Cryptography

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

        If GV.SPP2Launcher.WorldON And Not IsNothing(GV.SPP2Launcher.WorldProcess) Then

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

                Dim dr = GetAccountInfo(TextBox_UserName.Text)
                If Not IsNothing(dr) Then
                    ComboBox_Expansion.SelectedIndex = CInt(dr("expansion"))
                    ComboBox_AccountType.SelectedIndex = CInt(dr("gmlevel"))
                End If

            End If

        Else
            ' Необходим допуск к консоли World
            MessageBox.Show(My.Resources.P037_WorldNotStarted,
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

                    If GV.SPP2Launcher.WorldON And Not IsNothing(GV.SPP2Launcher.WorldProcess) Then

                        ' Проверяем наличие пользователей с таким именем
                        Dim _err = New Tuple(Of Boolean, String)(False, "OK")
                        Dim dr = MySqlDataBases.REALMD.ACCOUNT.SELECT_ACCOUNT(TextBox_UserName.Text.Trim.ToUpper, _err)

                        If _err.Item1 = False Then

                            ' Пользователь с таким именем уже существует
                            MessageBox.Show(String.Format(My.Resources.P056_UserFound, TextBox_UserName.Text.Trim),
                                        My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)

                        Else

                            ' Отправляем команду на создание аккаунта
                            Dim cmd = String.Format(".account create {0} {1}", TextBox_UserName.Text.Trim, TextBox_Password.Text.Trim)
                            Dim cm = New ConsoleCommand(cmd, ECommand.AccountCreate, AddressOf AccountCreated)
                            Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
                            t.Start(cm)

                            ' Запрещаем доступ к кнопке СОЗДАТЬ
                            Button_Create.Enabled = False

                        End If

                    Else
                        ' Необходим допуск к консоли World
                        MessageBox.Show(My.Resources.P037_WorldNotStarted,
                                        My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

                If GV.SPP2Launcher.WorldON And Not IsNothing(GV.SPP2Launcher.WorldProcess) Then

                    ' Отправляем команду на изменение пароля
                    Dim cmd = String.Format(".account set password {0} {1} {1}", TextBox_UserName.Text.Trim, TextBox_Password.Text.Trim)
                    Dim cm = New ConsoleCommand(cmd, ECommand.AccountSetPassword, AddressOf PasswordChanged)
                    Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
                    t.Start(cm)

                    ' Запрещаем доступ к кнопке ИЗМЕНИТЬ
                    Button_Create.Enabled = False

                Else
                    ' Необходим допуск к консоли World
                    MessageBox.Show(My.Resources.P037_WorldNotStarted,
                                    My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Close()
                End If

            Else

                If GV.SPP2Launcher.WorldON Then

                    ' Меняем уровень GM
                    Dim cmd = String.Format(".account set gmlevel {0} {1}", TextBox_UserName.Text.Trim, ComboBox_AccountType.SelectedIndex)
                    Dim cm = New ConsoleCommand(cmd, ECommand.SetGmLevel, AddressOf LevelStep2Changed)
                    Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
                    t.Start(cm)

                    ' Запрещаем доступ к кнопке ИЗМЕНИТЬ
                    Button_Create.Enabled = False

                Else
                    ' Необходим допуск к консоли World
                    MessageBox.Show(My.Resources.P037_WorldNotStarted,
                                    My.Resources.E003_ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Close()
                End If

            End If

        End If

    End Sub

#Region " === СОЗДАТЬ АККАУНТ === "

    ''' <summary>
    ''' Аккаунт успешно создан?
    ''' </summary>
    ''' <param name="result"></param>
    Public Sub AccountCreated(result As Boolean)
        If Not result Then Close()

        ' Меняем уровень GM
        Dim cmd = String.Format(".account set gmlevel {0} {1}",
                                Me.Invoke(Function() As Object
                                              Return TextBox_UserName.Text.Trim
                                          End Function),
                                Me.Invoke(Function() As Object
                                              Return ComboBox_AccountType.SelectedIndex
                                          End Function))
        Dim cm = New ConsoleCommand(cmd, ECommand.SetGmLevel, AddressOf LevelStep1Changed)
        Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
        t.Start(cm)

    End Sub

    ''' <summary>
    ''' Уровень GM успешно изменён?
    ''' </summary>
    ''' <param name="result"></param>
    Public Sub LevelStep1Changed(result As Boolean)
        If Not result Then Close()

        ' Устанавливаем аддон
        Dim cmd = String.Format(".account set addon {0} {1}",
                                Me.Invoke(Function() As Object
                                              Return TextBox_UserName.Text.Trim
                                          End Function),
                                Me.Invoke(Function() As Object
                                              Return ComboBox_Expansion.SelectedIndex
                                          End Function))
        Dim cm = New ConsoleCommand(cmd, ECommand.SetAddon, AddressOf AddonStep1Changed)
        Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
        t.Start(cm)

    End Sub

    ''' <summary>
    ''' Аддон успешно изменён?
    ''' </summary>
    ''' <param name="result"></param>
    Public Sub AddonStep1Changed(result As Boolean)
        If Not result Then Close()

        Me.Invoke(Sub()
                      ' Новый пользователь успешно создан
                      MessageBox.Show(My.Resources.P065_AccountCreated,
                                      My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                      Close()
                  End Sub)
    End Sub

#End Region

#Region " === ИЗМЕНИТЬ АККАУНТ === "

    ''' <summary>
    ''' Пароль успешно изменён?
    ''' </summary>
    ''' <param name="result"></param>
    Public Sub PasswordChanged(result As Boolean)
        If Not result Then Close()

        ' Меняем уровень GM
        Dim cmd = String.Format(".account set gmlevel {0} {1}",
                                Me.Invoke(Function() As Object
                                              Return TextBox_UserName.Text.Trim()
                                          End Function),
                                Me.Invoke(Function() As Object
                                              Return ComboBox_AccountType.SelectedIndex
                                          End Function))
        Dim cm = New ConsoleCommand(cmd, ECommand.SetGmLevel, AddressOf LevelStep2Changed)
        Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
        t.Start(cm)

    End Sub

    ''' <summary>
    ''' Уровень GM успешно изменён?
    ''' </summary>
    ''' <param name="result"></param>
    Public Sub LevelStep2Changed(result As Boolean)
        If Not result Then Close()

        ' Устанавливаем аддон
        Dim cmd = String.Format(".account set addon {0} {1}",
                                Me.Invoke(Function() As Object
                                              Return TextBox_UserName.Text.Trim
                                          End Function),
                                Me.Invoke(Function() As Object
                                              Return ComboBox_Expansion.SelectedIndex
                                          End Function))
        Dim cm = New ConsoleCommand(cmd, ECommand.SetAddon, AddressOf AddonStep2Changed)
        Dim t As New Threading.Thread(AddressOf WaitSuccessfull) With {.IsBackground = True, .CurrentCulture = GV.CI, .CurrentUICulture = GV.CI}
        t.Start(cm)

    End Sub

    ''' <summary>
    ''' Аддон успешно изменён?
    ''' </summary>
    ''' <param name="result"></param>
    Public Sub AddonStep2Changed(result As Boolean)
        If Not result Then Close()

        Me.Invoke(Sub()
                      ' Данные пользователя успешно изменены
                      MessageBox.Show(My.Resources.P066_AccountChanged,
                                      My.Resources.P007_MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                      Close()
                  End Sub)
    End Sub

#End Region

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