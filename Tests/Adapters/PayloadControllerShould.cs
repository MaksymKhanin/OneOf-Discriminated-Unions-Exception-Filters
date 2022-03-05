using Api.Controllers;
using Api.Errors.Models;
using Application;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OneOf;
using OneOf.Types;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Adapters
{
    [Trait("Category", "Unit")]
    public class PayloadControllerShould
    {
        private readonly PayloadController _sut;
        private readonly Mock<ILogger<PayloadController>> _loggerMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();

        public PayloadControllerShould() => _sut = new PayloadController(_mediatorMock.Object, _loggerMock.Object);

        [Theory(DisplayName = "When Sending correct payload should not occur exception")]
        [InlineAutoData]
        public async Task Test001(Guid ticketId)
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PayloadSendCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>>(default))
                .Verifiable("File was not sent.");

            Func<Task> f = async () => await _sut.Send(ticketId, default);

            await f.Should().NotThrowAsync();
        }

        [Theory(DisplayName = "When Sending correct payload result should be Accepted")]
        [InlineAutoData]
        public async Task Test002(Guid ticketId)
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PayloadSendCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>>(default))
                .Verifiable("File was not sent.");

            var result = await _sut.Send(ticketId, default);

            result.Should().BeOfType<AcceptedResult>();
        }

        [Theory(DisplayName = @"When Sending payload which does not exist in storage PayloadSourceNotFound 
               error should be in response")]
        [InlineAutoData]
        public async Task Test003(Guid ticketId)
        {
            // Simulate not found

            ticketId = Guid.Empty;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PayloadSendCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>>(new PayloadSourceNotFoundError(ticketId)))
                .Verifiable("File was not sent.");

            var result = await _sut.Send(ticketId, default);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<ErrorModel>()
                .Which.ErrorCode.Should().Be(ErrorCode.PayloadSourceNotFound);
        }

        [Theory(DisplayName = @"When Sending payload which can not be loaded PayloadSourceDownloadError 
               error should be in response")]
        [InlineAutoData]
        public async Task Test004(Guid ticketId)
        {
            // Simulate Source Download Error

            ticketId = new Guid("00000000-0000-0000-0000-000000000001");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PayloadSendCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<OneOf<Success, PayloadSourceNotFoundError, PayloadSourceDownloadError>>(new PayloadSourceDownloadError()))
                .Verifiable("File was not sent.");

            var result = await _sut.Send(ticketId, default);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<ErrorModel>()
                .Which.ErrorCode.Should().Be(ErrorCode.PayloadSourceDownloadError);
        }
    }
}