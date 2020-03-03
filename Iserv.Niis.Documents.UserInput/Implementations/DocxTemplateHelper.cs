using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Aspose.Words;
using Aspose.Words.Markup;
using ImageMagick;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Documents.UserInput.Helpers;

namespace Iserv.Niis.Documents.UserInput.Implementations
{
    public class DocxTemplateHelper : IDocxTemplateHelper
    {
        static DocxTemplateHelper()
        {
            new License().SetLicense(AsposeLicenseHelper.Stream);
        }

        public void FillUserInputContent(MemoryStream stream, List<KeyValuePair<string, string>> userInputFields, string defaultValue)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (userInputFields == null) return;

            var userInputFieldsDictionaty = userInputFields.ToDictionary(pair => pair.Key.Trim(), pair => pair.Value);

            stream.Seek(0, SeekOrigin.Begin);

            var document = new Document(stream, new LoadOptions { LoadFormat = LoadFormat.Docx });
            var builder = new DocumentBuilder(document);

            var foundUserInputFields = document
                .GetChildNodes(NodeType.StructuredDocumentTag, true)
                .ToArray()
                .Where(x => userInputFieldsDictionaty.Keys.Contains(((StructuredDocumentTag)x).Title.Trim()))
                .Select(x => (StructuredDocumentTag)x);

            var structuredDocumentTags = foundUserInputFields as IList<StructuredDocumentTag> ?? foundUserInputFields.ToList();
            if (!structuredDocumentTags.Any()) return;

            foreach (var std in structuredDocumentTags)
                FillUserInputControl(std, userInputFieldsDictionaty, document, builder, defaultValue);

            UpdateSourceStream(stream, document);
        }

        public void RemoveComments(MemoryStream stream)
        {
            var document = new Document(stream, new LoadOptions { LoadFormat = LoadFormat.Docx });

            var comments = document.GetChildNodes(NodeType.Comment, true);
            if (comments.Count == 0) return;

            foreach (Node comment in comments)
                comment.Remove();

            UpdateSourceStream(stream, document);
        }

        public Dictionary<string, string> GetUserInputFieldNames(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            Document document;
            using (var ms = new MemoryStream(buffer))
            {
                document = new Document(ms);
            }

            return document
                    .GetChildNodes(NodeType.StructuredDocumentTag, true)
                    .ToArray()
                    .Where(x => ((StructuredDocumentTag) x).Title.Trim().EndsWith("_RichUserInput")
                                || ((StructuredDocumentTag) x).Title.Trim().EndsWith("_UserInputDefault")
                                || ((StructuredDocumentTag) x).Title.Trim().EndsWith("_UserInput"))
                    .GroupBy(x => ((StructuredDocumentTag) x).Title.Trim())
                    .Select(x => x.First())
                    .ToDictionary(x => ((StructuredDocumentTag) x).Title.Trim(),
                        x => ((StructuredDocumentTag) x).GetText().Trim())
                ;
        }

        //public List<string> GetDocumentTags(byte[] buffer)
        //{
        //    if (buffer == null) throw new ArgumentNullException(nameof(buffer));

        //    Document document;
        //    using (var ms = new MemoryStream(buffer))
        //    {
        //        document = new Document(ms);
        //    }

        //    return document
        //        .GetChildNodes(NodeType.StructuredDocumentTag, true)
        //        .ToArray()
        //        .Select(x => ((StructuredDocumentTag)x).Title)
        //        .ToList();
        //}

