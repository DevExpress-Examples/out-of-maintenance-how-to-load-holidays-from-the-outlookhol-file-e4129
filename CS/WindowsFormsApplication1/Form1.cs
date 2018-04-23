using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Schedule;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Native;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        // File path to the Outlook.hol file.
        private string OutlookHolidaysFilePath =
            @"C:\Program Files\Microsoft Office\Office12\1033\Outlook.hol";

        // Locations for which holidays should be obtained.
        private string[] Locations = { "United Kingdom", "United States" };


        private void simpleButton1_Click(object sender, EventArgs e) {
            // Check whether the Outlook.hol file exists in the specified file path.
            if (File.Exists(OutlookHolidaysFilePath)) {

                // Load holidays from Outlook.hol for the specified locations.
                LoadHolidays();

                // Generate appointments for all  holidays.
                GenerateAppointments();
            } else {
                XtraMessageBox.Show("Outlook.hol file is not found.");
            }
        }

        // Load holidays from Outlook.hol for the specified locations.
        private void LoadHolidays() {
            OutlookHolidaysLoader loader = new OutlookHolidaysLoader();
            HolidayBaseCollection holidays = loader.FromFile(OutlookHolidaysFilePath,
                Locations);
            schedulerControl1.WorkDays.AddRange(holidays);
        }

        // Generate appointments for all  holidays.
        private void GenerateAppointments() {
            AppointmentBaseCollection apts = HolidaysHelper.GenerateHolidayAppointments(
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
