
Public Class SPP2Module

    Public Property ModuleType As GV.EModule

    Public Property MySQL As MySqlConf

    Sub New(type As GV.EModule)
        Me.ModuleType = type
    End Sub

    Public Class MySqlConf

    End Class


End Class


