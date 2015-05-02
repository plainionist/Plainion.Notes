using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using Plainion.Wiki.Utils;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Testing
{
    public class XAssert
    {
        public static T HasSingleChildOf<T>( PageNode node ) where T : PageLeaf
        {
            var childrenOfT = node.Children.OfType<T>().ToList();

            Assert.AreEqual( 1, childrenOfT.Count, "Found more than one child of type '" + typeof( T ) + "'" );

            var child = childrenOfT.Single();
            Assert.AreEqual( node, child.Parent );

            return child;
        }

        public static IList<T> HasNChildrenOf<T>( PageNode node, int count ) where T : PageLeaf
        {
            var childrenOfT = node.Children.OfType<T>().ToList();

            Assert.AreEqual( count, childrenOfT.Count, "Found more than " + count + " child of type '" + typeof( T ) + "'" );

            childrenOfT.ForEach( child => Assert.AreEqual( node, child.Parent ) );

            return childrenOfT;
        }

        public static T NthChildIsOf<T>( PageNode node, int index ) where T : PageLeaf
        {
            Assert.IsTrue( node.Children.Count() > index, "Given child index " + index + " is greater than child count " + node.Children.Count() );

            var child = node.Children.ElementAt( index );

            Assert.IsInstanceOf<T>( child );
            Assert.AreEqual( node, child.Parent );

            return (T)child;
        }

        public static void LinkEquals( string urlOfExpected, Link actual )
        {
            LinkEquals( new Link( urlOfExpected ), actual );
        }

        public static void LinkEquals( Link expected, Link actual )
        {
            Assert.AreEqual( expected.Url, actual.Url, "Link.Url differs" );
            Assert.AreEqual( expected.Text, actual.Text, "Link.Text differs" );
            Assert.AreEqual( expected.IsExternal, actual.IsExternal, "Link.IsExternal differs" );
        }

        public static void PlainTextEquals( string expectedText, PlainText actual )
        {
            PlainTextEquals( new PlainText( expectedText ), actual );
        }

        public static void PlainTextEquals( PlainText expected, PlainText actual )
        {
            Assert.AreEqual( expected.Text, actual.Text, "PlainText.Text differs" );
        }

        public static void AnchorEquals( Anchor expected, Anchor actual )
        {
            Assert.AreEqual( expected.Name, actual.Name, "Anchor.Name differs" );
        }

        public static void PageAttributeEquals( PageAttribute expected, PageAttribute actual )
        {
            Assert.AreEqual( expected.Type, actual.Type, "PageAttribute.Type differs" );
            Assert.AreEqual( expected.Name, actual.Name, "PageAttribute.Name differs" );
            Assert.AreEqual( expected.Value, actual.Value, "PageAttribute.Value differs" );
        }

        public static void ContentEquals( PageLeaf[] expected, PageLeaf[] actual )
        {
            for ( int i = 0; i < expected.Length; ++i )
            {
                var expectedChild = expected[ i ];

                Assert.IsTrue( i < actual.Length, "Expected following item in parser output: " + expectedChild );

                var actualChild = actual[ i ];

                XAssert.ContentEquals( expectedChild, actualChild );
            }
        }

        public static void ContentEquals( PageLeaf expected, PageLeaf actual )
        {
            Assert.AreEqual( expected.GetType(), actual.GetType(), "Expected and actual child types differ" );

            if ( expected is PlainText )
            {
                XAssert.PlainTextEquals( (PlainText)expected, (PlainText)actual );
            }
            else if ( expected is Link )
            {
                XAssert.LinkEquals( (Link)expected, (Link)actual );
            }
            else if ( expected is Anchor )
            {
                XAssert.AnchorEquals( (Anchor)expected, (Anchor)actual );
            }
            else if ( expected is PageAttribute )
            {
                XAssert.PageAttributeEquals( (PageAttribute)expected, (PageAttribute)actual );
            }
            else
            {
                Assert.Fail( "Dont know how to assert content of type " + expected.GetType() );
            }
        }

        public static void IsPageMatchOfPage( QueryMatch match, PageName name )
        {
            var link = match.DisplayText as Link;
            Assert.NotNull( link, "QueryMatch is no PageMatch" );

            Assert.That( link.Url, Is.EqualTo( name.FullName ), "QueryMatch does not reference given page" );
        }

        public static void PluginCatalogContains<TKey, TValue>( IDictionary<TKey, Type> expectedPlugins, IPluginCatalog<TKey, TValue> catalog )
        {
            foreach ( var pair in expectedPlugins )
            {
                bool keyExists = catalog.Plugins.Keys.Any( key => key.Equals( pair.Key ) );
                Assert.IsTrue( keyExists, "No plugin for key {0} found", pair.Key );

                var pluginInstance = catalog.Plugins[ pair.Key ];
                Assert.That( pluginInstance, Is.InstanceOf( pair.Value ),
                    "Plugin for stage {0} has wrong type", pair.Key );
            }
        }

        public static void IsChildOf( PageLeaf child, PageNode parent )
        {
            Assert.That( parent.Children, Contains.Item( child ) );
            Assert.That( child.Parent, Is.SameAs( parent ) );
        }

        /// <summary>
        /// Checks that a node which was a child previously is now nolonger
        /// a child of the given parent. Also checks that the parent of the
        /// old child is now null.
        /// </summary>
        public static void IsNoLongerChildOf( PageLeaf child, PageNode parent )
        {
            Assert.That( parent.Children, Is.Not.Contains( child ) );
            Assert.That( child.Parent, Is.Null );
        }
    }
}
