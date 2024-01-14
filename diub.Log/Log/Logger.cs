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

	//
	//
	//

	static public void Write (MsgLogLevel LogLevel, string Module, params object [] Infos) {
		int i;
		string [] stra;

		stra = new string [Infos.Length];
		for (i = 0; i < Infos.Length; i++)
			stra [i] = Infos [i].ToString ();
		Write (LogLevel, Module, stra);
	}

	/// <summary>
	/// Schreibt die Meldung in das Logfile, wenn: LogLevel <= UserLogLevel (Property)
	/// </summary>
	/// <param name="LogLevel"></param>
	/// <param name="Module"></param>
	/// <param name="Infos"></param>
	static public void Write (MsgLogLevel LogLevel, string Module, params string [] Infos) {
#if !DEBUG
		if ((int) UserLogLevel < (int) LogLevel)
			return;
#endif
		if (logfile == null)
			logfile = new ProgramsLogFile (AppOrServiceName);
		logfile.Write (LogLevel, Module, Infos);
	}

	static public void Write (MsgLogLevel LogLevel, UserLogLevel UserLogLevelHoE, string Module, params object [] Infos) {
		int i;
		string [] stra;

		stra = new string [Infos.Length];
		for (i = 0; i < Infos.Length; i++)
			stra [i] = Infos [i].ToString ();
		logfile.Write (LogLevel, Logger.UserLogLevel >= UserLogLevelHoE, Module, stra);
	}


	/// <summary>
	/// Schreibt die Meldung in das Logfile, wenn: LogLevel <= UserLogLevel (Parameter)
	/// </summary>
	/// <param name="LogLevel"></param>
	/// <param name="UserLogLevelHoE"></param>
	/// <param name="Module"></param>
	/// <param name="Infos"></param>
	static public void Write (MsgLogLevel LogLevel, UserLogLevel UserLogLevelHoE, string Module, params string [] Infos) {
		logfile.Write (LogLevel, Logger.UserLogLevel >= UserLogLevelHoE, Module, Infos);
	}

	/// <summary>
	/// Schreibt die Meldung in das Logfile, wenn: WriteIt == true
	/// </summary>
	/// <param name="LogLevel"></param>
	/// <param name="WriteIt"></param>
	/// <param name="Module"></param>
	/// <param name="Infos"></param>
	static public void Write (MsgLogLevel LogLevel, bool WriteIt, string Module, params string [] Infos) {
		if (logfile == null)
			logfile = new ProgramsLogFile (AppOrServiceName);
		logfile.Write (LogLevel, WriteIt, Module, Infos);
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

	static private UserLogLevel userloglevel = UserLogLevel.Verbose;
	[DefaultValue (UserLogLevel.Verbose)]
	static public UserLogLevel UserLogLevel {
		get {
			return userloglevel;
		}
		set {
			userloglevel = value;
			Logger.Write (MsgLogLevel.System, nameof (Logger), "User-Log level changed to: ", userloglevel);
		}
	} 

	static public string AppOrServiceName {
		set; get;
	} = Forms.Application.ProductName;


}   // class

// namespace
