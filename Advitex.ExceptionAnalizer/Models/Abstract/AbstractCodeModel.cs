#region

using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Models.Abstract
{
    /// <summary>
    /// Abstract model of code
    /// </summary>
    public abstract class AbstractCodeModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        /// <param name = "psiTreeElement"> Element of psi tree that corresponding with current code model </param>
        /// <exception cref = "ArgumentNullException"> Thrown when psiTreeElement is null </exception>
        protected AbstractCodeModel([NotNull] ICSharpTreeNode psiTreeElement)
        {
            if (psiTreeElement == null)
                throw new ArgumentNullException("psiTreeElement");

            PsiTreeElement = psiTreeElement;
            HasDeclaration = true;
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name = "declaredElement"> Element that decleared in compiled code </param>
        /// <exception cref = "ArgumentNullException"> Thrown when declaredElement is null </exception>
        protected AbstractCodeModel([NotNull] IDeclaredElement declaredElement)
        {
            if (declaredElement == null)
                throw new ArgumentNullException("declaredElement");

            DeclaredElement = declaredElement;
            HasDeclaration = false;
        }

        /// <summary>
        /// Model has declaration in code
        /// </summary>
        public bool HasDeclaration { get; protected set; }

        /// <summary>
        /// Element of PSI tree
        /// </summary>
        [CanBeNull]
        public ICSharpTreeNode PsiTreeElement { get; protected set; }

        #region Protected members

        /// <summary>
        /// Compiled element
        /// </summary>
        [CanBeNull] protected IDeclaredElement DeclaredElement;

        #endregion
    }
}