using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.FileSystem;

public class SaveToFileTests {

	[ Fact ]
	public void SaveToFileEmptyObject() {
		string temporaryFilePath = Path.GetTempFileName(); // Creates the file, but our method overwrites anyway
		
		JsonObject jsonObject = new();

		jsonObject.SaveToFile( temporaryFilePath );

		Assert.True( File.Exists( temporaryFilePath ) );
		Assert.NotEmpty( File.ReadAllText( temporaryFilePath ) );

		Assert.Equal( "{}", File.ReadAllText( temporaryFilePath ) );
		Assert.Equal( JsonSerializer.Serialize( jsonObject ), File.ReadAllText( temporaryFilePath ) );

		File.Delete( temporaryFilePath );
	}

	[ Fact ]
	public void SaveToFileFullObject() {
		string temporaryFilePath = Path.GetTempFileName(); // Creates the file, but our method overwrites anyway
		
		JsonObject originalJsonObject = new() {
			[ "one" ] = "hello",
			[ "two" ] = "world"
		};

		originalJsonObject.SaveToFile( temporaryFilePath );

		Assert.True( File.Exists( temporaryFilePath ) );
		Assert.NotEmpty( File.ReadAllText( temporaryFilePath ) );

		JsonObject fileJsonObject = JsonSerializer.Deserialize<JsonObject>( File.ReadAllText( temporaryFilePath ) )!;

		Assert.Equal( JsonSerializer.Serialize( originalJsonObject ), JsonSerializer.Serialize( fileJsonObject ) ); // Alternative to deep equal
		Assert.NotSame( originalJsonObject, fileJsonObject );
		Assert.NotStrictEqual( originalJsonObject, fileJsonObject );

		Assert.Equal( "hello", ( string? ) fileJsonObject[ "one" ] );
		Assert.Equal( "world", ( string? ) fileJsonObject[ "two" ] );

		File.Delete( temporaryFilePath );
	}

	[ Fact ]
	public void ReadFromFileSaveToSameFile() {
		string temporaryFilePath = Path.GetTempFileName(); // Creates the file, but our method overwrites anyway
		
		JsonObject originalJsonObject = new() {
			[ "one" ] = "hello",
			[ "two" ] = "world"
		};

		File.WriteAllText( temporaryFilePath, JsonSerializer.Serialize( originalJsonObject ) );

		JsonObject fileJsonObject = JsonExtensions.ReadFromFile( temporaryFilePath );
		fileJsonObject[ "one" ] = "goodbye";
		fileJsonObject.SaveToFile( temporaryFilePath );

		Assert.True( File.Exists( temporaryFilePath ) );
		Assert.NotEmpty( File.ReadAllText( temporaryFilePath ) );

		JsonObject newFileJsonObject = JsonSerializer.Deserialize<JsonObject>( File.ReadAllText( temporaryFilePath ) )!;

		Assert.Equal( JsonSerializer.Serialize( fileJsonObject ), JsonSerializer.Serialize( newFileJsonObject ) ); // Alternative to deep equal
		Assert.NotSame( fileJsonObject, newFileJsonObject );
		Assert.NotStrictEqual( fileJsonObject, newFileJsonObject );

		Assert.Equal( "goodbye", ( string? ) newFileJsonObject[ "one" ] );

		File.Delete( temporaryFilePath );
	}

}
