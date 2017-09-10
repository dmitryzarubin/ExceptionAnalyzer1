#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Models.InClassMemberModels;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.ModelConstructors
{
    /// <summary>
    /// Constructor of object creation model
    /// </summary>
    public class ObjectCreationModelConstructor : AbstractModelConstructor
    {
        /// <summary>
        /// Can use constructor for the element
        /// </summary>
        public override bool CanConstruct(object element)
        {
            return element is IObjectCreationExpression;
        }

        /// <summary>
        /// Create the object creation model
        /// </summary>
        /// <param name = "element"> Element </param>
        /// <returns> Model </returns>
        /// <exception cref = "InvalidOperationException"> Unsupported type of element </exception>
        /// <exception cref = "InvalidOperationException"> Referenced member is null !!!! </exception>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        public override AbstractCodeModel CreateModel(object element)
        {
            if (element is IObjectCreationExpression)
            {
                var el = element as IObjectCreationExpression;
                return ModelFactory.CreateModel(el.ConstructorReference) == null
                           ? null
                           : new ObjectCreationModel(el);
            }

            throw new InvalidOperationException("Unsupported type of element");
        }
    }
}