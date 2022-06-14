
Imports DevCake.WoW.SPP2Launcher.MySqlProvider
Imports MySql.Data.MySqlClient

Namespace MySqlDataBases.REALMD

    Public Class REALMLIST

        Shared Function SELECT_REALMS(ByRef _err As Tuple(Of Boolean, String)) As DataRow()
            If CheckProcess(EProcess.mysqld) Then
                Dim table = New DataTable
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        With sqlComm
                            .Connection = sqlConn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT * FROM realmlist"
                        End With
                        Try
                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            sqlComm.ExecuteNonQuery()
                            Dim DA = New MySqlDataAdapter(sqlComm)
                            DA.Fill(table)
                            If table.Rows.Count > 0 Then
                                GV.Log.WriteSQL("Realms count = " & table.Rows.Count)
                                _err = New Tuple(Of Boolean, String)(False, "OK")
                                Return table.Select()
                            Else
                                GV.Log.WriteSQL(My.Resources.P060_RealmsNotFound)
                                _err = New Tuple(Of Boolean, String)(True, My.Resources.P060_RealmsNotFound)
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
        ''' Изменяет параметры аккаунта.
        ''' </summary>
        ''' <param name="id">Идентификатор мира.</param>
        ''' <param name="name">Имя мира.</param>
        ''' <param name="address">IP адрес.</param>
        ''' <returns></returns>
        Shared Function UPDATE_REALM(id As String, name As String, address As String) As Tuple(Of Boolean, String)
            If CheckProcess(EProcess.mysqld) Then
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand
                        Try
                            Dim cmd As String = ""
                            cmd &= "UPDATE realmlist "
                            cmd &= "SET name = @Name, address = @Address "
                            cmd &= "WHERE id = @Id"
                            With sqlComm
                                .Connection = sqlConn
                                .CommandType = CommandType.Text
                                .CommandText = cmd
                                .Parameters.AddWithValue("@Id", id)
                                .Parameters.AddWithValue("@Name", name)
                                .Parameters.AddWithValue("@Address", address)
                            End With

                            GV.Log.WriteSQL(sqlComm.CommandText)
                            sqlConn.Open()
                            Dim count = sqlComm.ExecuteNonQuery
                            GV.Log.WriteSQL(String.Format("Realm {0} changed.", id))
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
