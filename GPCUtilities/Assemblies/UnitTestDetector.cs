using System;
using System.Reflection;

namespace GPC.Utilities.Assemblies
{
    internal static class UnitTestDetector
    {
        private static bool _runningFromNUnit = false;
        public static bool DetectUnitTest()
        {
            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assem.FullName.ToLowerInvariant().Contains("unittest"))
                {
                    _runningFromNUnit = true;
                    break;
                }
            }
            return false;
        }

        public static bool IsRunningFromNUnit
        {
            get { return _runningFromNUnit; }
        }
    }
}
