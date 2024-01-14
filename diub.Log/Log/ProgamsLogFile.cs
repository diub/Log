namespace diub.Log;

/// <summary>
/// Zur einfachen Nutzung siehe <see cref=">Logger"/>.
/// Bietet das Thread-sichere Schreiben und das zyklisches Löschen alter Log-Dateien in einem automatisch festgelegten lokalen Pfad. 
/// </summary>
public partial class ProgramsLogFile {

	/// <summary>
	/// Wird ein Teil des Pfades: User/DEVELOPER_ID/AppName/Log/*
	/// </summary>
	readonly static private string DEVELOPER_ID = "DUMMY";

	readonly private System.Threading.Semaphore logfile_semaphore;


	private string path;
	private string path_filename;
	readonly private string app_or_service_name;
	private StreamWriter stream;
	private bool stream_valid = false;

	private int lines_written = 0;

	private StringBuilder tb_lifetime;
	private StringBuilder otb_lifetime;

	private int cleanup_days_left = 7;

	readonly private string uid;

	private DateTime date;

	/// <summary>
	/// Der Parameter <paramref name="CompanyName"/> legt die erste Verzeichnissebene fest, 
	/// <paramref name="AppOrServiceName"/> die zweite. Beispiel: ".../diub/Documenter/..."
	/// </summary>
	/// <param name="AppOrServiceName">Alpha-numerischer Name ohne Sonderzeichen.</param>
	/// <exception cref="ArgumentException"></exception>
	public ProgramsLogFile (string AppOrServiceName) {
		uid = System.Environment.MachineName.GetHashCode ().ToString ().Trim ('-');
		app_or_service_name = AppOrServiceName;
		try {
			logfile_semaphore = new System.Threading.Semaphore (1, 1);
			if (IsService) {
				path = Environment.GetFolderPath (Environment.SpecialFolder.CommonApplicationData) + "\\" + DEVELOPER_ID + "\\";
			} else {
				// für meine eigene Entwicklungsumgebung
				if (Environment.UserName.Contains ("diub") && Directory.Exists ("O:\\Daten\\Users\\diub"))
					path = "O:\\Daten\\Users\\diub\\Documents\\diub\\";
				else
					path = Environment.GetFolderPath (Environment.SpecialFolder.Personal) + "\\" + DEVELOPER_ID + "\\";
			}
			Directory.CreateDirectory (path);

			path += app_or_service_name + "\\";
			Directory.CreateDirectory (path);

			path += "Logs\\";
			Directory.CreateDirectory (path);

			OpenLogFile ();
		} catch (Exception) {
		}
	}

	public void CloseLogFile () {
		stream_valid = false;
		if (stream == null)
			return;
		Write (MsgLogLevel.System, "LogFile", "Log closed");
		stream.Close ();
		stream = null;
	}

	public void OpenLogFile () {
		int cycle;
		string str;
		DateTime dt;

		tb_lifetime = new StringBuilder ();
		otb_lifetime = new StringBuilder ();
		if (stream != null)
			throw new Exception ("Logfile : tried to open same logfile twice!");

		dt = DateTime.Now;
		cycle = 0;
		// Schleife, Namen ändern  für mehrere Instanzen!!!
		do {
			otb_lifetime.Clear ();
			otb_lifetime.Append (Path.GetDirectoryName (path) + "\\");
			otb_lifetime.Append (app_or_service_name + "_");
			otb_lifetime.Append (uid + "_");
			otb_lifetime.Append (cycle.ToString () + "_");
			otb_lifetime.Append (dt.Date.ToString ("yyyy-MM-dd"));
#if DEBUG
			otb_lifetime.Append ("_" + dt.TimeOfDay.ToString ("hh\\.mm\\.ss"));
#endif
			cycle++;
			otb_lifetime.Append (".log");
			path_filename = otb_lifetime.ToString ();
			try {
				if (!File.Exists (path_filename)) {
					stream = new System.IO.StreamWriter (path_filename, false, System.Text.Encoding.Unicode);
					str = string.Join ("\t", "ProcessId", "Timestamp", nameof (MsgLogLevel), "Modul", "Message_Info");
					stream.WriteLine (str.ToString ());
					stream.Flush ();
				} else {
					stream = new System.IO.StreamWriter (path_filename, true, System.Text.Encoding.Unicode);
				}
			} catch (Exception) { }
		} while (stream == null);
		stream.AutoFlush = true;
		date = dt;
		Write (MsgLogLevel.System, "LogFile", "Log opened");
		Write (MsgLogLevel.System, "Application:", Forms.Application.ProductName + " Version " + Forms.Application.ProductVersion);
		lines_written = 0;
		stream_valid = true;
		CleanUpOldLogfiles (cleanup_days_left);
	}

