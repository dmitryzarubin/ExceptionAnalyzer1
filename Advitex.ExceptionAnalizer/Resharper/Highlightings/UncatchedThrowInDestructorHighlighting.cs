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
    public class UncatchedThrowInDestructorHighlighting : CSharpHighlightingBase, IHighlighting
    {
        public UncatchedThrowInDestructorHighlighting([NotNull] ThrownException exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            Exception = exception;
            ToolTip = string.Format("Uncatched exception of type {0} in destructor [Exception Analyzer]",
                                    exception.ShortTypeName);
        }

        public const string SeverityId = "UncatchedThrowInDestructorHighlighting";

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

        public override bool IsValid()
        {
            return !IsCatchedExceptionSpecification.IsSpecified(Exception);
        }

        #endregion
    }
}