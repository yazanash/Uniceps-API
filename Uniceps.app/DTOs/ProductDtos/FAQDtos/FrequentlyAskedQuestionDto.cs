namespace Uniceps.app.DTOs.ProductDtos.FAQDtos
{
    public class FrequentlyAskedQuestionDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
        public string QuestionAr { get; set; } = "";
        public string AnswerAr { get; set; } = "";
        public int ProductId { get; set; }
    }
}
