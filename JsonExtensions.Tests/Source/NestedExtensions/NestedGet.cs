using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.NestedExtensions;

public class NestedGetTests {

	[ Fact ]
	public void NestedGetAsType() {
		JsonObject jsonObject = new() {
			[ "one" ] = "hello"
		};

		Assert.Equal( "hello", jsonObject.NestedGet<string?>( "one" ) );
	}

	[ Fact ]
	public void NestedGetAsCast() {
		JsonObject jsonObject = new() {
			[ "one" ] = "hello"
		};

		Assert.Equal( "hello", ( string? ) jsonObject.NestedGet( "one" ) );
	}

	[ Fact ]
	public void NestedGetNullPropertyValue() {
		JsonObject jsonObject = new() {
			[ "one" ] = null
		};

		Assert.Null( jsonObject.NestedGet( "one" ) );
	}

	[ Fact ]
	public void NestedGetNullParentObject() {
		JsonObject jsonObject = new() {
			[ "one" ] = null
		};

		Assert.Throws<JsonPropertyNullException>( () => {
			jsonObject.NestedGet( "one.two" );
		} );
	}

	[ Fact ]
	public void NestedGetNonExistentProperty() {
		JsonObject jsonObject = new() {};

		Assert.Throws<JsonPropertyNotFoundException>( () => {
			jsonObject.NestedGet( "one" );
		} );
	}

	[ Theory ]
	[ InlineData( "one.two", 123 ) ]
	[ InlineData( "one.two.three", 456 ) ]
	[ InlineData( "one.two.three.four", 789 ) ]
	public void NestedGetDepth( string path, int value ) {
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

		Assert.Equal( value, jsonObject.NestedGet<int?>( path ) );
	}

}
