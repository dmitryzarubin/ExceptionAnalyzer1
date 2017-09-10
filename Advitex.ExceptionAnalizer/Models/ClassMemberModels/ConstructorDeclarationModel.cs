#region

using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

#endregion

namespace Advitex.ExceptionAnalizer.Models.ClassMemberModels
{
    /// <summary>
    /// Model of constructor
    /// </summary>
    public class ConstructorDeclarationModel : AbstractTypeMemberModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        public ConstructorDeclarationModel([NotNull] IConstructor constructor)
            : base(constructor)
        {
        }
    }
}