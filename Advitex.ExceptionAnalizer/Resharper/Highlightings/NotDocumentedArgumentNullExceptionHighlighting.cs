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
    public class NotDocumentedArgumentNullExceptionHighlighting : CSharpHighlightingBase, IHighlighting
    {
        public NotDocumentedArgumentNullExceptionHighlighting([NotNull] ThrownException exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            Exception = exception;
            ToolTip = "Possible exception of type ArgumentNullException is not documented [Exception Analyzer]";
        }

        public const string SeverityId = "NotDocumentedExceptionHighlighting.ArgumentNullException";

        /// <summary>
        /// Не перехваченное исключение
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