
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

''' <summary>
''' Работа с файлом инициализации.
''' Последовательность символов "\n" заменяется vbCrLf
''' Символ "#" является комментарием и после него данные не обрабатываются.
''' В отсутствие указанного файла инициализации возвращается Exception.
''' В отсутствие секции, ключа или невозможности его преобразования в нужный базовый тип, всегда возвращается Exception,
''' исключением являются строковые типы - в таком случае возвращается String.Null
''' </summary>
Public Class IniFiles

    ''' <summary>
    ''' Размер буфера для значения ключа.
    ''' </summary>
    Const KEY_BUFFER_SIZE As Integer = 2047

    ''' <summary>
    ''' Размер буфера для считывания секций.
    ''' </summary>
    Const SECTION_BUFFER_SIZE As Integer = 16384

    ''' <summary>
    ''' Полный путь к файлу инициализации.
    ''' </summary>
    Dim p_fullFileName As String = String.Empty

    ''' <summary>
    ''' Короткое имя файла инициализации.
    ''' </summary>
    ReadOnly p_fileName As String = String.Empty

#Region " === НЕУПРАВЛЯЕМЫЕ МЕТОДЫ === "

    Friend NotInheritable Class [Imports]

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function GetPrivateProfileSection(
        ByVal lpAppName As String,
        ByVal lpReturnedString As String,
        ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function GetPrivateProfileSectionNames(
        ByVal lpszReturnBuffer As String,
        ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function GetPrivateProfileString(
        ByVal lpApplicationName As String,
        ByVal lpKeyName As String,
        ByVal lpDefault As String,
        ByVal lpReturnedString As String,
        ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function GetPrivateProfileString(
        ByVal lpApplicationName As String,
        ByVal lpKeyName As String,
        ByVal lpDefault As String,
        ByVal lpReturnedString As IntPtr,
        ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function GetPrivateProfileString(
        ByVal lpApplicationName As String,
        ByVal lpKeyName As String,
        ByVal lpDefault As String,
        ByVal lpReturnedString As StringBuilder,
        ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function GetPrivateProfileStruct(
        ByVal lpszSection As String,
        ByVal lpszKey As String,
        ByVal lpStruct As IntPtr,
        ByVal uSizeStruct As Integer,
        ByVal szFile As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function WritePrivateProfileSection(
        ByVal lpAppName As String,
        ByVal lpString As String,
        ByVal lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function WritePrivateProfileString(
        ByVal lpApplicationName As String,
        ByVal lpKeyName As String,
        ByVal lpString As String,
        ByVal lpFileName As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function WritePrivateProfileStruct(
        ByVal lpszSection As String,
        ByVal lpszKey As String,
        ByVal lpStruct As IntPtr,
        ByVal uSizeStruct As Integer,
        ByVal szFile As String) As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Function GetLastError() As Integer
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Shared Sub SetLastError(ByVal err As Integer)
        End Sub

    End Class

#End Region

#Region " === СВОЙСТВА === "

    ''' <summary>
    ''' Возвращает файл инициализации.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property FullFileName() As String
        Get
            Return p_fullFileName
        End Get
        'Set(value As String)
        '    p_fullFileName = value
        'If Not IO.File.Exists(value) Then
        'Dim swLogFile As IO.StreamWriter
        'swLogFile = IO.File.CreateText(value)
        'swLogFile.Flush()
        'swLogFile.Close()
        'End If
        'End Set
    End Property

    ''' <summary>
    ''' Возвращает короткое имя файла инициализации.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property FileName() As String
        Get
            Return p_fileName
        End Get
    End Property

#End Region

#Region " === КОНСТРУКТОРЫ ИНИЦИАЛИЗАЦИИ === "

    ''' <summary>
    ''' Инициализация класса с указанием файла инициализации.
    ''' </summary>
    ''' <param name="iniFile">Имя файла инициализации.</param>
    ''' <param name="createFile">Флаг создания нового файла инициализации.</param>
    Public Sub New(ByVal iniFile As String, Optional ByVal createFile As Boolean = False)
        Dim pfile = New IO.FileInfo(iniFile).Name
        Dim pdir = New IO.FileInfo(iniFile).DirectoryName
        Dim pf As New IO.FileInfo(pdir & "\" & pfile)
        If createFile Then
            Using fs As IO.FileStream = IO.File.Create(pdir & "\" & pfile)
            End Using
        End If
        If Not pf.Exists Then Throw New Exception("Указанный файл инициализации не найден: " & vbCrLf & pf.FullName)
        p_fullFileName = pdir & "\" & pfile
        p_fileName = pfile
    End Sub

#End Region

#Region " === ПУБЛИЧНЫЕ МЕТОДЫ === "

    ''' <summary>
    ''' Создаёт новый файл инициализации.
    ''' </summary>
    ''' <param name="fullFileName">Имя файла инициализации. Будет использоваться по умолчанию.</param>
    ''' <param name="err">Ссылка на сообщение об ошибке.</param>
    ''' <returns></returns>
    Public Function Create(ByVal fullFileName As String, Optional ByRef err As String = "") As Boolean
        Try
            If Not IO.File.Exists(fullFileName) Then
                Dim swIniFile As IO.StreamWriter
                swIniFile = IO.File.CreateText(fullFileName)
                swIniFile.Flush()
                swIniFile.Close()
            End If
            p_fullFileName = fullFileName
            Return True
        Catch ex As Exception
            err = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Удаляет текущий файл инициализации и создаёт новый.
    ''' </summary>
    Public Sub Clear()
        If Not IO.Directory.Exists(IO.Path.GetDirectoryName(p_fullFileName)) Then
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(p_fullFileName))
        End If
        My.Computer.FileSystem.WriteAllText(p_fullFileName, String.Empty, False)
    End Sub

    ''' <summary>
    ''' Получить список секций из файла инициализации.
    ''' </summary>
    ''' <returns>Массив секций. Nothing - при возникновении ошибки.</returns>
    Public Function ReadSections() As String()
        Try
            ' Получаем строку секций с разделителем vbNullChar секций
            Dim strSections As String = InternalReadString(Nothing, Nothing, "", EIniAtoms.Section)
            If strSections = "" Then Return Nothing
            ' Получаем массив секций (с разделителями через Chr(0))
            Return strSections.ToString.Split(CType(vbNullChar, Char()))
        Catch
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Получить список ключей из указанной секции.
    ''' </summary>
    ''' <param name="Section">Секция из которой следует вывести список.</param>
    ''' <returns>Массив ключей. Nothing - при возникновении ошибки.</returns>
    Public Function ReadSection(ByVal Section As String) As String()
        Try
            ' Получаем строку ключей с разделителем vbNullChar ключей
            Dim strKeys As String = InternalReadString(Section, Nothing, "", EIniAtoms.Section)
            If strKeys = "" Then Return Nothing
            ' Получаем массив ключей (с разделителями через Chr(0))
            Return ConRules(strKeys.ToString.Split(CType(vbNullChar, Char()))).ToArray
        Catch
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Получить значения всех ключей из указанной секции.
    ''' </summary>
    ''' <param name="Section">Секция из которой следует вывести список.</param>
    ''' <returns>Hashtable в формате Ключ=значение. Nothing - при возникновении ошибки.</returns>
    Public Function ReadSectionValues(ByVal Section As String) As Hashtable
        Try
            Dim KeyValueHash As New Hashtable
            ' Получаем все ключи секции
            Dim keys() As String = ReadSection(Section)
            If Not IsNothing(keys) Then
                ' Создаем хэш-таблицу пар Ключ=значение
                For Each key As String In keys
                    ' Получаем значение ключа
                    Dim value = InternalReadString(Section, key, "", EIniAtoms.Key)
                    ' Заменяем \n на vbCrLf
                    Dim pattern As String = "\n*"
                    Dim r1 As Regex = New Regex(pattern)
                    If r1.IsMatch(value) Then value = value.Replace("\n", vbCrLf)
                    ' Добавляем значение ключа
                    KeyValueHash(key) = value
                Next
                Return KeyValueHash
            Else
                Return Nothing
            End If
        Catch
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Получить строковое значение ключа.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение String. Nothing - если ключ пустой/не существует.</returns>
    Public Overloads Function ReadString(ByVal Section As String, ByVal Key As String) As String
        Return ReadString(Section, Key, "")
    End Function

    ''' <summary>
    ''' Получить строковое значение ключа.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение String. Nothing - если ключ пустой/не существует.</returns>
    Public Overloads Function ReadString(ByVal Section As String, ByVal Key As String, ByVal [Default] As String) As String
        Try
            Dim ret = Treatment(InternalReadString(Section, Key, [Default], EIniAtoms.Key))
            If ret = "" Then Return ret
            ' Заменяем \n на vbCrLf
            Dim pattern As String = "\n*"
            Dim r1 As Regex = New Regex(pattern)
            If r1.IsMatch(ret) Then ret = ret.Replace("\n", vbCrLf)
            Return ret
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Boolean.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение Boolean. False - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadBool(ByVal Section As String, ByVal Key As String) As Boolean
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return False
            Else
                Select Case UCase(ret)
                    Case "TRUE"
                        ret = "True"
                    Case "ON"
                        ret = "True"
                    Case "1"
                        ret = "True"
                    Case "YES"
                        ret = "True"
                    Case "NO"
                        ret = "False"
                    Case Else
                        ret = "False"
                End Select
                Return Convert.ToBoolean(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Boolean.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение Boolean. False - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadBool(ByVal Section As String, ByVal Key As String, ByVal [Default] As Boolean) As Boolean
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return False
            Else
                Select Case UCase(ret)
                    Case "TRUE"
                        ret = "True"
                    Case "ON"
                        ret = "True"
                    Case "1"
                        ret = "True"
                    Case "YES"
                        ret = "True"
                    Case Else
                        ret = "False"
                End Select
                Return Convert.ToBoolean(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Byte.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение Byte. Ноль - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadByte(ByVal Section As String, ByVal Key As String) As Byte
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToByte(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Byte.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение Byte. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadByte(ByVal Section As String, ByVal Key As String, ByVal [Default] As Byte) As Byte
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToByte(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Short.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение Short. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadInt16(ByVal Section As String, ByVal Key As String) As Short
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToInt16(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Short.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение Short. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadInt16(ByVal Section As String, ByVal Key As String, ByVal [Default] As Short) As Short
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToInt16(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате UInt16.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение UInt16. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadUInt16(ByVal Section As String, ByVal Key As String) As UInt16
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToUInt16(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате UInt16.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение UInt16. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadUInt16(ByVal Section As String, ByVal Key As String, ByVal [Default] As Short) As UInt16
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToUInt16(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Integer.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение Integer. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadInt32(ByVal Section As String, ByVal Key As String) As Integer
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToInt32(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Integer.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение Integer. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadInt32(ByVal Section As String, ByVal Key As String, ByVal [Default] As Integer) As Integer
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToInt32(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате UInteger.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение UInteger. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadUInt32(ByVal Section As String, ByVal Key As String) As UInteger
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToUInt32(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате UInteger.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение UInteger. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadUInt32(ByVal Section As String, ByVal Key As String, ByVal [Default] As Integer) As UInteger
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToUInt32(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Long.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение Long. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadInt64(ByVal Section As String, ByVal Key As String) As Long
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToInt64(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Long.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение Long. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadInt64(ByVal Section As String, ByVal Key As String, ByVal [Default] As Long) As Long
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToInt64(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате ULong.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение ULong. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadUInt64(ByVal Section As String, ByVal Key As String) As ULong
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToUInt64(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате ULong.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение ULong. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadUInt64(ByVal Section As String, ByVal Key As String, ByVal [Default] As Long) As ULong
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToUInt64(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Double.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение Double. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadDouble(ByVal Section As String, ByVal Key As String) As Double
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToDouble(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Double.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение Double. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadDouble(ByVal Section As String, ByVal Key As String, ByVal [Default] As Double) As Double
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return 0
            Else
                Return Convert.ToDouble(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Date.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <returns>Значение Date. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadDate(ByVal Section As String, ByVal Key As String) As Date
        Try
            Dim ret = Treatment(ReadString(Section, Key))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return Nothing
            Else
                Return Convert.ToDateTime(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Получить значение ключа в формате Date.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение возвращаемое по умолчанию.</param>
    ''' <returns>Значение Date. Nothing - если ключ пустой/не существует, или преобразование невозможно.</returns>
    Public Overloads Function ReadDate(ByVal Section As String, ByVal Key As String, ByVal [Default] As Date) As Date
        Try
            Dim ret = Treatment(ReadString(Section, Key, Convert.ToString([Default])))
            If ret = "" Then
                GenerateError("Ключ " & Key & " не существует!")
                ' Дань требованию
                Return Nothing
            Else
                Return Convert.ToDateTime(ret)
            End If
        Catch ex As Exception
            GenerateError(ex.Message)
            ' Дань требованию
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Читает значение ключа из секции результат возвращает в ReturnedValue. 
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение по умолчанию.</param>
    ''' <param name="ReturnedValue">Полученное значение ключа.</param>
    ''' <returns>True - если было что-то прочитано.</returns>
    Public Overloads Function ReadString(ByVal Section As String, ByVal Key As String, ByVal [Default] As String, ByRef ReturnedValue$) As Boolean
        Return InternalReadString(Section, Key, [Default], ReturnedValue, EIniAtoms.Key)
    End Function

    ''' <summary>
    ''' Изменить значение ключа в формате строки.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение String.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As String)
        If [Imports].WritePrivateProfileString(Section, Key, Value, p_fullFileName) = 0 Then
            GenerateError()
        End If
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Boolean.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Boolean.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Boolean)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Byte.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Byte.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Byte)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Short.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Short.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Short)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Integer.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Integer.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Integer)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Long.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Long.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Long)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Double.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Double.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Double)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Date.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Date.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Date)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа в формате Object.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Value">Значение Object.</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Key As String, ByVal Value As Object)
        Write(Section, Key, Convert.ToString(Value))
    End Sub

    ''' <summary>
    ''' Изменить значение ключа используя элемент Hashtable.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Values">Hashtable</param>
    Public Overloads Sub Write(ByVal Section As String, ByVal Values As Hashtable)
        For Each de As DictionaryEntry In Values
            Write(Section, de.Key.ToString, de.Value)
        Next
    End Sub

    ''' <summary>
    ''' Создать секцию.
    ''' </summary>
    Public Sub CreateSection(ByVal Section As String)
        If [Imports].WritePrivateProfileSection(Section, "", p_fullFileName) = 0 Then
            GenerateError()
        End If
    End Sub

    ''' <summary>
    ''' Удалить секцию.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    Public Sub EraseSection(ByVal Section As String)
        If [Imports].WritePrivateProfileString(Section, Nothing, Nothing, p_fullFileName) = 0 Then
            GenerateError()
        End If
    End Sub

    ''' <summary>
    ''' Удалить ключ.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    Public Sub DeleteKey(ByVal Section As String, ByVal Key As String)
        If [Imports].WritePrivateProfileString(Section, Key, Nothing, p_fullFileName) = 0 Then
            GenerateError()
        End If
    End Sub

#End Region

#Region " === ПРИВАТНЫЕ МЕТОДЫ === "

    Private Enum EIniAtoms
        Key
        Section
    End Enum

    ''' <summary>
    ''' Получение значения из файла инициализации - нижний уровень.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение по умолчанию.</param>
    ''' <param name="ReturnedValue">Возвращаемое значение.</param>
    ''' <param name="IniAtom">Размер буфера чтения.</param>
    ''' <returns>True - есть возвращаемые данные.</returns>
    Private Function InternalReadString(ByVal Section As String, ByVal Key As String, ByVal [Default] As String, ByRef ReturnedValue As String, ByVal IniAtom As EIniAtoms) As Boolean
        ReturnedValue = New String(CType(vbNullChar, Char), SECTION_BUFFER_SIZE)
        Dim readed As Integer = [Imports].GetPrivateProfileString(Section, Key, [Default], ReturnedValue, GetBufferSize(IniAtom), p_fullFileName)
        If readed > 0 Then
            If ReturnedValue.Chars(readed - 1) = Nothing Then
                ReturnedValue = ReturnedValue.Substring(0, readed - 1)
            Else
                ReturnedValue = ReturnedValue.Substring(0, readed)
            End If
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Получение значения из файла инициализации - верхний уровень.
    ''' </summary>
    ''' <param name="Section">Секция.</param>
    ''' <param name="Key">Ключ.</param>
    ''' <param name="Default">Значение по умолчанию.</param>
    ''' <param name="IniAtom">Размер буфера для чтения.</param>
    ''' <returns>Строка содержащая значение.</returns>
    Private Function InternalReadString(ByVal Section As String, ByVal Key As String, ByVal [Default] As String, ByVal IniAtom As EIniAtoms) As String
        Dim ret As String = String.Empty
        If InternalReadString(Section, Key, [Default], ret, IniAtom) Then
            Return ret
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' Возвращает константу размера буфера.
    ''' </summary>
    ''' <param name="IniAtom"></param>
    ''' <returns>Размер буфера.</returns>
    Private Function GetBufferSize(ByVal IniAtom As EIniAtoms) As Integer
        Select Case IniAtom
            Case EIniAtoms.Key
                Return KEY_BUFFER_SIZE
            Case EIniAtoms.Section
                Return SECTION_BUFFER_SIZE
            Case Else
                Return -1
        End Select
    End Function

    ''' <summary>
    ''' Удаляет всё, что попадает под ограничения
    ''' </summary>
    ''' <param name="slist">Строковый список для проверки на ограничения.</param>
    ''' <returns>Строковый список соответствующий ограничениям.</returns>
    Private Function ConRules(ByVal slist() As String) As List(Of String)
        ' Если slist.ToList выдает ошибку - в ссылках включите в импортированных пространствах имен System.Linq
        Dim ret As List(Of String) = slist.ToList
        For Each str As String In slist
            If Treatment(str) = "" Then
                ret.Remove(str)
            End If
        Next
        Return ret
    End Function

    ''' <summary>
    ''' Обрабатываем строку
    ''' </summary>
    ''' <param name="svalue">Строка для обработки.</param>
    ''' <returns>Обработанная согласно правил строка.</returns>
    Private Function Treatment(ByVal svalue As String) As String
        Dim x As Integer
        If Left(svalue, 1) = "#" Then svalue = ""
        x = svalue.IndexOf("#")
        If x > 0 Then svalue = Mid(svalue, 1, x)
        Return Trim(svalue)
    End Function

    ''' <summary>
    ''' Генерируем исключение.
    ''' </summary>
    Private Sub GenerateError(Optional ByVal err As String = "")
        If err = "" Then
            Throw New Exception(String.Format("Невозможно выполнить оперцию! Внутренний код ошибки [{0}]", [Imports].GetLastError))
        Else
            Throw New Exception(err)
        End If
    End Sub

#End Region

End Class


