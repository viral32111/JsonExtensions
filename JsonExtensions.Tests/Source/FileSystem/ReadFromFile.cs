using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.FileSystem;

public class ReadFromFileTests {

	[ Fact ]
	public void ReadFromFileEmptyObject() {
		string temporaryFilePath = Path.GetTempFileName(); // Creates the file, but our method overwrites anyway
		
		File.WriteAllText( temporaryFilePath, "{}" );

		JsonObject jsonObject = JsonExtensions.ReadFromFile( temporaryFilePath );
		Assert.Equal( "{}", JsonSerializer.Serialize( jsonObject ) );
		Assert.Empty( jsonObject );

		File.Delete( temporaryFilePath );
	}

	[ Fact ]
	public void ReadFromFileFullObject() {
		string temporaryFilePath = Path.GetTempFileName(); // Creates the file, but our method overwrites anyway

		JsonObject originalJsonObject = new() {
			[ "one" ] = "hello",
			[ "two" ] = "world"
		};

		File.WriteAllText( temporaryFilePath, JsonSerializer.Serialize( originalJsonObject ) );

		JsonObject fileJsonObject = JsonExtensions.ReadFromFile( temporaryFilePath );
		Assert.NotEmpty( fileJsonObject );
		Assert.Equal( JsonSerializer.Serialize( originalJsonObject ), JsonSerializer.Serialize( fileJsonObject ) ); // Alternative to deep equal

		Assert.NotSame( originalJsonObject, fileJsonObject );
		Assert.NotStrictEqual( originalJsonObject, fileJsonObject );

		Assert.Equal( "hello", ( string? ) fileJsonObject[ "one" ] );
		Assert.Equal( "world", ( string? ) fileJsonObject[ "two" ] );

		File.Delete( temporaryFilePath );
	}

}
