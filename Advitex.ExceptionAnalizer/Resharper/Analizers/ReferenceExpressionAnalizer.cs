#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Models.ClassMemberModels;
using Advitex.ExceptionAnalizer.Resharper.Highlightings;
using Advitex.ExceptionAnalizer.Resharper.Specifications;
using Advitex.ExceptionAnalizer.Utils;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Analizers
{
    [ElementProblemAnalyzer(new[] {typeof (IReferenceExpression)},
        HighlightingTypes =
            new[]
                {
                    typeof (NotDocumentedExceptionHighlighting),
                    typeof (PossibleNotDocumentedExceptionHighlighting),
                    typeof (NotDocumentedArgumentNullExceptionHighlighting),
                    typeof (PossibleNotDocumentedArgumentNullExceptionHighlighting),
                    typeof (UncatchedExceptionInDestructorHighlighting)
                })]
    public class ReferenceExpressionAnalizer : ElementProblemAnalyzer<IReferenceExpression>
    {
        /// <summary>
        /// ...
        /// </summary>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        protected override void Run(IReferenceExpression element, ElementProblemAnalyzerData data,
                                    IHighlightingConsumer consumer)
        {
            try
            {
                var callModel = ModelFactory.CreateModel(element) as BaseCallModel;
                if (callModel == null)
                    return;

                var exSource = callModel as IExceptionSource;
                if (exSource == null)
                    return;

                var range = callModel.ReferenceExpression.GetDocumentRange();
                var sourceFile = callModel.ReferenceExpression.GetContainingFile();

                foreach (var ex in exSource.GetThrownExceptions())
                {
                    // Highlighting of exception that was thrown in destructor
                    if (callModel.Owner is DestructorDeclarationModel)
                    {
                        consumer.AddHighlighting(new UncatchedExceptionInDestructorHighlighting(ex), range, sourceFile);
                        continue;
                    }

                    if (ex.ShortTypeName == "ArgumentNullException")
                    {
                        if (!StrongDocumentedExceptionSpecification.IsSpecified(ex))
                        {
                            if (!WeakDocumentedExceptionSpecification.IsSpecified(ex))
                                consumer.AddHighlighting(new NotDocumentedArgumentNullExceptionHighlighting(ex), range, sourceFile);
                            else
                                consumer.AddHighlighting(new PossibleNotDocumentedArgumentNullExceptionHighlighting(ex), range, sourceFile);
                        }
                    }
                    else
                    {
                        if (!StrongDocumentedExceptionSpecification.IsSpecified(ex))
                        {
                            if (!WeakDocumentedExceptionSpecification.IsSpecified(ex))
                                consumer.AddHighlighting(new NotDocumentedExceptionHighlighting(ex), range, sourceFile);
                            else
                                consumer.AddHighlighting(new PossibleNotDocumentedExceptionHighlighting(ex), range, sourceFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}