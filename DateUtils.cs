using System;
using System.Runtime.InteropServices.ComTypes;

namespace KeePassBrowserImporter
{
	static class DateUtils
	{
		private const long DaysPerYear = 365;
		private const long DaysPer4Years = DaysPerYear * 4 + 1;
		private const long DaysPer100Years = DaysPer4Years * 25 - 1;
		private const long DaysPer400Years = DaysPer100Years * 4 + 1;
		private const long DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear;
		private const long UnixEpochTicks = TimeSpan.TicksPerDay * DaysTo1970;
		private const long kMillisecondsPerSecond = 1000;
		private const long kMicrosecondsPerMillisecond = 1000;
		private const long MicrosecondsPerSecond = kMicrosecondsPerMillisecond * kMillisecondsPerSecond;
		private const long TimeToMicrosecondsOffset = 11644473600000000;

		/// <summary>Create a DateTime from the given unix time seconds.</summary>
		public static DateTime FromUnixTimeSeconds(long seconds)
		{
			if (seconds == 0)
			{
				return DateTime.Now;
			}

			// should use DateTimeOffset.FromUnixTimeSeconds (.NET 4.6)
			long ticks = seconds * TimeSpan.TicksPerSecond + UnixEpochTicks;
			return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
		}

		/// <summary>Create a DateTime from the given unix time milliseconds.</summary>
		public static DateTime FromUnixTimeMilliseconds(long milliseconds)
		{
			if (milliseconds == 0)
			{
				return DateTime.Now;
			}

			// should use DateTimeOffset.FromUnixTimeMilliseconds (.NET 4.6)
			long ticks = milliseconds * TimeSpan.TicksPerMillisecond + UnixEpochTicks;
			return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
		}

		/// <summary>Create a DateTime from the given filetime.</summary>
		public static DateTime FromFileTime(FILETIME fileTime)
		{
			long val = (((long)fileTime.dwHighDateTime) << 32) | (uint)fileTime.dwLowDateTime;
			return DateTime.FromFileTime(val);
		}

		/// <summary>Create a DateTime from the given Chromium time.</summary>
		public static DateTime FromChromiumTime(long microseconds)
		{
			if (microseconds == 0)
			{
				return DateTime.Now;
			}

			long milliseconds = (microseconds - TimeToMicrosecondsOffset) / MicrosecondsPerSecond;
			return FromUnixTimeSeconds(milliseconds);
		}
	}
}
