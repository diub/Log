namespace diub.Log;

/// <summary>
/// Zum Schreiben einer Log-Datei ohne weitere Angaben
/// Der gewählte Pfad ist abhängig von <see cref="ProgramsLogFile.IsService"/>!
/// true: SystemDrive/ProgramData/diub/AppName/Logs
/// false: UserData/Documents/diub/AppName/Logs
/// </summary>
static public class Logger {

	static public ProgramsLogFile logfile = null;

	static public ProgramsLogFile Logfile {
		get {
			return logfile;
		}
	}

	/// <summary>
	/// Schreibt die Meldung in das Logfile, wenn: LogLevel <= UserLogLevel (Property)
	/// </summary>
	/// <param name="LogLevel"></param>
	/// <param name="Module"></param>
	/// <param name="Text"></param>
	static public void Write (MsgLogLevel LogLevel, string Module, string Text) {
		if ((int) UserLogLevel < (int) LogLevel)
			return;
		if (logfile == null)
			logfile = new ProgramsLogFile (AppOrServiceName);
		logfile.Write (LogLevel, Module, Text);
	}

	/// <summary>
	/// Schreibt die Meldung in das Logfile, wenn: LogLevel <= UserLogLevel (Parameter)
	/// </summary>
	/// <param name="LogLevel"></param>
	/// <param name="UserLogLevelHoE"></param>
	/// <param name="Module"></param>
	/// <param name="Text"></param>
	static public void Write (MsgLogLevel LogLevel, UserLogLevel UserLogLevelHoE, string Module, string Text) {
		logfile.Write (LogLevel, Logger.UserLogLevel >= UserLogLevelHoE, Module, Text);
	}

	/// <summary>
	/// Schreibt die Meldung in das Logfile, wenn: WriteIt == true
	/// </summary>
	/// <param name="LogLevel"></param>
	/// <param name="WriteIt"></param>
	/// <param name="Module"></param>
	/// <param name="Text"></param>
	static public void Write (MsgLogLevel LogLevel, bool WriteIt, string Module, string Text) {
		if (logfile == null)
			logfile = new ProgramsLogFile (AppOrServiceName);
		logfile.Write (LogLevel, WriteIt, Module, Text);
	}

	/// <summary>
	/// Schreibt Details einer Ausnahme in das LogFile
	/// </summary>
	/// <param name="Module"></param>
	/// <param name="Exception"></param>
	static public void WriteExeption (string Module, Exception Exception) {
		if (logfile == null)
			logfile = new ProgramsLogFile (Forms.Application.ProductName);
		logfile.WriteExeption (Module, Exception);
	}


	static public void Flush () {
		if (logfile == null)
			return;
		logfile.Flush ();
	}

	/// <summary>
	/// Schliest expliziet die Datei
	/// </summary>
	static public void CloseLogFile () {
		if (logfile == null)
			return;
		logfile.CloseLogFile ();
		logfile = null;
	}

	/// <summary>
	/// Löscht Log-Dateien, die älter als die in OutdatedDaysLeft (Property) angegebenen Tage sind
	/// </summary>
	static public void CleanUpOutdatedLogs (int OutdatedDaysLeft = 7) {
		if (logfile == null)
			logfile = new ProgramsLogFile (Forms.Application.ProductName);
		logfile.CleanUpOldLogfiles (OutdatedDaysLeft);
	}

	//
	// Properties
	//

	[DefaultValue (UserLogLevel.Verbose)]
	static public UserLogLevel UserLogLevel { get; set; } = UserLogLevel.Verbose;

	static public string AppOrServiceName {
		set; get;
	} = Forms.Application.ProductName;


}   // class

// namespace
