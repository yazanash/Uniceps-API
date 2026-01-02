using Uniceps.app.DTOs.ProductDtos.FAQDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.Extensions.ProductMappers
{
    public class FAQMapperExension : IMapperExtension<FrequentlyAskedQuestion, FrequentlyAskedQuestionDto, FrequentlyAskedQuestionCreationDto>
    {
        public FrequentlyAskedQuestion FromCreationDto(FrequentlyAskedQuestionCreationDto data)
        {
            return new FrequentlyAskedQuestion 
            {
                Question = data.Question,
                Answer = data.Answer,
                QuestionAr = data.QuestionAr,
                AnswerAr = data.AnswerAr,
                ProductId = data.ProductId 
            };
        }


        public FrequentlyAskedQuestionDto ToDto(FrequentlyAskedQuestion data)
        {
            return new FrequentlyAskedQuestionDto 
            { 
                Id = data.Id,
                Question = data.Question,
                Answer = data.Answer,
                ProductId = data.ProductId,
                QuestionAr = data.QuestionAr,
                AnswerAr = data.AnswerAr
            };
        }
    }
}
