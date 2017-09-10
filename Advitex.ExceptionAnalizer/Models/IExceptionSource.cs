#region

using System.Collections.Generic;

#endregion

namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Interface of exception source
    /// </summary>
    public interface IExceptionSource
    {
        /// <summary>
        /// Get exceptions that was thrown by the code model
        /// </summary>
        IEnumerable<ThrownException> GetThrownExceptions();
    }
}