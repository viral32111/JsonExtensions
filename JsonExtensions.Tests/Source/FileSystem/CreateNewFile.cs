using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.FileSystem;

public class CreateNewFileTests {

	[ Fact ]
	public void CreateNewFileEmpty() {
		string temporaryFilePath = Path.GetTempFileName(); // Creates the file, but our method overwrites anyway
		
		JsonObject jsonObject = JsonExtensions.CreateNewFile( temporaryFilePath );

		Assert.True( File.Exists( temporaryFilePath ) );
		Assert.NotEmpty( File.ReadAllText( temporaryFilePath ) );
		Assert.Equal( "{}", File.ReadAllText( temporaryFilePath ) );
		Assert.Equal( JsonSerializer.Serialize( jsonObject ), File.ReadAllText( temporaryFilePath ) );

		File.Delete( temporaryFilePath );
	}

	[ Fact ]
	public void CreateNewFileFromObject() {
		string temporaryFilePath = Path.GetTempFileName(); // Creates the file, but our method overwrites anyway

		JsonObject originalJsonObject = new() {
			[ "one" ] = "hello",
			[ "two" ] = "world"
		};

		JsonObject returnedJsonObject = JsonExtensions.CreateNewFile( temporaryFilePath, originalJsonObject );
		Assert.True( File.Exists( temporaryFilePath ) );
		Assert.NotEmpty( File.ReadAllText( temporaryFilePath ) );
		Assert.Same( originalJsonObject, returnedJsonObject );
		Assert.Equal( originalJsonObject, returnedJsonObject );

		JsonObject fileJsonObject = JsonSerializer.Deserialize<JsonObject>( File.ReadAllText( temporaryFilePath ) )!;
		Assert.Equal( JsonSerializer.Serialize( originalJsonObject ), JsonSerializer.Serialize( fileJsonObject ) ); // Alternative to deep equal
		Assert.NotSame( originalJsonObject, fileJsonObject );
		Assert.NotStrictEqual( originalJsonObject, fileJsonObject );

		Assert.Equal( "hello", ( string? ) fileJsonObject[ "one" ] );
		Assert.Equal( "world", ( string? ) fileJsonObject[ "two" ] );

		File.Delete( temporaryFilePath );
	}

}
