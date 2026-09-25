using System;
using System.ComponentModel;

namespace GPC.Utilities.Units
{
	public static class UnitsConvert
	{
		public const ForceUnits DefaultForceUnits = ForceUnits.N;
		public const LengthUnits DefaultLengthUnits = LengthUnits.mm;
		public const MassUnits DefaultMassUnits = MassUnits.ton;
		public const PressureUnits DefaultPressureUnits = PressureUnits.MPa;
		public const TemperatureUnits DefaultTemperatureUnits = TemperatureUnits.C;

		public enum ForceUnits
		{
			N,
			daN,
			kN,
			lbf,
			kip
		}

		//[TypeConverter(typeof(EnumDescriptionTypeConverter))]
		public enum LengthUnits
		{
			mm,
			cm,
			m,
			[Description("In")] inch,
			ft
		}

		//[TypeConverter(typeof(EnumDescriptionTypeConverter))]
		public enum MassUnits
		{
			kg,
			[Description("T")] ton,
			lb
		}

		public enum PressureUnits
		{
			Pa,
			kPa,
			MPa,
			psi,
			ksi
		}

		public enum TemperatureUnits
		{
			C,
			F
		}

		public enum EnergyUnits
		{
			J,
			kJ,
			mJ,
			btu
		}

		public static double Convert(double value, ForceUnits startingForceUnits, ForceUnits targetForceUnits, int exponent)
		{
			return value * GetFactor(startingForceUnits, targetForceUnits, exponent);
		}

		private static double GetFactor(ForceUnits startingForceUnits, ForceUnits targetForceUnits, int exponent)
		{
			double factor = 1;
			// Moving to standardUnits N
			switch (startingForceUnits)
			{
				case ForceUnits.N:
					break;

				case ForceUnits.daN:
					factor *= 10;
					break;

				case ForceUnits.kN:
					factor *= 1000;
					break;

				case ForceUnits.lbf:
					factor *= 4.44822;
					break;

				case ForceUnits.kip:
					factor *= 4448.2216;
					break;

				default:
					throw new NotSupportedException();
			}

			switch (targetForceUnits)
			{
				case ForceUnits.N:
					break;

				case ForceUnits.daN:
					factor /= 10;
					break;

				case ForceUnits.kN:
					factor /= 1000;
					break;

				case ForceUnits.lbf:
					factor /= 4.44822;
					break;

				case ForceUnits.kip:
					factor /= 4448.2216;
					break;

				default:
					throw new NotSupportedException();
			}
			return Math.Pow(factor, exponent);
		}

		public static double Convert(double value, LengthUnits startingLengthUnits, LengthUnits targetLengthUnits, int exponent)
		{
			return value * GetFactor(startingLengthUnits, targetLengthUnits, exponent);
		}

		private static double GetFactor(LengthUnits startingLengthUnits, LengthUnits targetLengthUnits, int exponent)
		{
			double factor = 1;
			// Moving to standardUnits mm
			switch (startingLengthUnits)
			{
				case LengthUnits.mm:
					break;

				case LengthUnits.cm:
					factor *= 10;
					break;

				case LengthUnits.m:
					factor *= 1000;
					break;

				case LengthUnits.inch:
					factor *= 25.4;
					break;

				case LengthUnits.ft:
					factor *= 304.8;
					break;

				default:
					throw new NotSupportedException();
			}

			switch (targetLengthUnits)
			{
				case LengthUnits.mm:
					break;

				case LengthUnits.cm:
					factor /= 10;
					break;

				case LengthUnits.m:
					factor /= 1000;
					break;

				case LengthUnits.inch:
					factor /= 25.4;
					break;

				case LengthUnits.ft:
					factor /= 304.8;
					break;

				default:
					throw new NotSupportedException();
			}
			return Math.Pow(factor, exponent);
		}

