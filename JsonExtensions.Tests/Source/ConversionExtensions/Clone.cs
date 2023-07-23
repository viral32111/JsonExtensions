using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.ConversionExtensions;

public class ConversionClone {

	[ Fact ]
	public void Clone() {
		JsonObject jsonObject = new() {
			[ "one" ] = 123
		};

		JsonObject? cloneObject = jsonObject.Clone()!.AsObject(); // We are criminals, but this is just a test

		Assert.Equal( JsonSerializer.Serialize( jsonObject ), JsonSerializer.Serialize( cloneObject ) ); // Alternative to deep equal
		Assert.NotSame( jsonObject, cloneObject ); // Not the same reference/pointer
		Assert.NotStrictEqual( jsonObject, cloneObject ); // Not the same object

		Assert.Contains( "one", cloneObject );
		Assert.Equal( 123, ( int? ) cloneObject[ "one" ] );
	}

	[ Theory ]
	[ InlineData( "one.two", 123 ) ]
	[ InlineData( "one.two.three", 456 ) ]
	[ InlineData( "one.two.three.four", 789 ) ]
	public void CloneDepth( string path, int value ) {
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

		JsonObject? cloneObject = jsonObject.Clone()!.AsObject(); // We are criminals, but this is just a test

		Assert.Equal( JsonSerializer.Serialize( jsonObject ), JsonSerializer.Serialize( cloneObject ) ); // Alternative to deep equal
		Assert.NotSame( jsonObject, cloneObject ); // Not the same reference/pointer
		Assert.NotStrictEqual( jsonObject, cloneObject ); // Not the same object

		JsonNode? nestedValue = jsonObject;
		foreach ( string propertyName in path.Split( Defaults.NestedPropertyDelimiter ) ) {
			nestedValue = nestedValue?[ propertyName ];
		}

		Assert.Equal( value, ( int? ) nestedValue );
	}

	[ Fact ]
	public void AsArrayInteger() {
		JsonArray jsonArray = new() { 1, 2, 3 };

		Assert.Equal( 3, jsonArray.AsArray<int>().Length );
		Assert.Equal( new int[] { 1, 2, 3 }, jsonArray.AsArray<int>() );
	}

	[ Fact ]
	public void AsArrayString() {
		JsonArray jsonArray = new() { "one", "two", "three" };

		Assert.Equal( 3, jsonArray.AsArray<string>().Length );
		Assert.Equal( new string[] { "one", "two", "three" }, jsonArray.AsArray<string>() );
	}

}
