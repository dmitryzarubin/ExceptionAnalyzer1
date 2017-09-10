#region

using System;
using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using Advitex.ExceptionAnalizer.Utils;

#endregion

namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Exception model
    /// </summary>
    public class ThrownException
    {
        /// <summary>
        /// Create exception model
        /// </summary>
        /// <param name = "exceptionSource"> Model that raise the exception </param>
        /// <param name = "exceptionType"> Exception type </param>
        /// <param name = "description"> Exception description </param>
        /// <exception cref = "ArgumentNullException"> Thrown when exceptionSource is null </exception>
        /// <exception cref = "ArgumentNullException"> Thrown when exceptionType is null </exception>
        /// <exception cref = "ArgumentNullException"> Thrown when description is null </exception>
        public ThrownException([NotNull] AbstractInMemberModel exceptionSource, [NotNull] ITypeElement exceptionType,
                               [NotNull] string description)
        {
            if (exceptionSource == null)
                throw new ArgumentNullException("exceptionSource");
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description");

            ExceptionSource = exceptionSource;
            ExceptionType = exceptionType;
            ExceptionDescription = description.Trim();
        }

        /// <summary>
        /// Create exception model
        /// </summary>
        /// <param name = "exceptionSource"> Model that raise the exception </param>
        /// <param name = "exceptionInfo"> Exception info </param>
        /// <param name = "arguments"> Arguments </param>
        /// <exception cref = "ArgumentNullException"> Thrown when exceptionSource is null </exception>
        /// <exception cref = "ArgumentNullException"> Thrown when exceptionInfo is null </exception>
        /// <exception cref = "ArgumentNullException"> Thrown when arguments is null </exception>
        public ThrownException([NotNull] AbstractInMemberModel exceptionSource, [NotNull] ExceptionInfo exceptionInfo,
                               [NotNull] IEnumerable<Argument> arguments)
        {
            if (exceptionSource == null)
                throw new ArgumentNullException("exceptionSource");
            if (exceptionInfo == null)
                throw new ArgumentNullException("exceptionInfo");
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            ExceptionSource = exceptionSource;
            ExceptionType = exceptionInfo.ExceptionType;


            var args = arguments.ToArray();
            string desc = exceptionInfo.ExceptionDescription;
            foreach (var exParam in exceptionInfo.DescriptionParams)
            {
                var arg = args.FirstOrDefault(a => a.ParameterName == exParam.Name);
                if (arg != null)
                {
                    string p = (exParam.Type == DescriptionParamType.ArgumentName)
                                   ? "{" + string.Format("name-of:{0}", exParam.Name) + "}"
                                   : "{" + string.Format("value-of:{0}", exParam.Name) + "}";

                    string v = (exParam.Type == DescriptionParamType.ArgumentName)
                                   ? arg.ArgumentName
                                   : arg.HasValue ? (string) arg.Value : "N/A";

                    desc = desc.Replace(p, v);
                }
            }
            ExceptionDescription = desc.Trim();
        }

        /// <summary>
        /// Model that raise the exception
        /// </summary>
        public AbstractInMemberModel ExceptionSource { get; internal set; }

        /// <summary>
        /// Exception type
        /// </summary>
        [NotNull]
        public ITypeElement ExceptionType { get; private set; }

        /// <summary>
        /// Short type name of exception
        /// </summary>
        [NotNull]
        public string ShortTypeName
        {
            get { return ExceptionType.GetClrName().ShortName; }
        }

        /// <summary>
        /// Full type name of exception
        /// </summary>
        [NotNull]
        public string FullTypeName
        {
            get { return ExceptionType.GetClrName().FullName; }
        }

        /// <summary>
        /// Exception description
        /// </summary>
        [NotNull]
        public string ExceptionDescription { get; private set; }

        /// <summary>
        /// Catch status of exception
        /// </summary>
        public CatchStatusEnum CatchStatus
        {
            get { return ExceptionSource.PsiTreeElement.CheckIsCatched(ExceptionType); }
        }

        /// <summary>
        /// Is two exceptions have equals types and descriptions
        /// </summary>
        /// <param name = "other"> Other exception </param>
        public bool FullEquality([CanBeNull] ThrownException other)
        {
            if (other == null)
                return false;

            return TypeEquality(other) &&
                   ExceptionDescription != string.Empty &&
                   other.ExceptionDescription != string.Empty &&
                   ExceptionDescription == other.ExceptionDescription;
        }

        /// <summary>
        /// Is two exceptions have equals types
        /// </summary>
        /// <param name = "other"> Other exception </param>
        public bool TypeEquality([CanBeNull] ThrownException other)
        {
            if (other == null)
                return false;
            if (this == other)
                return true;

            return FullTypeName == other.FullTypeName;
        }
    }
}