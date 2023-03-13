namespace diub.Log;

//
// Je höher der LogLevel, desto weniger Wichtig die Information
//

/// <summary>
/// Bereich / Einstufung einer Log-Datei Meldung
/// </summary>
public enum MsgLogLevel {

	System = 0,     // Info-Icon: Meldungen die unbedingt ausgegeben werden müsssen, warum auch immer
	Exception = 1,  // Meldungen die unbedingt ausgegeben werden müsssen, Fehler des Programms
	Error = 2,      // Meldungen die unbedingt ausgegeben werden müsssen, Fehler im Programmablauf
	Ok,             // für Tick
	Warning = 4,    // Fehler durch falsche Daten, zum Beispiel: zu viele Login, falsche Passwörter usw.
	Verbose = 5,    // Details für die Ablaufverfolgung
					//#if DEBUG
	Debug = 10      // nur im Debug-Mode ?
					//#endif

}   // class

// namespace