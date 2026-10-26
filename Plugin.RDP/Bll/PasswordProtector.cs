using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.RDP.Bll
{
	/// <summary>Password encryption the same way as mstsc.exe (DPAPI, current user)</summary>
	internal static class PasswordProtector
	{
		/// <summary>Description that mstsc.exe uses for DPAPI protected passwords</summary>
		private const String PasswordDescription = "psw";

		/// <summary>Hex encoded header of every DPAPI blob: blob version and provider GUID</summary>
		private const String DpapiBlobHeader = "01000000D08C9DDF0115D1118C7A00C04FC297EB";

		/// <summary>Check whether the value is a hex encoded DPAPI protected password</summary>
		/// <param name="value">Plain text or protected password</param>
		/// <returns>The value is protected by DPAPI</returns>
		public static Boolean IsProtected(String value)
		{
			if(value == null || value.Length <= DpapiBlobHeader.Length || value.Length % 2 != 0
				|| !value.StartsWith(DpapiBlobHeader, StringComparison.OrdinalIgnoreCase))
				return false;

			foreach(Char c in value)
				if(!Uri.IsHexDigit(c))
					return false;
			return true;
		}

		/// <summary>Encrypt the password the same way as mstsc.exe (DPAPI, current user)</summary>
		/// <param name="password">Plain text password</param>
		/// <returns>Hex encoded encrypted password</returns>
		public static String Protect(String password)
		{
			Byte[] encrypted = PasswordProtector.InvokeDpapi(Encoding.Unicode.GetBytes(password), true)
				?? throw new InvalidOperationException("Failed to encrypt the password. Error: " + Marshal.GetLastWin32Error());

			StringBuilder result = new StringBuilder(encrypted.Length * 2);
			foreach(Byte b in encrypted)
				result.Append(b.ToString("X2", CultureInfo.InvariantCulture));
			return result.ToString();
		}

		/// <summary>Decrypt the password encrypted by mstsc.exe</summary>
		/// <param name="hex">Hex encoded encrypted password</param>
		/// <returns>Plain text password or null if it was encrypted by another user or on another computer</returns>
		public static String Unprotect(String hex)
		{
			if(String.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
				return null;

			Byte[] encrypted = new Byte[hex.Length / 2];
			for(Int32 loop = 0; loop < encrypted.Length; loop++)
				if(!Byte.TryParse(hex.Substring(loop * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out encrypted[loop]))
					return null;

			Byte[] decrypted = PasswordProtector.InvokeDpapi(encrypted, false);
			return decrypted == null ? null : Encoding.Unicode.GetString(decrypted).TrimEnd('\0');
		}

		private static Byte[] InvokeDpapi(Byte[] data, Boolean protect)
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			Native.DataBlob output = new Native.DataBlob();
			try
			{
				Native.DataBlob input = new Native.DataBlob() { cbData = data.Length, pbData = handle.AddrOfPinnedObject(), };
				Boolean success = protect
					? Native.CryptProtectData(ref input, PasswordDescription, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Native.CRYPTPROTECT_UI_FORBIDDEN, ref output)
					: Native.CryptUnprotectData(ref input, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Native.CRYPTPROTECT_UI_FORBIDDEN, ref output);
				if(!success)
					return null;

				Byte[] result = new Byte[output.cbData];
				Marshal.Copy(output.pbData, result, 0, output.cbData);
				return result;
			} finally
			{
				handle.Free();
				if(output.pbData != IntPtr.Zero)
					Native.LocalFree(output.pbData);
			}
		}
	}
}
