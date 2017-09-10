#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Advitex.ExceptionAnalizer.Models.Abstract;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Tree;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models.XmlDocumentationModels
{
    /// <summary>
    /// xml documentation of class member
    /// </summary>
    public class ClassMemberXmlDocumentation
    {
        /// <summary>
        /// Create xml documentation for class member
        /// </summary>
        /// <param name = "typeMemberModel"> Model of type member </param>
        public ClassMemberXmlDocumentation([NotNull] AbstractTypeMemberModel typeMemberModel)
        {
            _declarationsCache =
                typeMemberModel.CompiledClassMember.GetPsiServices()
                               .CacheManager.GetDeclarationsCache(DeclarationCacheLibraryScope.FULL, true);

            if (typeMemberModel.HasDeclaration)
            {
                CanModifyDocumentation = typeMemberModel.ClassMemberDeclaration is IDocCommentBlockOwnerNode;
                _docCommentOwner = typeMemberModel.ClassMemberDeclaration as IDocCommentBlockOwnerNode;
                if (_docCommentOwner == null)
                    return;
            }
            else
            {
                CanModifyDocumentation = false;
            }

            _xmlNode = typeMemberModel.CompiledClassMember.GetXMLDoc(true);
            Parse();
        }

        /// <summary>
        /// Is documentation can be modified
        /// </summary>
        public bool CanModifyDocumentation { get; private set; }

        /// <summary>
        /// Is summary block exists
        /// </summary>
        public bool HasSummary { get; private set; }

        /// <summary>
        /// Is exception blocks exists
        /// </summary>
        public bool HasExceptions
        {
            get { return Exceptions.Any(); }
        }

        /// <summary>
        /// Class member summary
        /// </summary>
        [CanBeNull]
        public string Summary { get; private set; }

        /// <summary>
        /// Exceptions
        /// </summary>
        [NotNull]
        public IEnumerable<XmlDocException> Exceptions
        {
            get { return _exceptions; }
        }

        /// <summary>
        /// Documentate an exception
        /// </summary>
        /// <exception cref = "InvalidOperationException"> Xml documentation can't be modified </exception>
        /// <exception cref = "InvalidOperationException"> DocComment owner can't be null </exception>
        public void AddXmlDocException([NotNull] string exceptionTypeName, [NotNull] string exceptionDescription)
        {
            if (!CanModifyDocumentation)
                throw new InvalidOperationException("Xml documentation can't be modified");
            if (_docCommentOwner == null)
                throw new InvalidOperationException("DocComment owner can't be null");

            var comment = _docCommentOwner.GetDocCommentBlockNode() as ICSharpDocCommentBlockNode;
            var psiModule = _docCommentOwner.GetPsiModule();
            var sharpElementFactory = CSharpElementFactory.GetInstance(psiModule);

            if (comment == null)
            {
                var sb = new StringBuilder();
                // ReSharper disable NotDocumentedExceptionHighlighting
                sb.AppendLine("<summary>");
                sb.AppendLine("...");
                sb.AppendLine("</summary>");
                // ReSharper restore NotDocumentedExceptionHighlighting

                comment = sharpElementFactory.CreateDocCommentBlock(sb.ToString()) as ICSharpDocCommentBlockNode;
                _docCommentOwner.SetDocCommentBlockNode(comment);
                comment = _docCommentOwner.GetDocCommentBlockNode() as ICSharpDocCommentBlockNode;
            }

            if ((comment == null) || (comment.DocComments.Count == 0))
                return;

            // ReSharper disable NotDocumentedExceptionHighlighting
            var exText = String.Format(ExceptionMaskForCreation, exceptionTypeName, exceptionDescription);
            var lastDocComment = comment.DocComments.Last();
            // ReSharper restore NotDocumentedExceptionHighlighting

            var exceptionBlock = sharpElementFactory.CreateDocComment(exText);
            comment.AddDocCommentAfter(exceptionBlock, lastDocComment);
        }

        #region Private members

        [CanBeNull] private readonly IDocCommentBlockOwnerNode _docCommentOwner;
        [NotNull] private readonly IDeclarationsCache _declarationsCache;
        [CanBeNull] private readonly XmlNode _xmlNode;
        private readonly List<XmlDocException> _exceptions = new List<XmlDocException>();
        private const string ExceptionMaskForCreation = " <exception cref= \"{0}\" > {1} </exception>";
        private const string ExceptionMaskForParsing = "<exceptioncref=\"{0}\"";

        /// <summary>
        /// Parse the xml documentation
        /// </summary>
        private void Parse()
        {
            if (_xmlNode == null)
                return;

            foreach (var node in _xmlNode.OfType<XmlNode>())
            {
                if (node.Name.Trim().ToLower() == "summary")
                {
                    HasSummary = true;
                    ParseSummary(node);
                    continue;
                }

                if (node.Name.Trim().ToLower() == "exception")
                {
                    ParseExceptions(node);
                }
            }
        }

        /// <summary>
        /// Parse the summary
        /// </summary>
        private void ParseSummary(XmlNode node)
        {
            Summary = node.InnerText;
        }

        /// <summary>
        /// Parse exceptions
        /// </summary>
        private void ParseExceptions(XmlNode node)
        {
            if (node.Attributes == null)
                return;

            var range = new DocumentRange();

            foreach (var att in node.Attributes.OfType<XmlNode>())
            {
                if (att.Name.ToLower().Trim() == "cref")
                {
                    // ReSharper disable NotDocumentedExceptionHighlighting
                    var exName = att.Value;

                    var exKind = exName.Substring(0, 2);
                    if ((exKind == "T:") || (exKind == "!:"))
                        exName = exName.Substring(2);

                    var exShortName = exName.Split('.').LastOrDefault();

                    if (_docCommentOwner != null)
                    {
                        var xmlDoc = _docCommentOwner.GetDocCommentBlockNode() as ICSharpDocCommentBlockNode;
                        if (xmlDoc != null)
                        {
                            range = xmlDoc.GetDocumentRange();

                            foreach (var comment in xmlDoc.DocComments)
                            {
                                var s = comment.CommentText.Replace(" ", string.Empty);
                                if ((s.Contains(string.Format(ExceptionMaskForParsing, exName))) ||
                                    (s.Contains(string.Format(ExceptionMaskForParsing, exShortName))))
                                {
                                    range = comment.GetDocumentRange();
                                    break;
                                }
                            }
                        }
                    }
                    // ReSharper restore NotDocumentedExceptionHighlighting

                    ITypeElement exceptionType = _declarationsCache.GetTypeElementByCLRName(exName);
                    if (exceptionType == null)
                    {
                        var shortType = _declarationsCache.GetElementsByShortName(exShortName).FirstOrDefault();
                        if (shortType != null)
                        {
                            // ReSharper disable NotDocumentedExceptionHighlighting
                            exShortName = shortType.ToString().Substring(5);
                            // ReSharper restore NotDocumentedExceptionHighlighting
                            exceptionType = _declarationsCache.GetTypeElementByCLRName(exShortName);
                        }
                    }

                    var xmlDocException = new XmlDocException(exName, node.InnerText, range, exceptionType);

                    _exceptions.Add(xmlDocException);

                    return;
                }
            }
        }

        #endregion
    }
}