using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Schedule;
using DevExpress.XtraScheduler;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            schedulerControl1.Start = DateTime.Today;
        }

        // File path to the Outlook.hol file.
        private string OutlookHolidaysFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + 
            @"\Microsoft Office 2013\Office15\1033\Outlook.hol";

        // Locations for which holidays should be obtained.
        private string[] Locations = { "United Kingdom", "United States" };


        private void btnFromOutlook_Click(object sender, EventArgs e) {
            // Check whether the Outlook.hol file exists in the specified file path.
            if (File.Exists(OutlookHolidaysFilePath)) {
                // Load holidays from Outlook.hol for the specified locations.
                LoadHolidaysFromOutlook();
                // Generate appointments for all  holidays.
                GenerateAppointments();
            } else {
                XtraMessageBox.Show("Outlook.hol file is not found.");
            }
        }

        // Load holidays from Outlook.hol for the specified locations.
        private void LoadHolidaysFromOutlook()
        {
            #region #holidaysfromoutlook
            OutlookHolidaysLoader loader = new OutlookHolidaysLoader();
            HolidayBaseCollection holidays = loader.FromFile(OutlookHolidaysFilePath, Locations);
            schedulerControl1.WorkDays.AddRange(holidays);
            #endregion #holidaysfromoutlook
        }

        private void btnFromXml_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImportHolidaysFromXmlFile(openFileDialog1.FileName);
                GenerateAppointments();
            }
        }

        private void ImportHolidaysFromXmlFile(string path)
        {
            #region #holidaysfromxml
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(path);
            HolidayBaseCollection holidays = DevExpress.Schedule.Serializing.HolidayCollectionXmlPersistenceHelper.ObjectFromXml(doc.OuterXml);
            var holidaysRussian = from holiday in holidays
                                                    where holiday.Location == "Russia"
                                                    select holiday;
            schedulerControl1.WorkDays.AddRange(holidaysRussian.ToList<Holiday>());
            #endregion #holidaysfromxml
        }

        // Generate appointments for all  holidays.
        private void GenerateAppointments() {
            AppointmentBaseCollection apts = DevExpress.XtraScheduler.Native.HolidaysHelper.GenerateHolidayAppointments(
                schedulerStorage1, schedulerControl1.WorkDays);

            schedulerStorage1.BeginUpdate();
            try {
                schedulerStorage1.Appointments.Items.AddRange(apts);
                XtraMessageBox.Show(String.Format("{0} appointments were added.", apts.Count));
            } finally {
                schedulerStorage1.EndUpdate();
            }
        }


    }
}
