using AspNetSerilog.Extractors;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace AspNetSerilogTests.Extractors
{
    public class HttpContextExtractorTest
    {
        private HttpContext _httpContext;

        public HttpContextExtractorTest()
        {
            _httpContext = new DefaultHttpContext();
        }

        [Fact]
        public void GetPath_InformingThePathInTheBlackList_ReturnsMaskedPath()
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");

            var blackList = new[] {"Path"};

            var expected = "/******";
            var actual = HttpContextExtractor.GetPath(_httpContext, blackList);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPath_NotInformingThePathInTheBlackList_ReturnsTheValueGivenInThePath()
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");

            var blackList = new string[] { };

            var expected = "/foo/bar";
            var actual = HttpContextExtractor.GetPath(_httpContext, blackList);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPathBase_InformingThePathBaseInTheBlackList_ReturnsMaskedPath()
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");
            _httpContext.Request.PathBase = new PathString("/sample-alias/");

            var blackList = new[] {"PathBase"};

            var expected = "/******";
            var actual = HttpContextExtractor.GetPathBase(_httpContext, blackList);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPathBase_NotInformingThePathBaseInTheBlackList_ReturnsTheValueGivenInThePathBase()
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");
            _httpContext.Request.PathBase = new PathString("/sample-alias/");

            var blackList = new string[] { };

            var expected = "/sample-alias/";
            var actual = HttpContextExtractor.GetPathBase(_httpContext, blackList);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFullUrl_InformingThePathInTheBlackList_ReturnsMaskedPath()
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Scheme = "http";
            _httpContext.Request.Host = new HostString("localhost:80");
            _httpContext.Request.PathBase = new PathString("/sample-alias");
            _httpContext.Request.Path = new PathString("/foo/bar");

            string[] queryBlackList = {};
            var httpContextBlackList = new[] {"Path"};

            var expected = "http://localhost:80/sample-alias/******?";
            
            var actual = HttpContextExtractor.GetFullUrl(_httpContext, queryBlackList, httpContextBlackList);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void GetFullUrl_InformingThePathBaseInTheBlackList_ReturnsMaskedPath()
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Scheme = "http";
            _httpContext.Request.Host = new HostString("localhost:80");
            _httpContext.Request.PathBase = new PathString("/sample-alias");
            _httpContext.Request.Path = new PathString("/foo/bar");

            string[] queryBlackList = {};
            var httpContextBlackList = new[] {"PathBase"};

            var expected = "http://localhost:80/******/foo/bar?";
            
            var actual = HttpContextExtractor.GetFullUrl(_httpContext, queryBlackList, httpContextBlackList);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void GetFullUrl_InformingThePathAndPathBaseInTheBlackList_ReturnsMaskedPath()
        {
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Scheme = "http";
            _httpContext.Request.Host = new HostString("localhost:80");
            _httpContext.Request.PathBase = new PathString("/sample-alias");
            _httpContext.Request.Path = new PathString("/foo/bar");

            string[] queryBlackList = {};
            var httpContextBlackList = new[] {"PathBase", "Path"};

            var expected = "http://localhost:80/******/******?";
            
            var actual = HttpContextExtractor.GetFullUrl(_httpContext, queryBlackList, httpContextBlackList);

            Assert.Equal(expected, actual);
        }
    }
}