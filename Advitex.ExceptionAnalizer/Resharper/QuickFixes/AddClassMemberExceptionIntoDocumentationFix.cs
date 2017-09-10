#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Resharper.Highlightings;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using JetBrains.Util;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.QuickFixes
{
    [QuickFix]
    public class AddClassMemberExceptionIntoDocumentationFix : QuickFixBase
    {
        public AddClassMemberExceptionIntoDocumentationFix(
            [NotNull] NotDocumentedExceptionHighlighting highlighting)
        {
            if (highlighting == null)
                throw new ArgumentNullException("highlighting");

            _highlighting = highlighting;
            _exception = highlighting.Exception;
        }

        public AddClassMemberExceptionIntoDocumentationFix(
            [NotNull] PossibleNotDocumentedExceptionHighlighting highlighting)
        {
            if (highlighting == null)
                throw new ArgumentNullException("highlighting");

            _highlighting = highlighting;
            _exception = highlighting.Exception;
        }

        public AddClassMemberExceptionIntoDocumentationFix(
            [NotNull] NotDocumentedArgumentNullExceptionHighlighting highlighting)
        {
            if (highlighting == null)
                throw new ArgumentNullException("highlighting");

            _highlighting = highlighting;
            _exception = highlighting.Exception;
        }

        /// <summary>
        /// Executes QuickFix or ContextAction. Returns post-execute method.
        /// </summary>
        /// <returns>
        /// Action to execute after document and PSI transaction finish. Use to open TextControls, navigate caret, etc.
        /// </returns>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            return textControl =>
                {
                    var psiManager = PsiManager.GetInstance(solution);
                    psiManager.DoTransaction(
                        () =>
                            {
                                AbstractTypeMemberModel docOwner = _exception.ExceptionSource.Owner;
                                if (docOwner == null)
                                    return;

                                if (docOwner.XmlDocumentation.CanModifyDocumentation)
                                    docOwner.XmlDocumentation.AddXmlDocException(
                                        _exception.ShortTypeName,
                                        _exception.ExceptionDescription);
                            }, "Adding xml comment for exception");
                };
        }

        /// <summary>
        /// Popup menu item text
        /// </summary>
        public override string Text
        {
            get
            {
                return string.Format("Add {0} to the xml documentation [Advitex]",
                                     _exception.ShortTypeName);
            }
        }

        /// <summary>
        /// Check if this action is available at the constructed context.
        /// Actions could store precalculated info in <paramref name = "cache" /> to share it between different actions
        /// </summary>
        /// <returns>
        /// true if this bulb action is available, false otherwise.
        /// </returns>
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighting.IsValid();
        }

        [NotNull] private readonly IHighlighting _highlighting;
        [NotNull] private readonly ThrownException _exception;
    }
}