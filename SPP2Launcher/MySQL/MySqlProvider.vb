
Imports MySql.Data.MySqlClient

Public Class MySqlProvider

    Public Enum EDataBase

        DbArmory

        DbCaractes

        DbLogs

        DbWorld

        DbPlayerbots

        DbRealm

    End Enum

#Region " === ДРУЖЕСТВЕННЫЕ СВОЙСТВА === "

    ''' <summary>
    ''' Строка подключения к базе MySQL.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property ConnectionString As String

    ''' <summary>
    ''' БД Armory.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property DbArmoryName As String

    ''' <summary>
    ''' БД Characters.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property DbCharName As String

    ''' <summary>
    ''' БД Logs.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property DbLogName As String

    ''' <summary>
    ''' БД Mangos.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property DbWorldName As String

    ''' <summary>
    ''' БД Palyerbots.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property DbBotName As String

    ''' <summary>
    ''' БД Realmd.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared ReadOnly Property DbRealmName As String

#End Region

#Region " === КОНСТРУКТОР ИНИЦИАЛИЗАЦИИ === "

    ''' <summary>
    ''' Конструктор инициализации.
    ''' </summary>
    Sub New()
        Dim server, port, userId, password As String
        If My.Settings.UseIntMySQL Then
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    server = My.Settings.MySqlClassicIntHost
                    port = My.Settings.MySqlClassicIntPort
                    userId = My.Settings.MySqlClassicIntUserName
                    password = My.Settings.MySqlClassicIntPassword
                    _DbArmoryName = My.Settings.MySqlClassicIntArmory
                    _DbCharName = My.Settings.MySqlClassicIntCharacters
                    _DbLogName = My.Settings.MySqlClassicIntLogs
                    _DbWorldName = My.Settings.MySqlClassicIntMangos
                    _DbBotName = My.Settings.MySqlClassicIntPlayerbots
                    _DbRealmName = My.Settings.MySqlClassicIntRealmd
                Case GV.EModule.Tbc.ToString
                    server = My.Settings.MySqlTbcIntHost
                    port = My.Settings.MySqlTbcIntPort
                    userId = My.Settings.MySqlTbcIntUserName
                    password = My.Settings.MySqlTbcIntPassword
                    _DbArmoryName = My.Settings.MySqlTbcIntArmory
                    _DbCharName = My.Settings.MySqlTbcIntCharacters
                    _DbLogName = My.Settings.MySqlTbcIntLogs
                    _DbWorldName = My.Settings.MySqlTbcIntMangos
                    _DbBotName = My.Settings.MySqlTbcIntPlayerbots
                    _DbRealmName = My.Settings.MySqlTbcIntRealmd
                Case GV.EModule.Wotlk.ToString
                    server = My.Settings.MySqlWotlkIntHost
                    port = My.Settings.MySqlWotlkIntPort
                    userId = My.Settings.MySqlWotlkIntUserName
                    password = My.Settings.MySqlWotlkIntPassword
                    _DbArmoryName = My.Settings.MySqlWotlkIntArmory
                    _DbCharName = My.Settings.MySqlWotlkIntCharacters
                    _DbLogName = My.Settings.MySqlWotlkIntLogs
                    _DbWorldName = My.Settings.MySqlWotlkIntMangos
                    _DbBotName = My.Settings.MySqlWotlkIntPlayerbots
                    _DbRealmName = My.Settings.MySqlWotlkIntRealmd
                Case Else
                    Throw New Exception(String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType))
            End Select
        Else
            Select Case My.Settings.LastLoadedServerType
                Case GV.EModule.Classic.ToString
                    server = My.Settings.MySqlClassicExtHost
                    port = My.Settings.MySqlClassicExtPort
                    userId = My.Settings.MySqlClassicExtUserName
                    password = My.Settings.MySqlClassicExtPassword
                    _DbArmoryName = My.Settings.MySqlClassicExtArmory
                    _DbCharName = My.Settings.MySqlClassicExtCharacters
                    _DbLogName = My.Settings.MySqlClassicExtLogs
                    _DbWorldName = My.Settings.MySqlClassicExtMangos
                    _DbBotName = My.Settings.MySqlClassicExtPlayerbots
                    _DbRealmName = My.Settings.MySqlClassicExtRealmd
                Case GV.EModule.Tbc.ToString
                    server = My.Settings.MySqlTbcExtHost
                    port = My.Settings.MySqlTbcExtPort
                    userId = My.Settings.MySqlTbcExtUserName
                    password = My.Settings.MySqlTbcExtPassword
                    _DbArmoryName = My.Settings.MySqlTbcExtArmory
                    _DbCharName = My.Settings.MySqlTbcExtCharacters
                    _DbLogName = My.Settings.MySqlTbcExtLogs
                    _DbWorldName = My.Settings.MySqlTbcExtMangos
                    _DbBotName = My.Settings.MySqlTbcExtPlayerbots
                    _DbRealmName = My.Settings.MySqlTbcExtRealmd
                Case GV.EModule.Wotlk.ToString
                    server = My.Settings.MySqlWotlkExtHost
                    port = My.Settings.MySqlWotlkExtPort
                    userId = My.Settings.MySqlWotlkExtUserName
                    password = My.Settings.MySqlWotlkExtPassword
                    _DbArmoryName = My.Settings.MySqlWotlkExtArmory
                    _DbCharName = My.Settings.MySqlWotlkExtCharacters
                    _DbLogName = My.Settings.MySqlWotlkExtLogs
                    _DbWorldName = My.Settings.MySqlWotlkExtMangos
                    _DbBotName = My.Settings.MySqlWotlkExtPlayerbots
                    _DbRealmName = My.Settings.MySqlWotlkExtRealmd
                Case Else
                    Throw New Exception(String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType))
            End Select
        End If
        _ConnectionString = "server=" & server & ";UID=" & userId & ";PWD=" & password & ";port=" & port
    End Sub

#End Region

#Region " === ПУБЛИЧНЫЕ МЕТОДЫ === "

    ''' <summary>
    ''' Возвращает полную строку подключения с указанной базой данных.
    ''' </summary>
    ''' <param name="db">Имя базы данных.</param>
    ''' <returns></returns>
    Friend Shared Function GetConnectionString(db As EDataBase) As String
        Select Case db
            Case EDataBase.DbArmory
                Return ConnectionString & ";database=" & DbArmoryName
            Case EDataBase.DbCaractes
                Return ConnectionString & ";database=" & DbCharName
            Case EDataBase.DbLogs
                Return ConnectionString & ";database=" & DbLogName
            Case EDataBase.DbPlayerbots
                Return ConnectionString & ";database=" & DbBotName
            Case EDataBase.DbRealm
                Return ConnectionString & ";database=" & DbRealmName
            Case EDataBase.DbWorld
                Return ConnectionString & ";database=" & DbWorldName
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Тест соединения с сервером MySQL.
    ''' При ошибке возвращает True.
    ''' </summary>
    ''' <param name="_err"></param>
    ''' <returns></returns>
    Friend Shared Function TestSQL(ByRef _err As String) As Boolean
        Using SqlConn = New MySqlConnection(ConnectionString)
            Try
                SqlConn.Open()
                Return False
            Catch ex As Exception
                _err = ex.Message
                Return True
            End Try
        End Using
    End Function

#End Region

End Class
