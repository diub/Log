namespace diub.Log;

//
// Je höher der LogLevel, desto weniger Wichtig die Information
//

/// <summary>
/// Benutzer-Level der Meldungen, die in ein Log geschrieben werden sollen
/// </summary>
public enum UserLogLevel {

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

// namespace