using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.ErrorManagement
{
    /// <summary>
    /// Exception raised during validaton process
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IdentityValidationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public IdentityValidationException(string message) : base(message) { }
    }
}
