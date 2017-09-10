#region

using Advitex.ReSharperPlugin.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;

#endregion

namespace Advitex.ReSharperPlugin.Models.CodeModels
{
    /// <summary>
    /// Destructor model
    /// </summary>
    public class DestructorDeclarationModel : AbstractTypeMemberModel
    {
        public DestructorDeclarationModel([NotNull] IDestructorDeclaration constructorDeclaration)
            : base(constructorDeclaration)
        {
        }

        public DestructorDeclarationModel([NotNull] IReference reference)
            : base(reference)
        {
        }
    }
}