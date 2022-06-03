Imports System
Imports System.IO
Imports System.Numerics
Imports System.Text
Imports System.Runtime.CompilerServices

Public Module Extensions

    <Extension()>
    Public Function ToHexString(ByVal array As Byte()) As String
        Dim builder As StringBuilder = New StringBuilder()

        Dim i = array.Length - 1

        While i >= 0
            builder.Append(array(i).ToString("X2"))
            Threading.Interlocked.Decrement(i)
        End While

        Return builder.ToString()
    End Function

    ''' <summary>
    ''' places a non-negative value (0) at the MSB, then converts to a BigInteger.
    ''' This ensures a non-negative value without changing the binary representation.
    ''' </summary>
    <Extension()>
    Public Function ToBigInteger(ByVal array As Byte()) As BigInteger
        Dim temp As Byte()
        If (array(array.Length - 1) And &H80) = &H80 Then
            temp = New Byte(array.Length + 1 - 1) {}
            temp(array.Length) = 0
        Else
            temp = New Byte(array.Length - 1) {}
        End If

        System.Array.Copy(array, temp, array.Length)
        Return New BigInteger(temp)
    End Function

    ''' <summary>
    ''' Removes the MSB if it is 0, then converts to a byte array.
    ''' </summary>
    <Extension()>
    Public Function ToCleanByteArray(ByVal b As BigInteger) As Byte()
        Dim array As Byte() = b.ToByteArray()
        If array(array.Length - 1) <> 0 Then Return array

        Dim temp = New Byte(array.Length - 1 - 1) {}
        System.Array.Copy(array, temp, temp.Length)
        Return temp
    End Function

    <Extension()>
    Public Function ModPow(ByVal value As BigInteger, ByVal pow As BigInteger, ByVal [mod] As BigInteger) As BigInteger
        Return BigInteger.ModPow(value, pow, [mod])
    End Function

    <Extension()>
    Public Function ReadCString(ByVal reader As BinaryReader) As String
        Dim builder As StringBuilder = New StringBuilder()

        While True
            Dim letter As Byte = reader.ReadByte()
            If letter = 0 Then Exit While

            builder.Append(Microsoft.VisualBasic.ChrW(letter))
        End While

        Return builder.ToString()
    End Function

    <Extension()>
    Public Function SubArray(ByVal array As Byte(), ByVal start As Integer, ByVal count As Integer) As Byte()
        Dim lSubArray = New Byte(count - 1) {}
        System.Array.Copy(array, start, lSubArray, 0, count)
        Return lSubArray
    End Function

    <Extension()>
    Public Function ToCString(ByVal str As String) As Byte()
        Dim data = New Byte(str.Length + 1 - 1) {}
        Array.Copy(Encoding.ASCII.GetBytes(str), data, str.Length)
        data(data.Length - 1) = 0
        Return data
    End Function

End Module

