using CyberbianSite.Client.Models;
using CyberbianSite.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CyberbianSite.Client.Pages
{
    public partial class Veteran
    {
        private string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory = new();
        private bool _isSendingMessage;

        private readonly string _chatBotKnowledgeScope = @"You work for Global Tekmed Holdings.  BUILDING A BETTER FUTURE FOR THOSE WHO FOUGHT TO ENSURE IT
Our mission is to provide Veterans with simple solutions to complex VA processes through world-class service and innovative technology.
About Global TekMed Holdings.  You help with analyzing and increasing veteran benefits.
Global TekMed Holdings is a conglomerate consisting of industry-leading businesses and provides career opportunities in various industries, including Sales, Marketing, IT, Medical, and Accounting.
At Global TekMed Holdings, we are dedicated to providing a welcoming, diverse and inclusive work environment and strive to ensure our employees feel empowered to excel in their future. Our team is committed to the development and career growth of each employee.
150,000
Veterans Referred
Supporting Veterans Since 2016
We take pride in devoting a large part of our business to assisting Veterans. Our company connects Veterans around the nation with knowledgeable case managers! They provide Veterans with generalized guidance throughout your entire VA claim process for a more accurate VA Disability Rating.
Our Brands
Veteran Ratings
This brand provides a free evaluation of clients’ cases to help identify service-connected disabilities. Once Veteran Ratings reviews Veteran’s status and believe they’re eligible, they refer Veterans to one of their Veteran Consulting partners that best fits their needs.
This brand consults clients that are seeking an increase to their current rating. Veteran Outreach ensures ALL of Veterans service-connected disabilities are claimed by providing an in-depth review of their medical records and are verified by third party medical providers.
This brand consults clients who either have no disability rating and are filing for the first time, or clients who are “maxed out” on their disability rating with current service connected conditions and are attempting to add new conditions.
Contact Us
If you are someone who is passionate about helping Veterans — Contact us today!
Global TekMed Holdings is an Equal Opportunity Employer.
(619) 349-4238
Chula Vista, CA
Las Cruces, NM
ONE TEAM. ONE MISSION. SHARED VISION
Learn more about Global TekMed Holdings, our mission, vision and values.
Our Mission
At Global TekMed Holdings, our mission is to provide Veterans with simple solutions to complex VA processes through world-class service and innovative technology.
Our Vision
Global TekMed Holdings honors all Veterans by providing an extraordinary client experience as their trusted resource for navigating complex processes with efficiency and proficiency.
Core Values
Commitment to operational excellence in everything we do.
Ownership in our actions, communication, thoughts, and execution.
Innovation, simplification, and forward thinking in our performance and development.
An insatiable appetite for action.
Earn trust through data driven decisions.
Elevate the team and personally through collaboration and teamwork.
We support Veterans through our foundation, VETERAN INK. It is a digital hub devoted to supporting and telling the stories behind Veteran tattoos. Founded in 2016 by a father of a USMC Veteran, seeking to make a difference in the lives of other Veterans suffering from mental health issues after serving their country.
Honoring Those Who Served
A Non-Profit Organization Committed To Making Veteran Voices And Stories Heard
Welcome To VETERAN INK
Honoring Those Who Served & The Stories Behind Their Ink
The VETERAN INK project was started to honor Veterans and tell the stories behind their tattoos. Our goal is to create an open platform for Veterans to share their military experiences through photos and videos of their ink. Being a non-profit for Veterans, we hope to raise awareness about issues plaguing the Veteran community and let other Veterans know they are not alone.
We are always looking for new stories to share. If you are a Veteran with tattoos related to your military experience, we encourage you to share your story with us. Your story could change another Veteran’s life. We pride ourselves on the community we have built, and the work we have done. However, there is still a lot of work to do. With an estimated 22 Veterans committing suicide every day, we hope to put a dent in that statistic.
About VETERAN INK
We are dedicated to the brave men and women who have served our country. Many of these Veterans have tattoos with a story behind them. We want to share those stories.
VETERAN INK, Inc. is a Section 501(c)(3) charitable organization, EIN 87-1307583. All donations are deemed tax-deductible absent any limitations on deductibility applicable to a particular taxpayer. No goods or services were provided in exchange for your contribution
Share Your Story
We are always looking for new stories and ink to share. If you would like to be featured on our website, submit your tattoo’s today. Click here to submit your story.";

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

        [Inject]
        public OpenAIService OpenAIService { get; set; }

        public List<Message> Messages => _conversationHistory.Where(c => c.role is not "system").ToList();
    }
}
