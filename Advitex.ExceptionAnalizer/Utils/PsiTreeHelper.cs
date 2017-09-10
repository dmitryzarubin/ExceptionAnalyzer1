#region

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Utils
{
    public static class PsiTreeHelper
    {
        /// <summary>
        /// Get parents of node
        /// </summary>
        /// <param name = "node"> Node </param>
        /// <returns> List of parent nodes </returns>
        [NotNull]
        public static IEnumerable<ITreeNode> Parents([NotNull] this ITreeNode node)
        {
            var x = node;

            while (x != null)
            {
                x = x.Parent;
                yield return x;
            }
        }

        /// <summary>
        /// Get parents of node of specified type
        /// </summary>
        /// <typeparam name = "T"> Type of parent nodes </typeparam>
        /// <param name = "node"> Node </param>
        /// <returns> List of parent nodes </returns>
        [NotNull]
        public static IEnumerable<T> Parents<T>([NotNull] this ITreeNode node)
            where T : ITreeNode
        {
            // ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException
            return Parents(node).OfType<T>();
            // ReSharper restore NotDocumentedExceptionHighlighting.ArgumentNullException
        }
    }
}