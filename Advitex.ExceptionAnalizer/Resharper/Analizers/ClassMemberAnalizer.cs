#region

using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Resharper.Highlightings;
using Advitex.ExceptionAnalizer.Resharper.Specifications;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Analizers
{
    [ElementProblemAnalyzer(new[] {typeof (IClassDeclaration)},
        HighlightingTypes = new[]
            {
                typeof (DocumentedExceptionTypeIsNotRecognizedHighlighting),
                typeof (DocumentedExceptionIsNotThrownHighlighting)
            })]
    public class ClassMemberAnalizer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data,
                                    IHighlightingConsumer consumer)
        {
            foreach (var member in element.MemberDeclarations)
            {
                var model = ModelFactory.CreateModel<AbstractTypeMemberModel>(member);
                if (model == null)
                    continue;

                if (model.ClassMemberDeclaration == null)
                    continue;

                // Searching for exception documentations without corresponding exceptions in code
                foreach (var docException in model.XmlDocumentation.Exceptions)
                {
                    if (NotRecognizedExceptionTypeSpecification.IsSpecified(docException))
                        consumer.AddHighlighting(new DocumentedExceptionTypeIsNotRecognizedHighlighting(docException),
                                                 docException.Range,
                                                 model.ClassMemberDeclaration.GetContainingFile());

                    var h2 = new DocumentedExceptionIsNotThrownHighlighting(docException, model);
                    if (h2.IsValid())
                        consumer.AddHighlighting(h2, docException.Range,
                                                 model.ClassMemberDeclaration.GetContainingFile());
                }
            }
        }
    }
}