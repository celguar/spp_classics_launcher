
'########################################################
'# (c) RafStudio inc. 2022
'# Не забываем про Upgrade при запуске приложения
'# и Save перед выходом их приложения.
'########################################################

Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Reflection
Imports System.Xml
Imports System.Xml.XPath

''' <summary>
''' Обеспечивает переносимые, постоянные настройки приложения.
''' </summary>
Public Class SPP2SettingsProvider
    Inherits SettingsProvider
    Implements IApplicationSettingsProvider

#Region " === СВОЙСТВА === "

    ''' <summary>
    ''' Только для чтения: Возвращает полный путь к файлу конфигурации.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property SettingsFile As String

    ''' <summary>
    ''' Только для чтения: Возвращает путь к каталогу к файла конфигурации.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property SettingsFolder As String

    ''' <summary>
    ''' Возвращает имя приложения.
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property ApplicationName As String
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
        End Get
        Set(ByVal value As String)
        End Set
    End Property

    ''' <summary>
    ''' Только для чтения: Возвращает понятное имя поставщика конфигурации настроек.
    ''' </summary>
    ''' <returns></returns>
    Public Overrides ReadOnly Property Name() As String
        Get
            Return "PortableSettingsProvider"
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Возвращает xml документ.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetXmlDoc() As XDocument
        Dim xmlDoc As XDocument = Nothing
        Dim xmlElement As XElement
        Dim initnew As Boolean = False

        If IO.File.Exists(SettingsFile) Then
            Try
                xmlDoc = XDocument.Load(SettingsFile)
            Catch
                initnew = True
            End Try
        Else
            initnew = True
        End If
        If initnew Then
            Return CreateDocument()
        Else
            ' Проверяем загруженный документ
            xmlElement = xmlDoc.Element("configuration")
            If IsNothing(xmlElement) Then
                My.Settings.Reset()
                Return CreateDocument()
            End If

            xmlElement = xmlDoc.Element("configuration").Element("userSettings")
            If IsNothing(xmlElement) Then
                My.Settings.Reset()
                Return CreateDocument()
            End If

            xmlElement = xmlDoc.Element("configuration").Element("userSettings").Element("Roaming")
            If IsNothing(xmlElement) Then
                My.Settings.Reset()
                Return CreateDocument()
            End If

            xmlElement = xmlDoc.Element("configuration").Element("appSettings")
            If IsNothing(xmlElement) Then
                My.Settings.Reset()
                Return CreateDocument()
            End If

            Return xmlDoc
        End If
    End Function

    ''' <summary>
    ''' Возвращает пустой документ с предустановленными элементами.
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateDocument() As XDocument
        ' Создаём новый XML документ
        Dim version As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        Return New XDocument(New XElement("configuration", New XElement("userSettings", New XElement("Roaming")), New XElement("appSettings")))
    End Function

    ''' <summary>
    ''' Инициализатор поставщика конфигурации настроек.
    ''' </summary>
    ''' <param name="name">Понятное имя поставщика конфигурации настроек.</param>
    ''' <param name="config">Коллекция связанных ключей и значений.</param>
    Public Overrides Sub Initialize(name As String, config As NameValueCollection)
        If String.IsNullOrEmpty(name) Then name = "PortableSettingsProvider"
        MyBase.Initialize(name, config)
    End Sub

    ''' <summary>
    ''' Применяет этот поставщик параметров к каждому свойству имеющихся параметров.
    ''' </summary>
    ''' <param name="configFilePath">Полный путь к файлу конфигурации.</param>
    ''' <param name="enableMyDocuments">При недоступности локального каталога разрешить сохранять файл конфигурации в папке Мои Документы?</param>
    ''' <param name="settingsList">Массив настроек.</param>
    Public Shared Sub ApplyProvider(configFilePath As String,
                                    enableMyDocuments As Boolean,
                                    ParamArray settingsList() As ApplicationSettingsBase)
        SetLanguage("ru-RU")
        Try
            ' Проверяем доступность каталога на запись
            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(configFilePath)) Then IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(configFilePath))
            _SettingsFolder = IO.Path.GetDirectoryName(configFilePath)
            IO.File.WriteAllText(IO.Path.GetDirectoryName(configFilePath) & "\test.file", "")
            IO.File.Delete(IO.Path.GetDirectoryName(configFilePath) & "\test.file")
            _SettingsFile = configFilePath
        Catch ex As Exception
            If enableMyDocuments Then
                ' Указанный каталог недоступен - сохраняем параметры в MyDocuments
                _SettingsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & Assembly.GetExecutingAssembly.FullName.Split(","c)(0)
                If Not IO.Directory.Exists(_SettingsFolder) Then IO.Directory.CreateDirectory(_SettingsFolder)
                If Not IO.Directory.Exists(_SettingsFolder & "\Settings") Then IO.Directory.CreateDirectory(_SettingsFolder & "\Settings")
                _SettingsFile = _SettingsFolder & "\Settings\" & IO.Path.GetFileName(configFilePath)
            Else
                Throw New Exception(ex.Message)
            End If
        End Try

        For Each settings In settingsList
            Dim provider = New SPP2SettingsProvider()
            settings.Providers.Add(provider)
            For Each prop As SettingsProperty In settings.Properties
                prop.Provider = provider
            Next prop
            settings.Reload()
        Next settings
    End Sub

    ''' <summary>
    ''' Сохраняет настройки используя объект settings в качестве контента.
    ''' При ошибке возвращает True.
    ''' </summary>
    ''' <param name="settings">Контент новых настроек.</param>
    Public Shared Function ReplaceSettings(settings As String, ByRef _err As String) As Boolean
        Dim xmlDoc As XDocument
        Try
            xmlDoc = XDocument.Parse(settings)
            Try
                ' Специальные символы, такие как '\ r \ n', сохраняем заменив их объектами char.
                Using writer = XmlWriter.Create(SettingsFile, New XmlWriterSettings With {.NewLineHandling = NewLineHandling.Entitize, .Indent = True})
                    xmlDoc.Save(writer)
                End Using
                _err = _lng.spr0000
                Return False
            Catch
                _err = _lng.spr0001
                Return True
            End Try
        Catch ex As Exception
            _err = _lng.spr0002
            Return True
        End Try
    End Function

    ''' <summary>
    ''' Возвращает предыдущую версию настроек. Не реализовано!
    ''' </summary>
    ''' <param name="context">Контекстная информация о параметрах.</param>
    ''' <param name="prop">Объект представляющий метаданные об отдельном свойстве конфигурации.</param>
    ''' <returns></returns>
    Public Function GetPreviousVersion(ByVal context As SettingsContext, ByVal prop As SettingsProperty) As SettingsPropertyValue Implements IApplicationSettingsProvider.GetPreviousVersion
        Throw New NotImplementedException()
    End Function

    ''' <summary>
    ''' Возвращает установки, связанные с указанным приложением к значениям по умолчанию.
    ''' </summary>
    ''' <param name="context">Контекстная информация о параметрах.</param>
    Public Sub Reset(ByVal context As SettingsContext) Implements IApplicationSettingsProvider.Reset
        If IO.File.Exists(SettingsFile) Then
            IO.File.Delete(SettingsFile)
        End If
    End Sub

    ''' <summary>
    ''' Обновляет версию коллекции значений свойства параметров. Не реализовано!
    ''' </summary>
    ''' <param name="context">Контекстная информация о параметрах.</param>
    ''' <param name="collection">Коллекция объектов свойств конфигурации.</param>
    Public Sub Upgrade(ByVal context As SettingsContext, ByVal collection As SettingsPropertyCollection) Implements IApplicationSettingsProvider.Upgrade
        Dim values = GetPropertyValues(context, collection)
        Dim xmlDoc As XDocument = CreateDocument()

        For Each value As SettingsPropertyValue In values
            SetXmlValue(xmlDoc, value)
        Next value
        Try
            ' Специальные символы, такие как '\ r \ n', сохраняем заменив их объектами char.
            Using writer = XmlWriter.Create(SettingsFile, New XmlWriterSettings With {.NewLineHandling = NewLineHandling.Entitize, .Indent = True})
                xmlDoc.Save(writer)
            End Using
        Catch ex As Exception
            Throw New Exception("Файл конфигурации недоступен для изменения!")
        End Try
    End Sub

    ''' <summary>
    ''' Возвращает коллекцию значений свойства параметров.
    ''' </summary>
    ''' <param name="context">Контекстная информация о параметрах.</param>
    ''' <param name="collection">Коллекция объектов свойств конфигурации.</param>
    ''' <returns></returns>
    Public Overrides Function GetPropertyValues(ByVal context As SettingsContext, ByVal collection As SettingsPropertyCollection) As SettingsPropertyValueCollection
        Dim xmlDoc As XDocument = GetXmlDoc()
        Dim values As New SettingsPropertyValueCollection()
        ' Перебираем настройки
        For Each setting As SettingsProperty In collection
            Dim value As New SettingsPropertyValue(setting) With {.IsDirty = False, .SerializedValue = GetXmlValue(xmlDoc, setting)}
            values.Add(value)
        Next setting
        Return values
    End Function

    ''' <summary>
    ''' Собирает коллекцию значений свойства параметров и сохраняет в файл.
    ''' </summary>
    ''' <param name="context">Контекстная информация о параметрах.</param>
    ''' <param name="collection">Коллекция объектов свойств конфигурации.</param>
    Public Overrides Sub SetPropertyValues(ByVal context As SettingsContext, ByVal collection As SettingsPropertyValueCollection)
        Dim xmlDoc As XDocument = GetXmlDoc()
        For Each value As SettingsPropertyValue In collection
            SetXmlValue(xmlDoc, value)
        Next value
        Try
            ' Специальные символы, такие как '\ r \ n', сохраняем заменив их объектами char.
            Using writer = XmlWriter.Create(SettingsFile, New XmlWriterSettings With {.NewLineHandling = NewLineHandling.Entitize, .Indent = True})
                xmlDoc.Save(writer)
            End Using
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Получает значение параметра из настроек с учётом параметра Roaming.
    ''' </summary>
    ''' <param name="xmlDoc">XML документ.</param>
    ''' <param name="prop">Объект представляющий метаданные об отдельном свойстве конфигурации.</param>
    ''' <returns></returns>
    Private Function GetXmlValue(ByVal xmlDoc As XDocument, ByVal prop As SettingsProperty) As Object
        Dim xmlSettings As XElement
        ' Определяем расположение свойства настроек
        If Not IsUserScoped(prop) Then
            xmlSettings = xmlDoc.Element("configuration").Element("appSettings")
        Else
            xmlSettings = xmlDoc.Element("configuration").Element("userSettings")
            If IsRoaming(prop) Then
                xmlSettings = xmlSettings.Element("Roaming")
            Else
                xmlSettings = xmlSettings.Element("PC_" & Environment.MachineName)
            End If
        End If
        Dim propElement = If(IsNothing(xmlSettings), Nothing, xmlSettings.XPathSelectElement(String.Format("setting[@name='{0}']", prop.Name)))
        Return If(IsNothing(propElement), prop.DefaultValue, propElement.Value)
    End Function

    ''' <summary>
    ''' Устанавливает значение параметра в зависимости от параметра Roaming.
    ''' </summary>
    ''' <param name="xmlDoc">XML документ.</param>
    ''' <param name="value">Объект представляющий метаданные об отдельном свойстве конфигурации.</param>
    Private Sub SetXmlValue(ByVal xmlDoc As XDocument, ByVal value As SettingsPropertyValue)
        Dim xmlSettings As XElement

        ' Определяем расположение свойства настроек
        If Not IsUserScoped(value.Property) Then
            xmlSettings = xmlDoc.Element("configuration").Element("appSettings")
        Else
            If IsRoaming(value.Property) Then
                xmlSettings = xmlDoc.Element("configuration").Element("userSettings").Element("Roaming")
            Else
                If IsNothing(xmlDoc.Element("configuration").Element("userSettings").Element("PC_" & Environment.MachineName)) Then
                    xmlDoc.Element("configuration").Element("userSettings").Add(New XElement("PC_" & Environment.MachineName))
                End If
                xmlSettings = xmlDoc.Element("configuration").Element("userSettings").Element("PC_" & Environment.MachineName)
            End If
        End If

        ' Устанавливаем значение
        Dim propElement As XElement = xmlSettings.XPathSelectElement(String.Format("setting[@name='{0}']", value.Name))
        If propElement IsNot Nothing Then
            propElement.SetAttributeValue("description", GetDescription(value.Property).Replace(Chr(34), ""))
            propElement.SetElementValue("value", value.SerializedValue)
        Else
            xmlSettings.Add(New XElement("setting",
                            New XAttribute("name", value.Name),
                            New XAttribute("description", GetDescription(value.Property).Replace(Chr(34), "")),
                            New XElement("value", value.SerializedValue)))
        End If

    End Sub

    ''' <summary>
    ''' Выясняем область действия атрибута. Если пользовательский - True.
    ''' </summary>
    ''' <param name="prop">Объект представляющий метаданные об отдельном свойстве конфигурации.</param>
    ''' <returns></returns>
    Private Function IsUserScoped(ByVal prop As SettingsProperty) As Boolean
        Return TypeOf prop.Attributes(GetType(UserScopedSettingAttribute)) Is UserScopedSettingAttribute
    End Function

    ''' <summary>
    ''' Выясняем облать действия атрибута. Если перемещаемый - True.
    ''' </summary>
    ''' <param name="prop">Объект представляющий метаданные об отдельном свойстве конфигурации.</param>
    ''' <returns></returns>
    Private Function IsRoaming(ByVal prop As SettingsProperty) As Boolean
        Return TypeOf prop.Attributes(GetType(SettingsManageabilityAttribute)) Is SettingsManageabilityAttribute
    End Function

    ''' <summary>
    ''' Возвращает значение атрибута Description.
    ''' </summary>
    ''' <param name="prop">Объект представляющий метаданные об отдельном свойстве конфигурации.</param>
    ''' <returns></returns>
    Private Function GetDescription(ByVal prop As SettingsProperty) As String
        Dim att = CType(prop.Attributes(GetType(SettingsDescriptionAttribute)), SettingsDescriptionAttribute)
        Return If(IsNothing(att), "", att.Description)
    End Function

#Region " === ЛОКАЛИЗАЦИЯ === "

    ''' <summary>
    ''' Структура содержащая текущую локализацию.
    ''' </summary>
    Private Shared _lng As TLanguage

