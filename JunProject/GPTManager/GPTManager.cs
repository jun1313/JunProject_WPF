using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenAI_API;
using OpenAI_API.Completions;
using System.Runtime.Remoting.Contexts;
using MySqlX.XDevAPI.Common;
using Mysqlx.Session;
using OpenAI_API.Completions;
using OpenAI_API.Chat;
using OpenAI_API.Embedding;
namespace JunProject
{
    public class GPTManager
    {
        private static readonly string GptApiKey;
        private static OpenAIAPI openAiApi;
        public Conversation conversations;

        public GPTManager()
        {
            var GptApiKey = "API_Key";
            APIAuthentication aPIAuthentication = new APIAuthentication(GptApiKey);
            openAiApi = new OpenAIAPI(aPIAuthentication);
            conversations = new Conversation();
        }

        public async Task<string> GenerateText(string prompt)
        {
            try
            {
                // GPT-3 모델 선택
                //var model = OpenAI_API.Models.Model.GPT4;
                //var model = "gpt-4-turbo";
                var model = "gpt-3.5-turbo-0125";
                // 생성할 텍스트의 프롬프트 설정
                //var prompt = "Once upon a time";

                // 생성할 최대 토큰 수 설정
                var maxTokens = 200;

                // GPT-3에 요청 보내기
                var response = new OpenAI_API.Chat.ChatRequest
                {
                    Model = model,
                    Messages = new ChatMessage[]
                    {
                        new ChatMessage(ChatMessageRole.System, "여긴한국이야"),
                        new ChatMessage(ChatMessageRole.User, prompt),
                        new ChatMessage(ChatMessageRole.Assistant, conversations.LastResponse)
                    },
                    MaxTokens = maxTokens,
                    Temperature = 0.2F
                    
                    
                };
                
                var completionResult = await openAiApi.Chat.CreateChatCompletionAsync(response);
                var result = completionResult.Choices[0].Message.Content; //completionResult.Choices[0].Text.Trim();
                conversations.LastResponse = result;
                return result;
          

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return "Error";
            }
        }

    }
    public class Conversation
    {
        public string LastPrompt { get; set; }
        public string LastResponse { get; set; }
    }
    
}
