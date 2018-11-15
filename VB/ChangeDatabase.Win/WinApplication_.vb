Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Win
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.ExpressApp
Imports System.Configuration
Imports DevExpress.ExpressApp.Security
Imports ChangeDatabase.Module
Imports DevExpress.Persistent.BaseImpl
Imports ChangeDatabase.Module.Win

Namespace ChangeDatabase.Win
	Partial Public Class ChangeDatabaseWindowsFormsApplication
		Inherits WinApplication
		Implements IApplicationFactory
		Protected Overrides Sub ReadLastLogonParametersCore(ByVal storage As DevExpress.ExpressApp.Utils.SettingsStorage, ByVal logonObject As Object)
			Dim standardLogonParameters As AuthenticationStandardLogonParameters = TryCast(logonObject, AuthenticationStandardLogonParameters)
			If (standardLogonParameters IsNot Nothing) AndAlso String.IsNullOrEmpty(standardLogonParameters.UserName) Then
				MyBase.ReadLastLogonParametersCore(storage, logonObject)
			End If
		End Sub
		Protected Overrides Sub OnLoggingOn(ByVal args As LogonEventArgs)
			MyBase.OnLoggingOn(args)
			ChangeDatabaseHelper.UpdateDatabaseName(Me, (CType(args.LogonParameters, IDatabaseNameParameter)).DatabaseName)
		End Sub
		Protected Overrides Function OnLogonFailed(ByVal logonParameters As Object, ByVal e As Exception) As Boolean
			If WinChangeDatabaseHelper.SkipLogonDialog Then
				Return True
			End If
			Return MyBase.OnLogonFailed(logonParameters, e)
		End Function
		Private Function IApplicationFactory_CreateApplication() As WinApplication Implements IApplicationFactory.CreateApplication
			Return CreateApplication()
		End Function
		Public Shared Function CreateApplication() As ChangeDatabaseWindowsFormsApplication
			Dim winApplication As New ChangeDatabaseWindowsFormsApplication()

			CType(winApplication.Security, SecurityBase).Authentication = New WinChangeDatabaseStandardAuthentication()

			'CType(winApplication.Security, SecurityBase).Authentication = New WinChangeDatabaseActiveDirectoryAuthentication()

			If ConfigurationManager.ConnectionStrings("ConnectionString") IsNot Nothing Then
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			End If
			Return winApplication
		End Function

	End Class
End Namespace