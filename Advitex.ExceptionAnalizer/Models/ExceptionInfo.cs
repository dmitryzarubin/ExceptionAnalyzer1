#region

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Information about exception that could be thrown by class member
    /// </summary>
    public class ExceptionInfo
    {
        /// <summary>
        /// Create exception info
        /// </summary>
        /// <param name = "exceptionType"> Exception type </param>
        /// <param name = "description"> Exception description </param>
        /// <exception cref = "ArgumentNullException"> Thrown when exceptionType argument is null </exception>
        /// <exception cref = "ArgumentNullException"> Thrown when description argument is null or empty </exception>
        internal ExceptionInfo([NotNull] ITypeElement exceptionType, [NotNull] string description)
        {
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description");

            ExceptionType = exceptionType;
            ExceptionDescription = description.Trim();

            ParseDescriptionParameters();
        }

        /// <summary>
        /// Exception type
        /// </summary>
        [NotNull]
        public ITypeElement ExceptionType { get; private set; }

        /// <summary>
        /// Exception description
        /// </summary>
        [NotNull]
        public string ExceptionDescription { get; private set; }

        /// <summary>
        /// Description parameters
        /// </summary>
        [NotNull]
        public IEnumerable<ExceptionDescriptionParam> DescriptionParams
        {
            get { return _descriptionParams; }
        }

        /// <summary>
        /// Is exception types are equals
        /// </summary>
        public bool IsTypeEquals([CanBeNull] ExceptionInfo other)
        {
            if (other == null)
                return false;
            if (this == other)
                return true;

            return ExceptionType.GetClrName().FullName.Trim() == other.ExceptionType.GetClrName().FullName.Trim();
        }

        /// <summary>
        /// Is exception types are equals
        /// </summary>
        public bool IsTypeEquals([CanBeNull] ThrownException other)
        {
            if (other == null)
                return false;

            return ExceptionType.GetClrName().FullName == other.ExceptionType.GetClrName().FullName;
        }

        /// <summary>
        /// Is exception types and descriptions are equals
        /// </summary>
        public bool IsFullEquals([CanBeNull] ExceptionInfo other)
        {
            if (other == null)
                return false;

            return IsTypeEquals(other) &&
                   ExceptionDescription != string.Empty &&
                   other.ExceptionDescription != string.Empty &&
                   ExceptionDescription == other.ExceptionDescription;
        }

        /// <summary>
        /// Is exception types and descriptions are equals
        /// </summary>
        public bool IsFullEquals([CanBeNull] ThrownException other)
        {
            if (other == null)
                return false;

            return IsTypeEquals(other) &&
                   ExceptionDescription != string.Empty &&
                   other.ExceptionDescription != string.Empty &&
                   ExceptionDescription == other.ExceptionDescription;
        }

        #region Private members

        private const string NameMark = "{name-of:";
        private const string ValueMark = "{value-of:";
        private readonly List<ExceptionDescriptionParam> _descriptionParams = new List<ExceptionDescriptionParam>();

        private void ParseDescriptionParameters()
        {
            ParseParametersByMask(NameMark, DescriptionParamType.ArgumentName);
            ParseParametersByMask(ValueMark, DescriptionParamType.ArgumentValue);
        }

        private void ParseParametersByMask([NotNull] string mask, DescriptionParamType paramType)
        {
            int idx = 0;
            int maskLength = mask.Length;

            while (true)
            {
                if (idx > (ExceptionDescription.Length - 1))
                    break;

                // ReSharper disable NotDocumentedExceptionHighlighting
                var si = ExceptionDescription.IndexOf(mask, idx, StringComparison.InvariantCulture);
                if (si == -1)
                    break;

                var ei = ExceptionDescription.IndexOf("}", si, StringComparison.InvariantCulture);
                if (ei == -1)
                    break;

                var str = ExceptionDescription.Substring(si + maskLength, ei - si - maskLength);
                _descriptionParams.Add(new ExceptionDescriptionParam(paramType, str));
                // ReSharper restore NotDocumentedExceptionHighlighting
                idx = ei;
            }
        }

        #endregion
    }
}