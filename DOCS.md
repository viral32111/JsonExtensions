# Developer Documentation

This documentation outlines what each method does, what it returns or the exceptions it throws, and changes that have been made across versions.

## Read JSON from a file

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a> <a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/FileSystem.cs#L13-L31">viral32111.JsonExtensions.JsonExtensions.ReadFromFile</a>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string</a> filePath )
</pre>

### Description

Parses the contents of the file at the given path (`filePath`) as a JSON object.

### Returns

A [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) representing the parsed JSON from the file.

### Exceptions

* [`viral32111.JsonExtensions.JsonParseException`](https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Exceptions.cs#L5-L11) if the JSON within the file is malformed or otherwise invalid.
* [`System.IO.FileNotFoundException`](https://docs.microsoft.com/dotnet/api/system.io.filenotfoundexception) if the file at the specified path cannot be found.
  * In this case, a new JSON file should be created using [`viral32111.JsonExtensions.JsonExtensions.CreateNewFile()`](#create-a-new-json-file).

### History

* Introduced in [`0.1.0`](https://github.com/viral32111/JsonExtensions/tree/0.1.0).

###

## Create a new JSON file

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a> <a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/FileSystem.cs#L33-L48">viral321111.JsonExtensions.JsonExtensions.CreateNewFile</a>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string</a> filePath, <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">JsonObject?</a> jsonObject = <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a> )
</pre>

### Description

Creates or overwrites the file at the given path (`filePath`) with the given JSON object (`jsonObject`), or an empty object if not given.

### Returns

A [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) representing the JSON that was written to the file.

### Exceptions

None.

### History

* Fixed the file path not being remembered for use with [`JsonObject.SaveToFile()`](#save-json-to-file) in [`0.1.1`](https://github.com/viral32111/JsonExtensions/tree/0.1.1).
* Introduced in [`0.1.0`](https://github.com/viral32111/JsonExtensions/tree/0.1.0).

###

## Save JSON to file

<pre>
void <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/FileSystem.cs#L51-L63">SaveToFile</a>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string?</a> filePath = <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a> )
</pre>

### Description

Saves this JSON object to the file at the given path (`filePath`).

Defaults to the path used when reading or creating the file if no path is given.

### Returns

Nothing.

### Exceptions

None.

### History

* Introduced in [`0.1.0`](https://github.com/viral32111/JsonExtensions/tree/0.1.0).

###

## Set nested JSON property

<pre>
void <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/NestedExtensions.cs#L13-L38">NestedSet</a>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string</a> propertyPath, <a href="https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonnode">JsonNode?</a> newValue, <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char">char</a> propertyDelimiter = <a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Defaults.cs#L10-L13">viral32111.JsonExtensions.Defaults.NestedPropertyDelimiter</a> )
</pre>

### Description

Sets the nested property in this JSON object at the given path (`propertyPath`), separated by the given delimiter (`propertyDelimiter`), to the given value (`newValue`).

### Returns

Nothing.

### Exceptions

* [`viral32111.JsonExtensions.JsonPropertyNullException`](https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Exceptions.cs#L21-L27) if one of the property names expected to be a [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) is <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a>.

### History

* Added `propertyDelimiter` parameter in [`0.3.0`](https://github.com/viral32111/JsonExtensions/tree/0.3.0).
* Introduced in [`0.1.0`](https://github.com/viral32111/JsonExtensions/tree/0.1.0).

###

## Get nested JSON property

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode">System.Text.Json.Nodes.JsonNode?</a> <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/NestedExtensions.cs#L40-L66">NestedGet</a>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string</a> propertyPath, <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char">char</a> propertyDelimiter = <a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Defaults.cs#L10-L13">viral32111.JsonExtensions.Defaults.NestedPropertyDelimiter</a> )
</pre>

### Description

Gets the nested property in this JSON object at the given path (`propertyPath`), separated by the given delimiter (`propertyDelimiter`).

### Returns

A [`JsonNode?`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode) representing the value of the property. May be <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a>.

### Exceptions

* [`viral32111.JsonExtensions.JsonPropertyNullException`](https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Exceptions.cs#L21-L27) if one of the nested parent JSON object properties is <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a>.
* [`viral32111.JsonExtensions.JsonPropertyNotFoundException`](https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Exceptions.cs#L13-L19) if one of the nested JSON object properties does not exist.

### History

* Added `propertyDelimiter` parameter in [`0.3.0`](https://github.com/viral32111/JsonExtensions/tree/0.3.0).
* Throw [`viral32111.JsonExtensions.JsonPropertyNotFoundException`](https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Exceptions.cs#L13-L19) instead of [`System.Text.Json.JsonException`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonexception) when a nested property does not exist in [`0.1.2`](https://github.com/viral32111/JsonExtensions/tree/0.1.2).
* Introduced in [`0.1.0`](https://github.com/viral32111/JsonExtensions/tree/0.1.0).

###

## Get nested JSON property as type

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.type">System.Type</a> <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/NestedExtensions.cs#L68-L87">NestedGet</a><<a href="https://docs.microsoft.com/dotnet/api/system.type">T</a>>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string</a> propertyPath, <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char">char</a> propertyDelimiter = <a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Defaults.cs#L10-L13">viral32111.JsonExtensions.Defaults.NestedPropertyDelimiter</a> )
</pre>

### Description

Gets the nested property in this JSON object at the given path (`propertyPath`), separated by the given delimiter (`propertyDelimiter`), as the given data type (`T`).

Best sticking to primitive data types, [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) and [`JsonArray`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonarray).

### Returns

The value of the property as the given data type (`T`).

### Exceptions

* [`viral32111.JsonExtensions.JsonPropertyNullException`](https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Exceptions.cs#L21-L27) if the value of the property cannot be casted because it is <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a>.

See [`viral32111.JsonExtensions.JsonObject.NestedGet()`](#get-nested-json-property) for more exceptions that may be thrown.

### History

* Added `propertyDelimiter` parameter in [`0.3.0`](https://github.com/viral32111/JsonExtensions/tree/0.3.0).
* Introduced in [`0.1.0`](https://github.com/viral32111/JsonExtensions/tree/0.1.0).

###

## Has nested JSON property

<pre>
<a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool">bool</a> <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/NestedExtensions.cs#L89-L112">NestedHas</a>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string</a> propertyPath, <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char">char</a> propertyDelimiter = <a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Defaults.cs#L10-L13">viral32111.JsonExtensions.Defaults.NestedPropertyDelimiter</a> )
</pre>

### Description

Checks if a nested property exists in this JSON object at the given path (`propertyPath`), separated by the given delimiter (`propertyDelimiter`).

A <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a> property value counts as the property existing.

### Returns

True or False representing if the property exists.

### Exceptions

* [`viral32111.JsonExtensions.JsonPropertyNullException`](./Library/Source/JsonExtensions.cs#L292-L295) if one of the property names expected to be a [`JsonObject`](https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject) is <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a>.

### History

* Added `propertyDelimiter` parameter in [`0.3.0`](https://github.com/viral32111/JsonExtensions/tree/0.3.0).
* Introduced in [`0.2.0`](https://github.com/viral32111/JsonExtensions/tree/0.2.0).

###

## List nested JSON properties

<pre>
<a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/">string</a>[] <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonobject">System.Text.Json.Nodes.JsonObject</a>.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/NestedExtensions.cs#L114-L140">NestedList</a>( <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char">char</a> propertyDelimiter = <a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Defaults.cs#L10-L13">viral32111.JsonExtensions.Defaults.NestedPropertyDelimiter</a> )
</pre>

### Description

Creates a list of paths to all nested properties in this JSON object, with nested objects separated by the given delimiter (`propertyDelimiter`).

<a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">Null</a> property values are included.

### Returns

A list of paths to all nested JSON object properties.

### Exceptions

None.

### History

* Added `propertyDelimiter` parameter in [`0.3.0`](https://github.com/viral32111/JsonExtensions/tree/0.3.0).
* Introduced in [`0.2.0`](https://github.com/viral32111/JsonExtensions/tree/0.2.0).

###

## Get as typed array

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.type">System.Type</a>[] <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode">System.Text.Json.Nodes.JsonNode</a>.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/ConversionExtensions.cs#L12-L31">AsArray</a><<a href="https://docs.microsoft.com/dotnet/api/system.type">T</a>>()
</pre>

Converts this JSON node, which must be a JSON array, to a fixed-length typed array of the given data type (`T`).

### Returns

A typed array of the given data type (`T`) containing the elements within this JSON array.

### Exceptions

* [`viral32111.JsonExtensions.JsonPropertyNullException`](https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/Exceptions.cs#L21-L27) if a value in the JSON array is <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a>.

### History

* Introduced in [`0.1.0`](https://github.com/viral32111/JsonExtensions/tree/0.1.0).

###

## Clone JSON node

<pre>
<a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode">System.Text.Json.Nodes.JsonNode</a>? <a href="https://docs.microsoft.com/dotnet/api/system.text.json.nodes.jsonnode">System.Text.Json.Nodes.JsonNode</a>?.<a href="https://github.com/viral32111/JsonExtensions/blob/8814cd590eb3c37e4a635493dac8f17cf26d576e/JsonExtensions/Source/ConversionExtensions.cs#L33-L36">Clone</a>()
</pre>

Creates a copy of this JSON node.

### Returns

A copy of this JSON node, or <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a> if this JSON node is <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null">null</a>.

### Exceptions

None.

### History

* Introduced in [`0.2.0`](https://github.com/viral32111/JsonExtensions/tree/0.2.0).
