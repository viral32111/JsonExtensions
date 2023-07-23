using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.NestedExtensions;

public class NestedSetTests {

	[ Fact ]
	public void NestedSet() {
		JsonObject jsonObject = new();
		jsonObject.NestedSet( "one", "hello" );

		Assert.Equal( "hello", ( string? ) jsonObject[ "one" ] );
	}

	[ Fact ]
	public void NestedSetNullPropertyValue() {
		JsonObject jsonObject = new();
		jsonObject.NestedSet( "one", null );

		Assert.Null( jsonObject[ "one" ] );
	}

	[ Fact ]
	public void NestedSetNullParentObject() {
		JsonObject jsonObject = new() {
			[ "one" ] = null
		};

		Assert.Throws<JsonPropertyNullException>( () => {
			jsonObject.NestedSet( "one.two", null );
		} );
	}

	[ Theory ]
	[ InlineData( "one.two", 123 ) ]
	[ InlineData( "one.two.three", 456 ) ]
	[ InlineData( "one.two.three.four", 789 ) ]
	public void NestedSetDepth( string path, int value ) {
		JsonObject jsonObject = new();
		jsonObject.NestedSet( path, value );

		JsonNode? nestedValue = jsonObject;
		foreach ( string propertyName in path.Split( Defaults.NestedPropertyDelimiter ) ) {
			nestedValue = nestedValue?[ propertyName ];
		}

		Assert.Equal( value, ( int? ) nestedValue );
	}

}
