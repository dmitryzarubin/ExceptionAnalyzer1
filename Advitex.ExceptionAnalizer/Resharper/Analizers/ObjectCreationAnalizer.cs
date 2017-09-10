#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.ClassMemberModels;
using Advitex.ExceptionAnalizer.Models.InClassMemberModels;
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
    [ElementProblemAnalyzer(new[] {typeof (IObjectCreationExpression)},
        HighlightingTypes =
            new[]
                {
                    typeof (NotDocumentedExceptionHighlighting),
                    typeof (PossibleNotDocumentedExceptionHighlighting),
                    typeof (NotDocumentedArgumentNullExceptionHighlighting),
                    typeof (PossibleNotDocumentedArgumentNullExceptionHighlighting),
                    typeof (UncatchedExceptionInDestructorHighlighting)
                })]
    public class ObjectCreationAnalizer : ElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override void Run(IObjectCreationExpression element, ElementProblemAnalyzerData data,
                                    IHighlightingConsumer consumer)
        {
            try
            {
                var model = ModelFactory.CreateModel<ObjectCreationModel>(element);
                if (model == null)
                    return;

                var range = model.ObjectCreationExpression.GetDocumentRange();
                var sourceFile = model.ObjectCreationExpression.GetContainingFile();

                foreach (var ex in model.GetThrownExceptions())
                {
                    // Highlighting of exception that was thrown in destructor
                    if (model.Owner is DestructorDeclarationModel)
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