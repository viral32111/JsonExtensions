using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.NestedExtensions;

public class NestedHasTests {

	[ Fact ]
	public void NestedHas() {
		JsonObject jsonObject = new() {
			[ "one" ] = "hello"
		};

		Assert.True( jsonObject.NestedHas( "one" ) );
	}

	[ Fact ]
	public void NestedHasNullPropertyValue() {
		JsonObject jsonObject = new() {
			[ "one" ] = null
		};

		Assert.True( jsonObject.NestedHas( "one" ) );
	}

	[ Fact ]
	public void NestedHasNullParentObject() {
		JsonObject jsonObject = new() {
			[ "one" ] = null
		};

		Assert.Throws<JsonPropertyNullException>( () => {
			jsonObject.NestedHas( "one.two" );
		} );
	}

	[ Fact ]
	public void NestedHasNonExistentProperty() {
		JsonObject jsonObject = new();

		Assert.False( jsonObject.NestedHas( "one" ) );
	}

	[ Theory ]
	[ InlineData( "one.two", 123 ) ]
	[ InlineData( "one.two.three", 456 ) ]
	[ InlineData( "one.two.three.four", 789 ) ]
	public void NestedHasDepth( string path, int value ) {
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

		Assert.True( jsonObject.NestedHas( path ) );
	}

}
