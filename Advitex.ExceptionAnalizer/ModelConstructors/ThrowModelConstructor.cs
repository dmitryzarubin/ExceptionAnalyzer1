#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Models.InClassMemberModels;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.ModelConstructors
{
    /// <summary>
    /// Constructor of throw model
    /// </summary>
    public class ThrowModelConstructor : AbstractModelConstructor
    {
        /// <summary>
        /// Can use constructor for the element
        /// </summary>
        public override bool CanConstruct(object element)
        {
            return element is IThrowStatement;
        }

        /// <summary>
        /// Create the object creation model
        /// </summary>
        /// <param name = "element"> Element </param>
        /// <returns> Model </returns>
        /// <exception cref = "InvalidOperationException"> Unsupported type of element </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        public override AbstractCodeModel CreateModel(object element)
        {
            if (element is IThrowStatement)
            {
                if ((element as IThrowStatement).GetContainingTypeMemberDeclaration() == null)
                    return null;

                return new ThrowModel(element as IThrowStatement);
            }

            throw new InvalidOperationException("Unsupported type of element");
        }
    }
}