	/// <summary>
	/// Schliest bei Bedarf die gerade aktive Log-Datei und öffnet eine neue, wenn sich das Datum geändert hat.
	/// </summary>
	public void CycleLogFile () {
		if (date != null && date.Date == DateTime.Now.Date && stream != null)
			return;
		if (stream_valid == false)
			return;
		CloseLogFile ();
		OpenLogFile ();
	}

	/// <summary>
	/// Zum bedingten (nach LogLevel) Schreiben einer Zeile in die Log-Datei.
	/// </summary>
	/// <param name="MsgLogLevel">Stufe der Meldung</param>
	/// <param name="WriteIt">Logischer Ausdruck: True für die Ausgabe</param>
	/// <param name="Module">Angabe des Moduls, das die meldung ausgibt</param>
	/// <param name="Text">Text der Meldung</param>
	public void Write (MsgLogLevel MsgLogLevel, bool WriteIt, string Module, params string [] Text) {
		if (WriteIt)
			Write (MsgLogLevel, Module, Text);
	}

	public void Write (int MsgLogLevel, string Module, params string [] Infos) {
		Write ((MsgLogLevel) MsgLogLevel, Module, Infos);
	}

	/// <summary>
	/// Schreibt eine Zeile in die Log-Datei
	/// </summary>
	/// <param name="MsgLogLevel">Stufe der Meldung</param>
	/// <param name="Module">Angabe des Moduls, das die meldung ausgibt</param>
	/// <param name="Infos">Text der Meldung</param>
	public void Write (MsgLogLevel MsgLogLevel, string Module, params string [] Infos) {
		DateTime dt;

		if (stream == null)
			return;
		CycleLogFile ();
		dt = DateTime.Now;
		logfile_semaphore.WaitOne ();
		tb_lifetime.Clear ();
		tb_lifetime.Append (string.Join ("\t", Process.GetCurrentProcess ().Id.ToString (), dt.ToString ("yyyy-MM-dd HH:mm:ss"), MsgLogLevel.ToString (), Module));
		tb_lifetime.Append ("\t");
		tb_lifetime.Append ("\"");
		tb_lifetime.Append (string.Join (" ", Infos));
		tb_lifetime.Append ("\"");
		stream.WriteLine (tb_lifetime.ToString ());
		stream.Flush ();
		lines_written++;
		logfile_semaphore.Release ();
	}

	public void WriteExeption (string Module, Exception Exception) {
		Exception e = Exception;
		StackTrace trace = new System.Diagnostics.StackTrace (e, true);

		Write (MsgLogLevel.Error, ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>", "");
		Write (MsgLogLevel.Error, "Line: " + trace.GetFrame (0).GetFileLineNumber ());
		Write (MsgLogLevel.Error, "Exception in Module: ", Module);
		Write (MsgLogLevel.Error, "Application:", Forms.Application.ProductName + " Version " + Forms.Application.ProductVersion);
		Write (MsgLogLevel.Error, "Unhandled Execption", e.Message);
		Write (MsgLogLevel.Error, "Source: ", e.Source.ToString ());
		Write (MsgLogLevel.Error, "Target: ", e.TargetSite.ToString ());
		Write (MsgLogLevel.Error, "Stack: ", e.StackTrace.ToString ());
		Write (MsgLogLevel.Error, "Data: ", e.Data.ToString ());
		Write (MsgLogLevel.Error, "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<", "");
	}

	public void Flush () {
		stream.Flush ();
	}

	//
	//
	//

	public string PathFilename {
		get {
			return path_filename;
		}
	}

	[Category ("_diub")]
	[DefaultValue (null)]
	public bool IsService {
		get; set;
	} = false;

}   // class

// namespace