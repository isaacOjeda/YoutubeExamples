using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinimalApiArchitecture.Application.Domain.Entities;
using MinimalApiArchitecture.Application.Infrastructure.Persistence;

namespace MinimalApiArchitecture.Application.Features.Products.Commands
{
    public class UpdatePrice : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/products/price", async (IMediator mediator, UpdatePriceCommand command) =>
            {
                return await mediator.Send(command);
            })
            .WithName(nameof(UpdatePrice))
            .WithTags(nameof(Product))
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status404NotFound);
        }

        public class UpdatePriceCommand : IRequest<IResult>
        {
            public int ProductId { get; set; }
            public double Price { get; set; }
        }

        public class UpdatePriceHandler : IRequestHandler<UpdatePriceCommand, IResult>
        {
            private readonly ApiDbContext _context;

            public UpdatePriceHandler(ApiDbContext context)
            {
                _context = context;
            }

            public async Task<IResult> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.FindAsync(request.ProductId);

                if (product is null)
                {
                    return Results.NotFound();
                }

                product.UpdatePrice(request.Price);

                await _context.SaveChangesAsync();

                return Results.Accepted();
            }
        }

        public class UpdatePriceValidator : AbstractValidator<UpdatePriceCommand>
        {
            public UpdatePriceValidator()
            {
                RuleFor(r => r.ProductId).NotEmpty();
                RuleFor(r => r.Price).NotEmpty();
            }
        }
    }
}
