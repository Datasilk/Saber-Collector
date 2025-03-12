using Saber.Vendors.Collector.Models;
using OpenAI.Chat;
using Saber.Vendors.Collector.Models.Article;
using System.Text.Json;

namespace Saber.Vendors.Collector
{
    public static class LLM
    {
        public static string PrivateKey = "";

        public static string Prompt(string system, string assistant, string user)
        {
            ChatClient client = new ChatClient("qwen-turbo-latest", new System.ClientModel.ApiKeyCredential(PrivateKey), new OpenAI.OpenAIClientOptions()
            {
                Endpoint = new Uri("https://dashscope-intl.aliyuncs.com/compatible-mode/v1")
            });

            List<ChatMessage> prompt = new List<ChatMessage>()
            {
                new SystemChatMessage(system),
                new AssistantChatMessage(assistant),
                new UserChatMessage(user)
            };
            ChatCompletion completion = client.CompleteChat(prompt);
            return completion.Content[0].Text;
        }

        public static string GetArticleContents(AnalyzedArticle article)
        {
            var response = Prompt($@"#Objective#
analyze an HTML document and extract the following information:
* the article title, publish date (formatted as yyyy-MM-dd), and author name
* article text & images
* a list of people referenced in the article
* a list of companies referenced in the article
* a list of technologies referenced in the article
* a hierarchy of user comments

#rules#
* each element in the list of elements will be either text from the article, an image URL within the article, or a table of data
* create an array of elements based on the blocks of text, images, and tables of data found within the article, in the order they are found within the article
* do not include the article title, publish date, or author name in the list of elements
* if the block of text includes a heading, put the heading in the ""title"" property for the element
* if some article text is an anchor link, convert the HTML anchor link into a markdown link to include in the element value
* convert unordered lists to markdown
* convert tables of data to a JSON object array and use the ""data"" property to store the array for the element
* each element in the list of elements will contain the ""type"" property (either 0 for text or 1 for image URL as the type) and the ""value"" property will be either article text or an image URL
* if the article text is not primarily in English, translate the article to English
* translate all user comments to English
* when generating a list of people referenced in the article text, try to find information about their job position, the company they work for, and any quotes that they said within the article text. Make sure the quote belongs to them and not someone else in the article
* generate a list of company names found within the article text

#Output#
use the following json template as the output and only output the json and nothing else
{{
    ""title"":"",
    ""published"": """",
    ""author"": """",
    ""elements"":[
        {{
            ""type"": int, 
            ""title"":"""",
            ""value"": """",
            ""caption"":"""",
            ""data"":[]
        }}
    ],
    ""people"":[
        {{
            ""name"": """", 
            ""position"": """", 
            ""company"": """", 
            ""quotes"": [""""]
        }}
    ],
    ""companies"":[
        {{
            ""name"": """"
        }}
    ],
    ""technologies"":[""""],
    ""comments"":[
        {{
            ""name"": """", 
            ""company"": """"
        }}

    ],
}}",
            $@"You are a helpful assistant that can read HTML markup, extract text & URL content from the HTML while ignoring HTML elements that represent the website's user interfce or 3rd-party advertisements",
            article.rawHtml
            );

            return response.Replace("```json", "").Replace("```", "");
        }
    }
}
