﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Этот код создан программой.
'     Исполняемая версия:4.0.30319.42000
'
'     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
'     повторной генерации кода.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'Этот класс создан автоматически классом StronglyTypedResourceBuilder
    'с помощью такого средства, как ResGen или Visual Studio.
    'Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    'с параметром /str или перестройте свой проект VS.
    '''<summary>
    '''  Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("DevCake.WoW.SPP2Launcher.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Перезаписывает свойство CurrentUICulture текущего потока для всех
        '''  обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property apache() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("apache", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Остановка сервера Apache..
        '''</summary>
        Friend ReadOnly Property Apache001_Shutdown() As String
            Get
                Return ResourceManager.GetString("Apache001_Shutdown", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Запуск сервера Apache..
        '''</summary>
        Friend ReadOnly Property Apache002_Start() As String
            Get
                Return ResourceManager.GetString("Apache002_Start", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер Apache успешно запущен..
        '''</summary>
        Friend ReadOnly Property Apache003_Started() As String
            Get
                Return ResourceManager.GetString("Apache003_Started", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер Apache успешно остановлен..
        '''</summary>
        Friend ReadOnly Property Apache004_Stopped() As String
            Get
                Return ResourceManager.GetString("Apache004_Stopped", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Не удалось запустить сервер Apache..
        '''</summary>
        Friend ReadOnly Property Apache005_NotStarted() As String
            Get
                Return ResourceManager.GetString("Apache005_NotStarted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property close() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("close", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Icon, аналогичного (Значок).
        '''</summary>
        Friend ReadOnly Property cmangos_classic() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_classic", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property cmangos_classic_core() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_classic_core", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Icon, аналогичного (Значок).
        '''</summary>
        Friend ReadOnly Property cmangos_orange() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_orange", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Icon, аналогичного (Значок).
        '''</summary>
        Friend ReadOnly Property cmangos_realmd_started() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_realmd_started", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Icon, аналогичного (Значок).
        '''</summary>
        Friend ReadOnly Property cmangos_red() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_red", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Icon, аналогичного (Значок).
        '''</summary>
        Friend ReadOnly Property cmangos_tbc() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_tbc", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property cmangos_tbc_core() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_tbc_core", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Icon, аналогичного (Значок).
        '''</summary>
        Friend ReadOnly Property cmangos_wotlk() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_wotlk", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property cmangos_wotlk_core() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("cmangos_wotlk_core", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Каталог {0} не найден!.
        '''</summary>
        Friend ReadOnly Property E001_DirNotFound() As String
            Get
                Return ResourceManager.GetString("E001_DirNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Single Player Project v2 уже запущен!.
        '''</summary>
        Friend ReadOnly Property E002_AlreadyRunning() As String
            Get
                Return ResourceManager.GetString("E002_AlreadyRunning", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Ошибка!.
        '''</summary>
        Friend ReadOnly Property E003_ErrorCaption() As String
            Get
                Return ResourceManager.GetString("E003_ErrorCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Не найдено ни одного модуля сервера WoW!.
        '''</summary>
        Friend ReadOnly Property E004_ModulesNotFound() As String
            Get
                Return ResourceManager.GetString("E004_ModulesNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Не найден каталог расположения сервера MySQL!.
        '''</summary>
        Friend ReadOnly Property E005_MySqlNotFound() As String
            Get
                Return ResourceManager.GetString("E005_MySqlNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Не найден каталог расположения сервера Apache!.
        '''</summary>
        Friend ReadOnly Property E006_ApacheNotFound() As String
            Get
                Return ResourceManager.GetString("E006_ApacheNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Не найдены исполняемые файлы CMaNGOS.
        '''</summary>
        Friend ReadOnly Property E007_MangoNotFound() As String
            Get
                Return ResourceManager.GetString("E007_MangoNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Неизвестный модуль расширения {0}.
        '''</summary>
        Friend ReadOnly Property E008_UnknownModule() As String
            Get
                Return ResourceManager.GetString("E008_UnknownModule", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на MySQL выдал исключение:.
        '''</summary>
        Friend ReadOnly Property E009_MySqlException() As String
            Get
                Return ResourceManager.GetString("E009_MySqlException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Apache выдал исключение:.
        '''</summary>
        Friend ReadOnly Property E010_ApacheException() As String
            Get
                Return ResourceManager.GetString("E010_ApacheException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Ошибка загрузки основной конфигурации!.
        '''</summary>
        Friend ReadOnly Property E011_ErrorMainConfig() As String
            Get
                Return ResourceManager.GetString("E011_ErrorMainConfig", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Realmd выдал исключение:.
        '''</summary>
        Friend ReadOnly Property E012_RealmdException() As String
            Get
                Return ResourceManager.GetString("E012_RealmdException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Не найден каталог Settings модулей серверов..
        '''</summary>
        Friend ReadOnly Property E013_SettingsNotFound() As String
            Get
                Return ResourceManager.GetString("E013_SettingsNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на World выдал исключение:.
        '''</summary>
        Friend ReadOnly Property E014_WorldException() As String
            Get
                Return ResourceManager.GetString("E014_WorldException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер REALMD внезапно прекратил работу....
        '''</summary>
        Friend ReadOnly Property E015_RealmdCrashed() As String
            Get
                Return ResourceManager.GetString("E015_RealmdCrashed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер WORLD внезапно прекратил работу....
        '''</summary>
        Friend ReadOnly Property E016_WorldCrashed() As String
            Get
                Return ResourceManager.GetString("E016_WorldCrashed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property en() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("en", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property green_ball() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("green_ball", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property info() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("info", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property mysql() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("mysql", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Byte[].
        '''</summary>
        Friend ReadOnly Property notomono_regular() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("notomono_regular", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property open() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("open", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Инициализация базовых настроек приложения выполнена..
        '''</summary>
        Friend ReadOnly Property P001_BaseInit() As String
            Get
                Return ResourceManager.GetString("P001_BaseInit", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Добавлен серверный модуль {0}.
        '''</summary>
        Friend ReadOnly Property P002_ModuleAdded() As String
            Get
                Return ResourceManager.GetString("P002_ModuleAdded", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Файлы exe (*.exe)|*.exe|Все файлы (*.*)|*.*.
        '''</summary>
        Friend ReadOnly Property P003_SetWowClientPath() As String
            Get
                Return ResourceManager.GetString("P003_SetWowClientPath", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Выберите сервер.
        '''</summary>
        Friend ReadOnly Property P004_SelectServer() As String
            Get
                Return ResourceManager.GetString("P004_SelectServer", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Выход из приложения..
        '''</summary>
        Friend ReadOnly Property P005_Exiting() As String
            Get
                Return ResourceManager.GetString("P005_Exiting", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Изменения произойдут после перезагрузки..
        '''</summary>
        Friend ReadOnly Property P006_NeedReboot() As String
            Get
                Return ResourceManager.GetString("P006_NeedReboot", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сообщение.
        '''</summary>
        Friend ReadOnly Property P007_MessageCaption() As String
            Get
                Return ResourceManager.GetString("P007_MessageCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на установлен.
        '''</summary>
        Friend ReadOnly Property P008_Installed() As String
            Get
                Return ResourceManager.GetString("P008_Installed", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на не установлен.
        '''</summary>
        Friend ReadOnly Property P009_NotInstalled() As String
            Get
                Return ResourceManager.GetString("P009_NotInstalled", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Single Player Project v2.
        '''</summary>
        Friend ReadOnly Property P010_LauncherCaption() As String
            Get
                Return ResourceManager.GetString("P010_LauncherCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Настройки лаунчера.
        '''</summary>
        Friend ReadOnly Property P011_LauncherSettingsCaption() As String
            Get
                Return ResourceManager.GetString("P011_LauncherSettingsCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Это первый запуск приложения. Пробегитесь по настройкам....
        '''</summary>
        Friend ReadOnly Property P012_FirstStart() As String
            Get
                Return ResourceManager.GetString("P012_FirstStart", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Включить сайт.
        '''</summary>
        Friend ReadOnly Property P013_SiteAutostart() As String
            Get
                Return ResourceManager.GetString("P013_SiteAutostart", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Автоматический запуск сервера.
        '''</summary>
        Friend ReadOnly Property P014_ServerAutostart() As String
            Get
                Return ResourceManager.GetString("P014_ServerAutostart", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Запуск сервера World.
        '''</summary>
        Friend ReadOnly Property P015_WorldStart() As String
            Get
                Return ResourceManager.GetString("P015_WorldStart", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Настройки MySQL.
        '''</summary>
        Friend ReadOnly Property P016_MySqlSettingsCaption() As String
            Get
                Return ResourceManager.GetString("P016_MySqlSettingsCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер World успешно запущен..
        '''</summary>
        Friend ReadOnly Property P017_WorldStarted() As String
            Get
                Return ResourceManager.GetString("P017_WorldStarted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер World остановлен..
        '''</summary>
        Friend ReadOnly Property P018_WorldStopped() As String
            Get
                Return ResourceManager.GetString("P018_WorldStopped", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Контроль над сервером включен..
        '''</summary>
        Friend ReadOnly Property P019_ControlEnabled() As String
            Get
                Return ResourceManager.GetString("P019_ControlEnabled", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Остановка сервера по требованию..
        '''</summary>
        Friend ReadOnly Property P020_NeedServerStop() As String
            Get
                Return ResourceManager.GetString("P020_NeedServerStop", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Установлен таймер для {0} = {1}.
        '''</summary>
        Friend ReadOnly Property P021_TimerSetted() As String
            Get
                Return ResourceManager.GetString("P021_TimerSetted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на P022_ApacheSettingsCaption.
        '''</summary>
        Friend ReadOnly Property P022_ApacheSettingsCaption() As String
            Get
                Return ResourceManager.GetString("P022_ApacheSettingsCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Информация.
        '''</summary>
        Friend ReadOnly Property P023_InfoCaption() As String
            Get
                Return ResourceManager.GetString("P023_InfoCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Параметры сервера.
        '''</summary>
        Friend ReadOnly Property P024_ServerParametersCaption() As String
            Get
                Return ResourceManager.GetString("P024_ServerParametersCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Повторная загрузка лаунчера после селектора сервера..
        '''</summary>
        Friend ReadOnly Property P025_ReShowLauncher() As String
            Get
                Return ResourceManager.GetString("P025_ReShowLauncher", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Применены настройки сервера {0}.
        '''</summary>
        Friend ReadOnly Property P026_SettingsApplied() As String
            Get
                Return ResourceManager.GetString("P026_SettingsApplied", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Выбран сервер: {0}.
        '''</summary>
        Friend ReadOnly Property P027_ServerSelected() As String
            Get
                Return ResourceManager.GetString("P027_ServerSelected", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Перезагрузка приложения..
        '''</summary>
        Friend ReadOnly Property P028_ApplicationRestart() As String
            Get
                Return ResourceManager.GetString("P028_ApplicationRestart", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Запуск основного приложения..
        '''</summary>
        Friend ReadOnly Property P029_LaunchMain() As String
            Get
                Return ResourceManager.GetString("P029_LaunchMain", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Запуск сервера Realmd.
        '''</summary>
        Friend ReadOnly Property P030_RealmdStart() As String
            Get
                Return ResourceManager.GetString("P030_RealmdStart", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер Realmd успешно запущен..
        '''</summary>
        Friend ReadOnly Property P031_RealmdStarted() As String
            Get
                Return ResourceManager.GetString("P031_RealmdStarted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер Realmd остановлен..
        '''</summary>
        Friend ReadOnly Property P032_RealmdStopped() As String
            Get
                Return ResourceManager.GetString("P032_RealmdStopped", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сработал таймер {0}.
        '''</summary>
        Friend ReadOnly Property P033_TimerTriggered() As String
            Get
                Return ResourceManager.GetString("P033_TimerTriggered", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Попытка запуска сервера {0}.
        '''</summary>
        Friend ReadOnly Property P034_LaunchAttempt() As String
            Get
                Return ResourceManager.GetString("P034_LaunchAttempt", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property red_ball() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("red_ball", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property ru() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("ru", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property server() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("server", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property server_restart() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("server_restart", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property server_start() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("server_start", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property server_stop() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("server_stop", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property settings() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("settings", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property splash() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("splash", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Остановка сервера MySQL..
        '''</summary>
        Friend ReadOnly Property SQL001_Shutdown() As String
            Get
                Return ResourceManager.GetString("SQL001_Shutdown", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Запуск сервера MySQL..
        '''</summary>
        Friend ReadOnly Property SQL002_Start() As String
            Get
                Return ResourceManager.GetString("SQL002_Start", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер MySQL успешно запущен..
        '''</summary>
        Friend ReadOnly Property SQL003_Started() As String
            Get
                Return ResourceManager.GetString("SQL003_Started", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Ищет локализованную строку, похожую на Сервер MySQL успешно остановлен..
        '''</summary>
        Friend ReadOnly Property SQL004_Stopped() As String
            Get
                Return ResourceManager.GetString("SQL004_Stopped", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property tbc() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("tbc", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property tools() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("tools", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property vanilla() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("vanilla", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property warning() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("warning", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Поиск локализованного ресурса типа System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property wotlk() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("wotlk", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace