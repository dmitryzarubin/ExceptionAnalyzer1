#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Models.ClassMemberModels;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.ModelConstructors
{
    /// <summary>
    /// Constructor of class member model
    /// </summary>
    public class ClassMemberModelConstructor : AbstractModelConstructor
    {
        /// <summary>
        /// Can use constructor for the element
        /// </summary>
        public override bool CanConstruct(object element)
        {
            return (element is IClassMemberDeclaration) ||
                   ((element is IReference) && !(element is IReferenceExpression));
        }

        /// <summary>
        /// Create the call-model
        /// </summary>
        /// <param name = "element"> Element </param>
        /// <returns> Model </returns>
        public override AbstractCodeModel CreateModel(object element)
        {
            if (element is IFieldDeclaration)
                return null;

            IDeclaredElement declaredElement;

            if (element is IReference)
            {
                var reference = (element as IReference);
                if (reference.CurrentResolveResult == null)
                    return null;
                if (reference.CurrentResolveResult.DeclaredElement == null)
                    return null;

                declaredElement = reference.CurrentResolveResult.DeclaredElement;
            }
            else
            {
                declaredElement = (element as IDeclaration).DeclaredElement;
            }

            if (declaredElement is IParameter)
                return null;
            if (declaredElement is IField)
                return null;
            if (declaredElement is ILocalVariable)
                return null;
            if (declaredElement is IClass)
                return null;
            if (declaredElement is IStruct)
                return null;
            if (declaredElement is IEnum)
                return null;
            if (declaredElement is IAnonymousTypeProperty)
                return null;

            if (declaredElement is IConstructor)
                return new ConstructorDeclarationModel(declaredElement as IConstructor);
            if (declaredElement is IFunction)
            {
                if (declaredElement.ShortName.Contains("~") || declaredElement.ShortName == "Finalize")
                    return new DestructorDeclarationModel(declaredElement as IFunction);
                else
                    return new MethodDeclarationModel(declaredElement as IFunction);
            }
            if (declaredElement is IProperty)
                return new PropertyDeclarationModel(declaredElement as IProperty);

            throw new InvalidOperationException("Unsupported type of element " + declaredElement.GetType());
        }
    }
}