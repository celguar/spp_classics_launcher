
Imports System.Numerics
Imports System.Security.Cryptography

Public Class SRP

    Private Property m_Random() As Random

    Private Property m_SHA1Managed() As SHA1Managed

    Public Property AccountName() As String

    Public Property PasswordHash() As String

    Public ReadOnly Property g() As New BigInteger(7)

    Public ReadOnly Property k() As New BigInteger(3)

    Private privateK As BigInteger

    Public Property K() As BigInteger
        Get
            Return privateK
        End Get
        Private Set(ByVal value As BigInteger)
            privateK = value
        End Set
    End Property

    Public ReadOnly Property N() As New BigInteger(New Byte() {137, 75, 100, 94, 137, 225, 83, 91, 189, 173, 91, 139, 41, 6, 80, 83, 8, 1, 177, 142, 191, 191, 94, 143, 171, 60, 130, 135, 42, 62, 155, 183})

    Public Property A() As BigInteger

    Private privateb As BigInteger

    Public Property b() As BigInteger
        Get
            Return privateb
        End Get
        Private Set(ByVal value As BigInteger)
            privateb = value
        End Set
    End Property

    Private privateB As BigInteger

    Public Property B() As BigInteger
        Get
            Return privateb
        End Get
        Private Set(ByVal value As BigInteger)
            privateb = value
        End Set
    End Property

    Private privateu As BigInteger

    Public Property u() As BigInteger
        Get
            Return privateu
        End Get
        Private Set(ByVal value As BigInteger)
            privateu = value
        End Set
    End Property

    Public Property M1() As BigInteger

    Private privateM2 As BigInteger

    Public Property M2() As BigInteger
        Get
            Return privateM2
        End Get
        Private Set(ByVal value As BigInteger)
            privateM2 = value
        End Set
    End Property

    Public ReadOnly Property s() As New BigInteger(New Byte() {173, 208, 58, 49, 210, 113, 20, 70, 117, 242, 112, 126, 80, 38, 182, 210, 241, 134, 89, 153, 118, 2, 80, 170, 185, 69, 224, 158, 221, 42, 163, 69})

    Public Property v() As BigInteger

    Private privatex As BigInteger

    Public Property x() As BigInteger
        Get
            Return privatex
        End Get
        Private Set(ByVal value As BigInteger)
            privatex = value
        End Set
    End Property

    Public Sub New()
        m_Random = New Random()
        m_SHA1Managed = New SHA1Managed()
    End Sub

    Public Function OnLogonChallenge() As Boolean
        Dim saltNamePassword() As New Byte(){}.Concat(s.ToByteArray()).Concat(Encoding.UTF8.GetBytes(AccountName & ":" & PasswordHash)).ToArray()

			Dim hashedNamePasswordHash() As New Byte(){}.Concat(m_SHA1Managed.ComputeHash(saltNamePassword)).Concat(New Byte() { 0 }).ToArray()

			x = New BigInteger(hashedNamePasswordHash)

        v = BigInteger.ModPow(g, x, N)

        Dim tempBytes(31) As Byte
        m_Random.NextBytes(tempBytes)
        ' We want a positive number for calculations
        tempBytes(tempBytes.Length - 1) = 0

        b = New BigInteger(tempBytes)

        B_Conflict = ((k * v) + BigInteger.ModPow(g, b, N)) Mod N

        Return True
    End Function

    Public Function OnLogonProof() As Boolean
        If A Mod N = 0 Then
            Return False
        End If

        Dim ABBytes() As New Byte(){}.Concat(A.ToByteArray()).Concat(B_Conflict.ToByteArray()).ToArray()

			Dim hashedABBytes() As New Byte(){}.Concat(m_SHA1Managed.ComputeHash(ABBytes)).Concat(New Byte() { 0 }).ToArray()

			u = New BigInteger(hashedABBytes.ToArray())

        'INSTANT VB NOTE: The variable S was renamed since Visual Basic does not handle local variables named the same as class members well:
        Dim S_Conflict As BigInteger = BigInteger.ModPow((A * (BigInteger.ModPow(v, u, N))), b, N)

        '* ERROR **
        ' You have to make a custom SHA-1 Interleave hash function, the regular won't do.
        ' See http://tools.ietf.org/rfc/rfc2945.txt
        Dim hashedSBytes() As Byte = m_SHA1Managed.ComputeHash(S_Conflict.ToByteArray())
        K_Conflict = New BigInteger(hashedSBytes.ToArray())

        Dim hashedNBytes() As Byte = m_SHA1Managed.ComputeHash(N.ToByteArray())
        Dim hashedN As New BigInteger(hashedNBytes)

        Dim hashedGBytes() As Byte = m_SHA1Managed.ComputeHash(g.ToByteArray())
        Dim hashedG As New BigInteger(hashedGBytes.ToArray())

        Dim hashedAccountBytes() As Byte = m_SHA1Managed.ComputeHash(Encoding.UTF8.GetBytes(AccountName))

        '             BigInteger hashedAccountName = new BigInteger(hashedAccountBytes.ToArray());
        '                This is just wrong - it reverses the byte ordering and it shouldn't have. 
        '                Not here at least.
        '            
        Dim NxorG(19) As Byte
        For i As Integer = 0 To NxorG.Length - 1
            NxorG(i) = CByte(hashedN.ToByteArray()(i) Xor hashedG.ToByteArray()(i))
        Next i

        Dim allBytes() As New Byte(){}.Concat(NxorG).Concat(hashedAccountBytes).Concat(s.ToByteArray()).Concat(A.ToByteArray()).Concat(B_Conflict.ToByteArray()).Concat(K_Conflict.ToByteArray()).ToArray()

			' This is a server-side check against M1, sent from the client - they should match.
			Dim MS As New BigInteger(m_SHA1Managed.ComputeHash(allBytes))

        Dim testBytes() As New Byte(){}.Concat(A.ToByteArray()).Concat(M1.ToByteArray()).Concat(K_Conflict.ToByteArray()).ToArray()

			' i think this is actually the server calculation, not MS
			' Indeed, this is not MS (or M1 for that matter).
			' This is server's proof of session key, required to prove 
			' the client that this is indeed the server with which he/she registered.
			M2 = New BigInteger(m_SHA1Managed.ComputeHash(testBytes.ToArray()))

        If M2 <> M1 Then
            Return False
        End If

        Return True

    End Function

End Class
