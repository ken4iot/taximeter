using System;
using System.IO;

namespace TaxiMeter.Utils {

	public static class Helpers: Object {
		public static DateTime EpochToDateTime(UInt32 intDate) {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(intDate);
            return dt;
		}

		public static float GetSingle(byte[] data, int index) {
			byte[] bArray = new byte[4];
			Array.Copy(data, index, bArray, 0, 4);
			bArray.Reverse();
			return BitConverter.ToSingle(bArray, 0);
		}

		public static char[] GetString(byte[] data, int index, int length) {
			byte[] bArray = new byte[length];
			Array.Copy(data, index, bArray, 0, length);
			char[] cArray = System.Text.Encoding.ASCII.GetString(bArray).ToCharArray();
			return cArray;
		}

		public static UInt16 GetUInt16(byte[] data, int index) {
			byte[] bArray = new byte[2];
			Array.Copy(data, index, bArray, 0, 2);
			bArray.Reverse();
			return BitConverter.ToUInt16(bArray, 0);
		}

		public static UInt32 GetUInt32(byte[] data, int index) {
			byte[] bArray = new byte[4];
			Array.Copy(data, index, bArray, 0, 4);
			bArray.Reverse();
			return BitConverter.ToUInt32(bArray, 0);
		}

		public static byte[] ReadBytesRequired(this BinaryReader reader, int byteCount) {
			var result = reader.ReadBytes(byteCount);

			if (result.Length != byteCount)
				throw new EndOfStreamException (string.Format("{0} bytes required from stream, but only {1} returned.", byteCount, result.Length));

			return result;
		}

		public static Int16 ReadInt16BE(this BinaryReader reader) {
			return BitConverter.ToInt16(reader.ReadBytesRequired(sizeof(Int16)).Reverse(), 0);
		}

		public static Int32 ReadInt32BE(this BinaryReader reader) {
			return BitConverter.ToInt32(reader.ReadBytesRequired(sizeof(Int32)).Reverse(), 0);
		}

		public static float ReadSingleBE(this BinaryReader reader) {
			return BitConverter.ToSingle(reader.ReadBytesRequired(sizeof(float)).Reverse(), 0);
		}

		public static UInt16 ReadUInt16BE(this BinaryReader reader) {
			return BitConverter.ToUInt16(reader.ReadBytesRequired(sizeof(UInt16)).Reverse(), 0);
		}

		public static UInt32 ReadUInt32BE(this BinaryReader reader) {
			return BitConverter.ToUInt32(reader.ReadBytesRequired(sizeof(UInt32)).Reverse(), 0);
		}

		public static byte[] Reverse(this byte[] b) {
			Array.Reverse(b);
			return b;
		}
	}
}
