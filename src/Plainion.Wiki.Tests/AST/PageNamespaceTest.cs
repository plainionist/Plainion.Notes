using System.Linq;
using Plainion.Wiki.AST;
using NUnit.Framework;
using System;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PageNamespaceTest
    {
        [Test]
        public void Create_WithEmptyPath_ShouldCreateEmptyNamespace( [Values( null, "", " " )] string path )
        {
            var ns = PageNamespace.Create( path );

            Assert.IsTrue( ns.IsEmpty );
        }

        [Test]
        public void Create_WithEmptyElements_ShouldCreateEmptyNamespace( [Values( null, new string[] { } )] string[] elements )
        {
            var ns = PageNamespace.Create( elements );

            Assert.IsTrue( ns.IsEmpty );
        }

        [Test]
        public void Create_WithoutParameters_ShouldCreateEmptyNamespace()
        {
            var ns = PageNamespace.Create();

            Assert.IsTrue( ns.IsEmpty );
        }

        [Test]
        public void Create_WithElements_ShouldCopyElementsToProperty()
        {
            string[] elements = { "Plainion", "Wiki" };
            var originalElements = elements.ToArray();

            var ns = PageNamespace.Create( elements );

            // modify the original array to make sure that a copy has been created
            elements[ 1 ] = "changed";

            Assert.That( ns.Elements, Is.EquivalentTo( originalElements ) );
        }

        [Test]
        public void Elements_EmptyNamespace_ShouldContainEmptyArray()
        {
            var ns = PageNamespace.Create();

            Assert.That( ns.Elements, Is.Empty );
        }

        [Test]
        public void AsPath_EmptyNamespace_ShouldContainSingleSlashOnly()
        {
            var ns = PageNamespace.Create();

            Assert.That( ns.AsPath, Is.EqualTo( "/" ) );
        }

        [Test]
        public void AsPath_NonEmptyNamespace_ShouldJoinElementsWithSlashes()
        {
            string[] elements = { "Plainion", "Wiki" };

            var ns = PageNamespace.Create( elements );

            Assert.That( ns.AsPath, Is.EqualTo( "/Plainion/Wiki" ) );
        }

        [Test]
        public void Equals_NamespacesWithSameElements_ShouldBeTrue()
        {
            var ns1 = PageNamespace.Create( "e1", "e2" );
            var ns2 = PageNamespace.Create( "e1", "e2" );

            Assert.IsTrue( ns1.Equals( ns2 ) );
            Assert.IsTrue( ns1 == ns2 );
            Assert.IsFalse( ns1 != ns2 );
            Assert.AreEqual( ns1.GetHashCode(), ns2.GetHashCode() );
        }

        [Test]
        public void Equals_NamespacesWithDifferentElements_ShouldBeFalse()
        {
            var ns1 = PageNamespace.Create( "e1", "e2" );
            var ns2 = PageNamespace.Create( "e1", "e3" );

            Assert.IsFalse( ns1.Equals( ns2 ) );
            Assert.IsFalse( ns1 == ns2 );
            Assert.IsTrue( ns1 != ns2 );
            Assert.AreNotEqual( ns1.GetHashCode(), ns2.GetHashCode() );
        }

        [Test]
        public void Equals_WithNull_ShouldBeFalse()
        {
            var ns1 = PageNamespace.Create( "e1", "e2" );

            Assert.IsFalse( ns1.Equals( null ) );
            Assert.IsFalse( ns1 == null );
            Assert.IsTrue( ns1 != null );
        }

        [Test]
        public void StartsWith_ValidPrefix_ShouldBeTrue()
        {
            var ns1 = PageNamespace.Create( "e1", "e2" );
            var ns2 = PageNamespace.Create( "e1", "e2", "e3" );

            Assert.IsTrue( ns2.StartsWith( ns1 ) );
        }

        [Test]
        public void StartsWith_EmptyPrefix_ShouldBeTrue()
        {
            var ns1 = PageNamespace.Create();
            var ns2 = PageNamespace.Create( "e1" );

            Assert.IsTrue( ns2.StartsWith( ns1 ) );
        }

        [Test]
        public void StartsWith_InvalidPrefix_ShouldBeFalse()
        {
            var ns1 = PageNamespace.Create( "n3" );
            var ns2 = PageNamespace.Create( "e1" );

            Assert.IsFalse( ns2.StartsWith( ns1 ) );
        }

        [Test]
        public void StartsWith_TooLongInput_ShouldBeFalse()
        {
            var ns1 = PageNamespace.Create( "e1", "e2" );
            var ns2 = PageNamespace.Create( "e1" );

            Assert.IsFalse( ns2.StartsWith( ns1 ) );
        }

        [Test]
        public void CutOffLeft_ValidPrefix_Succeeds()
        {
            var ns1 = PageNamespace.Create( "e1" );
            var ns2 = PageNamespace.Create( "e1", "e2" );

            var result = ns2.CutOffLeft( ns1 );

            Assert.That( result.AsPath, Is.EqualTo( "/e2" ) );
        }

        [Test]
        public void CutOffLeft_Itself_ReturnsEmptyNamespace()
        {
            var ns1 = PageNamespace.Create( "e1" );
            var ns2 = PageNamespace.Create( "e1" );

            var result = ns2.CutOffLeft( ns1 );

            Assert.IsTrue( result.IsEmpty );
        }

        [Test]
        public void CutOffLeft_InvalidNamespace_Throws()
        {
            var ns1 = PageNamespace.Create( "e3" );
            var ns2 = PageNamespace.Create( "e1", "e2" );

            Assert.Throws<ArgumentException>( () => ns2.CutOffLeft( ns1 ) );
        }

        [Test]
        public void Add_WhenCalled_NamespaceIsAdded()
        {
            var ns1 = PageNamespace.Create( "e3" );
            var ns2 = PageNamespace.Create( "e1", "e2" );

            var nsSum = ns1.Add( ns2 );

            Assert.That( nsSum.AsPath, Is.EqualTo( "/e3/e1/e2" ) );
        }
    }
}
