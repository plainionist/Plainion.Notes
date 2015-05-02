using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.AST;

namespace Plainion.Wiki.Http.Views
{
    /// <summary/>
    public class HtmlUtils
    {
        /// <summary/>
        public static Content CreateEditForm(string action, params string[] textAreaContent)
        {
            var html = new HtmlBlock();

            html.AppendLine("<form id='edit-form' name='content' method=\"post\" action=\"" + action + "\">");
            html.AppendLine("<p style=\"width:100%\">");
            html.AppendLine("<input type=\"submit\" name=\"Save\" value=\" Save \" />");
            html.AppendLine("<input type=\"submit\" name=\"SaveAndEdit\" value=\" Save & Edit \" />");
            html.AppendLine("</p>");

            html.AppendLine("<textarea id=\"edit-pagetext\" name=\"text\">");

            foreach (var line in textAreaContent)
            {
                html.AppendLine(line);
            }

            html.AppendLine("</textarea>");
            html.AppendLine("</form>");

            return new Content(html);
        }
    }
}
