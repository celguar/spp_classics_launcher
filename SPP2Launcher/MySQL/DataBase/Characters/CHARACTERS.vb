
Imports DevCake.WoW.SPP2Launcher.MySqlProvider
Imports MySql.Data.MySqlClient

Namespace MySqlDataBases

    Partial Public Class Backup

        ''' <summary>
        ''' Сохраняет базу данных Realmd.
        ''' </summary>
        Shared Function CHARACTERS(autosave As Boolean) As String
            If CheckProcess(EProcess.mysqld) Then
                Dim file As String = ""
                If My.Settings.UseSqlBackupProjectFolder Then
                    Select Case My.Settings.LastLoadedServerType
                        Case GV.EModule.Classic.ToString
                            file = My.Settings.DirSPP2 & "\" & SPP2SAVES & "\vanilla\autosave\characters.sql"
                        Case GV.EModule.Tbc.ToString
                            file = My.Settings.DirSPP2 & "\" & SPP2SAVES & "\tbc\autosave\characters.sql"
                        Case GV.EModule.Wotlk.ToString
                            file = My.Settings.DirSPP2 & "\" & SPP2SAVES & "\wotlk\autosave\characters.sql"
                        Case Else
                            Dim str = String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType)
                            GV.Log.WriteError(str)
                            Return str
                    End Select
                Else
                    Select Case My.Settings.LastLoadedServerType
                        Case GV.EModule.Classic.ToString
                            file = Application.StartupPath & "\Saves\classic\autosave\characters"
                        Case GV.EModule.Tbc.ToString
                            file = Application.StartupPath & "\Saves\tbc\autosave\characters"
                        Case GV.EModule.Wotlk.ToString
                            file = Application.StartupPath & "\Saves\wotlk\autosave\characters"
                        Case Else
                            Dim str = String.Format(My.Resources.E008_UnknownModule, My.Settings.LastLoadedServerType)
                            GV.Log.WriteError(str)
                            Return str
                    End Select
                    file = String.Format("{0}_{1}.sql", file, Strings.Format(Date.Now, "yyyy-MM-dd HH-mm-ss"))
                End If
                Using sqlConn As New MySqlConnection(GetConnectionString(EDataBase.DbCharacters))
                    Using sqlComm As New MySqlCommand()
                        Using mb As New MySqlBackup(sqlComm)
                            Try
                                GV.Log.WriteSQL(GV.SPP2Launcher.UpdateWorldConsole(String.Format(My.Resources.P022_BackupStart, file), QCONSOLE))
                                sqlConn.Open()
                                sqlComm.Connection = sqlConn
                                mb.ExportToFile(file)
                                GV.Log.WriteSQL(GV.SPP2Launcher.UpdateWorldConsole(String.Format(My.Resources.P024_BackupSuccess, "Characters"), QCONSOLE))
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
