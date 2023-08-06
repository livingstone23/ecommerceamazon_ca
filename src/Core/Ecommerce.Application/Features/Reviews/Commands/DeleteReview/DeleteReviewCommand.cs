using MediatR;

namespace Ecommerce.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest
{
    

    public int ReviewId { get; set; }

    //Permite eveluar si el review es enviado en la peticion
    public DeleteReviewCommand(int reviewId)
    {
       ReviewId = reviewId == 0 ? throw new ArgumentNullException(nameof(reviewId)) : reviewId;
    }
}

