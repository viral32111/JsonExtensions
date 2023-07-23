using System;
using System.IO;
using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace viral32111.JsonExtensions.Examples;

public class Program {

	// The file to create, read & write to
	private static readonly string filePath = Path.Combine( Path.GetTempPath(), "example.json" );

	// Program starting point...
	public static void Main( string[] arguments ) {

		// Will hold the root JSON object
		JsonObject? exampleObject;

		// Read the JSON from the file
		try {
			Console.WriteLine( "Reading from file '{0}'...", filePath );
			exampleObject = JsonExtensions.ReadFromFile( filePath );
			Console.WriteLine( "Read from file '{0}'.", filePath );

		// Create the file with an empty JSON object if it does not exist
		} catch ( FileNotFoundException ) {
			Console.WriteLine( "File '{0}' does not exist, creating it...", filePath );
			exampleObject = JsonExtensions.CreateNewFile( filePath );
			Console.WriteLine( "Created file '{0}' with empty JSON object.", filePath );
		}

		// Fail if the root JSON object is invalid
		if ( exampleObject == null ) throw new Exception( "Root JSON object is invalid" );

		/******************************************************************/

		// Set a property to a string
		exampleObject[ "hello" ] = "World!";
		Console.WriteLine( "hello: '{0}'", exampleObject[ "hello" ] );

		/******************************************************************/

		// Add a property as a simple JSON object
		if ( exampleObject.TryAdd( "object", new JsonObject() { [ "xyz" ] = 123 } ) ) {
			Console.WriteLine( "object added: '{0}'", exampleObject[ "object" ] );
		} else {
			Console.WriteLine( "object exists: '{0}'", exampleObject[ "object" ] );
		}

		// Get that property without a desired type, it could be null
		JsonNode? objectUnknownType = exampleObject.NestedGet( "object" );
		Console.WriteLine( "objectUnknownType: {0}", objectUnknownType );

		// Get that property as a JSON object
		JsonObject objectKnownType = exampleObject.NestedGet<JsonObject>( "object" );
		Console.WriteLine( "objectKnownType: {0}", objectKnownType );

		/******************************************************************/

		// Set a nested property to a number
		exampleObject.NestedSet( "one.two.three.four", 5678 );
		Console.WriteLine( "one.two.three.four: {0}", exampleObject.NestedGet<int>( "one.two.three.four" ) );

		// Get a nested boolean property that does not exist
		try {
			Console.WriteLine( "do.i.exist: {0}", exampleObject.NestedGet<bool>( "do.i.exist" ) );
		} catch ( JsonPropertyNotFoundException exception ) {
			Console.WriteLine( "Property does not exist: '{0}'", exception.Message );
		}

		/******************************************************************/

		// Set a property to an array of numbers
		exampleObject[ "numbers" ] = new JsonArray( 1, 2, 3, 4, 5, 6 );
		Console.WriteLine( "numbers: {0}", exampleObject[ "numbers" ] );

		// Get that property as a normal JSON array
		JsonArray numbersGenericArray = exampleObject.NestedGet<JsonArray>( "numbers" );
		Console.WriteLine( "numbersGenericArray: {0}", numbersGenericArray );

		// Get that property as an integer array
		int[] numbersIntArray = exampleObject[ "numbers" ]!.AsArray<int>();
		Console.WriteLine( "numbersIntArray: {0}", numbersIntArray.Length );

		// Get that property as an boolean array
		try {
			bool[] numbersBoolArray = exampleObject[ "numbers" ]!.AsArray<bool>();
			Console.WriteLine( "numbersBoolArray: {0}", numbersBoolArray.Length );
		} catch ( InvalidOperationException exception ) {
			Console.WriteLine( "Cannot get property as boolean array: '{0}'", exception.Message );
		}

		/******************************************************************/

		JsonNode? nodeCopy = exampleObject.NestedGet( "hello" ).Clone();
		Console.WriteLine( "Copied node: {0}", nodeCopy );

		/******************************************************************/

		bool oneTwoThreeFour = exampleObject.NestedHas( "one.two.three.four" );
		Console.WriteLine( "one.two.three.four: {0}", oneTwoThreeFour );

		bool doIExist = exampleObject.NestedHas( "do.i.exist" );
		Console.WriteLine( "do.i.exist: {0}", doIExist );

		/******************************************************************/

		exampleObject.NestedSet( "hello", null );

		string[] listOfPropertyPaths = exampleObject.NestedList();
		foreach ( string propertyPath in listOfPropertyPaths ) Console.WriteLine( " - {0}", propertyPath );

		/******************************************************************/

		Console.WriteLine( "Saving to file: '{0}'...", filePath );
		exampleObject.SaveToFile();
		Console.WriteLine( "Saved to file: '{0}'...", filePath );

		/******************************************************************/

		Console.WriteLine( "Deleting file: '{0}'...", filePath );
		File.Delete( filePath );
		Console.WriteLine( "Deleted file: '{0}'...", filePath );

	}
}
