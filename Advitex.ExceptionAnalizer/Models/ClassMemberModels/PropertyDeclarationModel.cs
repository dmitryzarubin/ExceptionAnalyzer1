#region

using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

#endregion

namespace Advitex.ExceptionAnalizer.Models.ClassMemberModels
{
    /// <summary>
    /// Model of property
    /// </summary>
    public class PropertyDeclarationModel : AbstractTypeMemberModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        public PropertyDeclarationModel([NotNull] IProperty property)
            : base(property)
        {
        }
    }
}