namespace TrafficManager.Compatibility.Check {
    using CSUtil.Commons;
    using System;

    /// <summary>
    /// Checks version equality with added logging.
    /// </summary>
    public class Versions {

        /// <summary>
        /// Verifies that expected version matches actual verison.
        ///
        /// Versions are matched on Major.minor.build. Revision is ignored.
        /// </summary>
        /// 
        /// <param name="expected">The version you are expecting.</param>
        /// <param name="actual">The actual version.</param>
        /// 
        /// <returns>Returns <c>true</c> if versions match, otherwise <c>false</c>.</returns>
        public static bool Verify(Version expected, Version actual) {
            Log.InfoFormat(
                    "Compatibility.Check.Versions.Verify({0}, {1})",
                    expected.ToString(3),
                    actual.ToString(3));

            return expected == actual;
        }
    }
}