#region

using Advitex.ReSharperPlugin.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;

#endregion

namespace Advitex.ReSharperPlugin.Models.CodeModels
{
    /// <summary>
    /// Модель конструктора
    /// </summary>
    public class ConstructorDeclarationModel : AbstractTypeMemberModel
    {
        /// <summary>
        /// Создать модель конструктора
        /// </summary>
        public ConstructorDeclarationModel([NotNull] IConstructorDeclaration constructorDeclaration)
            : base(constructorDeclaration)
        {
        }

        public ConstructorDeclarationModel([NotNull] IReference reference)
            : base(reference)
        {
        }
    }
}