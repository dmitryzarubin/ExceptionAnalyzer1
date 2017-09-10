#region

using System;
using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.ModelConstructors;
using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Models factory
    /// </summary>
    public static class ModelFactory
    {
        /// <summary>
        /// Create the factory
        /// </summary>
        static ModelFactory()
        {
            Constructors.Add(new ClassMemberModelConstructor());
            Constructors.Add(new ThrowModelConstructor());
            Constructors.Add(new CallModelConstructor());
            Constructors.Add(new ObjectCreationModelConstructor());
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <param name = "element"> element </param>
        /// <returns> model </returns>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        [CanBeNull]
        public static AbstractCodeModel CreateModel(object element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            foreach (var ctor in Constructors.Where(c => c.CanConstruct(element)))
            {
                return ctor.CreateModel(element);
            }

            throw new InvalidOperationException("Can't create model for current PSI tree element");
        }

        /// <summary>
        /// Create model
        /// </summary>
        /// <typeparam name = "T"> Type of model </typeparam>
        /// <param name = "element"> element </param>
        /// <returns> Model </returns>
        /// <exception cref = "ArgumentNullException"> Exception thrown when argument with name "element" is null </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown when model can't be created </exception>
        /// <exception cref = "InvalidOperationException"> Exception thrown can't create model for current PSI tree element </exception>
        [CanBeNull]
        public static T CreateModel<T>(object element) where T : class
        {
            dynamic a = CreateModel(element);

            return a;
        }

        #region Private members

        /// <summary>
        /// Constructors list
        /// </summary>
        private static readonly List<AbstractModelConstructor> Constructors = new List<AbstractModelConstructor>();

        #endregion
    }
}