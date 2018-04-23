# How to load holidays from the Outlook.hol file


<p>This example demonstrates how to load holidays from the <em>Outlook.hol</em> file or from an XML file to the scheduler's <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraSchedulerWorkDaysCollectiontopic"><u>WorkDaysCollection</u></a> collection and generate all-day appointments for these holidays.</p>
<p>For example, if you have installed Microsoft Office 2007, by default this file is located in the following path: <em>C:\Program Files\Microsoft Office\Office12\1033\Outlook.hol</em></p>
<p>1. Use the <strong>FromFile</strong> method of the supplementary <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressScheduleOutlookHolidaysLoadertopic"><u>OutlookHolidaysLoader</u></a> class to obtain the collection of holidays for the specified locations.</p>
<p>2. Add obtained holidays to the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerSchedulerControl_WorkDaystopic"><u>SchedulerControl.WorkDays</u></a> collection.</p>
<p>3. Use the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerNativeHolidaysHelper_GenerateHolidayAppointmentstopic"><u>HolidaysHelper.GenerateHolidayAppointments</u></a> method to create appointments for all loaded holidays.</p>

<br/>


