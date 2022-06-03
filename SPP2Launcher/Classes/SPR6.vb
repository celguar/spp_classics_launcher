
Imports System.Text
Imports System.Numerics
Imports System.Security.Cryptography

''' <summary>
''' Класс создания s и v для WoW.
''' </summary>
Public Class SPR6

    ''' <summary>
    ''' Соль.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Salt As String
        Get
            Return ByteArrayToString(s.ToByteArray).ToUpper
        End Get
    End Property

    ''' <summary>
    ''' Верификатор.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Verifier As String
        Get
            Return ByteArrayToString(v.ToByteArray).ToUpper
        End Get
    End Property

    ReadOnly rnd As RandomNumberGenerator = RNGCryptoServiceProvider.Create()

    'ReadOnly base() As Byte = (894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7)h
    Dim n As New BigInteger(New Byte() {137, 75, 100, 94, 137, 225, 83, 91, 189, 173, 91, 139, 41, 6, 80, 83, 8, 1, 177, 142, 191, 191, 94, 143, 171, 60, 130, 135, 42, 62, 155, 183})

    Dim g As New BigInteger(7)

    Dim s As New BigInteger(New Byte() {173, 208, 58, 49, 210, 113, 20, 70, 117, 242, 112, 126, 80, 38, 182, 210, 241, 134, 89, 153, 118, 2, 80, 170, 185, 69, 224, 158, 221, 42, 163, 69})

    Dim v As BigInteger

    Dim h2 As BigInteger

    ''' <summary>
    ''' Конструктор инициализации.
    ''' </summary>
    ''' <param name="username">Имя пользователя.</param>
    ''' <param name="password">Пароль.</param>
    Public Sub New(username As String, password As String)

        ' Генерим Salt
        Dim buff() As Byte = New Byte(31) {}
        rnd.GetBytes(buff)
        s = If(New BigInteger(buff) < 0, New BigInteger(buff) * -1, New BigInteger(buff))

        ' Генерим H2 = H(s | H(P))
        Dim p = Encoding.UTF8.GetBytes(username.Trim.ToUpper & ":" & password.Trim.ToUpper)
        Dim h1 = SHA1.Create().ComputeHash(p)
        h2 = New BigInteger(SHA1.Create().ComputeHash(ConcatBytes(s.ToByteArray, h1)))

        ' Генерим v = g ^ H2 % N
        v = BigInteger.ModPow(g, h2, n)

    End Sub

    ''' <summary>
    ''' Объединяет пару массивов байт.
    ''' </summary>
    ''' <param name="data1">M1</param>
    ''' <param name="data2">M2</param>
    ''' <returns></returns>
    Private Function CombineData(ByVal data1() As Byte, ByVal data2() As Byte) As Byte()
        Return New Byte() {}.Concat(data1).Concat(data2).ToArray()
    End Function

    Public Shared Function ConcatBytes(ByVal a() As Byte, ByVal b() As Byte) As Byte()
        Dim bytes((a.Length + b.Length) - 1) As Byte
        Array.Copy(a, bytes, a.Length)
        Array.Copy(b, 0, bytes, a.Length, b.Length)
        Return bytes
    End Function

    ''' <summary>
    ''' Переводит массив байт в строку.
    ''' </summary>
    ''' <param name="data">Массив байт.</param>
    ''' <returns></returns>
    Private Shared Function ByteArrayToString(ByVal data() As Byte) As String
        Dim hex As New StringBuilder(data.Length * 2)
        For Each b As Byte In data
            hex.AppendFormat("{0:x2}", b)
        Next b
        Return hex.ToString()
    End Function

End Class
