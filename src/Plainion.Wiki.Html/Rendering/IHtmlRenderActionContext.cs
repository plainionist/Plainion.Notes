using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering
{
    /// <summary/>
    public interface IHtmlRenderActionContext : IRenderActionContext
    {
        /// <summary/>
        HtmlStylesheet Stylesheet { get; }
    }
}
