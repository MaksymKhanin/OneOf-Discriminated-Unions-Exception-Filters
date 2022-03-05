using Application;
using Application.Configuration;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Business
{
    [Trait("Category", "Unit")]
    public class PayloadSendHandlingShould
    {
        private IMediator Sut => _host.Services.GetRequiredService<IMediator>();

        private readonly IHost _host;

        public PayloadSendHandlingShould() =>
            _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddPayloadApplicationMediatR();
            })
            .Build();


        [Theory(DisplayName = "Fail when the file was not found in storage")]
        [InlineAutoData]
        public async Task Test001()
        {
            // SimulateNotFound
            var command = new PayloadSendCommand(Guid.Empty);

            var result = await Sut.Send(command);

            result.Switch(
                _ => throw new XunitException("Unexpected success"),
                error => error.Should().NotBeNull(),
                error => throw new XunitException($"Unexpected error '{error.Message}'")
            );
        }

        [Theory(DisplayName = "Fail could not download the source of the file")]
        [InlineAutoData]
        public async Task Test002()
        {
            // SimulateSourceDownloadException
            var command = new PayloadSendCommand(new Guid("00000000-0000-0000-0000-000000000001"));

            var result = await Sut.Send(command);

            result.Switch(
                _ => throw new XunitException("Unexpected success"),
                error => throw new XunitException($"Unexpected error '{error.Message}'"),
                error => error.Should().NotBeNull()
            );
        }
    }
}