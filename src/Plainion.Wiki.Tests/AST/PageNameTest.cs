using System.Linq;
using Plainion.Wiki.AST;
using NUnit.Framework;
using System;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PageNameTest
    {
        [Test]
        public void Create_WithEmptyName_ShouldThrow( [Values( null, "", " " )] string name )
        {
            Assert.Throws<ArgumentNullException>( () => PageName.Create( null, name ) );
        }

        [Test]
        public void Create_WithNullNamespace_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>( () => PageName.Create( null, "SamplePage" ) );
        }

        [Test]
        public void Create_WithNameAndNamespace_NameAndNamespacePropertiesAreSet()
        {
            var name = "p";
            var ns = PageNamespace.Create( "e1", "e2" );

            var pageName = PageName.Create( ns, name );

            Assert.That( pageName.Name, Is.EqualTo( name ) );
            Assert.That( pageName.Namespace, Is.EqualTo( ns ) );
        }

        [Test]
        public void Create_WithNameAndNamespace_FullNameJoinsPathAndName()
        {
            var name = "p";
            var ns = PageNamespace.Create( "e1", "e2" );

            var pageName = PageName.Create( ns, name );

            Assert.AreEqual( "/e1/e2/p", pageName.FullName );
        }

        [Test]
        public void Create_WithEmptyNamespace_FullNameContainsNameOnly()
        {
            var pageName = PageName.Create( PageNamespace.Create(), "p" );

            Assert.AreEqual( "/p", pageName.FullName );
        }

        [Test]
        public void Create_WithNameOnly_NamePropertyShouldBeSet()
        {
            var name = "p";

            var pageName = PageName.Create( name );

            Assert.AreEqual( name, pageName.Name );
        }

        [Test]
        public void Create_WithNameOnly_NamespaceShouldBeEmpty()
        {
            var pageName = PageName.Create( "p" );

            Assert.IsTrue( pageName.Namespace.IsEmpty );
        }

        [Test]
        public void Create_NameOnly_FullNameContainsNameOnly()
        {
            var pageName = PageName.Create( "p" );

            Assert.AreEqual( "/p", pageName.FullName );
        }

        [Test]
        public void Create_WithEmptyNameOnly_ShouldThrowException( [Values( null, "", " " )] string name )
        {
            Assert.Throws<ArgumentNullException>( () => PageName.Create( name ) );
        }

        [Test]
        public void CreateFromPath_WithPath_NameAndNamespacePropertiesShouldBeSet()
        {
            var path = "/e1/p";

            var pageName = PageName.CreateFromPath( path );

            Assert.That( pageName.Name, Is.EqualTo( "p" ) );
            Assert.That( pageName.Namespace, Is.EqualTo( PageNamespace.Create( "e1" ) ) );
        }

        [Test]
        public void CreateFromPath_WithPath_FullNameShouldEqualInput()
        {
            var path = "/e1/p";

            var pageName = PageName.CreateFromPath( path );

            Assert.AreEqual( path, pageName.FullName );
        }

        [Test]
        public void CreateFromPath_WithEmptyPath_ShouldThrowException( [Values( null, "", " " )] string path )
        {
            Assert.Throws<ArgumentNullException>( () => PageName.Create( path ) );
        }

        [Test]
        public void Equals_PageNamesWithSameFullName_ShouldBeTrue()
        {
            var ns1 = PageName.CreateFromPath( "/e1/p1" );
            var ns2 = PageName.CreateFromPath( "/e1/p1" );

            Assert.IsTrue( ns1.Equals( ns2 ) );
            Assert.IsTrue( ns1 == ns2 );
            Assert.IsFalse( ns1 != ns2 );
            Assert.AreEqual( ns1.GetHashCode(), ns2.GetHashCode() );
        }

        [Test]
        public void Equals_PageNamesWithDifferentFullName_ShouldBeFalse()
        {
            var ns1 = PageName.CreateFromPath( "/e1/p1" );
            var ns2 = PageName.CreateFromPath( "/e1/p2" );

            Assert.IsFalse( ns1.Equals( ns2 ) );
            Assert.IsFalse( ns1 == ns2 );
            Assert.IsTrue( ns1 != ns2 );
            Assert.AreNotEqual( ns1.GetHashCode(), ns2.GetHashCode() );
        }

        [Test]
        public void Equals_WithNull_ShouldBeFalse()
        {
            var ns1 = PageName.CreateFromPath( "/e1/p1" );

            Assert.IsFalse( ns1.Equals( null ) );
            Assert.IsFalse( ns1 == null );
            Assert.IsTrue( ns1 != null );
        }
    }
}
