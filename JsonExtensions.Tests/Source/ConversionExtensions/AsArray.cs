using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace viral32111.JsonExtensions.Tests.ConversionExtensions;

public class ConversionAsArrayTests {

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
