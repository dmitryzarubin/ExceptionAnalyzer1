#region

using System;
using JetBrains.Annotations;

#endregion

namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Parameter of exception description
    /// </summary>
    public class ExceptionDescriptionParam
    {
        /// <summary>
        /// Create parameter
        /// </summary>
        /// <param name = "paramType"> Parameter type </param>
        /// <param name = "paramName"> Parameter name </param>
        /// <exception cref = "ArgumentNullException"> Thrown when paramName argument is null or empty </exception>
        internal ExceptionDescriptionParam(DescriptionParamType paramType, [NotNull] string paramName)
        {
            if (paramName == null)
                throw new ArgumentNullException("paramName");

            Type = paramType;
            Name = paramName;
        }

        /// <summary>
        /// Parameter type
        /// </summary>
        public DescriptionParamType Type { get; private set; }

        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; private set; }
    }
}