#region

using System;
using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.Models.XmlDocumentationModels;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Caches;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models.Abstract
{
    /// <summary>
    /// Base model of type member
    /// </summary>
    public abstract class AbstractTypeMemberModel : AbstractCodeModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        /// <exception cref = "InvalidOperationException"> Declaration is null </exception>
        /// <exception cref = "InvalidOperationException"> Reference is not resolved !!!!! </exception>
        protected AbstractTypeMemberModel([NotNull] ITypeMember typeMember)
            : base(typeMember)
        {
            HasDeclaration = typeMember.GetDeclarations().Count >= 1;
            Name = typeMember.ShortName;

            if (HasDeclaration)
            {
                // ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException
                // ReSharper disable NotDocumentedThrowHighlighting
                var declaration = (typeMember.GetDeclarations().FirstOrDefault() as IClassMemberDeclaration);
                if (declaration == null)
                    throw new InvalidOperationException("Declaration is null");
                // ReSharper restore NotDocumentedExceptionHighlighting.ArgumentNullException
                // ReSharper restore NotDocumentedThrowHighlighting

                PsiTreeElement = declaration;
            }

            AdjustAttributes();
            _ownerLazy = new Lazy<AbstractTypeModel>(GetOwner);
            _documentationLazy = new Lazy<ClassMemberXmlDocumentation>(CreateClassMemberXmlDoc);
            _exceptionInfosLazy = new Lazy<IEnumerable<ExceptionInfo>>(GetExceptionInfos);
        }

        /// <summary>
        /// Class member name
        /// </summary>
        [NotNull]
        public string Name { get; private set; }

        /// <summary>
        /// Owned type
        /// </summary>
        [NotNull]
        public AbstractTypeModel Owner
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            get { return _ownerLazy.Value; }
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        /// <summary>
        /// Declaration of class member
        /// </summary>
        [CanBeNull]
        public IClassMemberDeclaration ClassMemberDeclaration
        {
            get { return (IClassMemberDeclaration) PsiTreeElement; }
        }

        /// <summary>
        /// Reference to compiled type member
        /// </summary>
        [NotNull]
        public ITypeMember CompiledClassMember
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
        /// Is abstract type memeber
        /// </summary>
        public bool IsAbstract { get; private set; }

        /// <summary>
        /// Is private type member
        /// </summary>
        public bool IsPrivate { get; private set; }

        /// <summary>
        /// Is internal type memeber
        /// </summary>
        public bool IsInternal { get; private set; }

        /// <summary>
        /// Is protected type member
        /// </summary>
        public bool IsProtected { get; private set; }

        /// <summary>
        /// Is public type member
        /// </summary>
        public bool IsPublic { get; private set; }

        /// <summary>
        /// Xml documentation
        /// </summary>
        [NotNull]
        public ClassMemberXmlDocumentation XmlDocumentation
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            get { return _documentationLazy.Value; }
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        /// <summary>
        /// Information about exceptions that can be thrown by type member
        /// </summary>
        [NotNull]
        public IEnumerable<ExceptionInfo> ExceptionInfos
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            get { return _exceptionInfosLazy.Value; }
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        #region Private members

        private readonly Lazy<AbstractTypeModel> _ownerLazy;
        private readonly Lazy<ClassMemberXmlDocumentation> _documentationLazy;
        private readonly Lazy<IEnumerable<ExceptionInfo>> _exceptionInfosLazy;

        private void AdjustAttributes()
        {
            AdjustVisibuilityScope();

            IsAbstract = CompiledClassMember.IsAbstract;
        }

        private void AdjustVisibuilityScope()
        {
            var rights = CompiledClassMember.GetAccessRights();

            IsPublic = rights == AccessRights.PUBLIC;
            IsInternal = rights == AccessRights.INTERNAL;
            IsPrivate = rights == AccessRights.PRIVATE;
            IsProtected = rights == AccessRights.PROTECTED;
        }

        private AbstractTypeModel GetOwner()
        {
            // ReSharper disable NotDocumentedThrowHighlighting
            // ReSharper disable NotDocumentedExceptionHighlighting
            var ownedType = CompiledClassMember.GetContainingType() as ITypeMember;
            if (ownedType == null)
                throw new InvalidOperationException("Can't create class members's owner");
            var model = ModelFactory.CreateModel<AbstractTypeModel>(ownedType);
            if (model == null)
                throw new InvalidOperationException("Can't create class members's owner");
            // ReSharper restore NotDocumentedThrowHighlighting
            // ReSharper restore NotDocumentedExceptionHighlighting
            return model;
        }

        private ClassMemberXmlDocumentation CreateClassMemberXmlDoc()
        {
            return new ClassMemberXmlDocumentation(this);
        }

        private IEnumerable<ExceptionInfo> GetExceptionInfos()
        {
            var lang = CompiledClassMember.PresentationLanguage;

            var attributes =
                CompiledClassMember.
                    GetAttributeInstances(true).
                    Where(a => a.AttributeType.GetPresentableName(lang) == "ExceptionContractAttribute").
                    ToArray();

            if (attributes.Any())
            {
                return (from att in attributes
                        let declarationCache =
                            CompiledClassMember.
                            GetPsiServices().
                            CacheManager.
                            GetDeclarationsCache(DeclarationCacheLibraryScope.FULL, true)
                        let typeParam = att.NamedParameter("ExceptionType")
                        let descParam = att.NamedParameter("Description")
                        where !typeParam.IsBadValue && typeParam.IsType &&
                              !descParam.IsBadValue && descParam.IsConstant
                        let t = declarationCache.
                            GetTypeElementByCLRName(
                                typeParam.TypeValue.
                                          GetLongPresentableName(CompiledClassMember.PresentationLanguage))
                        let d = (string) descParam.ConstantValue.Value
                        where t != null
                        select new ExceptionInfo(t, d)).ToList();
            }
            else
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return XmlDocumentation.Exceptions.
                                        Where(e => e.ExceptionType != null)
                                       .Select(e => new ExceptionInfo(e.ExceptionType, e.ExceptionDescription));
                // ReSharper restore AssignNullToNotNullAttribute                
            }
        }

        #endregion
    }
}