using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;

namespace JsonFileWrapper {
	public class Program {

		public static void Main() {

			string fileName = "test.json";

			JsonObject testData;
			try {
				testData = JsonExtensions.ReadFromFile( fileName );
				Console.WriteLine( "Read from file" );
			} catch ( FileNotFoundException ) {
				testData = JsonExtensions.CreateNewFile( fileName );
				Console.WriteLine( "Created new file" );
			}

			testData[ "example" ] = "Hello World";
			Console.WriteLine( "example: {0}", testData[ "example" ] );

			bool addSuccess = testData.TryAdd( "abc", new JsonObject() {
				[ "xyz" ] = 123
			} );
			Console.WriteLine( "addSuccess: {0}", addSuccess );

			testData.NestedSet( "one.two.three.four", 5678 );
			Console.WriteLine( "one.two.three.four: {0}", testData.NestedGet<int>( "one.two.three.four" ) );

			try {
				Console.WriteLine( "one.two.three.five: {0}", testData.NestedGet<bool>( "one.two.three.five" ) );
			} catch ( JsonPropertyNotFoundException ) {
				Console.WriteLine( "one.two.three.five does not exist" );
			}

			JsonNode? abc = testData.NestedGet( "abc" );
			Console.WriteLine( "abc: {0}", abc );

			JsonObject abcObj = testData.NestedGet<JsonObject>( "abc" );
			Console.WriteLine( "abcObj: {0}", abcObj );

			testData[ "things" ] = new JsonArray( 1, 2, 3, 4, 5, 6 );
			Console.WriteLine( "things: {0}", testData[ "things" ] );

			JsonArray thingsArr = testData.NestedGet<JsonArray>( "things" );
			Console.WriteLine( "thingsArr: {0}", thingsArr );

			int[] thingsIntArr = testData[ "things" ]!.AsArray<int>();
			Console.WriteLine( "thingsIntArr: {0}", thingsIntArr.Length );

			testData.SaveToFile();
			Console.WriteLine( "Saved to file" );

		}

	}
}