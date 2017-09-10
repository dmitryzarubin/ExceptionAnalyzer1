#region

using Advitex.ReSharperPlugin.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;

#endregion

namespace Advitex.ReSharperPlugin.Models.CodeModels
{
    /// <summary>
    /// Модель метода
    /// </summary>
    public class MethodDeclarationModel : AbstractTypeMemberModel
    {
        /// <summary>
        /// Создать модель метода
        /// </summary>
        public MethodDeclarationModel([NotNull] IMethodDeclaration methodDeclaration)
            : base(methodDeclaration)
        {
        }

        public MethodDeclarationModel([NotNull] IReference reference)
            : base(reference)
        {
        }
    }
}