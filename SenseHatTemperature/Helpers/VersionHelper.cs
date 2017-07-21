using Windows.ApplicationModel;
using Windows.System.Profile;

namespace SenseHatTelemeter.Helpers
{
    public static class VersionHelper
    {
        public static string GetPackageVersion()
        {
            var packageVersion = Package.Current.Id.Version;

            return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
        }

        public static string GetWindowsVersion()
        {
            var deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;

            var version = ulong.Parse(deviceFamilyVersion);

            var major = GetWindowsVersionComponent(version, VersionComponent.Major);
            var minor = GetWindowsVersionComponent(version, VersionComponent.Minor);
            var build = GetWindowsVersionComponent(version, VersionComponent.Build);
            var revision = GetWindowsVersionComponent(version, VersionComponent.Revision);

            return $"{major}.{minor}.{build}.{revision}";
        }

        public enum VersionComponent
        {
            Major = 48, Minor = 32, Build = 16, Revision = 0
        }

        private static ulong GetWindowsVersionComponent(ulong version, VersionComponent versionComponentType)
        {
            var shift = (int)versionComponentType;

            return (version & (0xFFFFUL << shift)) >> shift;
        }
    }
}
