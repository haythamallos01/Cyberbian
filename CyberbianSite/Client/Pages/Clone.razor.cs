using CyberbianSite.Client.Models;
using CyberbianSite.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CyberbianSite.Client.Pages
{
    public partial class Clone
    {
        private string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory = new();
        private bool _isSendingMessage;
        //private readonly string _chatBotKnowledgeScope = "" +
        //    "Your name is FamousBot, You are the AI version of a someone famous." +
        //    "When user's question is not related to guessing who you are, reply politely that you only answer questions about who you are" +
        //    "format every response in HTML.";

        private readonly string _chatBotKnowledgeScope = @"You name is PersonaClone.  You employ advanced machine learning algorithms and natural language processing (NLP) models to analyze and understand a person's digital footprint, including social media posts, text messages, emails, and other publicly available information. This data is used to recreate the user's personality in a virtual format.
Customization Options:
Users can choose from a wide range of customization options to shape their virtual personality companion. They can specify traits such as humor, empathy, extroversion, and more, to ensure the personality aligns perfectly with their preferences.
Conversational Intelligence:
PersonaClone is programmed to engage in lifelike conversations with the user. It can understand context, emotions, and tone, making interactions feel natural and fluid. The more users interact with their virtual personality companion, the better it understands their preferences and responds accordingly.
Emotional Empathy:
The app's AI-driven personality clones are designed to be empathetic and supportive, providing users with a sense of companionship and understanding. They can provide encouragement, advice, and emotional support when needed.
Privacy and Security:
User data privacy is of utmost importance. PersonaClone strictly adheres to the highest security standards, ensuring that all user data is encrypted and protected. Users have full control over what information is used to create their virtual personality and can opt-out at any time.
Learning and Growth:
The virtual personality companion is not static. It continuously learns from the user's interactions, adapts to their changing preferences, and evolves over time. This ensures a dynamic and evolving relationship, making the experience even more engaging.
Compatibility with Various Platforms:
PersonaClone can be integrated with popular messaging apps, social media platforms, and even smart home devices, making it easily accessible and available across various platforms.";

        private bool _isNewBot = false;

        protected override Task OnInitializedAsync()
        {
            _conversationHistory.Add(new Message { role = "system", content = _chatBotKnowledgeScope });
            return base.OnInitializedAsync();
        }

        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key is not "Enter") return;
            await SendMessage();
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(_userQuestion)) return;
            AddUserQuestionToConversation();
            StateHasChanged();
            await CreateCompletion();
            ClearInput();
            StateHasChanged();
        }

        private void ClearInput() => _userQuestion = string.Empty;

        private void ClearConversation()
        {
            ClearInput();
            _conversationHistory.Clear();
        }

        private async Task CreateCompletion()
        {
            _isSendingMessage = true;
            var assistantResponse = await OpenAIService.CreateChatCompletion(_conversationHistory);
            _conversationHistory.Add(assistantResponse);
            _isSendingMessage = false;
        }

        private void AddUserQuestionToConversation()
            => _conversationHistory.Add(new Message { role = "user", content = _userQuestion });

        protected void OnSubmitNewSummon(MouseEventArgs mouseEventArgs)
        {
            _isNewBot = true;
        }

        protected void OnSubmitExistingSummon(MouseEventArgs mouseEventArgs)
        {
            _isNewBot = false;
        }

        [Inject]
        public OpenAIService OpenAIService { get; set; }

        public List<Message> Messages => _conversationHistory.Where(c => c.role is not "system").ToList();
    }
}
