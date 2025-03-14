﻿// Copyright 2014 Pēteris Ņikiforovs
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LanguageDetection
{
    class LanguageProfile
    {
        [JsonPropertyName("name")]
        public string Code { get; set; }
        [JsonPropertyName("freq")]
        public Dictionary<string, int> Frequencies { get; set; }
        [JsonPropertyName("n_words")]
        public int[] WordCount { get; set; }
    }
}
