﻿#region

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
    public class UncatchedExceptionInDestructorHighlighting : CSharpHighlightingBase, IHighlighting
    {
        public UncatchedExceptionInDestructorHighlighting([NotNull] ThrownException exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            Exception = exception;
            ToolTip = string.Format("Possible exception of type {0} in destructor [Exception Analyzer]",
                                    Exception.ShortTypeName);
        }

        public const string SeverityId = "UncatchedExceptionInDestructorHighlighting";

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
            return !IsCatchedExceptionSpecification.IsSpecified(Exception);
        }

        #endregion
    }
}