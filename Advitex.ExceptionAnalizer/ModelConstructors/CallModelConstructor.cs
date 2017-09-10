#region

using System;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Models.ClassMemberModels;
using Advitex.ExceptionAnalizer.Models.InClassMemberModels;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.ModelConstructors
{
    /// <summary>
    /// Constructor of call-models
    /// </summary>
    public class CallModelConstructor : AbstractModelConstructor
    {
        /// <summary>
        /// Can use constructor for element
        /// </summary>
        public override bool CanConstruct(object element)
        {
            return element is IReferenceExpression;
        }

        /// <summary>
        /// Create the call-model
        /// </summary>
        /// <param name = "element"> Element </param>
        /// <returns> Model </returns>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        /// <exception cref = "InvalidOperationException"> Unsupported type of model </exception>
        public override AbstractCodeModel CreateModel(object element)
        {
            if (element is IReferenceExpression)
            {
                var el = element as IReferenceExpression;

                var model = ModelFactory.CreateModel(el.Reference);
                if (model == null)
                    return null;

                if (model is MethodDeclarationModel)
                    return new MethodCallModel(el);
                if (model is PropertyDeclarationModel)
                    return new PropertyCallModel(el);
            }

            throw new InvalidOperationException("Unsupported type of model");
        }
    }
}