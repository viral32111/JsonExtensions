# JSON Extensions

This is a NuGet package for .NET 6.0 that adds several creation and extension methods to [`System.Text.Json`](https://docs.microsoft.com/dotnet/api/system.text.json) data types, primarily for filesystem manipulation and accessing nested properties.

## Usage

1. Add the latest version of the [`viral32111.JsonExtensions`](https://github.com/viral32111/JsonExtensions/packages/1617512) NuGet package to your .NET project.

2. Include the namespace at the beginning of your C# source file(s) with the following code:

```csharp
using viral32111.JsonExtensions;
```

## Documentation

### Read JSON from a file

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a> <a href="./Library/Source/JsonExtensions.cs#L35-L52">JsonExtensions.ReadFromFile</a>( string filePath )
</pre>

Parses the contents of the file at the specified path as JSON and returns a [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject).

The following exceptions may be thrown:
 * [`viral32111.JsonExtensions.JsonParseException`](./Library/Source/JsonExtensions.cs#L204-L207) if the JSON within the file is invalid.
 * [`System.IO.FileNotFoundException`](https://docs.microsoft.com/dotnet/api/system.io.filenotfoundexception) if the file at the specified path cannot be found.
   * *In this case you should create a new JSON file with [`JsonExtensions.CreateNewFile()`](#create-a-new-json-file).*

Version history:
 * This method has not changed since `0.1.0`.

### Create a new JSON file

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a> <a href="./Library/Source/JsonExtensions.cs#L55-L69">JsonExtensions.CreateNewFile</a>( string filePath, JsonObject? jsonObject )
</pre>

Creates (or overwrites) the file at the specified path with the specified [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) (or an empty one if one is not specified), then returns it.

The following exceptions may be thrown:
 * No exceptions should be thrown when using this method.

Version history:
 * **`0.1.1`:** Fixed file path not being stored for use in the [`JsonObject.SaveToFile()`](#save-json-to-file) method.

### Save JSON to file

<pre>
void <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="./Library/Source/JsonExtensions.cs#L72-L83">SaveToFile</a>( string? filePath )
</pre>

Saves this [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) to the file at the specified path. Defaults to the path used to read or create the file if no path is specified.

The following exceptions may be thrown:
 * No exceptions should be thrown when using this method.

Version history:
 * This method has not changed since `0.1.0`.

### Set nested property

<pre>
void <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="./Library/Source/JsonExtensions.cs#L86-L120">NestedSet</a>( string propertyPath, JsonNode? newValue )
</pre>

Sets the specified nested property in this [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) to the specified value. Dots (`.`) are used to separate nested property names.

The following exceptions may be thrown:
 * [`viral32111.JsonExtensions.JsonPropertyNullException`](./Library/Source/JsonExtensions.cs#L216-L219) if one of the property names expected to be a [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) is null.

Version history:
 * This method has not changed since `0.1.0`.

### Get nested property

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode">System.Text.Json.Nodes.JsonNode?</a> <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="./Library/Source/JsonExtensions.cs#L123-L157">NestedGet</a>( string propertyPath )
</pre>

Gets the specified nested property in this [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject), and returns it as a [`JsonNode`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode) (may be null). Dots (`.`) are used to separate nested property names.

The following exceptions may be thrown:
 * [`viral32111.JsonExtensions.JsonPropertyNullException`](./Library/Source/JsonExtensions.cs#L216-L219) if one of the property names expected to be a [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) is null.
 * [`viral32111.JsonExtensions.JsonPropertyNotFoundException`](./Library/Source/JsonExtensions.cs#L210-L213) if one of the property names cannot be found.
 * [`viral32111.JsonExtensions.JsonPropertyNotFoundException`](./Library/Source/JsonExtensions.cs#L210-L213) if the nested property could not be found.

Version history:
 * **`0.1.2`**: [`viral32111.JsonExtensions.JsonPropertyNotFoundException`](./Library/Source/JsonExtensions.cs#L210-L213) is now thrown instead of [`System.Text.Json.JsonException`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonexception) when the nested property cannot be found.

### Get nested property as type

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.type">System.Type</a> <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="./Library/Source/JsonExtensions.cs#L160-L177">NestedGet</a><<a href="https://docs.microsoft.com/dotnet/api/system.type">System.Type</a>>( string propertyPath )
</pre>

Gets the specified nested property in this [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) as the specified data type. See [`JsonObject.NestedGet()`](#get-nested-property) for more information.

The following exceptions may be thrown:
 * [`viral32111.JsonExtensions.JsonPropertyNullException`](./Library/Source/JsonExtensions.cs#L216-L219) if the nested property is null.
 * See [`JsonObject.NestedGet()`](#get-nested-property) for more exceptions that may be thrown.

Version history:
 * This method has not changed since `0.1.0`.

### Get array as type

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.type">System.Type</a>[] <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode">System.Text.Json.Nodes.JsonNode</a>.<a href="./Library/Source/JsonExtensions.cs#L180-L201">AsArray</a><<a href="https://docs.microsoft.com/dotnet/api/system.type">System.Type</a>>()
</pre>

Converts this [`JsonNode`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode) to a [`JsonArray`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonarray) then to an array of the specified data type.

The following exceptions may be thrown:
 * [`viral32111.JsonExtensions.JsonPropertyNullException`](./Library/Source/JsonExtensions.cs#L216-L219) if a value in the array is null.

Version history:
 * This method has not changed since `0.1.0`.

## License

Copyright (C) 2022 [viral32111](https://viral32111.com).

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, either version 3 of the
License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program. If not, see https://www.gnu.org/licenses.
