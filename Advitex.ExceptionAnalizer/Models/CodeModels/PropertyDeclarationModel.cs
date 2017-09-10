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
    public class PropertyDeclarationModel : AbstractTypeMemberModel
    {
        /// <summary>
        /// Создать модель свойства
        /// </summary>
        public PropertyDeclarationModel([NotNull] IPropertyDeclaration propertyDeclaration)
            : base(propertyDeclaration)
        {
        }

        public PropertyDeclarationModel([NotNull] IReference reference)
            : base(reference)
        {
        }
    }
}