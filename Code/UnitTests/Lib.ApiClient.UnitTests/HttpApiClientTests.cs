using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;

namespace Lib.ApiClient.UnitTests;

[TestFixture]
public class HttpApiClientTests
{
    private readonly Uri _uri = new("http://localhost/test");

    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpApiClient _httpApiClient;

    [SetUp]
    public void Setup()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _httpApiClient = new HttpApiClient(_httpMessageHandlerMock.CreateClient(),
            new Mock<ILogger<HttpApiClient>>().Object);
    }

    [Test]
    public void Get_WhenUriIsNull_ThrowsArgumentNullException()
    {
        Assert.That(
            () => _httpApiClient.Get<string>(null, "token"),
            Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'uri')"));
    }

    [Test]
    public void Get_WhenTokenIsNull_ThrowsArgumentNullException()
    {
        Assert.That(
            () => _httpApiClient.Get<string>(_uri, null),
            Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'token')"));
    }

    [Test]
    public void Get_WhenTokenIsEmpty_ThrowsArgumentException()
    {
        Assert.That(
            () => _httpApiClient.Get<string>(_uri, string.Empty),
            Throws.ArgumentException.With.Message.EqualTo("The value cannot be an empty string. (Parameter 'token')"));
    }

    [Test]
    public async Task Get_WhenResponseError_ReturnsResultError()
    {
        _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, _uri)
            .ReturnsResponse(HttpStatusCode.BadRequest, "Bad request response");

        var result = await _httpApiClient.Get<string>(_uri, "token");

        Assert.Multiple(() =>
        {
            Assert.IsTrue(result.IsFaulted);
            Assert.AreEqual(
                "Call to HTTP get endpoint 'http://localhost/test' method failed with status code: 'BadRequest' and message: 'Bad request response'.",
                result.Message);
        });
    }

    [Test]
    public async Task Get_WhenHttpClientThrowsException_ReturnsResultError()
    {
        _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, _uri)
            .Throws(new HttpRequestException("exception string"));

        var result = await _httpApiClient.Get<string>(_uri, "token");

        Assert.Multiple(() =>
        {
            Assert.IsTrue(result.IsFaulted);
            Assert.AreEqual("exception string", result.Message);
        });
    }

    [Test]
    public async Task Get_WhenResponseSuccess_ReturnsResultSuccess()
    {
        _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, _uri)
            .ReturnsResponse(HttpStatusCode.OK, "\"data\"");

        var result = await _httpApiClient.Get<string>(_uri, "token");

        Assert.Multiple(() =>
        {
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("data", result.Data);
        });
    }

    [Test]
    public void Post_WhenUriIsNull_ThrowsArgumentNullException()
    {
        Assert.That(
            () => _httpApiClient.Post(null, "token"),
            Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'uri')"));
    }

    [Test]
    public void Post_WhenTokenIsNull_ThrowsArgumentNullException()
    {
        Assert.That(
            () => _httpApiClient.Post(_uri, null),
            Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'token')"));
    }

    [Test]
    public void Post_WhenTokenIsEmpty_ThrowsArgumentException()
    {
        Assert.That(
            () => _httpApiClient.Post(_uri, string.Empty),
            Throws.ArgumentException.With.Message.EqualTo("The value cannot be an empty string. (Parameter 'token')"));
    }

    [Test]
    public async Task Post_WhenResponseError_ReturnsResultError()
    {
        _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, _uri,
                async message => await message.Content.ReadAsStringAsync() == "\"payload\"")
            .ReturnsResponse(HttpStatusCode.BadRequest, "Bad request response");

        var result = await _httpApiClient.Post(_uri, "token", "payload");

        Assert.Multiple(() =>
        {
            Assert.IsTrue(result.IsFaulted);
            Assert.AreEqual(
                "Call to HTTP post endpoint 'http://localhost/test' method failed with status code: 'BadRequest' and message: 'Bad request response'.",
                result.Message);
        });
    }

    [Test]
    public async Task Post_WhenHttpClientThrowsException_ReturnsResultError()
    {
        _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, _uri,
                async message => await message.Content.ReadAsStringAsync() == "\"payload\"")
            .Throws(new HttpRequestException("exception string"));

        var result = await _httpApiClient.Post(_uri, "token", "payload");

        Assert.Multiple(() =>
        {
            Assert.IsTrue(result.IsFaulted);
            Assert.AreEqual("exception string", result.Message);
        });
    }

    [Test]
    public async Task Post_WhenResponseSuccess_ReturnsResultSuccess()
    {
        _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, _uri,
                async message => await message.Content.ReadAsStringAsync() == "\"payload\"")
            .ReturnsResponse(HttpStatusCode.OK);

        var result = await _httpApiClient.Post(_uri, "token", "payload");

        Assert.IsTrue(result.IsSuccess);
    }
}