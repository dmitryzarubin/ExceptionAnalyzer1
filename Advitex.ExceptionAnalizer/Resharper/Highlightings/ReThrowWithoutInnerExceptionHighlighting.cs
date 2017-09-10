#region

using System;
using System.Linq;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.InClassMemberModels;
using Advitex.ExceptionAnalizer.Resharper.Specifications;
using Advitex.ExceptionAnalizer.Utils;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.Util.Collections;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE)]
    public class ReThrowWithoutInnerExceptionHighlighting : CSharpHighlightingBase, IHighlighting
    {
        public ReThrowWithoutInnerExceptionHighlighting([NotNull] ThrowModel throwModel)
        {
            if (throwModel == null)
                throw new ArgumentNullException("throwModel");

            ThrowModel = throwModel;
            ToolTip = string.Format(
                "Re-thrown exception should include inner exception [Exception Analyzer]");
        }

        public const string SeverityId = "ReThrowWithoutInnerExceptionHighlighting";

        /// <summary>
        /// Исключение
        /// </summary>
        [NotNull]
        public ThrowModel ThrowModel { get; private set; }

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
            return ThrowModel.RethrowStatus == RethrowStatusEnum.RethrowNewExceptionWithoutInnerException;
        }

        #endregion
    }
}