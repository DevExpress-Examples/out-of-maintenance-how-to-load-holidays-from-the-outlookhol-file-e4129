Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.Schedule
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Native

Namespace WindowsFormsApplication1
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		' File path to the Outlook.hol file.
		Private OutlookHolidaysFilePath As String = "C:\Program Files\Microsoft Office\Office12\1033\Outlook.hol"

		' Locations for which holidays should be obtained.
		Private Locations() As String = { "United Kingdom", "United States" }


        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles simpleButton1.Click
            ' Check whether the Outlook.hol file exists in the specified file path.
            If File.Exists(OutlookHolidaysFilePath) Then

                ' Load holidays from Outlook.hol for the specified locations.
                LoadHolidays()

                ' Generate appointments for all  holidays.
                GenerateAppointments()
            Else
                XtraMessageBox.Show("Outlook.hol file is not found.")
            End If
        End Sub

		' Load holidays from Outlook.hol for the specified locations.
		Private Sub LoadHolidays()
			Dim loader As New OutlookHolidaysLoader()
			Dim holidays As HolidayBaseCollection = loader.FromFile(OutlookHolidaysFilePath, Locations)
			schedulerControl1.WorkDays.AddRange(holidays)
		End Sub

		' Generate appointments for all  holidays.
		Private Sub GenerateAppointments()
            Dim apts As AppointmentBaseCollection = HolidaysHelper.GenerateHolidayAppointments _
                                                    (schedulerStorage1, schedulerControl1.WorkDays)

			schedulerStorage1.BeginUpdate()
			Try
				schedulerStorage1.Appointments.Items.AddRange(apts)
				XtraMessageBox.Show(String.Format("{0} appointments were added.", apts.Count))
			Finally
				schedulerStorage1.EndUpdate()
			End Try
		End Sub
	End Class
End Namespace
