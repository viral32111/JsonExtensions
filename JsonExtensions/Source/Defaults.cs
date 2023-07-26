using System.Text.Json;

namespace viral32111.JsonExtensions;

/// <summary>
/// Default parameter values for methods within this library.
/// </summary>
public static class Defaults {

	/// <summary>
	/// Property separator character for accessing nested properties.
	/// </summary>
	public const char NestedPropertyDelimiter = '.';

	/// <summary>
	/// Options for controlling seralization & deseralizaion.
	/// Retains property names, ignores comments/trailing commas, and intents.
	/// </summary>
	public static readonly JsonSerializerOptions SerializerOptions = new() {

		// Keep property names as they are
		PropertyNamingPolicy = null,
		PropertyNameCaseInsensitive = false,

		// Ignore human comments & mistakes
		ReadCommentHandling = JsonCommentHandling.Skip,
		AllowTrailingCommas = true,

		// Make human editing easier
		WriteIndented = true

	};

}
