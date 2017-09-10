#region

using System;
using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;

#endregion

namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Models constructor of certain type
    /// </summary>
    public abstract class AbstractModelConstructor
    {
        /// <summary>
        /// Is model can be created
        /// </summary>
        public virtual bool CanConstruct(object element)
        {
            return false;
        }

        /// <summary>
        /// Create the model
        /// </summary>
        /// <param name = "element"> Based PSI tree element </param>
        /// <returns> Created model </returns>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        [CanBeNull]
        public abstract AbstractCodeModel CreateModel(object element);
    }
}