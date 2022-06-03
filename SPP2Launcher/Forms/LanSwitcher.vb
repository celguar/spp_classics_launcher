
Public Class LanSwitcher

    Private _IsLoading As Boolean = True

    ''' <summary>
    ''' ПРИ ЗАГРУЗКЕ ФОРМЫ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LanSwitcher_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Заполняем список локальных IpV4 адресов
        ComboBox_Host.Items.Clear()
        ComboBox_Host.Items.AddRange(GetLocalIpAddresses().ToArray)

    End Sub

    ''' <summary>
    ''' ПРИ ИЗМЕНЕНИИ IP
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBox_Host_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_Host.SelectedIndexChanged

    End Sub

End Class