using System;

namespace Hassa.Essentials
{
    public partial class EnsureThat
    {

        public void IsNotNull(object value)
        {
            if (value == null) {
                throw new ArgumentNullException(paramName, "Value cannot be null.");
            }
        }

        public void IsNull(object value)
        {
            if (value != null) {
                throw new ArgumentNullException(paramName, "Value must be null.");
            }
        }

    }
}