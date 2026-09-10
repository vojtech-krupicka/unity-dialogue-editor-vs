using System;

namespace Hassa.Essentials
{
    public static class Ensure
    {

        private static readonly EnsureThat instance = new EnsureThat();

        public static EnsureThat That(string paramName)
        {
            instance.paramName = paramName;
            return instance;
        }

    }
}