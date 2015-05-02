
namespace Plainion.Wiki.Html.Rendering
{
    /// <summary>
    /// Describes the layout of a rendered Wiki page.
    /// </summary>
    public class HtmlStylesheet
    {
        public HtmlStylesheet()
        {
            ExternalStylesheet = null;
            ExternalJavascript = null;
        }

        public string ExternalStylesheet
        {
            get;
            set;
        }

        public string ExternalJavascript
        {
            get;
            set;
        }
    }
}
