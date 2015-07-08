using System;
using System.IO;

namespace BubbleEngine
{
	unsafe static class BinaryReaderExtensions
	{
		static readonly byte[] bytes = new byte[4];
		public static int ReadInt32BE(this BinaryReader reader)
		{
			reader.Read (bytes, 0, 4);
			if (BitConverter.IsLittleEndian) {
				int x = (bytes [0] << 24) | (bytes [1] << 16) | (bytes [2] << 8) | bytes [3];
				return x;
			} else {
				fixed (byte *ptr = bytes) {
					return *(int*)ptr;
				}
			}
		}
	}
}

