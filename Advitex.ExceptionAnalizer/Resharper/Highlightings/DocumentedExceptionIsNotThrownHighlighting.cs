#region

using System;
using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Models.InClassMemberModels;
using Advitex.ExceptionAnalizer.Models.XmlDocumentationModels;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Highlightings
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE)]
    public class DocumentedExceptionIsNotThrownHighlighting : CSharpHighlightingBase, IHighlighting
    {
        public DocumentedExceptionIsNotThrownHighlighting([NotNull] XmlDocException xmlDocException,
                                                          [NotNull] AbstractTypeMemberModel typeMemberModel)
        {
            if (xmlDocException == null)
                throw new ArgumentNullException("xmlDocException");
            if (typeMemberModel == null)
                throw new ArgumentNullException("typeMemberModel");

            XmlDocException = xmlDocException;
            TypeMemberModel = typeMemberModel;
        }


        public const string SeverityId = "DocumentedExceptionIsNotThrownHighlighting";

        /// <summary>
        /// Исключение
        /// </summary>
        [NotNull]
        public XmlDocException XmlDocException { get; private set; }

        /// <summary>
        /// Object creation model
        /// </summary>
        [NotNull]
        public AbstractTypeMemberModel TypeMemberModel { get; private set; }

        #region IHighlighting Members

        [NotNull]
        public string ToolTip
        {
            get { return "Documented exception is not thrown by type member [Exception Analyzer]"; }
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
            if (XmlDocException.ExceptionType == null)
                return false;

            if ((!TypeMemberModel.HasDeclaration) || (TypeMemberModel.IsAbstract))
                return false;

            string exceptionClassName = XmlDocException.ExceptionType.GetClrName().FullName;

            return GetAllPossibleExceptions(TypeMemberModel).All(e => e.FullTypeName != exceptionClassName);
        }

        #endregion

        [NotNull]
        private IEnumerable<ThrownException> GetAllPossibleExceptions(AbstractTypeMemberModel model)
        {
            if (!model.HasDeclaration)
                throw new InvalidOperationException("Procedure can be called only when model has source code");

            var lst = new List<ThrownException>();

            model.ClassMemberDeclaration.ProcessDescendants(new RecursiveElementProcessor<IThrowStatement>(t =>
            {
                var model1 = ModelFactory.CreateModel<ThrowModel>(t);
                if (model1 == null)
                    return;

                lst.AddRange(model1.GetThrownExceptions());
            }));

            model.ClassMemberDeclaration.ProcessDescendants(new RecursiveElementProcessor<IObjectCreationExpression>(t =>
            {
                var model2 = ModelFactory.CreateModel<ObjectCreationModel>(t);
                if (model2 == null)
                    return;

                lst.AddRange(model2.GetThrownExceptions());
            }));

            model.ClassMemberDeclaration.ProcessDescendants(new RecursiveElementProcessor<IReferenceExpression>(t =>
            {
                var callModel = ModelFactory.CreateModel<BaseCallModel>(t);
                if (callModel == null)
                    return;

                var exSource = callModel as IExceptionSource;
                if (exSource == null)
                    return;

                lst.AddRange(exSource.GetThrownExceptions());
            }));

            return lst;
        }
    }
}