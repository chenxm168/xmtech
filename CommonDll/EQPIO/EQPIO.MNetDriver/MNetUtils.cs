using System;
using System.Text;

namespace EQPIO.MNetDriver
{
	internal class MNetUtils
	{
		public static byte HiNibble(byte b)
		{
			return (byte)((b >> 4) & 0xF);
		}

		public static byte LoNibble(byte b)
		{
			return (byte)(b & 0xF);
		}

		public static byte HiByte(ushort w)
		{
			return (byte)((w >> 8) & 0xFF);
		}

		public static byte LoByte(ushort w)
		{
			return (byte)(w & 0xFF);
		}

		public static ushort HiWord(uint dw)
		{
			return (ushort)((dw >> 16) & 0xFFFF);
		}

		public static ushort LoWord(uint dw)
		{
			return (ushort)(dw & 0xFFFF);
		}

		public static ushort HiWord(int dw)
		{
			return (ushort)((dw >> 16) & 0xFFFF);
		}

		public static ushort LoWord(int dw)
		{
			return (ushort)(dw & 0xFFFF);
		}

		public static string CharRevcrse(string data)
		{
			char[] array = data.ToCharArray();
			Array.Reverse(array);
			string text = string.Empty;
			char[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				char c = array2[i];
				text += c.ToString();
			}
			return text;
		}

		public static byte MakeByte(byte hi, byte lo)
		{
			return (byte)(((hi << 4) & 0xF0) | (lo & 0xF));
		}

		public static ushort MakeWord(byte hi, byte lo)
		{
			return (ushort)(((hi << 8) & 0xFF00) | (lo & 0xFF));
		}

		public static uint MakeDWord(ushort hi, ushort lo)
		{
			return (uint)(((hi << 16) & -65536) | (lo & 0xFFFF));
		}

		public static ulong MakeQWord(uint hi, uint lo)
		{
			return (ulong)(((long)((ulong)hi << 32) & -4294967296L) | ((long)lo & 4294967295L));
		}

		public static ushort SwapByte(ushort w)
		{
			return (ushort)(((w << 8) & 0xFF00) | ((w >> 8) & 0xFF));
		}

		public static uint SwapWord(uint dw)
		{
			return (uint)(((int)(dw << 16) & -65536) | (int)((dw >> 16) & 0xFFFF));
		}

		public static void WriteStringToMem(ushort[] buf, int nStart, int nLength, string s)
		{
			s = ((s.Length > nLength * 2) ? s.Substring(0, nLength * 2) : s.PadRight(nLength * 2));
			byte[] bytes = Encoding.Default.GetBytes(s);
			Buffer.BlockCopy(bytes, 0, buf, nStart * 2, Math.Min(nLength * 2, bytes.Length));
		}

		public static void WriteBitStringToMem(ushort[] buf, int start, int len, string bitstr)
		{
			if (!string.IsNullOrEmpty(bitstr))
			{
				Array.Clear(buf, start, len);
				int length = bitstr.Length;
				for (int i = 0; i < length && i < len * 16; i++)
				{
					if (bitstr[i] == '1')
					{
						int num = i / 16;
						buf[start + num] = (ushort)(buf[start + num] | (1 << i % 16));
					}
				}
			}
		}

		public static bool TryParseStrToDateTime(string s, out DateTime dt)
		{
			try
			{
				dt = new DateTime(int.Parse(s.Substring(0, 4)), int.Parse(s.Substring(4, 2)), int.Parse(s.Substring(6, 2)), int.Parse(s.Substring(8, 2)), int.Parse(s.Substring(10, 2)), int.Parse(s.Substring(12, 2)));
				return true;
			}
			catch (Exception)
			{
				dt = default(DateTime);
				return false;
			}
		}

		public static string BitStringAND(string s1, string s2)
		{
			int num = Math.Min(s1.Length, s2.Length);
			StringBuilder stringBuilder = new StringBuilder(s1);
			for (int i = 0; i < num; i++)
			{
				stringBuilder[i] = ((s1[i] == '1' && s2[i] == '1') ? '1' : '0');
			}
			return stringBuilder.ToString();
		}

		public static string BitStringOR(string s1, string s2)
		{
			int num = Math.Min(s1.Length, s2.Length);
			StringBuilder stringBuilder = new StringBuilder(s1);
			for (int i = 0; i < num; i++)
			{
				stringBuilder[i] = ((s1[i] == '1' || s2[i] == '1') ? '1' : '0');
			}
			return stringBuilder.ToString();
		}

		public static string BitStringXOR(string s1, string s2)
		{
			int num = Math.Min(s1.Length, s2.Length);
			StringBuilder stringBuilder = new StringBuilder(s1);
			for (int i = 0; i < num; i++)
			{
				stringBuilder[i] = ((s1[i] == s2[i]) ? '0' : '1');
			}
			return stringBuilder.ToString();
		}

		public static string BitStringNOT(string s1)
		{
			StringBuilder stringBuilder = new StringBuilder(s1);
			for (int i = 0; i < s1.Length; i++)
			{
				stringBuilder[i] = ((s1[i] == '1') ? '0' : '1');
			}
			return stringBuilder.ToString();
		}
	}
}
