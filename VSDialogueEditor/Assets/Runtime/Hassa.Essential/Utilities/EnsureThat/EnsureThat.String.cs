using System;

namespace Hassa.Essentials
{
    public partial class EnsureThat
    {

        public void IsNotNullOrWhiteSpace(string value)
        {
            IsNotNullOrEmpty(value);

            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(paramName, "The string can't be left empty, null or consist of only whitespaces.");
            }
        }

        public void IsNotNullOrEmpty(string value)
        {
            IsNotNull(value);

            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentException(paramName, "The string can't be null or empty.");
            }
        }

        public void IsNotNull(string value)
        {
            if (value == null) {
                throw new ArgumentNullException(paramName, "Value cannot be null.");
            }
        }

        public void IsNull(string value)
        {
            if (value != null) {
                throw new ArgumentNullException(paramName, "Value must be null.");
            }
        }

    }
}