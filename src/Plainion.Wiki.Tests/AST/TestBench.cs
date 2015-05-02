using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plainion.Wiki.AST;
using NUnit.Framework;
using Plainion.Diagnostics;

namespace Plainion.Wiki.UnitTests.AST
{
    internal class TestBench
    {
        private IEnumerable<Type> myAstTypes;

        public TestBench()
        {
            myAstTypes = GetAllAstTypes();
        }

        private IEnumerable<Type> GetAllAstTypes()
        {
            return typeof( PageLeaf ).Assembly.GetTypes()
                 .Where( type => typeof( PageLeaf ).IsAssignableFrom( type ) )
                 .Where( type => !type.IsAbstract )
                 .Where( type => !typeof( CompiledQuery ).IsAssignableFrom( type ) ) // no "dynamic" ast type
                 .ToList();
        }

        public void ExpectConsumesOnly( PageNode nodeToTest, params Type[] allowedChildren )
        {
            ExpectConsumesOnly( nodeToTest, allowedChildren.ToList() );
        }

        public void ExpectConsumesOnly( PageNode nodeToTest, IEnumerable<Type> allowedChildren )
        {
            var content = new Content( nodeToTest );

            foreach ( var type in myAstTypes )
            {
                int nodeChildCount = nodeToTest.Children.Count();
                int parentChildCount = content.Children.Count();

                var instance = CreateInstance( type );
                nodeToTest.Consume( instance );

                if ( allowedChildren.Any( allowedType => allowedType.IsAssignableFrom( type ) ) )
                {
                    // cannot test this at the moment because we dont know "how" the node
                    // is consuming the other node (e.g. see Paragraph)
                    //Assert.AreEqual( nodeChildCount + 1, nodeToTest.Children.Count );
                    Assert.AreEqual( parentChildCount, content.Children.Count(),
                        string.Format( "Node should have consumed '{0}' but parent does", type ) );
                }
                else
                {
                    Assert.AreEqual( nodeChildCount, nodeToTest.Children.Count(),
                        string.Format( "Node consumed '{0}' but should not", type ) );
                    Assert.AreEqual( parentChildCount + 1, content.Children.Count(),
                        string.Format( "Node consumed '{0}' but should not", type ) );
                }
            }
        }

        private PageLeaf CreateInstance( Type astType )
        {
            var constructors = astType.GetConstructors();

            var constructor = constructors.OrderBy( ctor => ctor.GetParameters().Count() ).First();

            var parameters = constructor.GetParameters()
                .Select( param => GetParameter( astType, param ) );

            try
            {
                return (PageLeaf)constructor.Invoke( parameters.ToArray() );
            }
            catch ( Exception ex )
            {
                var message = string.Format( "Failed to create an instance of type {0}", astType );
                var exception = new Exception( message, ex );
                exception.Data[ "Constructor" ] = constructor.ToString();
                exception.Data[ "Parameters" ] = parameters.ToHuman();

                throw exception;
            }
        }

        private object GetParameter( Type astType, ParameterInfo paramInfo )
        {
            if ( paramInfo.ParameterType == typeof( string ) )
            {
                // types that do not accept empty strings
                if ( astType == typeof( Anchor ) )
                {
                    return "a"; // any valid string
                }
                else if ( astType == typeof( HighlightText ) )
                {
                    return "a"; // any valid string
                }
                else if ( astType == typeof( QueryDefinition ) )
                {
                    return "a"; // any valid string
                }

                return string.Empty;
            }

            if ( paramInfo.ParameterType == typeof( int ) )
            {
                if ( astType == typeof( HighlightText ) )
                {
                    return 1;
                }

                return 0;
            }

            if ( paramInfo.ParameterType == typeof( PageName ) )
            {
                return PageName.CreateFromPath( "a" ); 
            }

            if ( paramInfo.ParameterType.IsArray )
            {
                var arrayType = paramInfo.ParameterType.GetElementType().MakeArrayType();
                return Activator.CreateInstance( arrayType, new object[] { 0 } );
            }

            return null;
        }
    }
}
