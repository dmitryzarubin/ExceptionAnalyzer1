#region

using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

#endregion

namespace Advitex.ExceptionAnalizer.Models.ClassMemberModels
{
    /// <summary>
    /// Model of method
    /// </summary>
    public class MethodDeclarationModel : AbstractTypeMemberModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        public MethodDeclarationModel([NotNull] IFunction method)
            : base(method)
        {
        }
    }
}