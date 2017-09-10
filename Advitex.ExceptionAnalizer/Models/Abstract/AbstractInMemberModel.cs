#region

using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models.Abstract
{
    /// <summary>
    /// Base model of in-class member code statement
    /// </summary>
    public abstract class AbstractInMemberModel : AbstractCodeModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        protected AbstractInMemberModel([NotNull] ICSharpTreeNode psiTreeElement)
            : base(psiTreeElement)
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            _ownerLazy = new Lazy<AbstractTypeMemberModel>(GetOwner, true);
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        /// <summary>
        /// Class member that owned of model
        /// </summary>
        [NotNull]
        public AbstractTypeMemberModel Owner
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            get { return _ownerLazy.Value; }
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        #region Private members

        private readonly Lazy<AbstractTypeMemberModel> _ownerLazy;

        /// <summary>
        /// Get tree node owner
        /// </summary>
        /// <exception cref = "InvalidOperationException"> Throw when can't get tree node owner </exception>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        /// <exception cref = "InvalidOperationException"> Can't get psi tree element </exception>
        [NotNull]
        private AbstractTypeMemberModel GetOwner()
        {
            if (PsiTreeElement == null)
                throw new InvalidOperationException("Can't get psi tree element");
            var element = PsiTreeElement.GetContainingTypeMemberDeclaration();
            if (element == null)
                throw new InvalidOperationException("Can't get tree node owner");

            var model = ModelFactory.CreateModel<AbstractTypeMemberModel>(element);
            if (model == null)
                throw new InvalidOperationException(string.Format("Can't get owner. Model type is {0}", element.GetType().FullName));

            return model;
        }

        #endregion
    }
}