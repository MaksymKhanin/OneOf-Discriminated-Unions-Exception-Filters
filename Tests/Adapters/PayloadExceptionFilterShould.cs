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
using System.Threading.Tasks;
using Xunit;

namespace Adapters
{
    [Trait("Category", "Unit")]
    public class PayloadExceptionFilterShould
    {
        private readonly PayloadExceptionFilterAttribute _sut;
        private readonly ExceptionContext _exceptionCtx;

        public PayloadExceptionFilterShould()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeRequest = new Mock<HttpRequest>();
            fakeHttpContext.Setup(ctx => ctx.Request).Returns(fakeRequest.Object);
            _exceptionCtx =
                new ExceptionContext(
                    new ActionContext(fakeHttpContext.Object, new RouteData(), new ActionDescriptor(),
                        new ModelStateDictionary()), new List<IFilterMetadata>());

            _sut = new PayloadExceptionFilterAttribute(NullLogger<PayloadExceptionFilterAttribute>.Instance);
        }

        [Theory(DisplayName = "System exception should not be handled")]
        [InlineAutoData]
        public async Task DoNotHandleSystemException(NotImplementedException exception)
        {
            // Arrange
            _exceptionCtx.Exception = exception;

            // Act
            await _sut.OnExceptionAsync(_exceptionCtx);

            // Assert
            _exceptionCtx.ExceptionHandled.Should().BeFalse();
            _exceptionCtx.Result.Should().BeNull();
        }

        [Theory(DisplayName = "Domain Payload exception should return BadRequest")]
        [InlineAutoData]
        public async Task HandleDomainException(PayloadSourceDownloadException exception)
        {
            // Arrange
            _exceptionCtx.Exception = exception;

            // Act
            await _sut.OnExceptionAsync(_exceptionCtx);

            // Assert
            _exceptionCtx.ExceptionHandled.Should().BeTrue();
            _exceptionCtx.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<ErrorModel>()
                .Which.ErrorCode.Should().Be(ErrorCode.UnhandledPayloadError);
        }
    }
}
