using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Query;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class NoParentFromClauseTests
    {
        [Test]
        public void Ctor_WithNull_Throws()
        {
            Assert.Throws<ArgumentNullException>( () => new NoParentFromClause( null ) );
        }

        [Test]
        public void IsQueryFromPageAllowed_WithChild_ReturnsTrue()
        {
            var clause = new NoParentFromClause( PageName.CreateFromPath( "/a" ) );
            var body = new PageBody( PageName.CreateFromPath( "/a/b" ) );

            bool isAllowed = clause.IsQueryFromPageAllowed( body );

            Assert.True( isAllowed );
        }

        [Test]
        public void IsQueryFromPageAllowed_WithSibling_ReturnsTrue()
        {
            var clause = new NoParentFromClause( PageName.CreateFromPath( "/a" ) );
            var body = new PageBody( PageName.CreateFromPath( "/b" ) );

            bool isAllowed = clause.IsQueryFromPageAllowed( body );

            Assert.True( isAllowed );
        }

        [Test]
        public void IsQueryFromPageAllowed_WithParent_ReturnsFalse()
        {
            var clause = new NoParentFromClause( PageName.CreateFromPath( "/a/b" ) );
            var body = new PageBody( PageName.CreateFromPath( "/a" ) );

            bool isAllowed = clause.IsQueryFromPageAllowed( body );

            Assert.False( isAllowed );
        }

        [Test]
        public void IsQueryFromPageAllowed_WithDifferentParent_ReturnsFalse()
        {
            var clause = new NoParentFromClause( PageName.CreateFromPath( "/a/b" ) );
            var body = new PageBody( PageName.CreateFromPath( "/x/c" ) );

            bool isAllowed = clause.IsQueryFromPageAllowed( body );

            Assert.False( isAllowed );
        }
    }
}
