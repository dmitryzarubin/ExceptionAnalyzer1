#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Resharper.Specifications;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Psi.CSharp;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE)]
    public class NotDocumentedThrowHighlighting : CSharpHighlightingBase, IHighlighting
    {
        public NotDocumentedThrowHighlighting([NotNull] ThrownException exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            Exception = exception;
            ToolTip = string.Format("Thrown exception of type {0} is not documented [Exception Analyzer]",
                                    Exception.ShortTypeName);
        }

        public const string SeverityId = "NotDocumentedThrowHighlighting";

        /// <summary>
        /// Исключение
        /// </summary>
        [NotNull]
        public ThrownException Exception { get; private set; }

        #region IHighlighting Members

        [NotNull]
        public string ToolTip { get; private set; }

        [NotNull]
        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        /// <summary>
        /// Returns true if data (PSI, text ranges) associated with highlighting is valid
        /// </summary>
        public override bool IsValid()
        {
            return !StrongDocumentedExceptionSpecification.IsSpecified(Exception) ||
                   !IsCatchedExceptionSpecification.IsSpecified(Exception);
        }

        #endregion
    }
}