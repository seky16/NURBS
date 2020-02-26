// ReSharper disable once StyleCop.SA1633
// ReSharper disable StyleCop.SA1616
namespace NURBS
{
    /// <summary>
    /// A set of extensions to allow the convenient comparison of decimal values based on a given precision.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Determines if the decimal value is less than or equal to the decimal parameter according to the defined precision.
        /// </summary>
        /// <param name="decimal1">The decimal1.</param>
        /// <param name="decimal2">The decimal2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool LessThan(this decimal decimal1, decimal decimal2, int precision = 20)
        {
            var foo = System.Math.Round(decimal1 - decimal2, precision);
            var bar = foo < 0;
            return System.Math.Round(decimal1 - decimal2, precision) < 0;
        }

        /// <summary>
        /// Determines if the decimal value is less than or equal to the decimal parameter according to the defined precision.
        /// </summary>
        /// <param name="decimal1">The decimal1.</param>
        /// <param name="decimal2">The decimal2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool LessThanOrEqualTo(this decimal decimal1, decimal decimal2, int precision = 20)
        {
            return System.Math.Round(decimal1 - decimal2, precision) <= 0;
        }

        /// <summary>
        /// Determines if the decimal value is greater than (>) the decimal parameter according to the defined precision.
        /// </summary>
        /// <param name="decimal1">The decimal1.</param>
        /// <param name="decimal2">The decimal2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool GreaterThan(this decimal decimal1, decimal decimal2, int precision = 20)
        {
            return System.Math.Round(decimal1 - decimal2, precision) > 0;
        }

        /// <summary>
        /// Determines if the decimal value is greater than or equal to (>=) the decimal parameter according to the defined precision.
        /// </summary>
        /// <param name="decimal1">The decimal1.</param>
        /// <param name="decimal2">The decimal2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool GreaterThanOrEqualTo(this decimal decimal1, decimal decimal2, int precision = 20)
        {
            var foo = System.Math.Round(decimal1 - decimal2, precision);
            var bar = foo >= 0;
            return System.Math.Round(decimal1 - decimal2, precision) >= 0;
        }

        /// <summary>
        /// Determines if the decimal value is equal to (==) the decimal parameter according to the defined precision.
        /// </summary>
        /// <param name="decimal1">The decimal1.</param>
        /// <param name="decimal2">The decimal2.</param>
        /// <param name="precision">The precision.  The number of digits after the decimal that will be considered when comparing.</param>
        /// <returns></returns>
        public static bool AlmostEquals(this decimal decimal1, decimal decimal2, int precision = 20)
        {
            return System.Math.Round(decimal1 - decimal2, precision) == 0;
        }
    }
}