


Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class GDI

    <DllImport("user32.dll")>
    Public Shared Function SendMessage(hWnd As IntPtr, wMsg As Int32, wParam As Boolean, lParam As Int32) As Integer
    End Function

    Private Const WM_SETREDRAW As Integer = 11

    ''' <summary>
    ''' Запрещает перерисовку графического элемента.
    ''' </summary>
    ''' <param name="target"></param>
    Public Shared Sub SuspendDrawing(target As Control)
        Try
            If target.InvokeRequired Then
                target.Invoke(New Action(Of Control)(AddressOf SuspendDrawing), target)
            End If
            SendMessage(target.Handle, WM_SETREDRAW, False, 0)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Разрешает перерисовку графического элемента.
    ''' </summary>
    ''' <param name="target"></param>
    Public Shared Sub ResumeDrawing(target As Control)
        Try
            If target.InvokeRequired Then
                target.Invoke(New Action(Of Control)(AddressOf ResumeDrawing), target)
            Else
                SendMessage(target.Handle, WM_SETREDRAW, True, 0)
                target.Refresh()
            End If
        Catch
        End Try
    End Sub

End Class

