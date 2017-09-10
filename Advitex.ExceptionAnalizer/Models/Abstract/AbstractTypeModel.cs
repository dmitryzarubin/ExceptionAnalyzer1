#region

using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Caches;

#endregion

namespace Advitex.ExceptionAnalizer.Models.Abstract
{
    /// <summary>
    /// Base model of type
    /// </summary>
    public class AbstractTypeModel : AbstractCodeModel
    {
        public AbstractTypeModel([NotNull] ITypeMember typeMember)
            : base(typeMember)
        {
            HasDeclaration = typeMember.GetDeclarations().Count >= 1;
            Name = typeMember.ShortName;

            if (HasDeclaration)
            {
                // ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException
                // ReSharper disable NotDocumentedThrowHighlighting
                var declaration = (typeMember.GetDeclarations().FirstOrDefault() as IClassLikeDeclaration);
                if (declaration == null)
                    throw new InvalidOperationException("Declaration is null");
                // ReSharper restore NotDocumentedExceptionHighlighting.ArgumentNullException
                // ReSharper restore NotDocumentedThrowHighlighting

                PsiTreeElement = declaration;
            }

            AdjustAttributes();
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Compiled version of type
        /// </summary>
        [NotNull]
        public ITypeMember CompiledType
        {
            get
            {
                var value = (ITypeMember) DeclaredElement;
                if (value == null)
                {
                    // ReSharper disable NotDocumentedThrowHighlighting
                    throw new InvalidOperationException("Value of CompiledType property can't be null");
                    // ReSharper restore NotDocumentedThrowHighlighting
                }

                return value;
            }
        }

        /// <summary>
        /// Type declaration
        /// </summary>
        [CanBeNull]
        public IClassLikeDeclaration TypeDeclaration
        {
            get { return (IClassLikeDeclaration) PsiTreeElement; }
        }


        public bool IsAbstract { get; private set; }

        public bool IsDisposable { get; private set; }

        public bool IsPrivate { get; private set; }

        public bool IsInternal { get; private set; }

        public bool IsPublic { get; private set; }

        #region Private members

        private void AdjustAttributes()
        {
            AdjustVisibuilityScope();
            AdjustIsDisposable();

            IsAbstract = CompiledType.IsAbstract;
        }

        private void AdjustVisibuilityScope()
        {
            var rights = CompiledType.GetAccessRights();

            IsPublic = rights == AccessRights.PUBLIC;
            IsInternal = rights == AccessRights.INTERNAL;
            IsPrivate = rights == AccessRights.PRIVATE;
        }

        private void AdjustIsDisposable()
        {
            var cache =
                CompiledType.
                    GetPsiServices().
                    CacheManager.
                    GetDeclarationsCache(DeclarationCacheLibraryScope.FULL, true);

            var disposable = cache.GetTypeElementByCLRName("System.IDisposable");

            // ReSharper disable AssignNullToNotNullAttribute
            IsDisposable = (CompiledType is ITypeElement) && ((CompiledType as ITypeElement).IsDescendantOf(disposable));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion
    }
}