        private void FillUserInputControl(StructuredDocumentTag std,
            Dictionary<string, string> userInputFieldsDictionaty, Document document,
            DocumentBuilder builder, string defaultValue)
        {
            //std.RemoveAllChildren();

            Paragraph paragraph;
            var html = userInputFieldsDictionaty[std.Title.Trim()];
            if (string.IsNullOrEmpty(html)) return;
            html = TrimParagraphTag(html);
            html = ResizeBase64Images(html);

            //была проблема при вставке html если тег находился в параграфе
            //если Content Control находится не в Body, а во вложенном теге
            if (std.ParentNode.NodeType != NodeType.Body)
            {
                //проверяем текущее поле на простой текст без форматирования
                if (std.Title.Trim().EndsWith("_UserInput"))
                {
                    if (std.ParentNode.NodeType == NodeType.Paragraph)
                    {
                        var run = new Run(document, html);

                        // set font
                        Inline inlineElement = GetFirstInline(std.ChildNodes);
                        if (inlineElement != null)
                            CopyFont(run.Font, inlineElement.Font);

                        std.RemoveAllChildren();
                        std.AppendChild(run);
                        std.RemoveSelfOnly();
                    }
                    else
                    {
                        paragraph = (Paragraph)std.ParentNode.InsertAfter(new Paragraph(document), std);

                        // set font
                        Paragraph paragraphSource = (Paragraph)std.FirstChild;
                        Inline inlineElement = GetFirstInline(paragraphSource.ChildNodes);
                        var run = new Run(document, html);
                        if (inlineElement != null)
                            CopyFont(run.Font, inlineElement.Font);

                        paragraph.AppendChild(run);
                        std.Remove();
                    }
                }
                //если поле со значение по умолчанию
                else if (std.Title.Trim().EndsWith("_UserInputDefault"))
                {
                    html = html.Equals(string.Empty) ? defaultValue : html;
                    if (std.ParentNode.NodeType == NodeType.Paragraph)
                    {
                        var run = new Run(document, html);

                        // set font
                        Inline inlineElement = GetFirstInline(std.ChildNodes);
                        if (inlineElement != null)
                            CopyFont(run.Font, inlineElement.Font);

                        std.RemoveAllChildren();
                        std.AppendChild(run);
                        std.RemoveSelfOnly();
                    }
                    else
                    {
                        paragraph = (Paragraph)std.ParentNode.InsertAfter(new Paragraph(document), std);

                        // set font
                        Paragraph paragraphSource = (Paragraph)std.FirstChild;
                        Inline inlineElement = GetFirstInline(paragraphSource.ChildNodes);
                        var run = new Run(document, html);
                        if (inlineElement != null)
                            CopyFont(run.Font, inlineElement.Font);

                        paragraph.AppendChild(run);
                        std.Remove();
                    }
                }
                //если текст из wysiwyg редактора
                else
                {
                    std.RemoveAllChildren();

                    if (std.ParentNode.NodeType == NodeType.Paragraph)
                    {
                        builder.MoveTo(std);
                        builder.InsertHtml(html, true);
                        std.RemoveSelfOnly();
                    }
                    else
                    {
                        paragraph = (Paragraph)std.ParentNode.InsertAfter(new Paragraph(document), std);
                        builder.MoveTo(paragraph);
                        builder.InsertHtml(html, true);

                        std.AppendChild(paragraph);
                        std.RemoveSelfOnly();
                    }
                }
            }
            //если Content Control находится в корне тега Body
            else
            {
                if (std.Title.Trim().EndsWith("_UserInput"))
                {
                    paragraph = (Paragraph)std.ParentNode.InsertAfter(new Paragraph(document), std);

                    // set font
                    Paragraph paragraphSource = (Paragraph)std.FirstChild;
                    Inline inlineElement = GetFirstInline(paragraphSource.ChildNodes);
                    var run = new Run(document, html);
                    if (inlineElement != null)
                        CopyFont(run.Font, inlineElement.Font);

                    paragraph.AppendChild(run);
                    std.Remove();
                }
                else if (std.Title.Trim().EndsWith("_UserInputDefault"))
                {
                    html = html.Equals(string.Empty) ? defaultValue : html;
                    paragraph = (Paragraph)std.ParentNode.InsertAfter(new Paragraph(document), std);

                    // set font
                    Paragraph paragraphSource = (Paragraph)std.FirstChild;
                    Inline inlineElement = GetFirstInline(paragraphSource.ChildNodes);
                    var run = new Run(document, html);
                    if (inlineElement != null)
                        CopyFont(run.Font, inlineElement.Font);

                    paragraph.AppendChild(run);
                    std.Remove();
                }
                //если текст из wysiwyg редактора
                else
                {
                    paragraph = (Paragraph)std.ParentNode.InsertAfter(new Paragraph(document), std);

                    builder.MoveTo(paragraph);
                    builder.InsertHtml(html, true);
                    std.Remove();
                }
            }
        }

        /// <summary>
        ///     Изменяет размер изображения в соответствии со значением в аттрибуте width
        ///     При вставке через Aspose.Words высота изображения оставалась прежней (вытягивалось по высоте)
        /// </summary>
        /// <param name="html"></param>
        private string ResizeBase64Images(string html)
        {
            var matches = Regex.Matches(html,
                "src=\\\"(?<base64image>data:image\\/(?<type>[a-zA-Z]*);base64,(?<string>[^\\\"]*))\\\".+?width=\\\"(?<width>\\d+)\\\"");

            foreach (Match match in matches)
            {
                var buffer = Convert.FromBase64String(match.Groups["string"].Value);

                if (!int.TryParse(match.Groups["width"].Value, out var newWidth)) continue;

                using (var image = new MagickImage(buffer))
                {
                    var newHeight = newWidth * image.Height / image.Width;

                    image.Resize(newWidth, newHeight);

                    var base64String = image.ToBase64(MagickFormat.Png);

                    html = html.Replace(match.Groups["base64image"].Value,
                        $@"data:image/png;base64,{base64String}");
                }
            }

            return html;
        }

        /// <summary>
        ///     Удаляет тег 'p', чтобы при вставке в текст не было переноса на новую строку
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string TrimParagraphTag(string html)
        {
            return html.Trim().StartsWith("<p>")
                ? html.Substring(3, html.Length - 7)
                : html;
        }

        /// <summary>
        /// Установить шрифт из документа для пользовательского ввода
        /// https://forum.aspose.com/t/get-precise-font-information-from-sdt-control-properties/58473/6
        /// </summary>
        /// <param name="destinationFont">Шрифт приемник</param>
        /// <param name="sourceFont">Шрифт источник</param>
        private void CopyFont(Font destinationFont, Font sourceFont)
        {
            destinationFont.Size = sourceFont.Size;
            destinationFont.Color = sourceFont.Color;
            destinationFont.Bold = sourceFont.Bold;
            destinationFont.Italic = sourceFont.Italic;
        }

        /// <summary>
        /// Получить Класс Inline из коллекции элементов (Node)
        /// </summary>
        /// <param name="nodeCollection"> коллекция элементов</param>
        /// <returns>Inline объект (обычно это Run)</returns>
        private Inline GetFirstInline(NodeCollection nodeCollection)
        {
            if (nodeCollection == null || nodeCollection.Count == 0) return null;

            foreach (Node node in nodeCollection)
            {
                if (node is Inline)
                    return (Inline)node;
            }

            return null;
        }

        private static void UpdateSourceStream(MemoryStream stream, Document document)
        {
            using (var outputMemoryStream = new MemoryStream())
            {
                document.Save(outputMemoryStream, SaveFormat.Docx);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Write(outputMemoryStream.ToArray(), 0, (int)outputMemoryStream.Length);
                stream.SetLength(stream.Position);
            }
        }
    }
}