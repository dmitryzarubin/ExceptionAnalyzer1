#region

using System;
using JetBrains.Annotations;

#endregion

namespace Advitex.ExceptionAnalizer
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property,
        AllowMultiple = true, Inherited = false)]
    public class ExceptionContractAttribute : Attribute
    {
        /// <summary>
        /// Exception type
        /// </summary>
        /// <exception cref = "ArgumentException"> Thrown when null or empty value is setted to the ExceptionType property </exception>
        [NotNull]
        public Type ExceptionType
        {
            get { return _exceptionType; }
            set
            {
                if (value == null)
                    throw new ArgumentException("Can't set null value to the ExceptionType property");
                _exceptionType = value;
            }
        }

        /// <summary>
        /// Exception description
        /// </summary>
        /// <exception cref = "ArgumentException"> Thrown when null or empty value is setted to the Description property </exception>
        [NotNull]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == null)
                    throw new ArgumentException("Can't set null or empty value to the Description property");
                _description = value;
            }
        }

        #region Private members

        private Type _exceptionType;
        private string _description;

        #endregion
    }
}