using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.NestedExtensions;

public class NestedListTests {

	[ Fact ]
	public void NestedList() {
		JsonObject jsonObject = new() {
			[ "one" ] = "hello"
		};

		Assert.Single( jsonObject.NestedList() );
		Assert.Contains( "one", jsonObject.NestedList() );
	}

	[ Fact ]
	public void NestedListEmpty() {
		JsonObject jsonObject = new();

		Assert.Empty( jsonObject.NestedList() );
	}

	[ Fact ]
	public void NestedListNullPropertyValue() {
		JsonObject jsonObject = new() {
			[ "one" ] = null
		};

		Assert.Single( jsonObject.NestedList() );
		Assert.Contains( "one", jsonObject.NestedList() );
	}

	[ Theory ]
	[ InlineData( "one.two", 123 ) ]
	[ InlineData( "one.two.three", 456 ) ]
	[ InlineData( "one.two.three.four", 789 ) ]
	public void NestedListDepth( string path, int value ) {
		JsonObject jsonObject = new();

		JsonObject nextObject = jsonObject;
		string[] propertyNames = path.Split( Defaults.NestedPropertyDelimiter );
		foreach ( string propertyName in propertyNames ) {
			if ( propertyName == propertyNames.Last() ) {
				nextObject[ propertyName ] = value;
			} else {
				if ( !nextObject.ContainsKey( propertyName ) ) nextObject.Add( propertyName, new JsonObject() );
				nextObject = nextObject[ propertyName ]!.AsObject(); // We are criminals, but this is just a test
			}
		}

		Assert.Single( jsonObject.NestedList() );
		Assert.Contains( path, jsonObject.NestedList() );
	}

	[ Fact ]
	public void NestedListMultiple() {
		JsonObject jsonObject = new() {
			[ "one" ] = 123,
			[ "two" ] = 456,
			[ "three" ] = new JsonObject() {
				[ "four" ] = 789
			}
		};

		Assert.Equal( 3, jsonObject.NestedList().Length );
		Assert.Contains( "one", jsonObject.NestedList() );
		Assert.Contains( "two", jsonObject.NestedList() );
		Assert.Contains( "three.four", jsonObject.NestedList() );
	}

}
