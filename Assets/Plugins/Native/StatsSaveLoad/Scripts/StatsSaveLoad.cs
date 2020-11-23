using System.Runtime.InteropServices;

public class StatsSaveLoad : Singleton<StatsSaveLoad> {

	// Set in the inspector, drag the dll into the Library Object 
	public UnityEngine.Object libraryObject;

	const string SAVE_STATS_SYMBOL = "SaveStatsToFile";
	const string LOAD_STATS_SYMBOL = "LoadStatsFromFile";

	[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
	public delegate void SaveStatsDelegate(string a_path, float a_time, int a_accuracy);

	[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
	public delegate void LoadStatsDelegate(string a_path, out float a_time, out int a_accuracy);
	
	// ReSharper disable InconsistentNaming
	public SaveStatsDelegate SaveStats;
	public LoadStatsDelegate LoadStats;
	// ReSharper restore InconsistentNaming

	DynamicLibrary m_dynamicLibrary;

	protected override void Awake() {
		base.Awake();
		
		// Dynamically load the DLL
		var path = DynamicLibrary.PathFromLibraryObject(libraryObject);

		m_dynamicLibrary = new DynamicLibrary(path);

		SaveStats = m_dynamicLibrary.GetDelegate<SaveStatsDelegate>(SAVE_STATS_SYMBOL);
		LoadStats = m_dynamicLibrary.GetDelegate<LoadStatsDelegate>(LOAD_STATS_SYMBOL);
		
		transform.parent = null;

		DontDestroyOnLoad(this);
	}

	void OnDestroy() {
		m_dynamicLibrary?.Dispose();
	}
}
