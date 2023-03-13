namespace diub.Log;

//
// Je höher der LogLevel, desto weniger Wichtig die Information
//

/// <summary>
/// Benutzer-Level der Meldungen, die in ein Log geschrieben werden sollen
/// </summary>
public enum UserLogLevel {

	Error = 3,      // Meldungen die unbedingt ausgegeben werden müsssen, Fehler im Programmablauf
	Warning = 4,    // Fehler durch falsche Daten, zum Beispiel: zu viele Login, falsche Passwörter usw.
	Verbose = 5,    // Mehr Detail-Informationen
	Debug = 10      // Alles was es gibt, zB FogMirror: jede erfasste Datei, Vergleichsergebnisse ...

}   // class

// namespace