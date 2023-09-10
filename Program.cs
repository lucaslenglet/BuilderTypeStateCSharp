using Dumpify;

var req = RequestBuilder.Create()
    .WithMethod("GET")
    .WithUrl("http://example.com/")
    .AddHeader("Hello", "World")
    .Build();

req.Dump();

class Request
{
    public string Method { get; init; } = null!;
    public string Url { get; init; } = null!;
    public (string Key, string Value)[] Headers { get; init; } = null!;
}

interface IBuilderState { }

interface IMethodEmpty : IBuilderState { }
interface IMethodDefined : IBuilderState { }

interface IUrlEmpty : IBuilderState { }
interface IUrlDefined : IBuilderState { }

static class RequestBuilder
{
    public static RequestBuilder<IMethodEmpty, IUrlEmpty> Create() => new();
}

static class RequestBuilderExtensions
{
    public static RequestBuilder<IMethodDefined, TUrlState> WithMethod<TUrlState>(this RequestBuilder<IMethodEmpty, TUrlState> builder, string method)
        where TUrlState : IBuilderState
    {
        return new RequestBuilder<IMethodDefined, TUrlState>
        {
            Method = method,
            Url = builder.Url,
            Headers = builder.Headers,
        };
    }

    public static RequestBuilder<TMehodState, IUrlDefined> WithUrl<TMehodState>(this RequestBuilder<TMehodState, IUrlEmpty> builder, string url)
        where TMehodState : IBuilderState
    {
        return new RequestBuilder<TMehodState, IUrlDefined>
        {
            Method = builder.Method,
            Url = url,
            Headers = builder.Headers,
        };
    }

    public static Request Build(this RequestBuilder<IMethodDefined, IUrlDefined> builder)
    {
        return new Request()
        {
            Url = builder.Url!,
            Method = builder.Method!,
            Headers = builder.Headers.ToArray()
        };
    }
}

class RequestBuilder<TMethodState, TUrlState>
    where TMethodState : IBuilderState
    where TUrlState : IBuilderState
{

    public string? Method { get; set; }
    public string? Url { get; set; }
    public List<(string Key, string Value)> Headers { get; set; } = new();

    public RequestBuilder<TMethodState, TUrlState> AddHeader(string key, string value)
    {
        Headers.Add((key, value));
        return this;
    }
}