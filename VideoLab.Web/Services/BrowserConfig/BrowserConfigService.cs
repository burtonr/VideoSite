using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using Boilerplate.Web.Mvc;

namespace VideoLab.Web.Services.BrowserConfig
{
    public class BrowserConfigService : IBrowserConfigService
    {
        private readonly UrlHelper urlHelper;

        public BrowserConfigService(UrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        /// <summary>
        /// Gets the browserconfig XML for the current site. This allows you to customize the tile, when a user pins 
        /// the site to their Windows 8/10 start screen. See http://www.buildmypinnedsite.com and 
        /// https://msdn.microsoft.com/en-us/library/dn320426%28v=vs.85%29.aspx
        /// </summary>
        /// <returns>The browserconfig XML for the current site.</returns>
        public string GetBrowserConfigXml()
        {
            // The URL to the 70x70 small tile image.
            string square70x70logoUrl = this.urlHelper.Content("~/content/icons/mstile-70x70.png");
            // The URL to the 150x150 medium tile image.
            string square150x150logoUrl = this.urlHelper.Content("~/content/icons/mstile-150x150.png");
            // The URL to the 310x310 large tile image.
            string square310x310logoUrl = this.urlHelper.Content("~/content/icons/mstile-310x310.png");
            // The URL to the 310x150 wide tile image.
            string wide310x150logoUrl = this.urlHelper.Content("~/content/icons/mstile-310x150.png");
            // The colour of the tile. This colour only shows if part of your images above are transparent.
            string tileColour = "#1E1E1E";

            XDocument document = new XDocument(
                new XElement("browserconfig",
                    new XElement("msapplication",
                        new XElement("tile",
                            new XElement("square70x70logo",
                                new XAttribute("src", square70x70logoUrl)),
                            new XElement("square150x150logo",
                                new XAttribute("src", square150x150logoUrl)),
                            new XElement("square310x310logo",
                                new XAttribute("src", square310x310logoUrl)),
                            new XElement("wide310x150logo",
                                new XAttribute("src", wide310x150logoUrl)),
                            new XElement("TileColor", tileColour)))));

            return document.ToString(Encoding.UTF8);
        }
    }
}
