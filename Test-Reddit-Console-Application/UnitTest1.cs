using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using RedditConsoleApplication.Controller;
using RedditConsoleApplication.Controller.Interfaces;
using RedditConsoleApplication.Controller.Models;
using RedditConsoleApplication.Models;
using Xunit;
using Xunit.Abstractions;
namespace Test_Reddit_Console_Application;

public class UnitTest1
{
    private IRedditController<Root>? _sut;
    private string _apiAuthToken = "yJhbGciOiJSUzI1NiIsImtpZCI6IlNIQTI1NjpzS3dsMnlsV0";
    private string _userAgent = "usertest";
    private string _baseUrl = RedditRequestParams.BASEADDRESS;
    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData(["http://www.google.com"])]
    [InlineData([null])]
    [InlineData(["strts"])]
    public void BadBaseUrlThrowNullReferenceException(string baseUrl)
    {
        try
        {
            _sut = new RedditController(_apiAuthToken, _userAgent, baseUrl);
        }
        catch (System.Exception e)
        {
            Assert.IsType<NullReferenceException>(e);
            _output.WriteLine(e.Message);
        }
    }

    [Fact]
    public void VerifyRestClientHeadersExists()
    {
        _sut = new RedditController(_apiAuthToken, _userAgent, _baseUrl);

        var headers = _sut.RedditCtrlModel.RClient.RestRequest.Parameters.GetParameters(RestSharp.ParameterType.HttpHeader);

        List<string> headersContains = new List<string>() { RedditRequestParams.HEADER_USER_AGENT, RedditRequestParams.HEADER_AUTHORIZATION };
        
        Assert.Contains(headers, a => headersContains.Contains(a?.Name) && !string.IsNullOrEmpty(a.Value?.ToString()));
    }
}