using AspNetSerilog.Extractors;
using AspNetSerilogTests.Mocks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace AspNetSerilogTests.Extractors
{
    public class HttpContextExtractorTest
    {
        private HttpContext _httpContext;

        public HttpContextExtractorTest()
        {
            _httpContext = HttpContextMock.DefaultHttpContext();
        }

        [Fact]
        public void GetPath_InformingThePathInTheBlackList_ReturnsMaskedPath()
        {
            // Arrange
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");

            var blackList = new[] { "Path" };
            var expected = "/******";

            // Act
            var actual = HttpContextExtractor.GetPath(_httpContext, blackList);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPath_NotInformingThePathInTheBlackList_ReturnsTheValueGivenInThePath()
        {
            // Arrange
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");

            var blackList = new string[] { };
            var expected = "/foo/bar";

            // Act
            var actual = HttpContextExtractor.GetPath(_httpContext, blackList);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPathBase_InformingThePathBaseInTheBlackList_ReturnsMaskedPath()
        {
            // Arrange
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");
            _httpContext.Request.PathBase = new PathString("/sample-alias/");

            var blackList = new[] { "PathBase" };
            var expected = "/******";

            //Act
            var actual = HttpContextExtractor.GetPathBase(_httpContext, blackList);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPathBase_NotInformingThePathBaseInTheBlackList_ReturnsTheValueGivenInThePathBase()
        {
            // Arrange
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Path = new PathString("/foo/bar");
            _httpContext.Request.PathBase = new PathString("/sample-alias/");

            var blackList = new string[] { };
            var expected = "/sample-alias/";

            // Act
            var actual = HttpContextExtractor.GetPathBase(_httpContext, blackList);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFullUrl_InformingThePathInTheBlackList_ReturnsMaskedPath()
        {
            // Arrange
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Scheme = "http";
            _httpContext.Request.Host = new HostString("localhost:80");
            _httpContext.Request.PathBase = new PathString("/sample-alias");
            _httpContext.Request.Path = new PathString("/foo/bar");

            string[] queryBlackList = { };
            var httpContextBlackList = new[] { "Path" };

            var expected = "http://localhost:80/sample-alias/******?";

            // Act
            var actual = HttpContextExtractor.GetFullUrl(_httpContext, queryBlackList, httpContextBlackList);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFullUrl_InformingThePathBaseInTheBlackList_ReturnsMaskedPath()
        {
            // Arrange
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Scheme = "http";
            _httpContext.Request.Host = new HostString("localhost:80");
            _httpContext.Request.PathBase = new PathString("/sample-alias");
            _httpContext.Request.Path = new PathString("/foo/bar");

            string[] queryBlackList = { };
            var httpContextBlackList = new[] { "PathBase" };

            var expected = "http://localhost:80/******/foo/bar?";

            // Act
            var actual = HttpContextExtractor.GetFullUrl(_httpContext, queryBlackList, httpContextBlackList);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFullUrl_InformingThePathAndPathBaseInTheBlackList_ReturnsMaskedPath()
        {
            //Arrange
            _httpContext.Request.Method = "GET";
            _httpContext.Request.Scheme = "http";
            _httpContext.Request.Host = new HostString("localhost:80");
            _httpContext.Request.PathBase = new PathString("/sample-alias");
            _httpContext.Request.Path = new PathString("/foo/bar");

            string[] queryBlackList = { };
            var httpContextBlackList = new[] { "PathBase", "Path" };

            var expected = "http://localhost:80/******/******?";

            // Act
            var actual = HttpContextExtractor.GetFullUrl(_httpContext, queryBlackList, httpContextBlackList);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetContentAsObjectByContentTypeJson_WithFilledBlacklist_ReturnsMaskedBody()
        {
            // Arrange
            const string EXPECTED_VALUE = "{\"Test\":\"1\",\"Card\":{\"Number\":\"******\",\"Password\":\"******\",\"Email\":\"email@test.com\",\"Card\":{\"Number\":\"******\",\"Password\":\"******\"},\"Teste\":{\"Card\":{\"Number\":\"******\",\"Password\":\"******\",\"Email\":\"email@test.com\"}}}}";

            string[] queryBlackList = { "*card.number", "*password" };

            // Act
            var value = _httpContext.GetRequestBody(queryBlackList);

            // Assert
            Assert.Equal(EXPECTED_VALUE, JsonConvert.SerializeObject(value));
        }

        [Fact]
        public void GetContentAsObjectByContentTypeJson_WithEmptyBlacklist_ReturnsBodyWithoutMask()
        {
            // Arrange
            const string EXPECTED_VALUE = "{\"Test\":\"1\",\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\",\"Email\":\"email@test.com\",\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\"},\"Teste\":{\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\",\"Email\":\"email@test.com\"}}}}";

            string[] queryBlackList = { };

            // Act
            var value = _httpContext.GetRequestBody(queryBlackList);

            // Assert
            Assert.Equal(EXPECTED_VALUE, JsonConvert.SerializeObject(value));
        }

        [Fact]
        public void GetContentAsObjectByContentTypeJson_WithBlacklistAndPartialBlacklist_ReturnsBodyMaskedAndPartialMasked()
        {
            // Arrange
            const string EXPECTED_VALUE = "{\"Test\":\"1\",\"Card\":{\"Number\":\"462294*****9865\",\"Password\":\"******\",\"Email\":\"email@test.com\",\"Card\":{\"Number\":\"462294*****9865\",\"Password\":\"******\"},\"Teste\":{\"Card\":{\"Number\":\"462294*****9865\",\"Password\":\"******\",\"Email\":\"email@test.com\"}}}}";

            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            string[] queryBlackList = { "*card.number", "*password" };

            // Act
            var value = _httpContext.GetRequestBody(queryBlackList, blacklistPartialMock);

            // Assert
            Assert.Equal(EXPECTED_VALUE, JsonConvert.SerializeObject(value));
        }

        [Fact]
        public void GetContentAsObjectByContentTypeJson_WithEmptyBlacklistAndFilledPartialBlacklist_ReturnsBodyWithoutMask()
        {
            // Arrange
            const string EXPECTED_VALUE = "{\"Test\":\"1\",\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\",\"Email\":\"email@test.com\",\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\"},\"Teste\":{\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\",\"Email\":\"email@test.com\"}}}}";

            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            string[] queryBlackList = { };

            // Act
            var value = _httpContext.GetRequestBody(queryBlackList, blacklistPartialMock);

            // Assert
            Assert.Equal(EXPECTED_VALUE, JsonConvert.SerializeObject(value));
        }

        [Fact]
        public void GetContentAsObjectByContentTypeJson_WithFilledBlacklistAndEmptyPartialBlacklist_ReturnsBodyWithTotalMask()
        {
            // Arrange
            const string EXPECTED_VALUE = "{\"Test\":\"1\",\"Card\":{\"Number\":\"******\",\"Password\":\"******\",\"Email\":\"email@test.com\",\"Card\":{\"Number\":\"******\",\"Password\":\"******\"},\"Teste\":{\"Card\":{\"Number\":\"******\",\"Password\":\"******\",\"Email\":\"email@test.com\"}}}}";

            var blacklistPartialMock = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase) { };

            string[] queryBlackList = { "*card.number", "*password" };

            // Act
            var value = _httpContext.GetRequestBody(queryBlackList, blacklistPartialMock);

            // Assert
            Assert.Equal(EXPECTED_VALUE, JsonConvert.SerializeObject(value));
        }

        [Fact]
        public void GetContentAsObjectByContentTypeJson_WithInvalidFunctionInDictionaryOfPartialBlacklist_ShouldAbortJsonMaskingAndReturnValueUnmodified()
        {
            // Arrange
            const string EXPECTED_VALUE = "{\"Test\":\"1\",\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\",\"Email\":\"email@test.com\",\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\"},\"Teste\":{\"Card\":{\"Number\":\"4622943127049865\",\"Password\":\"somepass#here2\",\"Email\":\"email@test.com\"}}}}";
            const int INDEX_OUT_OF_RANGE = 100;

            var blacklistPartialWithInvalidFuncMock = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {"*card.number", value => value[INDEX_OUT_OF_RANGE..] }
            };

            string[] queryBlackList = { "*card.number", "*password" };

            // Act
            var value = _httpContext.GetRequestBody(queryBlackList, blacklistPartialWithInvalidFuncMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, value);
        }

        [Fact]
        public void GetContentAsObjectByContentTypeJson_WithInvalidFunctionInDictionaryOfPartialBlacklist_ShouldHandleExceptionIfTryCatchExistsInTheFunction()
        {
            // Arrange
            const string EXPECTED_VALUE = "{\"Test\":\"1\",\"Card\":{\"Number\":\"***HandlingException***\",\"Password\":\"******\",\"Email\":\"email@test.com\",\"Card\":{\"Number\":\"***HandlingException***\",\"Password\":\"******\"},\"Teste\":{\"Card\":{\"Number\":\"***HandlingException***\",\"Password\":\"******\",\"Email\":\"email@test.com\"}}}}";
            const int INDEX_OUT_OF_RANGE = 100;

            var blacklistPartialWithInvalidFuncMock = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "*card.number", value =>
                    {
                        try
                        {
                            return value[INDEX_OUT_OF_RANGE..];
                        }
                        catch (Exception)
                        {
                            return "***HandlingException***";
                        }                  
                    }
                }
            };

            string[] queryBlackList = { "*card.number", "*password" };

            // Act
            var value = _httpContext.GetRequestBody(queryBlackList, blacklistPartialWithInvalidFuncMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, JsonConvert.SerializeObject(value));
        }
    }
}