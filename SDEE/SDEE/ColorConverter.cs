using SFML.Graphics;
using System;

namespace SDEE
{
	public static class ColorConverter
	{
		public static Color ParseHex(string hex)
		{
			if (hex[0] != '#')
				throw new NotHexColorException();

			try
			{
				return new Color(
					byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
					byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
					byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
			}
			catch (Exception)
			{
				throw new NotHexColorException();
			}
		}

		public static string ToHex(this Color color)
			=> $"#{GetComponentString(color.R)}{GetComponentString(color.G)}{GetComponentString(color.B)}{GetComponentString(color.A)}";

		private static string GetComponentString(int v)
			=> v.ToString("X").PadLeft(2, '0');
	}

}