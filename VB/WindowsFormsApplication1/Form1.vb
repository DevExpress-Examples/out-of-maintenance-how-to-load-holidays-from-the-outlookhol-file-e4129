Imports System
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.Schedule
Imports DevExpress.XtraScheduler

Namespace WindowsFormsApplication1
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            schedulerControl1.Start = Date.Today
        End Sub

        ' File path to the Outlook.hol file.
        Private OutlookHolidaysFilePath As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Office 2013\Office15\1033\Outlook.hol"

        ' Locations for which holidays should be obtained.
        Private Locations() As String = { "United Kingdom", "United States" }


        Private Sub btnFromOutlook_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFromOutlook.Click
            ' Check whether the Outlook.hol file exists in the specified file path.
            If File.Exists(OutlookHolidaysFilePath) Then
                ' Load holidays from Outlook.hol for the specified locations.
                LoadHolidaysFromOutlook()
                ' Generate appointments for all  holidays.
                GenerateAppointments()
            Else
                XtraMessageBox.Show("Outlook.hol file is not found.")
            End If
        End Sub

        ' Load holidays from Outlook.hol for the specified locations.
        Private Sub LoadHolidaysFromOutlook()
'            #Region "#holidaysfromoutlook"
            Dim loader As New OutlookHolidaysLoader()
            Dim holidays As HolidayBaseCollection = loader.FromFile(OutlookHolidaysFilePath, Locations)
            schedulerControl1.WorkDays.AddRange(holidays)
'            #End Region ' #holidaysfromoutlook
        End Sub

        Private Sub btnFromXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton2.Click
            Dim openFileDialog1 As New OpenFileDialog()
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
            If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                ImportHolidaysFromXmlFile(openFileDialog1.FileName)
                GenerateAppointments()
            End If
        End Sub

        Private Sub ImportHolidaysFromXmlFile(ByVal path As String)
'            #Region "#holidaysfromxml"
            Dim doc As New System.Xml.XmlDocument()
            doc.Load(path)
            Dim holidays As HolidayBaseCollection = DevExpress.Schedule.Serializing.HolidayCollectionXmlPersistenceHelper.ObjectFromXml(doc.OuterXml)
            Dim holidaysRussian = From holiday In holidays _
                                  Where holiday.Location = "Russia" _
                                  Select holiday
            schedulerControl1.WorkDays.AddRange(holidaysRussian.ToList())
'            #End Region ' #holidaysfromxml
        End Sub

        ' Generate appointments for all  holidays.
        Private Sub GenerateAppointments()
            Dim apts As AppointmentBaseCollection = DevExpress.XtraScheduler.Native.HolidaysHelper.GenerateHolidayAppointments(schedulerStorage1, schedulerControl1.WorkDays)

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
