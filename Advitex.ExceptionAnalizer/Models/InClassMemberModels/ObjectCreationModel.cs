#region

using System;
using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Utils;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models.InClassMemberModels
{
    /// <summary>
    /// Model of object creation expression
    /// </summary>
    public class ObjectCreationModel : AbstractInMemberModel, IExceptionSource
    {
        /// <summary>
        /// Create model
        /// </summary>
        /// <exception cref = "InvalidOperationException"> Referenced member is null !!!! </exception>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        public ObjectCreationModel([NotNull] IObjectCreationExpression objectCreationExpression)
            : base(objectCreationExpression)
        {
            var model = ModelFactory.CreateModel<AbstractTypeMemberModel>(objectCreationExpression.ConstructorReference);
            if (model == null)
                throw new InvalidOperationException("Referenced member is null !!!!");

            ConstructorReference = model;
        }

        /// <summary>
        /// Reference to constructor
        /// </summary>
        [NotNull]
        public AbstractTypeMemberModel ConstructorReference { get; private set; }

        /// <summary>
        /// Object creation expression
        /// </summary>
        [NotNull]
        public IObjectCreationExpression ObjectCreationExpression
        {
            // ReSharper disable AssignNullToNotNullAttribute
            get { return (IObjectCreationExpression) PsiTreeElement; }
            // ReSharper restore AssignNullToNotNullAttribute
        }


        /// <summary>
        /// Cunstructor arguments
        /// </summary>
        public IEnumerable<Argument> Arguments
        {
            get { return ObjectCreationExpression.Arguments.Select(arg => new Argument(arg)); }
        }

        /// <summary>
        /// Get exceptions that was thrown by the code model
        /// </summary>
        public IEnumerable<ThrownException> GetThrownExceptions()
        {
            return
                from exInfo in ConstructorReference.ExceptionInfos
                where ObjectCreationExpression.CheckIsCatched(exInfo.ExceptionType) == CatchStatusEnum.NotCatched
                select new ThrownException(this, exInfo, Arguments);
        }
    }
}