		public static double Convert(double value, MassUnits startingMassUnits, MassUnits targetMassUnits, int exponent)
		{
			return value * GetFactor(startingMassUnits, targetMassUnits, exponent);
		}

		private static double GetFactor(MassUnits startingMassUnits, MassUnits targetMassUnits, int exponent)
		{
			double factor = 1;
			// Moving to standardUnits ton
			switch (startingMassUnits)
			{
				case MassUnits.kg:
					factor /= 1000d;
					break;

				case MassUnits.ton:
					break;

				case MassUnits.lb:
					factor /= 2204.623;
					break;

				default:
					throw new NotSupportedException();
			}

			switch (targetMassUnits)
			{
				case MassUnits.kg:
					factor *= 1000d;
					break;

				case MassUnits.ton:
					break;

				case MassUnits.lb:
					factor *= 2204.623;
					break;

				default:
					throw new NotSupportedException();
			}
			return Math.Pow(factor, exponent);
		}

		public static double Convert(
			double value, PressureUnits startPressureUnits, PressureUnits targetPressureUnits, int exponent)
		{
			return value * GetFactor(startPressureUnits, targetPressureUnits, exponent);
		}

		private static double GetFactor(PressureUnits startPressureUnits, PressureUnits targetPressureUnits, int exponent)
		{
			double factor = 1;
			// Moving to standardUnits MPa
			switch (startPressureUnits)
			{
				case PressureUnits.Pa:
					factor /= 1.0e+6;
					break;

				case PressureUnits.kPa:
					factor /= 1000;
					break;

				case PressureUnits.MPa:
					break;

				case PressureUnits.psi:
					factor *= 0.00689476;
					break;

				case PressureUnits.ksi:
					factor *= 6.89475908;
					break;

				default:
					throw new NotSupportedException();
			}

			switch (targetPressureUnits)
			{
				case PressureUnits.Pa:
					factor *= 1.0e+6;
					break;

				case PressureUnits.kPa:
					factor *= 1000;
					break;

				case PressureUnits.MPa:
					break;

				case PressureUnits.psi:
					factor /= 0.00689476;
					break;

				case PressureUnits.ksi:
					factor /= 6.89475908;
					break;

				default:
					throw new NotSupportedException();
			}
			return Math.Pow(factor, exponent);
		}

