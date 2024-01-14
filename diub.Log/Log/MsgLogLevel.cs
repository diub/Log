namespace diub.Log;

//
// Je höher der LogLevel, desto weniger Wichtig die Information
//

/// <summary>
/// Einstufung einer Log-Datei Meldung: Regelt die Ausgabe (ob ausgegeben werden muss oder nicht).
/// </summary>
public enum MsgLogLevel {

	/// <summary>
	/// Info-Icon: System-Meldungen die unbedingt ausgegeben werden müsssen, warum auch immer
	/// </summary>
	System = MsgLogResult.Info,
	/// <summary>
	/// für Tick
	/// </summary>
	Ok = MsgLogResult.Success,
	/// <summary>
	/// Ausnahme-Fehler: Meldungen die unbedingt ausgegeben werden müsssen, Fehler des Programms
	/// </summary>
	Exception = MsgLogResult.Exception,
	/// <summary>
	/// Fehler im Programmablauf, Meldungen die unbedingt ausgegeben werden müsssen
	/// </summary>
	Error = MsgLogResult.Error,
	/// <summary>
	/// Fehler durch falsche Daten, zum Beispiel: zu viele Login, falsche Passwörter usw.
	/// </summary>
	Warning = MsgLogResult.Warning,
	/// <summary>
	/// Nur im Debug-Mode. <para></para>Wichtig! <see cref="Verbose"/> ist noch ausführlicher!
	/// </summary>
	Debug = MsgLogResult.Debug,
	/// <summary>
	/// Details für die Ablaufverfolgung
	/// </summary>
	Verbose = MsgLogResult.Verbose,    

}   // class

static public class MapLogLevel {

	static public MsgLogLevel Map (MsgLogResult MsgLogResult) {
		MsgLogLevel msg_log_level;

		if (MsgLogResult <= MsgLogResult.Verbose) {
			msg_log_level = (MsgLogLevel) MsgLogResult;
		} else {
			msg_log_level = MsgLogLevel.Warning;
			//if (MsgLogResult == MsgLogResult.Retry)
			//	msg_log_level = MsgLogLevel.Warning;
		}
		return msg_log_level;
	}

}   // class

// namespace