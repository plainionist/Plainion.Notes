using System.Linq;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests
{
    internal static class AstExtensions
    {
        /// <summary>
        /// Returns the containing plain text if there is only one child
        /// which is of type PlainText, otherwise null.
        /// </summary>
        public static string Text( this TextBlock self )
        {
            if( self.Children.Count() != 1 )
            {
                return null;
            }

            var plainText = self.Children.OfType<PlainText>().SingleOrDefault();
            if( plainText == null )
            {
                return null;
            }

            return plainText.Text;
        }
    }
}