#Region " === БАЗОВАЯ СТРУКТУРА СТРОК ЛОКАЛИЗАЦИИ === "

    ''' <summary>
    ''' Структура содержащая строки локализации класса SettingsProvider.
    ''' </summary>
    Private Structure TLanguage

        ''' <summary>
        ''' Короткое обозначение языка локализации.
        ''' </summary>
        Public Language As String

        ''' <summary>
        ''' Автор локализации.
        ''' </summary>
        Public Author As String

        ''' <summary>
        ''' Успешное применение полученной конфигурации.
        ''' </summary>
        Public spr0000 As String

        ''' <summary>
        ''' Недопустимая структура строки конфигурации.
        ''' </summary>
        Public spr0001 As String

        ''' <summary>
        ''' Исключение при сохранении файла конфигурации.
        ''' </summary>
        Public spr0002 As String

    End Structure

#End Region

    ''' <summary>
    ''' Возвращает текущую локализацию.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property CurrentLanguage As String
        Get
            Return _lng.Language
        End Get
    End Property

    ''' <summary>
    ''' Автор локализации.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property LocalizationAuthor As String
        Get
            Return _lng.Author
        End Get
    End Property

    ''' <summary>
    ''' Возвращает локализованную коллекцию строковых ключей и их значений.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property LocalizedPairs As Dictionary(Of String, String)
        Get
            Dim _lng As New Dictionary(Of String, String)

            Return _lng
        End Get
    End Property

    ''' <summary>
    ''' Устанавливает локализацию для класса EasyLog из встроенных в класс (RU, EN).
    ''' </summary>
    ''' <param name="language">Доступная локализация из коробки. В данном случае RU или EN.</param>
    Public Shared Sub SetLanguage(ByVal language As String)
        If IsNothing(language) Then language = "ru-RU"
        Select Case UCase(language)
            Case "RU-RU"
                ' Локализация локализации :)
                _lng.Language = "ru-RU"
                _lng.Author = "© 2022 RafStudio inc."
                _lng.spr0000 = "Успешное применение полученной конфигурации."
                _lng.spr0001 = "Недопустимая структура строки конфигурации."
                _lng.spr0002 = "Исключение при сохранении файла конфигурации."
            Case Else
                ' Локализация локализации :)
                _lng.Language = "en-GB"
                _lng.Author = "© 2022 RafStudio inc."
                _lng.spr0000 = "Successful application of the received configuration."
                _lng.spr0001 = "Invalid configuration string structure."
                _lng.spr0002 = "Exception while saving configuration file."
        End Select

    End Sub

#End Region

End Class
