using System;

namespace RMDev_JwtExample.JsonWebTokenConfig
{
    public static class GuardExtensions
    {
        /// <summary>
        /// Verifica se o argunto é nulo.
        /// </summary>
        public static void CheckArgumentNull(this object o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }
    }
}