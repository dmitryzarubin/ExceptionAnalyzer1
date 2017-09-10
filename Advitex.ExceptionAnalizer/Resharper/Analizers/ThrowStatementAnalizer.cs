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
    [ElementProblemAnalyzer(new[] {typeof (IThrowStatement)},
        HighlightingTypes =
            new[]
                {
                    typeof (NotDocumentedThrowHighlighting),
                    typeof (PossibleNotDocumentedThrowHighlighting),
                    typeof (UncatchedThrowInDestructorHighlighting),
                    typeof (ReThrowWithoutInnerExceptionHighlighting)
                })]
    public class ThrowStatementAnalizer : ElementProblemAnalyzer<IThrowStatement>
    {
        protected override void Run(IThrowStatement element, ElementProblemAnalyzerData data,
                                    IHighlightingConsumer consumer)
        {
            try
            {
                var model = ModelFactory.CreateModel<ThrowModel>(element);
                if (model == null)
                    return;
                var range = model.ThrowStatement.GetDocumentRange();
                var sourceFile = model.ThrowStatement.GetContainingFile();

                // Re-throw
                if (model.RethrowStatus == RethrowStatusEnum.RethrowNewExceptionWithoutInnerException)
                    consumer.AddHighlighting(new ReThrowWithoutInnerExceptionHighlighting(model), range, sourceFile);
                if (model.RethrowStatus == RethrowStatusEnum.RethrowWithoutCallStackData)
                    consumer.AddHighlighting(new ReThrowWithoutFullCallStackHighlighting(model), range, sourceFile);
                if (model.ExceptionType.ShortName == "Exception")
                    consumer.AddHighlighting(new ThrownExceptionOfTypeExceptionHighlighting(model), range, sourceFile);


                foreach (var ex in model.GetThrownExceptions())
                {
                    // Highlighting of exception that was thrown in destructor
                    if (model.Owner is DestructorDeclarationModel)
                    {
                        consumer.AddHighlighting(new UncatchedThrowInDestructorHighlighting(ex), range, sourceFile);
                        continue;
                    }

                    if (!StrongDocumentedExceptionSpecification.IsSpecified(ex))
                    {
                        if (!WeakDocumentedExceptionSpecification.IsSpecified(ex))
                            consumer.AddHighlighting(new NotDocumentedThrowHighlighting(ex), range, sourceFile);
                        else
                            consumer.AddHighlighting(new PossibleNotDocumentedThrowHighlighting(ex), range, sourceFile);
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