namespace diub.Log;

/// <summary>
/// Übersteuert das Icon; liefert einen erweiterten Status.
/// </summary>
public enum MsgLogResult {

	/// <summary>
	/// Undefinierter Zustand.
	/// </summary>
	Undefined = -1, // keine Aussage möglich
	/// <summary>
	/// Info-Icon: System-Meldungen die unbedingt ausgegeben werden müsssen, warum auch immer
	/// </summary>	
	Info = 0,

	/// <summary>
	/// Erfolg.
	/// </summary>
	Success = 1,

	/// <summary>
	/// Ausnahme-Fehler: Meldungen die unbedingt ausgegeben werden müsssen, Fehler des Programms
	/// </summary>
	Exception = 2,
	/// <summary>
	/// Fehler im Programmablauf, Meldungen die unbedingt ausgegeben werden müsssen
	/// </summary>
	Error = 3,
	Failed = Error,

	/// <summary>
	/// Fehler durch falsche Daten, zum Beispiel: zu viele Login, falsche Passwörter usw.
	/// </summary>
	Warning = 5,
	/// <summary>
	/// Nur im Debug-Mode. <para></para>Wichtig! <see cref="Verbose"/> ist noch ausführlicher!
	/// </summary>
	Debug = 6,
	/// <summary>
	/// Details für die Ablaufverfolgung
	/// </summary>
	Verbose = 7,

	Retry = 10,
	Skipped = 11,
	Unknown


}   // class


// namespace	12.12.2016 - 14.26

