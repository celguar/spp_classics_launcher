
Imports DevCake.WoW.SPP2Launcher.MySqlProvider
Imports MySql.Data.MySqlClient

Namespace MySqlDataBases

    Partial Public Class Backup

        ''' <summary>
        ''' Сохраняет базу данных Realmd.
        ''' </summary>
        Shared Function REALMD(autosave As Boolean) As String
            If CheckProcess(EProcess.mysqld) Then
                Dim file As String = ""
                If My.Settings.UseSqlBackupProjectFolder Then
                    Select Case My.Settings.LastLoadedServerType
                        Case GV.EModule.Classic.ToString
                            file = My.Settings.DirSPP2 & "\" & SPP2SAVES & "\vanilla\autosave\realmd.sql"
                        Case GV.EModule.Tbc.ToString
                            file = My.Settings.DirSPP2 & "\" & SPP2SAVES & "\tbc\autosave\realmd.sql"
                        Case GV.EModule.Wotlk.ToString
                            file = My.Settings.DirSPP2 & "\" & SPP2SAVES & "\wotlk\autosave\realmd.sql"
                        Case Else
                            Dim str = String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType)
                            GV.Log.WriteError(str)
                            Return str
                    End Select
                Else
                    Select Case My.Settings.LastLoadedServerType
                        Case GV.EModule.Classic.ToString
                            file = Application.StartupPath & "\Saves\classic\autosave\realmd"
                        Case GV.EModule.Tbc.ToString
                            file = Application.StartupPath & "\Saves\tbc\autosave\realmd"
                        Case GV.EModule.Wotlk.ToString
                            file = Application.StartupPath & "\Saves\wotlk\autosave\realmd"
                        Case Else
                            Dim str = String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType)
                            GV.Log.WriteError(str)
                            Return str
                    End Select
                    file = String.Format("{0}_{1}.sql", file, Strings.Format(Date.Now, "yyyy-MM-dd HH-mm-ss"))
                End If
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbRealmd))
                    Using sqlComm As New MySqlCommand()
                        Using mb As New MySqlBackup(sqlComm)
                            Try
                                GV.Log.WriteSQL(GV.SPP2Launcher.UpdateWorldConsole(String.Format(My.Resources.P022_BackupStart, file), QCONSOLE))
                                sqlConn.Open()
                                sqlComm.Connection = sqlConn
                                mb.ExportToFile(file)
                                GV.Log.WriteSQL(GV.SPP2Launcher.UpdateWorldConsole(String.Format(My.Resources.P024_BackupSuccess, "Realmd"), QCONSOLE))
                                ' Удаляем "старые" бэкапы
                                RemoveOldBackups(autosave, file)
                                Return ""
                            Catch ex As Exception
                                GV.Log.WriteSQLException(ex)
                                Return My.Resources.E017_SeeLog
                            End Try
                        End Using
                    End Using
                End Using
            Else
                Return My.Resources.P054_BackupNeedMySQL
            End If
        End Function

    End Class

End Namespace
