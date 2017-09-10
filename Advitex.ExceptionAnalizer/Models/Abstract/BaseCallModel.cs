#region

using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Models.Abstract
{
    /// <summary>
    /// Base class of "call" model
    /// </summary>
    public abstract class BaseCallModel : AbstractInMemberModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        /// <exception cref = "InvalidOperationException"> Referenced member is null!!! </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        protected BaseCallModel([NotNull] IReferenceExpression callModel) : base(callModel)
        {
            if (callModel == null)
                throw new ArgumentNullException("callModel");

            var model = ModelFactory.CreateModel<AbstractTypeMemberModel>(callModel.Reference);
            if (model == null)
                throw new InvalidOperationException("Referenced member is null!!!");

            ReferencedMember = model;
        }

        /// <summary>
        /// Refference expression
        /// </summary>
        [NotNull]
        public IReferenceExpression ReferenceExpression
        {
            // ReSharper disable AssignNullToNotNullAttribute
            get { return (IReferenceExpression) PsiTreeElement; }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Referenced type member
        /// </summary>
        [NotNull]
        public AbstractTypeMemberModel ReferencedMember { get; protected set; }
    }
}