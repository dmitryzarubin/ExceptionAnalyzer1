#region

using System;
using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Argument of method invocation
    /// </summary>
    public class Argument : AbstractCodeModel
    {
        /// <summary>
        /// Create model
        /// </summary>
        /// <param name = "argument"> Element of psi tree that corresponding with current code model </param>
        /// <exception cref = "ArgumentNullException"> Thrown when argument is null </exception>
        public Argument([NotNull] ICSharpArgument argument)
            : base(argument)
        {
            if (argument == null)
                throw new ArgumentNullException("argument");

            ParameterName = argument.MatchingParameter != null
                                ? argument.MatchingParameter.Element.ShortName
                                : "N/A";

            ArgumentName = "N/A";
            var reference = (argument.Value as IReferenceExpression);
            if (reference != null && reference.Reference.CurrentResolveResult != null &&
                reference.Reference.CurrentResolveResult.DeclaredElement != null)
                ArgumentName = reference.Reference.CurrentResolveResult.DeclaredElement.ShortName;

            if (argument.Value != null && argument.Value.IsConstantValue())
            {
                HasValue = true;
                Value = argument.Value.ConstantValue.Value;
            }
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name = "quilifierReference"> Quilifier </param>
        /// <param name = "parameterName"> Parameter name </param>
        /// <exception cref = "ArgumentNullException"> Thrown when parameterName is null or empty </exception>
        /// <exception cref = "ArgumentNullException"> Thrown when quilifierReference is null </exception>
        public Argument(IReferenceExpression quilifierReference, string parameterName)
            : base(quilifierReference)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");
            if (quilifierReference == null)
                throw new ArgumentNullException("quilifierReference");

            ParameterName = parameterName;
            ArgumentName = quilifierReference.NameIdentifier.Name;
            HasValue = quilifierReference.IsConstantValue();
            if (HasValue)
                Value = quilifierReference.ConstantValue;
        }

        /// <summary>
        /// Matching parameter name
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Argument name
        /// </summary>
        public string ArgumentName { get; private set; }

        /// <summary>
        /// Is argument's value is recognized
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Argument value
        /// </summary>
        public object Value { get; private set; }
    }
}