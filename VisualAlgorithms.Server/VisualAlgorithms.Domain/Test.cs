﻿using System.Collections.Generic;

namespace VisualAlgorithms.Domain
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AlgorithmId { get; set; }
        public IEnumerable<TestQuestion> TestQuestions { get; set; }
    }
}