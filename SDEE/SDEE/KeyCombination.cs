using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace SDEE
{
	public class KeyCombination
	{
		public bool Alt { get; set; }
		public bool System { get; set; }
		public bool Shift { get; set; }
		public bool Control { get; set; }
		public IEnumerable<Keyboard.Key> Keys { get; set; }

		public override bool Equals(object obj)
		{
			return obj is KeyCombination combination &&
				   Alt == combination.Alt &&
				   System == combination.System &&
				   Shift == combination.Shift &&
				   Control == combination.Control &&
				   Keys.SequenceEqual(combination.Keys);
		}

		public override int GetHashCode()
		{
			int hashCode = 163722762;
			hashCode = hashCode * -1521134295 + Alt.GetHashCode();
			hashCode = hashCode * -1521134295 + System.GetHashCode();
			hashCode = hashCode * -1521134295 + Shift.GetHashCode();
			hashCode = hashCode * -1521134295 + Control.GetHashCode();

			foreach (var key in Keys)
			{
				hashCode = hashCode * -1521134295 + key.GetHashCode();
			}

			return hashCode;
		}

		public override string ToString()
		{
			var modifiers = (new string[] {
				Alt ? "Alt" : "",
				System ? "System" : "",
				Shift ? "Shift" : "",
				Control ? "Control" : "" }
			).Where(s => !string.IsNullOrEmpty(s));

			return string.Join("+", modifiers)
				+ (modifiers.Count() > 0 ? "+" : "")
				+ string.Join("+", Keys);
		}
	}
}
