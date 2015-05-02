using NUnit.Framework;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PlainTextTests : TestBase
    {
        [Test]
        public void Ctor_WithoutText_TextWillBeEmpty()
        {
            var text = new PlainText();

            Assert.That( text.Text, Is.EqualTo( string.Empty ) );
        }

        [Test]
        public void Ctor_WithText_TextWillBeSet()
        {
            var text = new PlainText( "a" );

            Assert.That( text.Text, Is.EqualTo( "a" ) );
        }

        [Test]
        public void Consume_WhenCalled_AddsText()
        {
            var text = new PlainText( "a" );

            text.Consume( "b" );

            Assert.That( text.Text, Is.EqualTo( "ab" ) );
        }

        [Test]
        public override void Clone_WhenCalled_ShouldNotThrow()
        {
            Objects.Clone( new PlainText() );
        }
    }
}
