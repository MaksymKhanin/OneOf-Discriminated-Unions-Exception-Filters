using Api.Errors;
using Api.Errors.Models;
using Application.Exceptions;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Adapters
{
    [Trait("Category", "Unit")]
    public class SystemExceptionFilterShould
    {
        public SystemExceptionFilterShould()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            _fakeResponse = new Mock<HttpResponse>();
            var fakeRequest = new Mock<HttpRequest>();
            _fakeResponse.SetupProperty(response => response.StatusCode);
            fakeHttpContext.Setup(ctx => ctx.Response).Returns(_fakeResponse.Object);
            fakeHttpContext.Setup(ctx => ctx.Request).Returns(fakeRequest.Object);
            _exceptionCtx =
                new ExceptionContext(
                    new ActionContext(fakeHttpContext.Object, new RouteData(), new ActionDescriptor(),
                        new ModelStateDictionary()), new List<IFilterMetadata>());

            _sut = new SystemExceptionFilterAttribute(NullLogger<SystemExceptionFilterAttribute>.Instance);
        }

        private readonly ExceptionContext _exceptionCtx;
        private readonly Mock<HttpResponse> _fakeResponse;
        private readonly SystemExceptionFilterAttribute _sut;

        [Theory(DisplayName = "Domain Payload exception should not be handled")]
        [InlineAutoData]
        public async Task DoNotHandleDomainException(PayloadSourceDownloadException exception)
        {
            // Arrange
            _exceptionCtx.Exception = exception;

            // Act
            await _sut.OnExceptionAsync(_exceptionCtx);

            // Assert
            _exceptionCtx.ExceptionHandled.Should().BeFalse();
            _exceptionCtx.Result.Should().BeNull();
        }

        [Theory(DisplayName = "NotImplementedException exception should return 501")]
        [InlineAutoData]
        public async Task HandleNotImplementedException(NotImplementedException exception)
        {
            // Arrange
            _exceptionCtx.Exception = exception;

            // Act
            await _sut.OnExceptionAsync(_exceptionCtx);

            // Assert
            _exceptionCtx.ExceptionHandled.Should().BeTrue();
            _fakeResponse.Object.StatusCode.Should().Be((int)HttpStatusCode.NotImplemented);
            _exceptionCtx.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<ErrorModel>()
                .Which.ErrorCode.Should().Be(ErrorCode.UnhandledSystemError);
        }

        [Theory(DisplayName = "System exception should return 500 if it is not NotImplementedException")]
        [InlineAutoData]
        public async Task HandleSystemException(ArgumentNullException exception)
        {
            // Arrange
            _exceptionCtx.Exception = exception;

            // Act
            await _sut.OnExceptionAsync(_exceptionCtx);

            // Assert
            _exceptionCtx.ExceptionHandled.Should().BeTrue();
            _fakeResponse.Object.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            _exceptionCtx.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<ErrorModel>()
                .Which.ErrorCode.Should().Be(ErrorCode.UnhandledSystemError);
        }
    }
}
