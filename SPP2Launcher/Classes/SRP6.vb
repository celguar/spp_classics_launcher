
Imports System.Text
Imports System.Numerics
Imports System.Security.Cryptography
Imports System.Globalization

''' <summary>
''' Класс создания s и v для эмулятора WoW CMaNGOS.
''' </summary>
Public Class SRP6

    Dim n As BigInteger = BigInteger.Parse("00894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7", NumberStyles.AllowHexSpecifier)

    Dim g As BigInteger = New Byte() {7}.ToBigInteger()

    ReadOnly v(31) As Byte

    ReadOnly s(31) As Byte

    ''' <summary>
    ''' Только для чтения: Соль.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Salt As String
        Get
            Return s.Reverse.ToHexString
        End Get
    End Property

    ''' <summary>
    ''' Только для чтения: Верификатор.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Verifier As String
        Get
            Return v.Reverse.ToHexString
        End Get
    End Property

    ''' <summary>
    ''' Конструктор инициализации.
    ''' </summary>
    ''' <param name="username">Имя пользователя.</param>
    ''' <param name="password">Пароль.</param>
    ''' <param name="salt">Предустановленная соль.</param>
    Public Sub New(username As String, password As String, Optional ByVal salt As String = "")

        If salt = "" Then
            ' Генерим Salt
            Using rng = RandomNumberGenerator.Create()
                rng.GetBytes(s)
            End Using
        Else
            s = salt.ToByteArray.Reverse
        End If

        ' Генерим H2 = H(s | H(P))
        Dim p = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(username.Trim.ToUpper & ":" & password.Trim.ToUpper))
        Dim h2 = SHA1.Create().ComputeHash(s.Combine(p)).ToBigInteger

        ' Генерим v = g ^ H2 % N
        v = BigInteger.ModPow(g, h2, n).ToByteArray

    End Sub

End Class

Module Extensions

    <System.Runtime.CompilerServices.Extension>
    Public Function Combine(ByVal data1() As Byte, ByVal data2() As Byte) As Byte()
        Return New Byte() {}.Concat(data1).Concat(data2).ToArray()
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToHexString(ByVal data() As Byte) As String
        Dim hex As New StringBuilder(data.Length * 2)
        For Each b As Byte In data
            hex.AppendFormat("{0:x2}", b)
        Next b
        Return hex.ToString().ToUpper
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToByteArray(ByVal s As String) As Byte()
        Dim data = New Byte((s.Length \ 2) - 1) {}
        For i As Integer = 0 To s.Length - 1 Step 2
            data(i \ 2) = Convert.ToByte(s.Substring(i, 2), 16)
        Next i
        Return data
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToBigInteger(ByVal value() As Byte, Optional ByVal isBigEndian As Boolean = False) As BigInteger
        If isBigEndian Then
            Array.Reverse(value)
        End If
        Return New BigInteger(value.Combine(New Byte() {0}))
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function Reverse(ByVal data() As Byte) As Byte()
        Array.Reverse(data)
        Return data
    End Function

End Module
