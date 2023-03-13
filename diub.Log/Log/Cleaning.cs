namespace diub.Log;

public partial class ProgramsLogFile {

	/// <summary>
	/// Löscht Log-Dateien die älter als x Tage sind.
	/// </summary>
	/// <param name="OutdatedDaysLeft">Anzahl der Tage zwischen 1 und 30, die eine Log-Datei alt sein muss, damit esie gelöscht wird</param>
	internal bool CleanUpOldLogfiles (int OutdatedDaysLeft = 7) {
		DateTime ts;
		bool cs;

		if (OutdatedDaysLeft < 1)
			OutdatedDaysLeft = 1;
		if (OutdatedDaysLeft > 30)
			OutdatedDaysLeft = 30;
		stream_valid = false;
		ts = DateTime.Now;
		ts = ts.AddDays (-OutdatedDaysLeft);
		Write (MsgLogLevel.System, "Log", "Cleanup initiated ...");
		cs = CleanFiles (ts, new DirectoryInfo (path), app_or_service_name + "_*.log", "UserLog_*.sqlite-journal", "AuditManager_*.log");
		Write (MsgLogLevel.System, "Log", "Cleanup done.");
		stream_valid = true;
		return cs;
	}

	/// <summary>
	/// Listet die Dateien gemäß Filter und löscht diese gegebenenfalls.
	/// </summary>
	/// <param name="TS"></param>
	/// <param name="di"></param>
	/// <param name="Filters"></param>
	/// <returns></returns>
	private bool CleanFiles (DateTime TS, DirectoryInfo di, params string [] Filters) {
		bool cs;
		FileInfo [] files;

		cs = true;
		foreach (string filter in Filters) {
			files = di.GetFiles (filter);
			foreach (FileInfo item in files) {
				if (item.LastAccessTime <= TS) {
					try {
						File.Delete (item.FullName);
						Write (MsgLogLevel.System, "Log", "Logfile successfully removed: " + item.FullName);
					} catch (Exception) {
						Write (MsgLogLevel.System, "Log", "Logfile removing failed: " + item.FullName);
						cs = false;
					}
				}
			}
		}
		return cs;
	}

}   // class

//	namespace	2023-01-16 - 13.51.07