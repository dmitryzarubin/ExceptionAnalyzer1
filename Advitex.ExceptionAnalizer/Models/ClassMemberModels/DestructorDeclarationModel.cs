#region

using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

#endregion

namespace Advitex.ExceptionAnalizer.Models.ClassMemberModels
{
    /// <summary>
    /// Model of destructor
    /// </summary>
    public class DestructorDeclarationModel : AbstractTypeMemberModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        public DestructorDeclarationModel([NotNull] IFunction destructor)
            : base(destructor)
        {
        }
    }
}