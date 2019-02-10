using System;

namespace CalcDmitriyElisov.Utility
{
    struct VersionInfo
    {
        public int Major;
        public int Minor;
        public int Build;

        public VersionInfo(int major, int minor, int build)
        {
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
        }

        public int CompareTo(VersionInfo other)
        {
            if (this.Major != other.Major)
            {
                return this.Major.CompareTo(other.Major);
            }
            else if (this.Minor != other.Minor)
            {
                return this.Minor.CompareTo(other.Minor);
            }
            else if (this.Build != other.Build)
            {
                return this.Build.CompareTo(other.Build);
            }
            else
            {
                return 0;
            }
        }

        public static bool operator >=(VersionInfo left, VersionInfo right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(VersionInfo left, VersionInfo right)
        {
            return left.CompareTo(right) <= 0;
        }
    }
}
