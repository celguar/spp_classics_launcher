
Imports DevCake.WoW.SPP2Launcher.MySqlProvider
Imports MySql.Data.MySqlClient

Namespace MySqlDB.REALMD

    ''' <summary>
    ''' Таблица Account базы Realmd
    ''' </summary>
    Public Class ACCOUNT

        ''' <summary>
        ''' Возвращает параметры указанного аккаунта.
        ''' </summary>
        ''' <param name="userName">Имя пользователя.</param>
        ''' <param name="_err">True - произошла ошибка.</param>
        ''' <returns></returns>
        Shared Function SELECT_ACCOUNT(userName As String, ByRef _err As Tuple(Of Boolean, String)) As DataRow
            If CheckProcess(EProcess.mysqld) Then
                Dim table = New DataTable
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        With sqlComm
                            .Connection = sqlConn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT * FROM account WHERE username = @UserName"
                            .Parameters.AddWithValue("@UserName", userName)
                        End With
                        Try
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            sqlComm.ExecuteNonQuery()
                            Dim DA = New MySqlDataAdapter(sqlComm)
                            DA.Fill(table)
                            If table.Rows.Count > 0 Then
                                GV.Log.WriteSQL("Accounts count = " & table.Rows.Count)
                                _err = New Tuple(Of Boolean, String)(False, "OK")
                                Return table.Select.First()
                            Else
                                GV.Log.WriteSQL(String.Format(My.Resources.P038_AccountNotFound, "name = " & userName))
                                _err = New Tuple(Of Boolean, String)(True, String.Format(My.Resources.P038_AccountNotFound, "name = " & userName))
                                Return Nothing
                            End If
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return Nothing
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                _err = New Tuple(Of Boolean, String)(True, My.Resources.P054_NeedMySQL)
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Возвращает список аккаунтов исключая ботов.
        ''' </summary>
        ''' <param name="botPrefix">Префикс имени для ботов.</param>
        ''' <returns></returns>
        Shared Function SELECT_REAL_ACCOUNT(botPrefix As String) As String()
            If CheckProcess(EProcess.mysqld) Then
                Dim accounts As New List(Of String)
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        With sqlComm
                            .Connection = sqlConn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT username FROM account WHERE username NOT LIKE @Prefix"
                            .Parameters.AddWithValue("@Prefix", botPrefix & "%")
                        End With
                        Try
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim reader = sqlComm.ExecuteReader()
                            While reader.Read()
                                accounts.Add(reader("username").ToString)
                            End While
                            GV.Log.WriteSQL("All Accounts = " & accounts.Count)
                            Return accounts.ToArray
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return Nothing
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Создаёт новую учётную запись.
        ''' </summary>
        ''' <param name="userName">Имя пользователя.</param>
        ''' <param name="verifier">Верификатор.</param>
        ''' <param name="salt">Соль.</param>
        ''' <param name="gmLevel">Уровень GM.</param>
        ''' <param name="expansion">Расширение.</param>
        ''' <returns></returns>
        Shared Function INSERT_ACCOUNT(userName As String,
                                       verifier As String,
                                       salt As String,
                                       gmLevel As Integer,
                                       expansion As Integer) As Tuple(Of Boolean, String)
            If CheckProcess(EProcess.mysqld) Then
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        Try
                            Dim cmd As String = ""
                            cmd &= "INSERT INTO account "
                            cmd &= "(username, gmlevel, v, s, expansion) "
                            cmd &= "VALUES (@UserName, @GmLevel, @Verifier, @Salt, @Expansion)"
                            With sqlComm
                                .Connection = sqlConn
                                .CommandType = CommandType.Text
                                .CommandText = cmd
                                .Parameters.AddWithValue("@UserName", userName)
                                .Parameters.AddWithValue("@Verifier", verifier)
                                .Parameters.AddWithValue("@Salt", salt)
                                .Parameters.AddWithValue("@GmLevel", gmLevel)
                                .Parameters.AddWithValue("@Expansion", expansion)
                            End With
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim count = sqlComm.ExecuteNonQuery
                            GV.Log.WriteSQL(String.Format("Account inserted {0}.", userName))
                            Return New Tuple(Of Boolean, String)(False, "OK")
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return New Tuple(Of Boolean, String)(True, ex.Message)
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                Return New Tuple(Of Boolean, String)(True, My.Resources.P054_NeedMySQL)
            End If
        End Function

        ''' <summary>
        ''' Изменяет параметры аккаунта.
        ''' </summary>
        ''' <param name="username">Имя аккаунта.</param>
        ''' <param name="expansion">Расширение.</param>
        ''' <returns></returns>
        Shared Function UPDATE_ACCOUNT(username As String, expansion As Integer) As Tuple(Of Boolean, String)
            If CheckProcess(EProcess.mysqld) Then
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        Try
                            Dim cmd As String = ""
                            cmd &= "UPDATE account "
                            cmd &= "SET username = @UserName, expansion = @Expansion "
                            cmd &= "WHERE username = @UserName"
                            With sqlComm
                                .Connection = sqlConn
                                .CommandType = CommandType.Text
                                .CommandText = cmd
                                .Parameters.AddWithValue("@UserName", username)
                                .Parameters.AddWithValue("@Expansion", expansion)
                            End With

                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim count = sqlComm.ExecuteNonQuery
                            GV.Log.WriteSQL(String.Format("Account {0} changed.", username))
                            Return New Tuple(Of Boolean, String)(False, "OK")
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return New Tuple(Of Boolean, String)(True, ex.Message)
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                Return New Tuple(Of Boolean, String)(True, My.Resources.P054_NeedMySQL)
            End If
        End Function

        ''' <summary>
        ''' Изменяет параметры аккаунта.
        ''' </summary>
        ''' <param name="dr">DataRow содержащий необходимые параметры.</param>
        ''' <returns></returns>
        Shared Function UPDATE_ACCOUNT(dr As DataRow) As Tuple(Of Boolean, String)
            If CheckProcess(EProcess.mysqld) Then
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        Try
                            Dim cmd As String = ""
                            cmd &= "UPDATE account "
                            cmd &= "SET gmlevel = @GmLevel, v = @V, s = @S, email = @Email, lockedip = @LockedIp, failed_logins = @FailedLogins, "
                            cmd &= "locked = @Locked, active_realm_id = @RealmId, expansion = @Expansion, mutetime = @MuteTime "
                            cmd &= "WHERE username = @UserName"
                            With sqlComm
                                .Connection = sqlConn
                                .CommandType = CommandType.Text
                                .CommandText = cmd
                                .Parameters.AddWithValue("@UserName", dr("username"))
                                .Parameters.AddWithValue("@GmLevel", dr("gmlevel"))
                                .Parameters.AddWithValue("@V", dr("v"))
                                .Parameters.AddWithValue("@S", dr("s"))
                                .Parameters.AddWithValue("@Email", dr("email"))
                                .Parameters.AddWithValue("@LockedIp", dr("lockedip"))
                                .Parameters.AddWithValue("@FailedLogins", dr("failed_logins"))
                                .Parameters.AddWithValue("@Locked", dr("locked"))
                                .Parameters.AddWithValue("@RealmId", dr("active_realm_id"))
                                .Parameters.AddWithValue("@Expansion", dr("expansion"))
                                .Parameters.AddWithValue("@MuteTime", dr("mutetime"))
                            End With

                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim count = sqlComm.ExecuteNonQuery
                            GV.Log.WriteSQL(String.Format("Account {0} changed.", dr("userName")))
                            Return New Tuple(Of Boolean, String)(False, "OK")
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return New Tuple(Of Boolean, String)(True, ex.Message)
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                Return New Tuple(Of Boolean, String)(True, My.Resources.P054_NeedMySQL)
            End If
        End Function

        ''' <summary>
        ''' Удаляет аккаунт с указанным именем.
        ''' </summary>
        ''' <param name="userName"></param>
        ''' <returns></returns>
        Shared Function DELETE_ACCOUNT(userName As String) As Tuple(Of Boolean, String)
            If CheckProcess(EProcess.mysqld) Then
                Dim accounts As New List(Of String)
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        Dim cmd As String = ""
                        cmd &= "DELETE FROM account "
                        cmd &= "WHERE username = @UserName"
                        With sqlComm
                            .Connection = sqlConn
                            .CommandType = CommandType.Text
                            .CommandText = cmd
                            .Parameters.AddWithValue("@UserName", userName)
                        End With
                        Try
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim count = sqlComm.ExecuteNonQuery
                            GV.Log.WriteSQL(String.Format("Account {0} deleted.", userName))
                            Return New Tuple(Of Boolean, String)(False, "OK")
                        Catch ex As Exception
                            GV.Log.WriteSQLException(ex)
                            Return New Tuple(Of Boolean, String)(True, ex.Message)
                        End Try
                    End Using
                End Using
            Else
                GV.Log.WriteSQL(My.Resources.P054_NeedMySQL)
                Return New Tuple(Of Boolean, String)(True, My.Resources.P054_NeedMySQL)
            End If
        End Function

    End Class

End Namespace
