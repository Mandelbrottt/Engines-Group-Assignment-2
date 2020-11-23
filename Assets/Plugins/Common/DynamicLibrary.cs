using System;
using System.Runtime.InteropServices;
using UnityEngine;

internal class DynamicLibrary : IDisposable {
	[DllImport("kernel32", EntryPoint = "LoadLibrary", SetLastError = true)]
	static extern IntPtr _LoadLibraryWin32(string a_path);

	[DllImport("kernel32", EntryPoint = "GetProcAddress", SetLastError = true)]
	static extern IntPtr _GetProcAddressWin32(IntPtr a_libHandle, string a_symbolName);

	[DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
	static extern bool _FreeLibraryWin32(IntPtr a_libHandle);

	IntPtr m_libHandle = IntPtr.Zero;

	public DynamicLibrary(string a_path) {
		Load(a_path);
	}

	~DynamicLibrary() {
		ReleaseUnmanagedResources();
	}

	public void Load(string a_path) {
		if (m_libHandle != IntPtr.Zero) {
			Free();
		}

		m_libHandle = _LoadLibraryWin32(a_path);
	}

	public void Free() {
		if (m_libHandle != IntPtr.Zero) {
			_FreeLibraryWin32(m_libHandle);
			m_libHandle = IntPtr.Zero;
		}
	}

	public T GetDelegate<T>(string a_funcName) where T : Delegate {
		IntPtr symbol = _GetProcAddressWin32(m_libHandle, a_funcName);
		if (symbol == IntPtr.Zero) {
			return null;
		}

		return Marshal.GetDelegateForFunctionPointer(symbol, typeof(T)) as T;
	}

	void ReleaseUnmanagedResources() {
		Free();
	}

	public void Dispose() {
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}

	// Get the runtime path of a dynamic library object in the editor and standalone
	public static string PathFromLibraryObject(UnityEngine.Object a_object) {
		#if UNITY_EDITOR
		UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(a_object, out var guid, out long localId);
		return UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
		#else
			string path = $"{UnityEngine.Application.productName}_Data/Plugins/x86_64/StatsSaveLoad";
			return path;
		#endif
	}
}
