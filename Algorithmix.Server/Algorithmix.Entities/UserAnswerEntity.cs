﻿namespace Algorithmix.Entities
{
    public class UserAnswerEntity
    {
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
    }
}