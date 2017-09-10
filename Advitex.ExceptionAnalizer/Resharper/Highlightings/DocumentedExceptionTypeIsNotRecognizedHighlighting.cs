#region

using System;
using Advitex.ExceptionAnalizer.Models.XmlDocumentationModels;
using Advitex.ExceptionAnalizer.Resharper.Specifications;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Psi.CSharp;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE)]
    public class DocumentedExceptionTypeIsNotRecognizedHighlighting : CSharpHighlightingBase, IHighlighting
    {
        public DocumentedExceptionTypeIsNotRecognizedHighlighting([NotNull] XmlDocException xmlDocException)
        {
            if (xmlDocException == null)
                throw new ArgumentNullException("xmlDocException");

            XmlDocException = xmlDocException;
        }


        public const string SeverityId = "DocumentedExceptionTypeIsNotRecognizedHighlighting";

        /// <summary>
        /// Исключение
        /// </summary>
        [NotNull]
        public XmlDocException XmlDocException { get; private set; }

        #region IHighlighting Members

        [NotNull]
        public string ToolTip
        {
            get { return "Exception type is not recognized [Exception Analyzer]"; }
        }

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
            return NotRecognizedExceptionTypeSpecification.IsSpecified(XmlDocException);
        }

        #endregion
    }
}