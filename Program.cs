
using Dumpify;

var req = RequestBuilder.Create()
    .WithMethod("GET")
    .WithUrl("http://exemple.com/")
    .AddHeader("Hello", "World")
    .Build();

req.Dump();




class Request
{
    public string Method { get; init; } = null!;
    public string Url { get; init; } = null!;
    public (string Key, string Value)[] Headers { get; init; } = null!;
}

class RequestBuilder
{
    private List<(string Key, string Value)> _headers { get; set; } = new();

    public string? Method { get; private set; }
    public string? Url { get; private set; }
    public IReadOnlyList<(string Key, string Value)> Headers => _headers;

    public static RequestBuilder Create() => new RequestBuilder();

    public RequestBuilder WithMethod(string method)
    {
        Method = method;
        return this;
    }

    public RequestBuilder WithUrl(string url)
    {
        Url = url;
        return this;
    }

    public RequestBuilder AddHeader(string key, string value)
    {
        _headers.Add((key, value));
        return this;
    }

    public Request Build()
    {
        return new Request()
        {
            Url = Url!,
            Method = Method!,
            Headers = _headers.ToArray()
        };
    }    
}