		/// <summary>
		/// Convert an absolute temperature
		/// </summary>
		/// <remarks>For temperature differences (e.g. thermal loads) use <see cref="ConvertDifference(double, TemperatureUnits, TemperatureUnits)"/>, the 32° offset must not be applied</remarks>
		public static double Convert(double value, TemperatureUnits startTemperatureUnits, TemperatureUnits targetTemperatureUnits)
		{
			if (startTemperatureUnits == targetTemperatureUnits)
				return value;

			switch (startTemperatureUnits)
			{
				case TemperatureUnits.C: //FROM C TO F
					return value * GetFactor(startTemperatureUnits, targetTemperatureUnits) + 32;

				case TemperatureUnits.F: //FROM F TO C
					return (value - 32) * GetFactor(startTemperatureUnits, targetTemperatureUnits);

				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Convert a temperature difference (only the scale factor is applied, without the 32° offset)
		/// </summary>
		public static double ConvertDifference(double value, TemperatureUnits startTemperatureUnits, TemperatureUnits targetTemperatureUnits)
		{
			return value * GetFactor(startTemperatureUnits, targetTemperatureUnits);
		}

		private static double GetFactor(TemperatureUnits startTemperatureUnits, TemperatureUnits targetTemperatureUnits)
		{
			double factor = 1;
			// Moving to standardUnits C
			switch (startTemperatureUnits)
			{
				case TemperatureUnits.C:
					break;

				case TemperatureUnits.F:
					factor /= 1.8;
					break;
			}

			switch (targetTemperatureUnits)
			{
				case TemperatureUnits.C:
					break;

				case TemperatureUnits.F:
					factor *= 1.8;
					break;
			}

			return factor;
		}

		public static double Convert(double value, ForceUnits startForceUnits, ForceUnits targetForceUnits, int forceExponent,
			LengthUnits startLengthUnits, LengthUnits targetLengthUnits, int lengthExponent)
		{
			return value * GetFactor(startForceUnits, targetForceUnits, forceExponent) * GetFactor(startLengthUnits, targetLengthUnits, lengthExponent);
		}

		public static double Convert(double value, MassUnits startingMassUnits, MassUnits targetMassUnits, int massExponent,
			LengthUnits startingLengthUnits, LengthUnits targetLengthUnits, int lengthExponent)
		{
			return value * GetFactor(startingMassUnits, targetMassUnits, massExponent) * GetFactor(startingLengthUnits, targetLengthUnits, lengthExponent);
		}

		public static double ConvertToDefaultUnits(double value, ForceUnits startingForceUnits, int forceExponent,
			LengthUnits startingLengthUnits, int lengthExponent)
		{
			return value * GetFactor(startingForceUnits, DefaultForceUnits, forceExponent) * GetFactor(startingLengthUnits, DefaultLengthUnits, lengthExponent);
		}

		public static double ConvertToDefaultUnits(double value, MassUnits startingMassUnits, int massExponent,
			LengthUnits startingLengthUnits, int lengthExponent)
		{
			return value * GetFactor(startingMassUnits, DefaultMassUnits, massExponent) * GetFactor(startingLengthUnits, DefaultLengthUnits, lengthExponent);
		}

		public static double ConvertToDefaultUnits(double value, ForceUnits startingForceUnits, int forceExponent)
		{
			return Convert(value, startingForceUnits, DefaultForceUnits, forceExponent);
		}

		public static double ConvertToDefaultUnits(double value, LengthUnits startingLengthUnits, int lengthExponent)
		{
			return Convert(value, startingLengthUnits, DefaultLengthUnits, lengthExponent);
		}

		public static double ConvertToDefaultUnits(double value, MassUnits startingMassUnits, int massExponent)
		{
			return Convert(value, startingMassUnits, DefaultMassUnits, massExponent);
		}

		public static double ConvertToDefaultUnits(double value, PressureUnits startingPressureUnits, int pressureExponent)
		{
			return Convert(value, startingPressureUnits, DefaultPressureUnits, pressureExponent);
		}

		public static double ConvertFromDefaultUnits(double value, ForceUnits targetForceUnits, int forceExponent,
			LengthUnits targetLengthUnits, int lengthExponent)
		{
			return value * GetFactor(DefaultForceUnits, targetForceUnits, forceExponent) * GetFactor(DefaultLengthUnits, targetLengthUnits, lengthExponent);
		}

		public static double ConvertFromDefaultUnits(double value, MassUnits targetMassUnits, int massExponent,
			LengthUnits targetLengthUnits, int lengthExponent)
		{
			return value * GetFactor(DefaultMassUnits, targetMassUnits, massExponent) * GetFactor(DefaultLengthUnits, targetLengthUnits, lengthExponent);
		}

		public static double ConvertFromDefaultUnits(double value, ForceUnits targetForceUnits, int forceExponent)
		{
			return Convert(value, DefaultForceUnits, targetForceUnits, forceExponent);
		}

		public static double ConvertFromDefaultUnits(double value, LengthUnits targetLengthUnits, int lengthExponent)
		{
			return Convert(value, DefaultLengthUnits, targetLengthUnits, lengthExponent);
		}

		public static double ConvertFromDefaultUnits(double value, MassUnits targetMassUnits, int massExponent)
		{
			return Convert(value, DefaultMassUnits, targetMassUnits, massExponent);
		}

		public static double ConvertFromDefaultUnits(double value, PressureUnits targetPressureUnits, int pressureExponent)
		{
			return Convert(value, DefaultPressureUnits, targetPressureUnits, pressureExponent);
		}
	}
}